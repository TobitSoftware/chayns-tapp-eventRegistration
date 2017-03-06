using System.Net;
using System.Web.Http;
using EventRegistration.Models;
using EventRegistration.Repositories;

namespace EventRegistration.Controllers
{
    public class UserController : ApiController
    {
        public IHttpActionResult Get(int id, int locationId, int tappId) //Id = groupId
        {
            //Get all user of a uac group
            var users = UserRepository.GetUacGroupMembers(locationId, tappId, id);

            //check if list of users has any user
            if (users == null || users.Count < 1) return StatusCode(HttpStatusCode.NoContent);

            //return list of users
            return Ok(users);
        }

        public IHttpActionResult Get(int id, int locationId, int tappId, int userId) //Id = groupId
        {
            //Get a user by userId and groupId
            var user = UserRepository.GetUacGroupMember(locationId, tappId, id, userId);

            //Check if user is null
            if (user == null) return StatusCode(HttpStatusCode.NoContent);

            //return single user
            return Ok(user);
        }

        public IHttpActionResult Post([FromBody] AddUserModel user)
        {
            //Add user to uac group
            var addUserResult = UserRepository.AddUserToUacGroup(user.LocationId, user.TappId, user.UserId, user.GroupId);

            //check if adding was successfull 
            if (!addUserResult) return Conflict();
            return Ok();
        }

        public IHttpActionResult Delete(int id, int locationId, int tappId, int userId)
        {
            //Remove user from UacGroup
            var removeUserResult = UserRepository.RemoveUserFromUacGroup(locationId, tappId, userId, id);
            
            //Check if removing was successfull
            if (!removeUserResult) return Conflict();
            return Ok();
        }
    }
}
