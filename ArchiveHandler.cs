using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Windows.Forms;

namespace SoftwareShelf_Desktop
{
    internal class ArchiveHandler
    {
        // Create a struct to store items from Archive.org
        public struct ArchiveItem
        {
            public string title;
            public string creator;
            public string description;
            public string identifier;
            public string date;
            public string topic;
            public double avgRating;
            public Int64 downloads;
            public Int64 size;
        }

        // Function to perform searches on Archive.org
        public static List<ArchiveItem> Search(string query, string mediaType = "", string creatorName = "", string topicName = "", string yearText = "")
        {
            //If query is blank, error out
            if (query == "" && creatorName == "" && topicName == "" && yearText == "")
            {
                MessageBox.Show("Error 02: Search queries cannot be blank.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            // Build the Query
            query = GetQuery(query, creatorName, topicName, yearText);
            string additionalQuery = BuildAdditionalQuery(creatorName, topicName, yearText);

            // Check if mediaType is not empty or blank
            if (!string.IsNullOrEmpty(mediaType))
            {
                // Set MediaType
                mediaType = "AND+mediatype:(" + mediaType + ")";
            }

            // Base URL for API
            string search_url = "http://archive.org/advancedsearch.php?q=(" + query + ")+" + mediaType + additionalQuery + "&fl[]=identifier&fl[]=description&fl[]=title&fl[]=item_size&fl[]=downloads&fl[]=avg_rating&fl[]=creator&fl[]=subject&fl[]=access-restricted-item&fl[]=date&sort[]=&sort[]=&sort[]=&rows=100&output=json";

            //MessageBox.Show(search_url);
            Console.WriteLine("[Info] Searching:" + search_url);

            string results_json = GetJsonResponse(search_url);

            if (string.IsNullOrEmpty(results_json))
            {
                MessageBox.Show("Error 01: Error retrieving results. Please check your Internet connection. Additionally, Archive.org may be down.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            return ParseSearchResults(results_json);
        }

        public static List<ArchiveItem> GetLatestItems (string mediaType = "")
        {
            // Logging
            Console.WriteLine("[Info] Grabbing latest items for selected type.");

            // Check if mediaType is not empty or blank
            if (!string.IsNullOrEmpty(mediaType))
            {
                // Set MediaType
                mediaType = "AND+mediatype:(" + mediaType + ")";
            }

            // Query URL for latest items
            string latest_url = "http://archive.org/advancedsearch.php?q=\"\"+" + mediaType + "&fl[]=identifier&fl[]=description&fl[]=title&fl[]=item_size&fl[]=downloads&fl[]=avg_rating&fl[]=creator&fl[]=subject&fl[]=access-restricted-item&fl[]=date&sort[]=addeddate+desc&rows=100&output=json";

            Console.WriteLine("[Info] Searching:" + latest_url);

            // Get JSON from API
            string results_json = GetJsonResponse(latest_url);

            if (string.IsNullOrEmpty(results_json))
            {
                MessageBox.Show("Error 01: Error retrieving results. Please check your Internet connection. Additionally, Archive.org may be down.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            return ParseSearchResults(results_json);
        }

        // Function to get available files for an identifier on Archive.org
        public static List<string> GetAvailableFiles(string identifier)
        {
            // Create a list of strings to store available files
            //List<string> availableFiles = new List<string>();

            // Create a string to store the URL to download the JSON response from
            string metadata_url = "http://archive.org/metadata/" + identifier;

            // Create a string to store the JSON response
            string metadata_json = GetJsonResponse(metadata_url);

            if (string.IsNullOrEmpty(metadata_json))
            {
                MessageBox.Show("Error 11: Error retrieving files. Please check your Internet connection. Additionally, Archive.org may be down.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            return ParseAvailableFiles(metadata_json);
        }

        private static string BuildAdditionalQuery(string creatorName, string topicName, string yearText)
        {
            // LOGGING
            Console.WriteLine("[Info] Building additional query.");

            string additionalQuery = "";

            if (!string.IsNullOrEmpty(creatorName))
            {
                additionalQuery += "+AND+creator:(" + creatorName + ")";
            }

            if (!string.IsNullOrEmpty(topicName))
            {
                additionalQuery += "+AND+subject:(" + topicName + ")";
            }

            if (!string.IsNullOrEmpty(yearText))
            {
                additionalQuery += "+AND+year:(" + yearText + ")";
            }

            return additionalQuery;
        }

        private static string GetQuery(string query, string creatorName, string topicName, string yearText)
        {
            // LOGGING
            Console.WriteLine("[Info] Grabbing query.");

            if (string.IsNullOrEmpty(query))
            {
                if (!string.IsNullOrEmpty(yearText))
                {
                    return yearText;
                }
                if (!string.IsNullOrEmpty(creatorName))
                {
                    return creatorName;
                }
                if (!string.IsNullOrEmpty(topicName))
                {
                    return topicName;
                }
            }
            return query;
        }

        private static string GetJsonResponse(string url)
        {
            // LOGGING
            Console.WriteLine("[Info] Getting JSON Response.");

            try
            {
                using (WebClient client = new WebClient())
                {
                    // Download JSON response from search_url
                    return client.DownloadString(url);
                }
            }
            catch
            {
                return null;
            }
        }

        private static List<ArchiveItem> ParseSearchResults(string resultsJson)
        {
            // LOGGING
            Console.WriteLine("[Info] Parsing results.");

            List<ArchiveItem> results = new List<ArchiveItem>();

            try
            {
                // Try to parse JSON response into JSON object
                JObject results_obj = JObject.Parse(resultsJson);
                JArray results_array = (JArray)results_obj["response"]["docs"];

                foreach (var item in results_array)
                {
                    // Skip items that are access restricted
                    if (item["access-restricted-item"] != null && (bool)item["access-restricted-item"])
                    {
                        continue;
                    }

                    // Create a new ArchiveItem
                    ArchiveItem result = new ArchiveItem
                    {
                        title = item["title"].ToString(),
                        identifier = item["identifier"].ToString(),
                        size = (Int64)item["item_size"] / 1024,
                        downloads = (Int64)item["downloads"]
                    };

                    // Set the description. If it doesn't exist, set it to "No description found."
                    if (item["description"] != null)
                    {
                        result.description = item["description"].ToString();
                        // Truncate descriptions at 30,000 characters
                        if (result.description.Length > 30000)
                        {
                            result.description = result.description.Substring(0, 30000);
                        }
                    }
                    else
                    {
                        result.description = "No description found.";
                    }

                    // If no value exists for avg_rating, set it to 0
                    if (item["avg_rating"] != null)
                    {
                        double.TryParse(item["avg_rating"].ToString(), out result.avgRating);
                    }
                    else
                    {
                        result.avgRating = 0;
                    }

                    // Set creator
                    if (item["creator"] != null)
                    {
                        try
                        {
                            result.creator = item["creator"][0].ToString();
                        }
                        catch
                        {
                            result.creator = item["creator"].ToString();
                        }
                    }
                    else
                    {
                        result.creator = "";
                    }

                    // Set date
                    if (item["date"] != null)
                    {
                        DateTime date;
                        DateTime.TryParseExact(item["date"].ToString(), "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
                        result.date = date.Year.ToString();
                    }
                    else
                    {
                        result.date = "";
                    }

                    // Set topic
                    if (item["subject"] != null)
                    {
                        try {
                            result.topic = item["subject"][0].ToString();
                        }
                        catch
                        {
                            result.topic = item["subject"].ToString();
                        }
                    }
                    else
                    {
                        result.topic = "";
                    }

                    results.Add(result);
                }

                return results;
            }
            catch
            {
                MessageBox.Show("Error 03: No results found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private static List<string> ParseAvailableFiles(string metadata_json)
        {
            // LOGGING
            Console.WriteLine("[Info] Parsing available files.");
            var availableFiles = new List<string>();

            try
            {
                // Try to parse JSON response into JSON object
                JObject files_obj = JObject.Parse(metadata_json);

                // Get the files array from the JSON response
                JArray files_array = (JArray)files_obj["files"];

                // Loop through each file in the files array
                foreach (var file in files_array)
                {
                    // Check if the file is private
                    if (file["private"] != null && file["private"].ToObject<bool>() == true)
                    {
                        // Skip this file
                        continue;
                    }

                    availableFiles.Add(file["name"].ToString());
                }

                return availableFiles;
            }
            catch
            {
                MessageBox.Show("Error 12: No results found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
    }
}
