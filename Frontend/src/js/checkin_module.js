import requestModule from "./request_module";

let request = new requestModule();

export default class checkin_module {
    /**
     * providing the two uac groups this tapp is based on. The module can access them anytime.
     */
    constructor(signupGroup, checkinGroup) {
        this.signupGroup = signupGroup;
        this.checkinGroup = checkinGroup;
    }

    init() {

        /**
         * This is used for future functions. This if-condition handles the view and the events for that functions.
         * More information at the end of this script.
         */
        //if (chayns.env.isApp)
        //{
        //    document.getElementById('mobileCheckIn').classList.remove("hidden");
        //    document.getElementById('qr').addEventListener('click', this.qr, false)
        //    document.getElementById('rfid').addEventListener('click', this.rfid, false)
        //}
        //else
            //document.getElementById('noApp').classList.remove("hidden");

        /**
         * Set the current 'signed in user list' to null. Only takes effect if the view is switched more than once.
         * Afterwards the getAllSignedIn()-Method loads all users that have signed up for the event and builds a list in the checkin view.
         */
        document.getElementById('signedInUsersList').innerHTML = null;
        this.getAllSignedIn();
    }

    /**
     *  Performs a request to check in a specific user. On success the specific list item will be removed
     */
    checkIn(userId, container) {
        request.addUserToGroup(this.checkinGroup, userId).then((response) => {
            if (response.status === 200 && container) {
                //Remove the html element representing the list item of the user
                container.remove();
                //If the user was the last user in the list, set a fallback information text.
                if (document.getElementById('signedInUsersList').innerHTML === null) {
                    document.getElementById('signedInUsers').innerHTML = "No more users left to check in."
                }
            } else {
                chayns.dialog.alert(null, "Something went wrong..")
            }
        })
    }

    /**
     * This method gets all users, that are in the 'signed up uac group' and builds a list for checking them in
     */
    getAllSignedIn() {

        request.getAllGroupMembers(this.signupGroup).then((data) => {
            //'On success' or if 'users are signed in', start building the list
           if (data.status === 200) {
               data.json().then((users) => { //get the data from the requests response..
                   //In a foor loop we're building list items that are structured as 'accordion__item's (see our documentation)
                   for (let i=0; i<users.length; i++) {
                       let container = document.createElement("div");
                       container.classList.add("accordion__item");
                       container.style.display = "list-item";

                       //This list item gets some personal information to identify the specific user
                       let username = document.createElement("span");
                       username.innerText = users[i].name;
                       username.style.float = "left";
                       container.appendChild(username);

                       //Also this list item gets a button to check the user in
                       let btn = document.createElement("button");
                       btn.classList.add("button");
                       btn.style.float = "right";
                       btn.innerText = "CheckIn";
                       btn.addEventListener("click", () => {
                           this.checkIn(users[i].userId, container);
                       });
                       container.appendChild(btn);
                       //These elements are combined an then append to the list.
                       document.getElementById('signedInUsersList').appendChild(container);
                   }
                   //If the list was build for the first time, it will be shown
                   document.getElementById('signedInUsers').classList.remove("hidden");
               });
           }
        });
    }

    /**
     *  The following two functions are planned for future checkin techniques.
     *  The user checking in other user will be able to scan a users qr code or to scan his rfid card (nfc), to check him in.
     */

    //qr() {
    //    console.log("qr");
    //    chayns.scanQRCode().then((result)=> {
    //        if (result) {
    //            request.getGroupMember(0, result).then((data) => {
    //
    //            });
    //        }
    //    });
    //}
    //
    //rfid() {
    //    console.log("rfid");
    //    setTimeout(()=> {
    //        chayns.stopNfcDetection();
    //    }, 10000);
    //    chayns.startNfcDetection((result) => {
    //        chayns.stopNfcDetection();
    //
    //        if (result) {
    //            request.getGroupMember(0, result).then((data) => {
    //
    //            });
    //        }
    //    }, 100, true);
    //}
}