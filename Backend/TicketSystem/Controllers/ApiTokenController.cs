using System.Web.Http;
using TicketSystem.Helper;
using TicketSystem.Models.Request;

namespace TicketSystem.Controllers
{
    [RoutePrefix("{locationId}/ApiToken/{tappId}")]
    public class ApiTokenController : ApiController
    {
        /// <summary>
        /// Returns a chayns API-Token.
        /// Requires an tobitAccessToken in an authorization header. (Authorization:Bearer {TobitAccessToken})
        /// User has to be in the Accounting Group. (GroupId:5677).
        /// </summary>
        /// <param name="req"></param>
        /// <returns>chayns API-Token with permissions "PublicInfo" and "UserInfo"</returns>
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get([FromUri] BaseRequest req)
        {

            var user = ApiHelper.GetTokenData(Request?.Headers?.Authorization?.Parameter);
            if (user == null || user.LocationId != req.LocationId || !ApiHelper.UserInGroup(req.LocationId, req.TappId, 5677, user.UserId))
            {
                return Unauthorized();
            }

            return Ok(ApiHelper.GetToken(req.LocationId, req.TappId, new[] { "PublicInfo", "UserInfo" }));
        }
    }
}
