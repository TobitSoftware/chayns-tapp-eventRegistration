[![license](https://img.shields.io/github/license/TobitSoftware/chayns-tapp-eventRegistration.svg)]() [![GitHub pull requests](https://img.shields.io/github/issues-pr/TobitSoftware/chayns-tapp-eventRegistration.svg)]() [![](https://img.shields.io/github/issues-pr-closed-raw/TobitSoftware/chayns-tapp-eventRegistration.svg)]()

# Event Registration

This example of a reservation Tapp provides your customers with the possibility to sign up for an event. The design is similar to the event Tapp with the addition of a register button that is used for the sign-in. Furthermore, the Tapp makes use of user account control groups (UAC-groups), to track the attendees of an event and check them in (everybody is allowed to checkin).<br>
Attendees can be checked in by clicking a button in a list next to their name.

It is splitted into a frontend folder and a backend folder that contain the different projects.
* The frontend projects is build using the [chayns-es6 template](https://github.com/TobitSoftware/chayns-template-es6) with Webpack
* The backend is written in C# and the Web.API follows the REST structure



### Requirements
To use this example, you need to fullfill following requirements
* Microsoft Visual Studio (backend project)
* Microsoft Visual Code (frontend project)
* NodeJS (for webpack)
* Member of the Tobit Software Parnter Network (TSPN)
  * This means that you have to register this tapp as a developer tapp and put the secret into your backend project properties. Install the tapp by using the install code in the tapp administration provided in the tspn.
  
### Planned Features
* CheckIn via QR/RFID: Scan users chayns-qr codes or their personalized rfid cards to check them in
* Use a CheckIn group, so that not only members of the chayns manager group can check in users
