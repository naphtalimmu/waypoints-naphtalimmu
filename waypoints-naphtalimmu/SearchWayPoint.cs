using System;
using System.Collections.Generic;

namespace WayPoints
{
    public class SearchWayPoint
    {
        private const double MetresToFeet = 0.3048;

        /// <summary>
        /// Search for a waypoint by name
        /// </summary>
        /// <param name="wayPoints">Dictionary of waypoints (name as key, WayPoint object as value)</param>
        /// <param name="name">The name to search for</param>
        /// <returns>Formatted string representation of WayPoint if found, null if not found</returns>
        public static string? SearchByName(Dictionary<string, WayPoint> wayPoints, string name)
        {
            // Convert dictionary keys to list for indexed for loop
            List<string> keys = new List<string>(wayPoints.Keys);

            for (int i = 0; i < keys.Count; i++)
            {
                if (keys[i].Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    WayPoint wp = wayPoints[keys[i]];
                    Console.WriteLine($"Type of : {wp.Elevation.GetType()}");
                    return $"{{ {wp.Name}, {wp.Code}, pos[{wp.Longitude},{wp.Latitude}], h:{wp.Elevation * MetresToFeet} m, {wp.Description} }}";
                }
            }
            return null;
        }

        /// <summary>
        /// Search for all waypoints containing the search term in their name (case-insensitive)
        /// </summary>
        /// <param name="wayPoints">Dictionary of waypoints</param>
        /// <param name="searchTerm">The text to search for</param>
        /// <returns>List of formatted string representations of matching WayPoint objects</returns>
        public static List<string> SearchByPartialName(Dictionary<string, WayPoint> wayPoints, string searchTerm)
        {
            List<string> results = new List<string>();
            string lowerSearchTerm = searchTerm.ToLower();

            // Convert dictionary keys to list for indexed for loop
            List<string> keys = new List<string>(wayPoints.Keys);

            for (int i = 0; i < keys.Count; i++)
            {
                string name = keys[i];
                if (name.ToLower().Contains(lowerSearchTerm))
                {
                    WayPoint wp = wayPoints[name];
                    results.Add($"{{ {wp.Name}, {wp.Code}, pos[{wp.Longitude},{wp.Latitude}], h:{wp.Elevation * MetresToFeet} m, {wp.Description} }}");
                }
            }

            return results;
        }
    }
}
