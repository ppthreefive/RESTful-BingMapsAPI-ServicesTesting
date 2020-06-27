/*  Author: Phillip Pham
 *  Course: CSE445, Instructor: Dr. Yinong Chen
 *  Date: 06/21/20
 *  
 *  Description: This service utilizes the Bing Maps API and allows the user to do the following:
 *                  1.) Convert an address to a latitude and longitude pair of coordinates using getGeocode().
 *                  2.) Convert a latitude and longitude pair of coordinates to an address using getAddress().
 *                  3.) Find a route from starting address to given destination address using findRoute().
 *                  4.) Get an image of the route from the starting address to given destination using getMap().
 *                  
 *  References used:
 *      https://docs.microsoft.com/en-us/bingmaps/rest-services/
 *      https://exceptionnotfound.net/geocoding-with-bing-maps-rest-services-in-net/
 */

using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace BingMapsService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        // This is the Bing Maps API Key that I obtained to use as a free developer license
        private static string key = "You must insert your own key here, I omitted my personal one for GitHub.";

        /// <summary>
        /// This method allows the user to convert an address to a latitude and longitude pair.
        /// </summary>
        /// <param name="address">A string representing an address.</param>
        /// <returns>Double precision numbers representing latitude and longitude separated by a comma.</returns>
        public string getGeocode(string address)
        {
            // Create a LatLong object by passing in an address to our Geocode method from our GeocodeHelper private class
            LatLong result = GeocodeHelper.Geocode(address);

            // Format our pair of coordinates so that it is easy for anyone to parse
            string s = string.Format("{0},{1}", result.latitude, result.longitude);

            // Return our coordinates string
            return s;
        }

        /// <summary>
        /// This method allows the user to convert coordinates to an address location.
        /// </summary>
        /// <param name="latitude">The latitude part of a coordinate pair, represented by a double precision number.</param>
        /// <param name="longitude">The longitude part of a coordinate pair, represented by a double precision number.</param>
        /// <returns>A string representing an address.</returns>
        public string getAddress(double latitude, double longitude)
        {
            // Create an Address object by passing in coordinates to our inverseGeocode method from our GeocodeHelper private class
            Address result = GeocodeHelper.inverseGeocode(latitude, longitude);

            // Fromat our resulting address string with each part separated by a comma.
            string s = string.Format("{0}, {1}, {2}, {3}", result.addressLine, result.locatlity, result.state, result.postal);

            // Return our address string
            return s;
        }

        /// <summary>
        /// This method will give the basic routing instructions for a driving user in string format.
        /// </summary>
        /// <param name="begin">The starting address in string format.</param>
        /// <param name="end">The destination address in string format.</param>
        /// <returns>Returns the instructions of a driving route in string format.</returns>
        public string findRoute(string begin, string end)
        {
            // Get our coordinates from by using the getGeocode() method, then splitting into string arrays by comma delimiter.
            string[] start = getGeocode(begin).Split(',');
            string[] destination = getGeocode(end).Split(',');

            // This is the Bing Maps REST api to obtain routing information between 2 coordinates.
            string url = string.Format("http://dev.virtualearth.net/REST/V1/Routes?wp.0={0},{1}&wp.1={2},{3}&key={4}", 
                                        start[0], start[1], destination[0], destination[1], key);

            // Initialize our return variable
            string instructions = "";

            // Download the JSON response from the Bing Maps API using the webclient
            using (var client = new WebClient())
            {
                string response = client.DownloadString(url);
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Response));

                // Read the string response as a byte array, then convert to a MemoryStream
                using (var es = new MemoryStream(Encoding.Unicode.GetBytes(response)))
                {
                    // Convert the response from a MemoryStream to a JSON object, and then to a Route object
                    var mapResponse = (ser.ReadObject(es) as Response);
                    Route route = (Route)mapResponse.ResourceSets.First().Resources.First();

                    // Now we have our route legs, we can obtain the itenerary items
                    RouteLeg[] legs =  route.RouteLegs;

                    // For each route leg, we grab its itenerary item, and then grab the instruction,
                    // then add to the return string
                    for (int i = 0; i < legs.Length; i++)
                    {
                        ItineraryItem[] its = legs[i].ItineraryItems;

                        for (int j = 0; j < its.Length; j++)
                        {
                            Instruction ins = its[j].Instruction;
                            instructions += ins.Text + ", ";
                        }
                    }
                }
            }

            // Return the cleaned string obtaining our route instructions
            return instructions.Remove(instructions.Length - 2, 2) + ".";
        }

        /// <summary>
        /// This method will return an image representing the route on a map from a starting point to a destination.
        /// </summary>
        /// <param name="begin">A string representing the starting address.</param>
        /// <param name="end">A string representing the destination address.</param>
        /// <returns>An image representing the map route.</returns>
        public string getMap(string begin, string end)
        {
            string[] start = begin.Split(',');
            string[] finish = end.Split(',');

            string newStart = "";
            string newFinish = "";

            for (int i = 0; i < start.Length; i++)
            {
                if (i == 0)
                {
                    start[i] = HttpUtility.UrlPathEncode(start[i]);
                }
                else
                {
                    start[i] = start[i].Replace(" ", ",");
                }

                newStart += start[i];
            }

            for (int i = 0; i < finish.Length; i++)
            {
                if (i == 0)
                {
                    finish[i] = HttpUtility.UrlPathEncode(finish[i]);
                }
                else
                {
                    finish[i] = finish[i].Replace(" ", ",");
                }

                newFinish += finish[i];
            }

            // The Bing Maps REST API for obtaining a road map that displays a route
            string url = string.Format("https://dev.virtualearth.net/REST/v1/Imagery/Map/Road/Routes?wp.0={0};64;1&wp.1={1};66;2&key={2}", newStart, newFinish, key);

            return url.ToString();
        }

        /// <summary>
        /// An Address class that represents every part of an address with a string.
        /// </summary>
        private class Address
        {
            public string addressLine { get; set; }
            public string locatlity { get; set; }
            public string state { get; set; }
            public string postal { get; set; }
        }

        /// <summary>
        /// A class that holds two double precision numbers that represent latitude and longitude coordinates.
        /// </summary>
        private class LatLong
        {
            public double latitude { get; set; }
            public double longitude { get; set; }
        }

        /// <summary>
        /// This is a helper class designed to hold methods to easily convert between coordinates and addresses.
        /// </summary>
        private static class GeocodeHelper
        {
            /// <summary>
            /// Converts an address to a LatLong object.
            /// </summary>
            /// <param name="address">A string representing an address.</param>
            /// <returns>Returns a LatLong object.</returns>
            public static LatLong Geocode(string address)
            {
                // This is the BingMaps REST api that allows conversion from an address to coordinates
                string url = "http://dev.virtualearth.net/REST/v1/Locations?query=" + address + "&key=" + key;
                
                using (var client = new WebClient())
                {
                    // Downloads our response to a string using the WebClient
                    string response = client.DownloadString(url);
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Response));

                    // Converts our response to byte array, and then to a MemoryStream object
                    using (var es = new MemoryStream(Encoding.Unicode.GetBytes(response)))
                    {
                        // Converts our response from a MemoryStream object to a JSON object
                        var mapResponse = (ser.ReadObject(es) as Response);

                        // Converts the JSON object to a Location object
                        Location location = (Location)mapResponse.ResourceSets.First().Resources.First();

                        // Returns the resulting LatLong object
                        return new LatLong()
                        {
                            latitude = location.Point.Coordinates[0],
                            longitude = location.Point.Coordinates[1]
                        };
                    }
                }
            }

            /// <summary>
            /// Converts a coordinate pair to an address.
            /// </summary>
            /// <param name="latitude">Double precision number representing latitude.</param>
            /// <param name="longitude">Double precision number representing longitude.</param>
            /// <returns>Returns an Address object.</returns>
            public static Address inverseGeocode(double latitude, double longitude)
            {
                // This is the BingMaps REST api that allows conversion from geocode to an address
                string url = "http://dev.virtualearth.net/REST/v1/Locations/" + latitude.ToString() + "," + longitude.ToString() + "?key=" + key;

                using (var client = new WebClient())
                {
                    // Downloads our response to a string using the WebClient
                    string response = client.DownloadString(url);
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Response));

                    // Converts our response to byte array, and then to a MemoryStream object
                    using (var es = new MemoryStream(Encoding.Unicode.GetBytes(response)))
                    {
                        // Converts our response from a MemoryStream object to a JSON object
                        var mapResponse = (ser.ReadObject(es) as Response);

                        // Converts the JSON object to a Location object
                        Location location = (Location)mapResponse.ResourceSets.First().Resources.First();

                        // Returns the resulting Address object
                        return new Address()
                        {
                            addressLine = location.Address.AddressLine,
                            locatlity = location.Address.Locality,
                            state = location.Address.AdminDistrict,
                            postal = location.Address.PostalCode
                        };
                    }
                }
            }
        }
    }
}