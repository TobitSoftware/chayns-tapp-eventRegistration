using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using EventRegistration.Models;
using EventRegistration.Properties;
using Newtonsoft.Json;
using RestSharp;

namespace EventRegistration.Repositories
{
    public class UserRepository
    {
        /// <summary>
        /// Get all members of a uac group
        /// </summary>
        /// <param name="locationId">location of the targeted group</param>
        /// <param name="tappId">tapp of the targeted group</param>
        /// <param name="groupId">targeted uac group</param>
        /// <returns>List of users</returns>
        public static List<UserModel> GetUacGroupMembers(int locationId, int tappId, int groupId)
        {
            try
            {
                var server = Resources.Server;
                var secret = Resources.Secret;

                RestClient restClient = new RestClient(server);
                RestRequest req = new RestRequest(locationId + "/UAC/" + groupId + "/User") { Method = Method.GET };
                req.AddHeader("Content-Type", "application/json");
                req.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(Convert.ToString(tappId) + ':' + secret)));

                req.RequestFormat = DataFormat.Json;

                IRestResponse resp = restClient.Execute(req);

                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    //if the result is ok, the returned data will be parsed to a List containing the users
                    UserDataModel model = JsonConvert.DeserializeObject<UserDataModel>(resp.Content);
                    List<UserModel> users = model.Data;

                    //Return the list containing all users that are member of the current group
                    return users;
                }
                return null;
            }
            catch (Exception)
            {
                //should Log
                return null;
            }
        }

        /// <summary>
        /// Gets a spezific user of a uac group
        /// </summary>
        /// <param name="locationId">location of the targeted group</param>
        /// <param name="tappId">tapp of the targeted group</param>
        /// <param name="groupId">targeted uac group</param>
        /// <param name="userId">specific user</param>
        /// <returns>User / null if not existing</returns>
        public static UserModel GetUacGroupMember(int locationId, int tappId, int groupId, int userId)
        {
            try
            {
                var server = Resources.Server;
                var secret = Resources.Secret;

                RestClient restClient = new RestClient(server);
                RestRequest req = new RestRequest(locationId + "/UAC/" + groupId + "/User/" + userId) { Method = Method.GET };
                req.AddHeader("Content-Type", "application/json");
                req.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(Convert.ToString(tappId) + ':' + secret)));

                req.RequestFormat = DataFormat.Json;

                IRestResponse resp = restClient.Execute(req);

                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    //if the result is ok, the returned data will be parsed and returned as a UserModel
                    UserDataModel model = JsonConvert.DeserializeObject<UserDataModel>(resp.Content);
                    return model.Data.FirstOrDefault();
                }
                return null;
            }
            catch (Exception)
            {
                //should log
                return null;
            }
        }

        /// <summary>
        /// Adds a user to a UAC group
        /// </summary>
        /// <param name="locationId">location of the targeted group</param>
        /// <param name="tappId">tapp of the targeted group</param>
        /// <param name="userId">user, which should be removed</param>
        /// <param name="groupId">targeted uac group</param>
        /// <returns>bool success</returns>
        public static bool AddUserToUacGroup(int locationId, int tappId, int userId, int groupId)
        {
            try
            {
                bool isInsertable = true;
                int? memberGroupId = null;
                //Get the Targeted UacGroup
                var uacGroup = GroupRepository.GetUacGroup(locationId, tappId, groupId);
                //Check if uac group is our checkin Group
                if (uacGroup.Name == "checkedIn_" + tappId || uacGroup.ShowName == "checkedIn_" + tappId)
                {
                    #region Check if user is allowed to be inserted in checkin group

                    //Get member group
                    GroupModel memberGroup = GroupRepository.GetMemberGroup(locationId, tappId);
                    if (memberGroup == null)
                    {
                        isInsertable = false;
                    }
                    else
                    {
                        memberGroupId = Convert.ToInt32(memberGroup.UserGroupId);
                        //Check if user is in member group
                        var user = GetUacGroupMember(locationId, tappId, Convert.ToInt32(memberGroup.UserGroupId), userId);
                        if (user == null)
                        {
                            isInsertable = false;
                        }
                    }
                    #endregion
                }

                if (isInsertable)
                {

                    var server = Resources.Server;
                    var secret = Resources.Secret;

                    RestClient restClient = new RestClient(server);
                    RestRequest req = new RestRequest(locationId + "/UAC/" + groupId + "/User/" + userId) { Method = Method.POST };
                    req.AddHeader("Content-Type", "application/json");
                    req.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(Convert.ToString(tappId) + ':' + secret)));

                    req.RequestFormat = DataFormat.Json;

                    IRestResponse resp = restClient.Execute(req);

                    if (resp.StatusCode == HttpStatusCode.Created)
                    {
                        if (memberGroupId != null)
                        {
                            //Remove User from Member UacGroup
                            RemoveUserFromUacGroup(locationId, tappId, userId, memberGroupId.Value);
                        }
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                //should log
                return false;
            }
        }

        /// <summary>
        /// Removes a user from a UAC group
        /// </summary>
        /// <param name="locationId">location of the targeted uac group</param>
        /// <param name="tappId">tapp of the targeted uac group</param>
        /// <param name="userId">user, which should be removed</param>
        /// <param name="groupId">targeted uac group</param>
        /// <returns>bool success</returns>
        public static bool RemoveUserFromUacGroup(int locationId, int tappId, int userId, int groupId)
        {
            try
            {
                var server = Resources.Server;
                var secret = Resources.Secret;

                RestClient restClient = new RestClient(server);
                RestRequest req = new RestRequest(locationId + "/UAC/" + groupId + "/User/" + userId) { Method = Method.DELETE };

                req.AddHeader("Content-Type", "application/json");
                req.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(Convert.ToString(tappId) + ':' + secret)));

                req.RequestFormat = DataFormat.Json;

                IRestResponse resp = restClient.Execute(req);

                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                //should log
                return false;
            }
        }
    }
}