import requestModule from "./request_module";

let request = new requestModule();

export default class user_module {
    /**
     + providing a status that determines whether the user is not signed in, signed in or checked in. The module can access this status anytime
     */
    constructor(status) {
        this.status = status;
    }

    /**Showing the correct view, dependent from the user status provided in the constructor
     * In case of status = -1 ( not signed in ) a eventlistener will be set for signing up
     */
    init(status, signupGroup) {
        switch (status) {
            case -1:
                document.getElementById('takePart').classList.remove("hidden");
                document.getElementById('signUpBtn').addEventListener('click', () => { this.signUp(signupGroup) }, false);
                break;
            case 0:
                document.getElementById('assigned').classList.remove("hidden");
                break;
            case 1:
                document.getElementById('checkedIn').classList.remove("hidden");
                break;
            default:
                document.getElementById('takePart').classList.remove("hidden");
                break;
        }
    }

    /**
     * This is the function for users to sign up. It performs a request to the backend. On sucess the view will be set to 'signed in'
     */
    signUp(groupId) {
        request.addUserToGroup(groupId).then((res) => {
            if (res.status === 200) {
                document.getElementById('takePart').classList.add("hidden");
                document.getElementById('assigned').classList.remove("hidden");
            }
        });
    }
}