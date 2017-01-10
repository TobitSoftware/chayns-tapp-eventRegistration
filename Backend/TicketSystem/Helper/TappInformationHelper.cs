using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TicketSystem.Models;

namespace TicketSystem.Helper
{
    public class TappInformationHelper
    {
        /// <summary>
        /// Dictionary that assigns tappId's to TappInformation objects.
        /// </summary>
        private static readonly Dictionary<int, TappInformation> TappInformation = new Dictionary<int, TappInformation>
        {
            //save your event(s) here : {tappId ,new TappInformation("Your Secret", UAC-Id Registered, UAC-Id checkIn, tickets)}
        };

        /// <summary>
        /// Gets information to a tapp.
        /// </summary>
        /// <param name="tappId"></param>
        /// <returns>TappInformation Object</returns>
        public static TappInformation Get(int tappId)
        {
            try
            {
                return TappInformation[tappId];
            }
            catch (Exception)
            {
                throw new KeyNotFoundException($"no event found to tappId {tappId}");
            }
        }

        /// <summary>
        /// Adds TappInformation to dictionary.
        /// </summary>
        /// <param name="tappId"></param>
        /// <param name="data"></param>
        public static void AddTappInformation(int tappId, TappInformation data)
        {
            if (string.IsNullOrEmpty(Get(tappId).Secret))
            {
                TappInformation.Add(tappId, data);
                SaveEvents();
            }
        }

        /// <summary>
        /// Returns all tappInformation as string, formated to use them in your source code.
        /// </summary>
        /// <returns></returns>
        public static string GetAllAsString()
        {
            return TappInformation.Aggregate("", (current, tappInfo) => current + $"{{{tappInfo.Key},new TappInformation(\"{tappInfo.Value.Secret}\", {tappInfo.Value.RegisterUacGroupId}, {tappInfo.Value.CheckInUacGroupId}, {tappInfo.Value.TicketCount})}},\n");
        }

        /// <summary>
        /// Saves a BackupString with all events (formated to use them in your source code) to a .TXT file in Project folder.
        /// </summary>
        /// <returns>success</returns>
        public static bool SaveEvents()
        {
            try
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "events.txt");
                File.AppendAllLines(path, new[] { "", GetAllAsString() });
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}