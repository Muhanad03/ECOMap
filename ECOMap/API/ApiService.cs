using Newtonsoft.Json;
using System.Net.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ECOMap.API
{
    public class ApiService
    {
        private HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://ct503824grp5-ct5038.uogs.co.uk/ECOMapAPI/treePoints/");
        }

        public async Task<List<treeData>> GetTreeDataAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("read.php");
            response.EnsureSuccessStatusCode();

            string jsonResponse = await response.Content.ReadAsStringAsync();

            // Deserialize the JSON response into a JObject
            JObject responseObject = JObject.Parse(jsonResponse);

            // Check if the response contains a "status" property with a value of 200
            if (responseObject["status"] != null && (int)responseObject["status"] == 200)
            {
                // Check if the response contains a "data" property
                if (responseObject["data"] != null)
                {
                    // Deserialize the "data" property into a JArray
                    JArray dataArray = (JArray)responseObject["data"];

                    // Create a list to store the deserialized treeData objects
                    List<treeData> treeDataList = new List<treeData>();

                    // Iterate over each item in the array and deserialize it into a treeData object
                    foreach (var item in dataArray)
                    {
                        treeData tree = new treeData
                        {
                            longitude = Convert.ToDouble(item["longitude"]),
                            latitude = Convert.ToDouble(item["latitude"]),
                            id = item["id"].ToString()
                        };

                        // Add the deserialized treeData object to the list
                        treeDataList.Add(tree);
                    }

                    return treeDataList;
                }
                else
                {
                    // Handle the case where the "data" property is missing or empty
                    Console.WriteLine("No tree data found");
                    return new List<treeData>();
                }
            }
            else
            {
                // Handle other status codes or error scenarios
                Console.WriteLine("API returned an error");
                return new List<treeData>();
            }
        }




    }

    public class treeData
    {
        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("longitude")]
        public double longitude { get; set; }

        [JsonProperty("latitude")]
        public double latitude { get; set; }
        
    }
}
