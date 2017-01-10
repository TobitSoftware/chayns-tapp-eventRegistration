## CampusDaysBackend - Event System

This solution is based on [chayns](http://chayns.tobit.software/ "chayns.tobit.software") from [Tobit.Software](https://de.tobit.software/ "tobit.software").

It is a simple project to manage events and its participants.

To run this project on your server you have to be a member of the "Tobit.Software Partner Network" (TSPN).     
Only developer tapps can be used to create an event. To create a developer tapp visit the [TSPN website](http://tspn.tobit.software/).

Run this Project with [Visual Studio](https://www.visualstudio.com/downloads/download-visual-studio-vs).    
Visual Studio Enterprise is not needed, the free Community version can be used as well.

In the README folder is a Postman collection for this project, click import in Postman and select the file to get the collection.

## Create Event
### Manuel
```c#
    // Helper.TappInformationhelper
    private static readonly Dictionary<int, TappInformation> TappInformation = new Dictionary<int, TappInformation>
    {
       {{TappId}, new TappInformation("Your Tapp Secret", {Register-UAC Group}, {CheckIn-UAC Group}, {Ticket-Limit})},
       {12345, new TappInformation("AB123-CDE45-FG678-HIJ91", 123, 124, 25)}
    };
```
### Endpoint
1.  Call `{Path to Project}/{locationId}/event/{tappId}`
    -   Authorization Header : `Authorization: Bearer {tobitAccessToken}` // User has to be in the Accounting UAC-Group
    -   Body : 
    ```
        {
            Secret: "Your Tapp Secret",
            TicketCount: {Ticket-Limit},
            EventName: "MyEvent"
        }
    ```
2.  Copy response into sourcecode
    ```c#
     // Helper.TappInformationhelper
     private static readonly Dictionary<int, TappInformation> TappInformation = new Dictionary<int, TappInformation>
     {
        //paste response here
     };
    ```
3. Build/Publish project

## Endpoints

### EventController (`{locationId}/Event/{tappId}`)
- GET
    - Returns all events as string, formated to use them in your code.     
- POST
    -   Authorization Header : `Authorization: Bearer {tobitAccessToken}` // User has to be in the Accounting UAC-Group
    -   Body : 
    ```
        {
            Secret: "Your Tapp Secret",
            TicketCount: {Ticket-Limit},
            EventName: "MyEvent"
        }
    ```
    - Response : All events as string, formated to use them in your code.

### EventUserController (`{locationId}/EventUser/{tappId}`)

- GET
    - Parameter: int: userId [optional]
    - Response: Information of the requested event and if the user is registered.
- POST
    - Registers user.
    - Authorization Header : `Authorization: Bearer {tobitAccessToken}`
    - Response: 
        - 200: Successfully registered.
        - 400: No more tickets available.
        - 401: Unauthorized.
        - 409: Failed.
- PATCH
    - Checks user in.
    - Parameter: int: userId
    - Authorization Header : `Authorization: Bearer {tobitAccessToken}` // User has to be in the Accounting UAC-Group
    - Response: 
        - 200: Successfully checked in.
        - 204: User already checked in.
        - 401: Unauthorized.
        - 404: User isn't registered.
        - 409: Failed.    

### ApiTokenController (`{locationId}/ApiToken/{tappId}`)
- GET
    - Authorization Header : `Authorization: Bearer {tobitAccessToken}` // User has to be in the Accounting UAC-Group
    - Response: Chayns BackendApi PageAccessToken with permissions "PublicInfo" and "UserInfo".
    


