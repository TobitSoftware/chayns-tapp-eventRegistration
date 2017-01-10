using System;
using System.Net;
using System.Web.Http;
using TicketSystem.Helper;
using TicketSystem.Models;
using TicketSystem.Models.Request;

namespace TicketSystem.Controllers
{
    [RoutePrefix("{locationId}/EventUser/{tappId}")]
    public class EventUserController : ApiController
    {
        /// <summary>
        /// Returns information about the requested event.
        /// </summary>
        /// <param name="req"></param>
        /// <param name="userId"></param>
        /// <returns>
        ///     user registered,
        ///     tickets sold,
        ///     tickets available,
        ///     user is checked in
        /// </returns>
        [Route("")]
        [HttpGet]
        public IHttpActionResult GetEventInfromation([FromUri] BaseRequest req, int? userId = null)
        {
            try
            {
                var tappInfo = TappInformationHelper.Get(req.TappId);

                var registered = (userId != null) && ApiHelper.UserInGroup(req.LocationId, req.TappId, tappInfo.RegisterUacGroupId,(int)userId);
                var sold = ApiHelper.UserMemberCount(req.LocationId, req.TappId, tappInfo.RegisterUacGroupId);
                var available = tappInfo.TicketCount - sold;
                var checkedIn = ApiHelper.UserMemberCount(req.LocationId, req.TappId, tappInfo.CheckInUacGroupId);

                return Ok(new EventInformation(registered, sold, available, checkedIn));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Registers an user to an event.
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        public IHttpActionResult Register([FromUri] BaseRequest req)
        {
            try
            {
                var tokenData = ApiHelper.GetTokenData(Request?.Headers?.Authorization?.Parameter);
                if (tokenData == null || tokenData.UserId == 0 || tokenData.LocationId != req.LocationId)
                {
                    return Unauthorized();
                }

                var tappInfo = TappInformationHelper.Get(req.TappId);
                if (ApiHelper.UserMemberCount(req.LocationId, req.TappId, tappInfo.RegisterUacGroupId) > tappInfo.TicketCount)
                {
                    return BadRequest("No more tickets available.");
                }

                if (!ApiHelper.AddUserToUac(req.LocationId, req.TappId, tappInfo.RegisterUacGroupId, tokenData.UserId))
                {
                    return Content(HttpStatusCode.Conflict, "Failed");
                }
                return Ok("Successfully registered.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Checks an user in.
        /// </summary>
        /// <param name="req"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("")]
        [HttpPatch]
        public IHttpActionResult CheckIn([FromUri] BaseRequest req, [FromBody] int userId)
        {
            try
            {
                var tokenData = ApiHelper.GetTokenData(Request?.Headers?.Authorization?.Parameter);
                if (tokenData == null || tokenData.UserId == 0 || tokenData.LocationId != req.LocationId || !ApiHelper.UserInGroup(req.LocationId, req.TappId, 5677, tokenData.UserId))
                {
                    return Unauthorized();
                }

                var tappInfo = TappInformationHelper.Get(req.TappId);
                if (!ApiHelper.UserInGroup(req.LocationId, req.TappId, tappInfo.RegisterUacGroupId, userId))
                {
                    return Content(HttpStatusCode.NotFound, "User isn't registered.");
                }
                if (ApiHelper.UserInGroup(req.LocationId, req.TappId, tappInfo.CheckInUacGroupId, userId))
                {
                    return Content(HttpStatusCode.NoContent, "User already checked in.");
                }

                if (!ApiHelper.AddUserToUac(req.LocationId, req.TappId, tappInfo.CheckInUacGroupId, userId))
                {
                    return Content(HttpStatusCode.Conflict, "Failed");
                }
                return Ok("Successfully checked in.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
