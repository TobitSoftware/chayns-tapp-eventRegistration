using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EventRegistration.Models;

namespace EventRegistration.Helper
{
    public class GroupHelper
    {
        /// <summary>
        /// Checks if member and checkin group are existing, creates them if not existing
        /// </summary>
        /// <param name="groups">List of groups which should be checked</param>
        /// <param name="tappId">tappId of the groups</param>
        /// <param name="locationId">locationId of the groups</param>
        /// <returns>Member and Checkin group</returns>
        public static List<GroupModel> CheckGroups(List<GroupModel> groups, int tappId, int locationId)
        {
            bool hasMemberGroup = false;
            bool hasCheckinGroup = false;
            List<GroupModel> resultGroups = new List<GroupModel>();

            //Iterate through all given uacGroups
            foreach (var groupModel in groups)
            {
                //check if current Uac Group is member Group
                if (groupModel.Name == "members_" + tappId || groupModel.ShowName == "members_" + tappId)
                {
                    resultGroups.Add(groupModel);
                    hasMemberGroup = true;
                }
                //check if current Uac Group is checkin Group
                if (groupModel.Name == "checkedIn_" + tappId || groupModel.ShowName == "checkedIn_" + tappId)
                {
                    resultGroups.Add(groupModel);
                    hasCheckinGroup = true;
                }
            }
            //Check if members Group is already existing
            if (!hasMemberGroup)
            {
                GroupModel group = CreateGroup(locationId, tappId, "members_" + tappId);
                if (group != null)
                {
                    resultGroups.Add(group);
                }
            }
            //Check if checkin Group is already existing
            if (!hasCheckinGroup)
            {
                GroupModel group = CreateGroup(locationId, tappId, "checkedIn_" + tappId);
                if (group != null)
                {
                    resultGroups.Add(group);
                }
            }
            //Return the member and checkin Group
            return resultGroups;
        }

        /// <summary>
        /// Creates a uac Group with the given locationId, tappId and name
        /// </summary>
        /// <param name="locationId">location, where the group should be created</param>
        /// <param name="tappId">tapp,where the group should be created</param>
        /// <param name="name">name of the uacGroup</param>
        /// <returns>created UAC group</returns>
        public static GroupModel CreateGroup(int locationId, int tappId, string name)
        {
            try
            {
                string server = Properties.Resources.Server;
                string secret = Properties.Resources.Secret;

                RestClient restClient = new RestClient(server);
                RestRequest req = new RestRequest(locationId + "/UAC") {Method = Method.POST};
                req.AddHeader("Content-Type", "application/json");
                req.AddHeader("Authorization",
                    "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(Convert.ToString(tappId) + ':' + secret)));

                req.RequestFormat = DataFormat.Json;

                AddGroupData body = new AddGroupData
                {
                    Name = name,
                    ShowName = name
                };

                req.AddBody(body);

                IRestResponse resp = restClient.Execute(req);
                GroupDataModel group = JsonConvert.DeserializeObject<GroupDataModel>(resp.Content);

                return group.Data.FirstOrDefault();
            }
            catch (Exception)
            {
                //should log
                return null;
            }
        }
    }
}