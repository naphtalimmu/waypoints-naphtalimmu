using System;
using System.Collections.Generic;

namespace WayPoints
{
    public class SearchWayPoint
    {
        /// <summary>
        /// Search for a waypoint by name
        /// </summary>
        /// <param name="wayPoints">Dictionary of waypoints (name as key, WayPoint object as value)</param>
        /// <param name="name">The name to search for</param>
        /// <returns>WayPoint object if found, null if not found</returns>
        public static WayPoint? SearchByName(Dictionary<string, WayPoint> wayPoints, string name)
        {
            // Convert dictionary keys to list for indexed for loop
            List<string> keys = new List<string>(wayPoints.Keys);

            for (int i = 0; i < keys.Count; i++)
            {
                if (keys[i] == name)
                {
                    return wayPoints[keys[i]];
                }
            }
            return null;
        }

        /// <summary>
        /// Search for all waypoints containing the search term in their name (case-insensitive)
        /// </summary>
        /// <param name="wayPoints">Dictionary of waypoints</param>
        /// <param name="searchTerm">The text to search for</param>
        /// <returns>List of matching WayPoint objects</returns>
        public static List<WayPoint> SearchByPartialName(Dictionary<string, WayPoint> wayPoints, string searchTerm)
        {
            List<WayPoint> results = new List<WayPoint>();
            string lowerSearchTerm = searchTerm.ToLower();

            // Convert dictionary keys to list for indexed for loop
            List<string> keys = new List<string>(wayPoints.Keys);

            for (int i = 0; i < keys.Count; i++)
            {
                string name = keys[i];
                if (name.ToLower().Contains(lowerSearchTerm))
                {
                    results.Add(wayPoints[name]);
                }
            }

            return results;
        }
    }
}
