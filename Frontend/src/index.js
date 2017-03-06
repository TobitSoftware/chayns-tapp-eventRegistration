/**
 * Importing additional javascript classes that are included in this project
 */
import requestModule from "./js/request_module";
import userModule from "./js/user_module";
import checkinModule from "./js/checkin_module";

/**
 * chayns ready promise
 */
chayns.ready.then(function resolved() {
    //If chayns is initialized, start initializing the tapp
    init();
}).catch(function rejected() {
    console.error('no chayns environment found. you need to implement this website in your chayns-website');
}).then(function always() {
    //Will always be executed
});


/**
 * Will be executed when the chayns-API is loaded
 */
const init = () => {
    'use strict';
    let request = new requestModule();
    let status = -1; //Represents the users event stauts. -1 means he has'nt signed in or checked in, 0 is signed in, 1 is checked in.
    let signupGroup; //The id of the uac group containing all members that signed up for this event
    let checkinGroup; //The id of the uac group containing all members that were checked in for this event

    /**
     * Load tapp uac groups and check if the user is in the signedIn oder checkedIn group
     */
    function initGroups() {
        return new Promise((resolve) => {
            request.getGroups().then((response) => {
                if (response)
                    response.json().then((data) => { //get the data..
                        if (data.length === 2) { //only the two tapp uac groups were allowed. If more are present, an error occured and the tapp groups should be checked manually
                            //specify the two necessary uac groups used in this tapp for signing in and checking in
                            for (let i = 0; i<data.length; i++) {
                                if (data[i].name === "members_" + chayns.env.site.tapp.id)
                                    signupGroup = data[i].userGroupId;
                                else if (data[i].name === "checkedIn_" + chayns.env.site.tapp.id)
                                    checkinGroup = data[i].userGroupId;
                            }

                            //check if the current user is member of any of the two groups. The status set in this if-condition is used in further methods.
                            request.checkGroup(data[0].userGroupId).then((res) => {
                                if (res && res.status == 200)
                                    if (data[0].name === "members_" + chayns.env.site.tapp.id)
                                        status = 0;
                                    else if (data[0].name === "checkedIn_" + chayns.env.site.tapp.id)
                                        status = 1;
                            }).then(() => {
                                //check the second group. The user cant be member of both.
                                request.checkGroup(data[1].userGroupId).then((res) => {
                                    if (res && res.status == 200)
                                        if (data[1].name === "members_" + chayns.env.site.tapp.id)
                                            status = 0;
                                        else if (data[1].name === "checkedIn_" + chayns.env.site.tapp.id)
                                            status = 1;
                                }).then(() => {
                                    //return the status two the further workflow
                                    resolve(status);
                                })
                            });
                        }
                    });
            });
        });
    }

    initGroups().then((eventStatus) => {
        //get the different modules for the two views: 'user' and 'checkIn'
        let user = new userModule(eventStatus);
        let checkin = new checkinModule(signupGroup, checkinGroup);
        user.init(eventStatus, signupGroup);

        if (chayns.utils.getJwtPayload(chayns.env.user.tobitAccessToken).IsAdmin) { //admins are allowed to switch to the checkin view. Using the app is recommended
            chayns.ui.modeSwitch.init({
                items: [{
                    name: chayns.env.user.name, //setting the username as a modes name
                    value: 0,
                    default: true
                }, {
                    name: 'CheckIn',
                    value: 1
                }],
                callback: (data) => {
                    //On mode switch display the correct view. Only possible if the user is a site admin.
                    if (data.value === 1) {
                        checkin.init();
                        document.getElementById('userView').classList.add('hidden');
                        document.getElementById('checkinView').classList.remove('hidden');
                    } else {
                        user.init(eventStatus, signupGroup);
                        document.getElementById('checkinView').classList.add('hidden');
                        document.getElementById('userView').classList.remove('hidden');
                    }
                }
            });
        }
    });

    /**
     * The webpack files use the webpack-replace loader for replacing the "server_url"
     * @type {string}
     */
    console.log('########################################################');
    console.log(`You are running the ##server_url##-Version of your Tapp`);
    console.log('########################################################');
};