(function (app) {
    var eventInfo = null,
        nfcCheckEnabled = false;

    var registerContainer = document.querySelector('.registerContainer'),
        registeredContainer = document.querySelector('.registeredContainer'),
        soldOutContainer = document.querySelector('.soldOutContainer'),
        views = {
            user: document.querySelector('.view__user'),
            checkIn: document.querySelector('.view__checkin')
        },
        checkInWrapper = document.querySelector('.checkin-wrapper');


    chayns.ready.then(function init() {
        chayns.hideTitleImage();
        eventJs.getEventInformation().then(function (data) {
            eventInfo = data;

            if (eventInfo.registered) {
                registeredContainer.classList.remove('hidden');
            } else {
                if (eventInfo.available <= 0) {
                    soldOutContainer.classList.remove('hidden')
                } else {
                    registerContainer.classList.remove('hidden');
                    document.querySelector('.register').addEventListener('click', register);
                }
            }

            if (isAccounting()) {
                initCheckIn();
            }

            document.querySelector('.tapp').classList.remove('hidden');
        });
    });

    /**
     * initialize check in
     */
    function initCheckIn() {
        showCheckInView();
        chayns.ui.modeSwitch.init({
            items: [
                {
                    name: chayns.env.user.name,
                    id: 0
                },
                {
                    name: 'Check-In',
                    default: true,
                    id: 1
                }
            ],
            callback: function (item) {
                if (item.id === 0) {
                    showUserView();
                } else {
                    showCheckInView();
                }
            }
        });
        document.querySelector('.rfid').addEventListener('click', checkInRFID);
        document.querySelector('.qr').addEventListener('click', checkInQR);

        if (chayns.env.isChaynsWeb) {
            checkInWrapper.classList.add('hidden');
            document.querySelector('.chaynsweb-wrapper').classList.remove('hidden');
        } else if (chayns.env.isIOS) {
            document.querySelector('.rfid').classList.add('hidden');
        } else {
            chayns.setNfcCallback(nfcCallback);
        }
    }

    /**
     * check in rfid cards
     */
    function checkInRFID() {
        if (nfcCheckEnabled) {
            nfcCheckEnabled = false;
            chayns.hideWaitCursor();
            return;
        }
        nfcCheckEnabled = true;
        chayns.showWaitCursor();
    }

    /**
     * callback for nfc cards
     * @param rfid
     */
    function nfcCallback(rfid) {
        if (nfcCheckEnabled) {
            chayns.hideWaitCursor();
            nfcCheckEnabled = false;
            eventJs.getUserByRfid(rfid)
                .then(function (user) {
                    checkIn(user.userId);
                })
        }

    }

    /**
     * check in qr codes
     */
    function checkInQR() {
        chayns.scanQRCode()
            .then(function (qrCode) {
                if (qrCode === undefined) {
                    return;
                }
                return eventJs.getUserByQrCode(qrCode);
            })
            .then(function (user) {
                if (user === null) {
                    return;
                }
                checkIn(user.userId);
            })
    }

    /**
     * show success response
     */
    function showSuccess() {
        checkInWrapper.classList.add('success');
        setTimeout(function () {
            checkInWrapper.classList.remove('success');
            checkInWrapper.classList.remove('failure');
        }, 500)
    }

    /**
     * show error response
     */
    function showFailure() {
        checkInWrapper.classList.add('failure');
        setTimeout(function () {
            checkInWrapper.classList.remove('success');
            checkInWrapper.classList.remove('failure');
        }, 500)
    }

    /**
     * show check-in view
     */
    function showCheckInView() {
        views.user.classList.add('hidden');
        views.checkIn.classList.remove('hidden');
    }

    /**
     * show user view
     */
    function showUserView() {
        views.checkIn.classList.add('hidden');
        views.user.classList.remove('hidden');
    }

    /**
     * register current user
     */
    function register() {
        if (!chayns.env.user.isAuthenticated) {
            chayns.login();
        }

        chayns.showWaitCursor();
        eventJs.register().then(function (data) {
            chayns.hideWaitCursor();
            switch (data) {
                case eventJs.result.register.success:
                    registerContainer.classList.add('hidden');
                    registeredContainer.classList.remove('hidden');
                    break;
                case eventJs.result.register.soldOut:
                    chayns.dialog.alert(null, 'Es Tut uns leid aber es sind bereits alle Tickets vergeben.');
                    soldOutContainer.classList.remove('hidden');
                    registerContainer.classList.add('hidden');
                    registeredContainer.classList.add('hidden');
                    break;
                default:
                    chayns.dialog.alert(null, 'Da hat etwas nicht geklappt, versuche es später ncoh einmal.');
                    break;
            }
        }).catch(function () {
            chayns.dialog.alert(null, 'Da hat etwas nicht geklappt, versuche es später ncoh einmal.');
        });
    }

    /**
     * check in
     * @param userId
     */
    function checkIn(userId) {
        chayns.showWaitCursor();
        eventJs.checkIn(userId).then(function (data) {
            chayns.hideWaitCursor();
            switch (data) {
                case eventJs.result.checkIn.success:
                    showSuccess();
                    break;
                case eventJs.result.checkIn.alreadyCheckedIn:
                    showFailure();
                    //chayns.dialog.alert(null, 'Der User ist bereits eingecheckt.');
                    break;
                case eventJs.result.checkIn.notRegistered:
                    showFailure();
                    //chayns.dialog.alert(null, 'Der User ist nicht registriert.');
                    break;
                default:
                    showFailure();
                    chayns.dialog.alert(null, 'Da hat etwas nicht geklappt, versuche es später noch einmal.');
                    break;
            }
        }).catch(function () {
            showFailure();
            chayns.dialog.alert(null, 'Da hat etwas nicht geklappt, versuche es später noch einmal.');
        });
    }

    /**
     * check if the current user is in the account uac group
     * @returns {boolean}
     */
    function isAccounting() {
        var groups = chayns.env.user.groups;
        for (var i = 0, l = groups.length; i < l; i++) {
            if (groups[i].id === 5677) {
                return true;
            }
        }
        return false;
    }

}(window.app = {}));