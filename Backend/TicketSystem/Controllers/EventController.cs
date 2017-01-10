using System;
using System.Net;
using System.Web.Http;
using TicketSystem.Helper;
using TicketSystem.Models;
using TicketSystem.Models.Request;

namespace TicketSystem.Controllers
{
    [RoutePrefix("{locationId}/Event/{tappId}")]
    public class EventController : ApiController
    {
        /// <summary>
        /// Returns all events as string, formated to use them in your source code.
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        public IHttpActionResult GetEvents([FromUri] BaseRequest req)
        {
            try
            {
                return Ok(TappInformationHelper.GetAllAsString());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Adds an event to TappInformation.
        /// Creates two UAC Groups for registration and checkin.
        /// </summary>
        /// <param name="req"></param>
        /// <param name="data"></param>
        /// <returns>all events as string, formated to use them in your source code</returns>
        [Route("")]
        [HttpPost]
        public IHttpActionResult AddEvent([FromUri] BaseRequest req, [FromBody] NewEvent data)
        {
            try
            {
                var user = ApiHelper.GetTokenData(Request?.Headers?.Authorization?.Parameter);
                if (user?.LocationId != req.LocationId || !ApiHelper.UserInGroup(req.LocationId, req.TappId, 5677, user.UserId, data.Secret))
                {
                    return Unauthorized();
                }
                if (!string.IsNullOrEmpty(TappInformationHelper.Get(req.TappId).Secret))
                {
                    return Content(HttpStatusCode.Conflict, $"Tapp already assign to an Tapp. - '{TappInformationHelper.GetAllAsString()}'");
                }

                var tappInfo = new TappInformation
                {
                    Secret = data.Secret,
                    TicketCount = data.TicketCount,
                    RegisterUacGroupId = ApiHelper.CreateUacGroup(req.LocationId, req.TappId, $"{data.EventName}-Angemeldet", data.Secret),
                    CheckInUacGroupId = ApiHelper.CreateUacGroup(req.LocationId, req.TappId, $"{data.EventName}-Eingecheckt", data.Secret)
                };
                TappInformationHelper.AddTappInformation(req.TappId, tappInfo);
                return Ok(TappInformationHelper.GetAllAsString());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
