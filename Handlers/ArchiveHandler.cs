using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
                return name + " (" + SizeFormatter.Format(size) + ")";
            }
        }

        // Items from Archive.org. Properties so the results grid can bind to them.
        public class ArchiveItem
        {
            public string title { get; set; }
            public string creator { get; set; }
            public string description { get; set; }
            public string identifier { get; set; }
            public string date { get; set; }
            public string topic { get; set; }
            public double avgRating { get; set; }
            public Int64 downloads { get; set; }
            public Int64 size { get; set; }
            // Type dropdown label of the search that returned this item.
            public string category { get; set; }
        }

        public class ArchiveCollection
        {
            public string identifier;
            public string title;
            // Type dropdown label the collection was listed or saved under.
            public string category;

            public override string ToString() => title;
        }

        private const string ItemFields = "identifier,title,description,creator,subject,downloads,item_size,avg_rating,date,year";

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
            mediaType = (mediaType ?? "").Trim();
            string query = string.IsNullOrEmpty(mediaType) ? "mediatype:*" : "mediatype:(" + mediaType + ")";
            return RunQuery(query, "addeddate:desc");
        }

        // A keyword search on Latest sorts by publicdate; updatedate does not move when items are added.
        public static List<ArchiveCollection> GetCollections(string mediaType, string category, bool byDownloads, string keyword)
        {
            string query = CollectionQuery(mediaType);
            if (query == null)
            {
                return new List<ArchiveCollection>();
            }

            keyword = (keyword ?? "").Trim();
            if (keyword.Length == 0 && !byDownloads)
            {
                return GetRecentlyUpdatedCollections(mediaType, category);
            }
            if (keyword.Length > 0)
            {
                query = "(" + EscapeLucene(keyword) + ") AND (" + query + ")";
            }

            JArray results_array = FetchResults(query, byDownloads ? "downloads:desc" : "publicdate:desc", "identifier,title");
            if (results_array == null)
            {
                return null;
            }

            List<ArchiveCollection> collections = new List<ArchiveCollection>();
            foreach (JToken item in results_array)
            {
                JToken fields = item["fields"] ?? item;
                if (fields["identifier"] == null)
                {
                    continue;
                }

                ArchiveCollection collection = new ArchiveCollection();
                collection.identifier = fields["identifier"].ToString();
                collection.title = fields["title"] != null ? fields["title"].ToString() : collection.identifier;
                collection.category = category;
                collections.Add(collection);
            }
            return collections;
        }

        // Collections have no field that changes when an item is added, so they are
        // ordered by the newest item of this type that each one contains.
        private static List<ArchiveCollection> GetRecentlyUpdatedCollections(string mediaType, string category)
        {
            string url = "http://archive.org/advancedsearch.php?q=" + Uri.EscapeDataString("mediatype:(" + mediaType + ")") + "&fl[]=collection&sort[]=" + Uri.EscapeDataString("addeddate desc") + "&rows=2000&output=json";
            string itemsJson = GetJsonResponse(url);
            JArray items = ReadResultsArray(itemsJson, false);
            if (items == null)
            {
                ShowFetchError(itemsJson == null);
                return null;
            }

            List<string> identifiers = new List<string>();
            foreach (JToken item in items)
            {
                JToken parents = item["collection"];
                if (parents == null)
                {
                    continue;
                }

                foreach (JToken parent in parents is JArray ? (IEnumerable<JToken>)parents : new JToken[] { parents })
                {
                    string identifier = parent.ToString().Replace("\"", "");
                    if (identifier.Length > 0 && identifiers.Count < 100 && !identifiers.Contains(identifier))
                    {
                        identifiers.Add(identifier);
                    }
                }
            }

            List<ArchiveCollection> collections = new List<ArchiveCollection>();
            if (identifiers.Count == 0)
            {
                return collections;
            }

            JArray titles = FetchResults("mediatype:collection AND identifier:(\"" + string.Join("\" OR \"", identifiers.ToArray()) + "\")", "", "identifier,title");
            if (titles == null)
            {
                return null;
            }

            Dictionary<string, string> titleById = new Dictionary<string, string>();
            foreach (JToken item in titles)
            {
                JToken fields = item["fields"] ?? item;
                if (fields["identifier"] != null && fields["title"] != null)
                {
                    titleById[fields["identifier"].ToString()] = fields["title"].ToString();
                }
            }

            foreach (string identifier in identifiers)
            {
                ArchiveCollection collection = new ArchiveCollection();
                collection.identifier = identifier;
                string title;
                collection.title = titleById.TryGetValue(identifier, out title) ? title : identifier;
                collection.category = category;
                collections.Add(collection);
            }
            return collections;
        }

        // Quoted, not escaped: the website search rejects a backslash before a hyphen.
        public static List<ArchiveItem> GetCollectionItems(string identifier, string mediaType)
        {
            string query = "collection:(\"" + identifier.Replace("\"", "") + "\") AND mediatype:(" + mediaType + ")";
            return RunQuery(query, "addeddate:desc");
        }

        private static string CollectionQuery(string mediaType)
        {
            switch (mediaType)
            {
                case "software":
                    return "mediatype:collection AND (subject:software OR collection:softwarelibrary OR collection:open_source_software)";
                case "movies":
                    return "mediatype:collection AND collection:moviesandfilms";
                case "audio":
                    return "mediatype:collection AND (collection:etree OR collection:librivoxaudio OR collection:audio_bookspoetry)";
                case "texts":
                    return "mediatype:collection AND (collection:americana OR collection:gutenberg OR subject:books)";
                case "image":
                    return "mediatype:collection AND (collection:flickrcommons OR subject:photographs)";
                default:
                    return null;
            }
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
            JArray results_array = FetchResults(query, sort, ItemFields);
            if (results_array == null)
            {
                return null;
            }

            return ParseSearchResults(results_array);
        }

        // Shows the connection or unreadable-response dialog and returns null on failure.
        private static JArray FetchResults(string query, string sort, string fields)
        {
            // Same metadata search the archive.org website uses. advancedsearch.php
            // rewrites bare words into a full-text query, which ranks different items.
            string url = "http://archive.org/services/search/beta/page_production/?user_query=" + Uri.EscapeDataString(query) + "&hits_per_page=100&page=1&aggregations=false&fields=" + fields;
            if (!string.IsNullOrEmpty(sort))
            {
                url += "&sort=" + Uri.EscapeDataString(sort);
            }

            string resultsJson = GetJsonResponse(url);
            JArray results_array = ReadResultsArray(resultsJson, true);

            // The website endpoint is an undocumented beta; fall back to advancedsearch.php
            // only when it fails, not when it returns no results.
            if (results_array == null)
            {
                string fallbackUrl = "http://archive.org/advancedsearch.php?q=" + Uri.EscapeDataString(query) + "&fl[]=access-restricted-item&rows=100&output=json";
                foreach (string field in fields.Split(','))
                {
                    fallbackUrl += "&fl[]=" + field;
                }
                if (!string.IsNullOrEmpty(sort))
                {
                    fallbackUrl += "&sort[]=" + Uri.EscapeDataString(sort.Replace(':', ' '));
                }

                string fallbackJson = GetJsonResponse(fallbackUrl);
                results_array = ReadResultsArray(fallbackJson, false);

                if (results_array == null)
                {
                    ShowFetchError(resultsJson == null && fallbackJson == null);
                    return null;
                }
            }

            return results_array;
        }

        private static void ShowFetchError(bool noResponse)
        {
            if (noResponse)
            {
                MessageBox.Show("Error 01: Error retrieving results. Please check your Internet connection. Additionally, Archive.org may be down.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Error 03: The search response could not be read.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static JArray ReadResultsArray(string resultsJson, bool websiteResponse)
        {
            try
            {
                JObject results_obj = JObject.Parse(resultsJson);
                return (JArray)(websiteResponse ? results_obj["response"]["body"]["hits"]["hits"] : results_obj["response"]["docs"]);
            }
            catch
            {
                return null;
            }
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

        private static List<ArchiveItem> ParseSearchResults(JArray results_array)
        {
            List<ArchiveItem> results = new List<ArchiveItem>();
            foreach (var item in results_array)
            {
                try
                {
                    JToken fields = item["fields"] ?? item;

                    // Skip items that are access restricted
                    if (fields["access-restricted-item"] != null && (bool)fields["access-restricted-item"])
                    {
                        continue;
                    }

                    // Create a new ArchiveItem
                    ArchiveItem result = new ArchiveItem
                    {
                        title = fields["title"].ToString(),
                        identifier = fields["identifier"].ToString(),
                        size = (Int64)fields["item_size"]
                    };

                    // If no value exists for downloads, set it to 0
                    if (fields["downloads"] != null)
                    {
                        result.downloads = (Int64)fields["downloads"];
                    }

                    // Set the description. If it doesn't exist, set it to "No description found."
                    if (fields["description"] != null)
                    {
                        result.description = fields["description"].ToString();
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
                    if (fields["avg_rating"] != null)
                    {
                        double rating;
                        double.TryParse(fields["avg_rating"].ToString(), out rating);
                        result.avgRating = rating;
                    }

                    result.creator = FirstValue(fields["creator"]);

                    result.date = "";
                    if (fields["year"] != null)
                    {
                        result.date = fields["year"].ToString();
                    }

                    result.topic = FirstValue(fields["subject"]);

                    results.Add(result);
                }
                catch
                {
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
