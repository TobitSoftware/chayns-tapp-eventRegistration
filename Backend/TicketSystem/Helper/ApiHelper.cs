using System;
using System.Collections.Generic;
using System.Net;
using TicketSystem.Models;
using TicketSystem.Models.ApiResults;

namespace TicketSystem.Helper
{
    public class ApiHelper
    {
        private const string ApiUrl = "https://api.chayns.net/v1.0";

        /// <summary>
        /// Requests an API token with custom permissions.
        /// </summary>
        /// <param name="locationId">LocationId</param>
        /// <param name="tappId">TappId</param>
        /// <param name="permissions">Array of Permissions([ "PublicInfo" , "UserInfo" , "DeviceInfo" , "SeeUAC" , "EditUAC" ])</param>
        /// <returns></returns>
        public static string GetToken(int locationId, int tappId, string[] permissions)
        {
            var res = RequestHelper.Post<ApiResult<ApiTokenResult>>(ApiUrl, "accesstoken", new { Permissions = permissions }, new Dictionary<string, string>
            {
                {"Authorization", $"Basic {Base64Encode($"{locationId}:{TappInformationHelper.Get(tappId).Secret}")}"}
            });

            return res.StatusCode != HttpStatusCode.OK ? null : res.Data?.Data?[0]?.PageAccessToken;
        }

        /// <summary>
        /// Encodes text with base64.
        /// </summary>
        /// <param name="plainText">Text to Encode</param>
        /// <returns>Encoded Text</returns>
        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            var decode = Convert.ToBase64String(plainTextBytes);
            return decode;
        }

        /// <summary>
        /// Gets user to a tobitAccessToken.
        /// </summary>
        /// <param name="tobitAccessToken"></param>
        /// <returns>UserData from TobitAccessToken</returns>
        public static TobitAccessToken GetTokenData(string tobitAccessToken)
        {
            var res = RequestHelper.Get<ApiResult<TobitAccessToken>>(ApiUrl, "accesstoken", new Dictionary<string, string>
            {
                {"Authorization", $"Bearer {tobitAccessToken}"}
            });

            return res.StatusCode != HttpStatusCode.OK ? null : res.Data?.Data?[0];
        }

        /// <summary>
        /// Adds user to an uac group.
        /// </summary>
        /// <param name="locationId">LocationId</param>
        /// <param name="tappId">TappId</param>
        /// <param name="groupId">GroupId</param>
        /// <param name="userId">UserId</param>
        /// <returns></returns>
        public static bool AddUserToUac(int locationId, int tappId, int groupId, int userId)
        {
            var res = RequestHelper.Post<ApiResult<ApiTokenResult>>(ApiUrl, $"{locationId}/UAC/{groupId}/User/{userId}", null, new Dictionary<string, string>
            {
                {"Authorization", $"Secret {TappInformationHelper.Get(tappId).Secret}"}
            });
            return res.StatusCode == HttpStatusCode.Created;
        }

        /// <summary>
        /// Checks if group contains a special user.
        /// </summary>
        /// <param name="locationId">LocationId</param>
        /// <param name="tappId">TappId</param>
        /// <param name="groupId">GroupId</param>
        /// <param name="userId">UserId</param>
        /// <param name="secret">only required when tappId is not assigned to an event</param>
        /// <returns>True if user is in Group.</returns>
        public static bool UserInGroup(int locationId, int tappId, int groupId, int userId, string secret = null)
        {
            var res = RequestHelper.Get<ApiResult<User>>(ApiUrl, $"{locationId}/UAC/{groupId}/User/{userId}", new Dictionary<string, string>
            {
                {"Authorization", $"Secret {secret??TappInformationHelper.Get(tappId).Secret}"}
            });
            return res.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// Gets count of members in an UAC group.
        /// </summary>
        /// <param name="locationId">LocationId</param>
        /// <param name="tappId">TappId</param>
        /// <param name="groupId">GroupId</param>
        /// <returns>Count of members in an UAC group</returns>
        public static int UserMemberCount(int locationId, int tappId, int groupId)
        {
            var res = RequestHelper.Get<ApiResult<User>>(ApiUrl, $"{locationId}/UAC/{groupId}/User", new Dictionary<string, string>
            {
                {"Authorization", $"Secret {TappInformationHelper.Get(tappId).Secret}"}
            });
            return res.StatusCode == HttpStatusCode.NotFound ? 0 : res.Data.Data.Count;
        }

        /// <summary>
        /// Creates an tapp uac group.
        /// </summary>
        /// <param name="locationId"></param>
        /// <param name="tappId"></param>
        /// <param name="name"></param>
        /// <param name="secret">not required if saved in tappInformation</param>
        /// <returns></returns>
        public static int CreateUacGroup(int locationId, int tappId, string name, string secret = null)
        {
            var res = RequestHelper.Post<ApiResult<UacGroup>>(ApiUrl, $"{locationId}/UAC", new
            {
                ShowName = name,
                Name = name
            }, new Dictionary<string, string>
            {
                {"Authorization", $"Secret {secret??TappInformationHelper.Get(tappId).Secret}"}
            });
            return res.Data.Data[0].UserGroupId;
        }

    }
}