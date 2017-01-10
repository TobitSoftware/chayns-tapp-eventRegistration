(function (event) {

    /**
     * returns the server url dependent on the location id, current controller and tapp id
     * @param controller
     * @returns {string}
     */
    function getServerUrl(controller) {
        return '//academy.tobit.com/api/eventSystem/backend/' + chayns.env.site.locationId + '/' + controller + '/' + chayns.env.site.tapp.id;
    }

    /**
     * returns the current api token
     * @returns {*}
     */
    function getApiToken() {
        return fetch(getServerUrl('ApiToken'), {
            method: 'GET',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': 'bearer ' + chayns.env.user.tobitAccessToken
            }
        }).then(function (res) {
            return res.json();
        }).catch(function (res) {
            chayns.hideWaitCursor();
            console.error(res);
            throw res;
        });
    }

    /**
     * returns the user that is mapped to the rfid
     * @param rfid
     * @returns {*}
     */
    event.getUserByRfid = function (rfid) {
        return getApiToken().then(function (token) {
            return fetch('https://api.chayns.net/v1.0/' + chayns.env.site.locationId + '/User?rfid=' + rfid, {
                method: 'GET',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + token
                }
            }).then(function (res) {
                return res.json();
            }).then(function (data) {
                if (data.data != null && data.data.length > 0) {
                    return data.data[0];
                } else {
                    return null;
                }
            }).catch(function (res) {
                chayns.hideWaitCursor();
                console.error(res);
                throw res;
            });
        })

    };

    /**
     * returns the user that is mapped to the qr code
     * @param qrCode
     * @returns {*}
     */
    event.getUserByQrCode = function (qrCode) {
        return getApiToken().then(function (token) {
            return fetch('https://api.chayns.net/v1.0/' + chayns.env.site.locationId + '/User?qrcode=' + qrCode, {
                method: 'GET',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + token
                }
            }).then(function (res) {
                return res.json();
            }).then(function (data) {
                if (data.data != null && data.data.length > 0) {
                    return data.data[0];
                } else {
                    return null;
                }
            }).catch(function (res) {
                chayns.hideWaitCursor();
                console.error(res);
                throw res;
            });
        })

    };

    /**
     * returns the event information
     * @returns {*}
     */
    event.getEventInformation = function () {
        return fetch(getServerUrl('EventUser') + '?userId=' + chayns.env.user.id, {
            method: 'GET',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            }
        }).then(function (res) {
            return res.json();
        }).catch(function (res) {
            chayns.hideWaitCursor();
            console.error(res);
            throw res;
        });
    };

    /**
     * register an event
     * @returns {*}
     */
    event.register = function () {
        return fetch(getServerUrl('EventUser'), {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + chayns.env.user.tobitAccessToken
            }
        }).then(function (res) {
            return res.status;
        }).catch(function (res) {
            chayns.hideWaitCursor();
            console.error(res);
            throw res;
        });
    };

    /**
     * check in for an event
     * @param userId
     * @returns {*}
     */
    event.checkIn = function (userId) {
        return fetch(getServerUrl('EventUser'), {
            method: 'PATCH',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + chayns.env.user.tobitAccessToken
            },
            body: JSON.stringify(userId)
        }).then(function (res) {
            return res.status;
        }).catch(function (res) {
            chayns.hideWaitCursor();
            console.error(res);
            throw res;
        });
    };

    event.result = {
        register: {
            success: 200,
            soldOut: 400,
            failed: 409,
            ServerError: 500
        },
        checkIn: {
            success: 200,
            alreadyCheckedIn: 204,
            notRegistered: 404,
            failed: 409,
            ServerError: 500
        }
    }

}((window.eventJs = {})));