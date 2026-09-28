using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace SoftwareShelf_Desktop
{
    internal class ArchiveHandler
    {
        private static Dictionary<string, string> itemMetadata = new Dictionary<string, string>();

        public struct ArchiveFile
        {
            public string name;
            public long size;

            public override string ToString()
            {
                if (size < 0)
                {
                    return name;
                }
                return name + " (" + (size / 1024) + " KiB)";
            }
        }

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
            query = (query ?? "").Trim();
            mediaType = (mediaType ?? "").Trim();
            creatorName = (creatorName ?? "").Trim();
            topicName = (topicName ?? "").Trim();
            yearText = (yearText ?? "").Trim();

            if (query.Length == 0 && creatorName.Length == 0 && topicName.Length == 0 && yearText.Length == 0)
            {
                MessageBox.Show("Error 02: Search queries cannot be blank.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            return RunQuery(BuildSearchQuery(query, mediaType, creatorName, topicName, yearText), "");
        }

        public static List<ArchiveItem> GetLatestItems(string mediaType = "")
        {
            Console.WriteLine("[Info] Grabbing latest items for selected type.");

            mediaType = (mediaType ?? "").Trim();
            string query = string.IsNullOrEmpty(mediaType) ? "mediatype:*" : "mediatype:(" + mediaType + ")";
            return RunQuery(query, "addeddate+desc");
        }

        public static List<Review> GetReviews(string identifier)
        {
            // Get metadata for item
            string metadata_json = GetItemMetadata(identifier);

            // Deserialize the JSON into a JObject.
            JObject metadataObj = JsonConvert.DeserializeObject(metadata_json) as JObject;

            List<Review> reviews = new List<Review>();

            // Check if the "reviews" property exists.
            JToken reviewsToken;
            if (metadataObj != null && metadataObj.TryGetValue("reviews", out reviewsToken))
            {
                JArray reviewsArray = reviewsToken as JArray;
                if (reviewsArray != null)
                {
                    foreach (JToken review in reviewsArray)
                    {
                        // Extract the necessary properties and store in a review object.
                        Review reviewObj = new Review
                        {
                            Title = (string)review["reviewtitle"] ?? string.Empty,
                            Stars = (string)review["stars"] ?? string.Empty,
                            Body = (string)review["reviewbody"] ?? string.Empty,
                            Reviewer = (string)review["reviewer"] ?? string.Empty
                        };

                        // Add review to list
                        reviews.Add(reviewObj);
                    }
                }
                return reviews;
            }
            else
            {
                return null;
            }
        }

        private static string BuildSearchQuery(string query, string mediaType, string creatorName, string topicName, string yearText)
        {
            Console.WriteLine("[Info] Building search query.");

            List<string> clauses = new List<string>();
            if (query.Length > 0)
            {
                clauses.Add("(" + EscapeLucene(query) + ")");
            }
            if (mediaType.Length > 0)
            {
                clauses.Add("mediatype:(" + mediaType + ")");
            }
            if (creatorName.Length > 0)
            {
                clauses.Add("creator:(" + EscapeLucene(creatorName) + ")");
            }
            if (topicName.Length > 0)
            {
                clauses.Add("subject:(" + EscapeLucene(topicName) + ")");
            }
            if (yearText.Length > 0)
            {
                clauses.Add("year:(" + EscapeLucene(yearText) + ")");
            }

            string combined = clauses[0];
            for (int i = 1; i < clauses.Count; i++)
            {
                combined += " AND " + clauses[i];
            }
            return combined;
        }

        private static string EscapeLucene(string value)
        {
            StringBuilder escaped = new StringBuilder();
            foreach (char c in value)
            {
                if ("\\+-&|!(){}[]^\"~*?:/".IndexOf(c) >= 0)
                {
                    escaped.Append('\\');
                }
                escaped.Append(c);
            }
            return escaped.ToString();
        }

        private static List<ArchiveItem> RunQuery(string query, string sort)
        {
            itemMetadata.Clear();
            string url = "http://archive.org/advancedsearch.php?q=" + Uri.EscapeDataString(query) + "&fl[]=identifier&fl[]=description&fl[]=title&fl[]=item_size&fl[]=downloads&fl[]=avg_rating&fl[]=creator&fl[]=subject&fl[]=access-restricted-item&fl[]=date&rows=100&output=json";
            if (!string.IsNullOrEmpty(sort))
            {
                url += "&sort[]=" + sort;
            }

            Console.WriteLine("[Info] Searching:" + url);

            string resultsJson = GetJsonResponse(url);
            if (resultsJson == null)
            {
                MessageBox.Show("Error 01: Error retrieving results. Please check your Internet connection. Additionally, Archive.org may be down.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            return ParseSearchResults(resultsJson);
        }

        internal static string GetItemMetadata(string identifier)
        {
            string json;
            if (itemMetadata.TryGetValue(identifier, out json))
            {
                return json;
            }

            json = GetJsonResponse("http://archive.org/metadata/" + identifier);
            if (json != null)
            {
                itemMetadata[identifier] = json;
            }
            return json;
        }

        internal static string GetJsonResponse(string url)
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
            Console.WriteLine("[Info] Parsing results.");

            JArray results_array;
            try
            {
                JObject results_obj = JObject.Parse(resultsJson);
                results_array = (JArray)results_obj["response"]["docs"];
            }
            catch
            {
                MessageBox.Show("Error 03: The search response could not be read.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            if (results_array == null)
            {
                MessageBox.Show("Error 03: The search response could not be read.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            if (results_array.Count == 0)
            {
                MessageBox.Show("No results found.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return new List<ArchiveItem>();
            }

            List<ArchiveItem> results = new List<ArchiveItem>();
            foreach (var item in results_array)
            {
                try
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
                        size = (Int64)item["item_size"] / 1024
                    };

                    // If no value exists for downloads, set it to 0
                    if (item["downloads"] != null)
                    {
                        result.downloads = (Int64)item["downloads"];
                    }
                    else
                    {
                        result.downloads = 0;
                    }

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

                    result.creator = FirstValue(item["creator"]);

                    result.date = "";
                    if (item["date"] != null)
                    {
                        DateTime date;
                        if (DateTime.TryParseExact(item["date"].ToString(), "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                        {
                            result.date = date.Year.ToString();
                        }
                    }

                    result.topic = FirstValue(item["subject"]);

                    results.Add(result);
                }
                catch
                {
                    continue;
                }
            }

            return results;
        }

        private static string FirstValue(JToken token)
        {
            if (token == null)
            {
                return "";
            }

            JArray values = token as JArray;
            if (values != null)
            {
                if (values.Count == 0)
                {
                    return "";
                }
                return values[0].ToString();
            }

            return token.ToString();
        }

        internal static List<ArchiveFile> ParseAvailableFiles(string metadata_json)
        {
            // LOGGING
            Console.WriteLine("[Info] Parsing available files.");
            List<ArchiveFile> availableFiles = new List<ArchiveFile>();

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

                    ArchiveFile item = new ArchiveFile();
                    item.name = file["name"].ToString();
                    item.size = -1;
                    long parsedSize;
                    if (file["size"] != null && long.TryParse(file["size"].ToString(), out parsedSize))
                    {
                        item.size = parsedSize;
                    }
                    availableFiles.Add(item);
                }

                return availableFiles;
            }
            catch
            {
                return null;
            }
        }
    }
}
