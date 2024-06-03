using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
            public string description;
            public string identifier;
            public Int64 downloads;
            public Int64 size;
        }

        // Function to perform searches on Archive.org
        public static List<ArchiveItem> Search(string query)
        {
            //If query is blank, error out
            if (query == "")
            {
                MessageBox.Show("Error 02: Search queries cannot be blank.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }


            string search_url = "http://archive.org/advancedsearch.php?q=(" + query + ")+AND+mediatype:(software)&fl[]=identifier&fl[]=description&fl[]=title&fl[]=item_size&fl[]=downloads&sort[]=&sort[]=&sort[]=&rows=100&output=json";
            string results_json = "";

            // Create a list of ArchiveItems
            List<ArchiveItem> results = new List<ArchiveItem>();

            // Try to download JSON response from search_url
            try
            {
                using (WebClient client = new WebClient())
                {
                    // Download JSON response from search_url
                    results_json = client.DownloadString(search_url);
                }
            }
            catch
            {
                // Show message that search failed
                MessageBox.Show("Error 01: Error retrieving results. Please check your Internet connection. Additionally, Archive.org may be down.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            // Try to parse JSON response into JSON object
            JObject results_obj = JObject.Parse(results_json);

            try
            {
                JArray results_array = (JArray)results_obj["response"]["docs"];

                foreach (var item in results_array)
                {
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

                    // Add ArchiveItem to results list
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

        // Function to get available files for an identifier on Archive.org
        public static List<string> GetAvailableFiles(string identifier)
        {
            // Create a list of strings to store available files
            List<string> availableFiles = new List<string>();

            // Create a string to store the JSON response
            string metadata_json = "";

            // Create a string to store the URL to download the JSON response from
            string metadata_url = "http://archive.org/metadata/" + identifier;

            // Try to download JSON response from metadata_url
            try
            {
                using (WebClient client = new WebClient())
                {
                    // Download JSON response from metadata_url
                    metadata_json = client.DownloadString(metadata_url);
                }
            }
            catch
            {
                // Show message that search failed
                MessageBox.Show("Error 11: Error retrieving files. Please check your Internet connection. Additionally, Archive.org may be down.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Try to parse JSON response into JSON object
            JObject files_obj = JObject.Parse(metadata_json);

            try
            {
                // Get the files array from the JSON response
                JArray files_array = (JArray)files_obj["files"];

                // Loop through each file in the files array
                foreach (var file in files_array)
                {
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
