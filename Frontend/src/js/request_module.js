export default class request_module {
    /**
     * This module contains all methods, that are performing any requests.
     * Usually the requests just get performed and then return the response to the caller function as a resolved promise.
     */
    constructor() {
        this.url = "Path/To/EventRegistration.Api"
    }

    /**
     * Get the uac groups for signed in and checked in users. These are used in further logics.
     */
    getGroups() {
        return new Promise((resolve) => {
            fetch(this.url + "/Group?locationId=" + chayns.env.site.locationId + "&tappId=" + chayns.env.site.tapp.id).then(function(r) {
                resolve(r);
            });
        });
    }

    /**
     * Check if a user is in a specific uac group. Used for determining whether a user is signed in OR checked in.
     */
    checkGroup(groupId) {
        return new Promise((resolve) => {
            fetch(this.url + "/User/"+ groupId + "?locationId=" + chayns.env.site.locationId + "&tappId=" + chayns.env.site.tapp.id + "&userId=" + chayns.env.user.id).then(function(r) {
                resolve(r);
            });
        });
    }

    /**
     * Add an user to an uac group. If he isnt signed in or checked in, he will be added to the members_ group.
     * Otherwise he will get into the checkedIn_ group and removed from the members_ group.
     */
    addUserToGroup(groupId, userId) {
        return new Promise((resolve) => {
            let config = {
                "LocationId": chayns.env.site.locationId,
                "TappId": chayns.env.site.tapp.id,
                "UserId": userId ? userId : chayns.env.user.id,
                "GroupId": groupId
            };
            fetch(this.url + "/User", {
                method: 'POST',
                body: JSON.stringify(config),
                headers: { "Content-Type" : "application/json" }
            }).then((r) => {
                resolve(r);
            });
        })
    }

    /**
     * This method is used to get all uac group members of the members_ group to display them in the checkin view.
     */
    getAllGroupMembers(groupId) {
        return new Promise((resolve) => {
            fetch(this.url + "/User/" +  groupId + "?locationId=" + chayns.env.site.locationId + "&tappId=" + chayns.env.site.tapp.id).then((r) => {
               resolve(r);
            });
        });
    }

    //For future functions ( checkin via rfid scan or qr scan
    getGroupMember(type, identifier) { //type: 0=rfid, 1=qr
        return new Promise((resolve) => {
           resolve(r);
        });
    }
}
