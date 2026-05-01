using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using WayPoints;

namespace StarterCode_WayPoints
{
    internal class Program
    {
        //directory containing file
        static string FILE_PATH = "../../../"; //set path to solution directory
        static string fileName = "UK_waypoints.csv"; //file to read

        static void Main(string[] args)
        {
            var wayPoints = ReadDisplayFileWayPoints(FILE_PATH + fileName);

            // Convert dictionary to array
            string[][] waypointsArray = DictionaryToArray(wayPoints);

            // Display the array of waypoints
            Console.WriteLine("All WayPoints:");
            foreach (string[] waypoint in waypointsArray)
            {
                Console.WriteLine("[" + string.Join(", ", waypoint) + "]");
            }

            // Search for waypoints by name
            Console.WriteLine("\n--- Search Results ---");

            // Search All WayPoints via name, return a waypoint object containing the name
            Console.Write("Enter waypoint name to search (exact match): ");
            string? searchName = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(searchName))
            {
                WayPoint? found = SearchWayPoint.SearchByName(wayPoints, searchName);
                if (found != null)
                {
                    Console.WriteLine("Found: " + found);
                }
                else
                {
                    Console.WriteLine($"WayPoint '{searchName}' not found");
                }
            }

            // Search All WayPoints via partialName (first n letters)
            Console.Write("\nEnter search term for partial match: ");
            string? searchTerm = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                Console.WriteLine($"\nSearch All WayPoints via partialName (first n letters) '{searchTerm}':");
                List<WayPoint> partialResults = SearchWayPoint.SearchByPartialName(wayPoints, searchTerm);

                if (partialResults.Count > 0)
                {
                    foreach (var waypoint in partialResults)
                    {
                        Console.WriteLine(waypoint);
                    }
                }
                else
                {
                    Console.WriteLine($"No waypoints found matching that search term.'{searchTerm}'");
                }
            }
        }

        // Convert the dictionary of waypoints to a 2D array of strings
        static string[][] DictionaryToArray(Dictionary<string, WayPoint> wayPoints)
        {
            string[][] result = new string[wayPoints.Count][];
            int index = 0;

            foreach (var (key, waypoint) in wayPoints)
            {
                result[index] = waypoint.ToArray();
                index++;
            }

            return result;
        }

        static Dictionary<string, WayPoint> ReadDisplayFileWayPoints(string fileName)
        {
            string[] linesInFile = File.ReadAllLines(fileName); //Read whole file into array of string
            int slicedLength = 6;
            int lineNumber = 0;

            // Dictionary to store waypoints with name as key and WayPoint object as value
            var WayPointArr = new Dictionary<string, WayPoint>();

            foreach (string line in linesInFile[0..slicedLength]) //take each line string from the file one at a time
            {
                lineNumber++; //increment the current line number
                              //split up line into separate features split by commas array of strings, each element is a word on the current line
                if (lineNumber != 1 && line != "") //ignore line 1 (column headers) and any empty lines
                {
                    string[] featuresInLine = line.Split(','); //csv file, split each line based on ','
                                                               //11 features, ignore: country,style,rwdir,rwlen,freq
                    string name = featuresInLine[0];
                    string code = featuresInLine[1];
                    string latitude = featuresInLine[3];
                    string longitude = featuresInLine[4];
                    int elevation = convertElevationToMeters(featuresInLine[5]); //some in ft, some in m - convert to meters
                    string description = buildDescription(featuresInLine);

                    // Add waypoint to dictionary
                    // name is unique for each waypoint, so we can use it as a key in the dictionary
                    WayPointArr.Add(name, new WayPoint(name, code, latitude, longitude, elevation, description));

                }
            }

            return WayPointArr;

        }

        static void DisplayWayPoint(string name, string code, string latitude, string longitude, int elevation, string description)
        {
            Console.Write("WayPoint: " + name + ",Code:" + code);
            Console.Write(",Position:[" + latitude + "," + longitude + "]");
            Console.WriteLine(",Elevation:" + elevation + "m");
            Console.WriteLine(description);
        }


        //Description starts at feature 11 but it may contain commas
        //so we need to concat all remaining strings on the line to be the description
        static string buildDescription(string[] featuresInLine)
        {
            StringBuilder description = new StringBuilder();
            int arrayPosition = 11; //position of start of Description
            string descriptionPart;
            while (arrayPosition < featuresInLine.Length)
            {
                descriptionPart = featuresInLine[arrayPosition];
                if (isText(descriptionPart))
                {
                    description.Append(descriptionPart + ",");
                }
                arrayPosition++;
            }
            return description.ToString();
        }
        static bool isText(string str)
        {
            if (str == "" || str == " ")
                return false;
            return true;
        }

        static int convertElevationToMeters(string elevationStr)
        {
            char[] unitChars = { 'f', 't', 'M', 'm' };
            if (elevationStr.ToLower().EndsWith("m"))
            {
                return (int)double.Parse(elevationStr.TrimEnd(unitChars));
            }

            double elevationFeet = double.Parse(elevationStr.TrimEnd(unitChars));
            return (int)(elevationFeet / 3.142); //constant for ft to m
        }
    }
}

