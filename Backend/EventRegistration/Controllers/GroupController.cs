using System.Net;
using System.Web.Http;
using EventRegistration.Repositories;

namespace EventRegistration.Controllers
{
    public class GroupController : ApiController
    {
        public IHttpActionResult Get(int locationId, int tappId)
        {
            //Get member and checkin group
            var uacGroups = GroupRepository.GetUacGroups(locationId, tappId);

            //Check if groups are existing
            if (uacGroups == null && uacGroups?.Count < 1) return StatusCode(HttpStatusCode.NoContent);

            //return both groups
            return Ok(uacGroups);
        }

        public IHttpActionResult Delete(int locationId, int tappId, int groupId)
        {
            //Delete UacGroup
            bool result = GroupRepository.DeleteUacGroup(locationId, tappId, groupId);

            //Check if deleting was successfull
            if (!result) return Conflict();
            return Ok();
        }


    }
}