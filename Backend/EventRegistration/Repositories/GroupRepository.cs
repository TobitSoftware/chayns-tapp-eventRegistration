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
    public class GroupRepository
    {
        /// <summary>
        /// Get member and checkin group
        /// </summary>
        /// <param name="locationId">location of the targeted group</param>
        /// <param name="tappId">tapp of the targeted group</param>
        /// <returns>Member and checkin group</returns>
        public static List<GroupModel> GetUacGroups(int locationId, int tappId)
        {
            var server = Resources.Server;
            var secret = Resources.Secret;

            RestClient restClient = new RestClient(server);
            RestRequest req = new RestRequest(locationId + "/UAC?TappId=" + tappId) { Method = Method.GET };
            req.AddHeader("Content-Type", "application/json");
            req.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(Convert.ToString(tappId) + ':' + secret)));

            req.RequestFormat = DataFormat.Json;

            //perform the request and save the result
            IRestResponse resp = restClient.Execute(req);

            if (resp.StatusCode == HttpStatusCode.OK)
            {
                GroupDataModel model = JsonConvert.DeserializeObject<GroupDataModel>(resp.Content);
                List<GroupModel> groupList = model.Data;
                //Check for member and checkin group
                List<GroupModel> finalGroups = Helper.GroupHelper.CheckGroups(groupList, tappId, locationId);

                return finalGroups;
            }
            return null;
        }

        /// <summary>
        /// Get a uac group by id
        /// </summary>
        /// <param name="locationId">location of the targeted group</param>
        /// <param name="tappId">tapp of the targeted group</param>
        /// <param name="groupId">targeted uac group</param>
        /// <returns>Uac Group</returns>
        public static GroupModel GetUacGroup(int locationId, int tappId, int groupId)
        {
            var server = Resources.Server;
            var secret = Resources.Secret;

            RestClient restClient = new RestClient(server);
            RestRequest req = new RestRequest(locationId + "/UAC/" + groupId) { Method = Method.GET };
            req.AddHeader("Content-Type", "application/json");
            req.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(Convert.ToString(tappId) + ':' + secret)));

            req.RequestFormat = DataFormat.Json;

            //perform the request and save the result
            IRestResponse resp = restClient.Execute(req);

            if (resp.StatusCode == HttpStatusCode.OK)
            {
                GroupDataModel model = JsonConvert.DeserializeObject<GroupDataModel>(resp.Content);
                List<GroupModel> groupList = model.Data;

                return groupList.FirstOrDefault();
            }

            return null;
        }

        /// <summary>
        /// Filters the UacGroups for the member group
        /// </summary>
        /// <param name="locationId">location of the targeted group</param>
        /// <param name="tappId">tapp of the targeted group</param>
        /// <returns>Member group</returns>
        public static GroupModel GetMemberGroup(int locationId, int tappId)
        {
            var groups = GetUacGroups(locationId, tappId);
            return groups.FirstOrDefault(groupModel => groupModel.Name == "members_" + tappId || groupModel.ShowName == "members_" + tappId);
        }

        /// <summary>
        /// Deletes a uac group
        /// </summary>
        /// <param name="locationId">location of the targeted group</param>
        /// <param name="tappId">tapp of the targeted group</param>
        /// <param name="groupId">targeted uac group</param>
        /// <returns>bool success</returns>
        public static bool DeleteUacGroup(int locationId, int tappId, int groupId)
        {
            try
            {
                var server = Resources.Server;
                var secret = Resources.Secret;

                RestClient restClient = new RestClient(server);
                RestRequest req = new RestRequest(locationId + "/UAC/" + groupId) { Method = Method.DELETE };
                req.AddHeader("Content-Type", "application/json");
                req.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(Convert.ToString(tappId) + ':' + secret)));

                req.RequestFormat = DataFormat.Json;

                //perform the request and save the result
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