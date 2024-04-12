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

            JObject responseObject = JObject.Parse(jsonResponse);

            if (responseObject["status"] != null && (int)responseObject["status"] == 200)
            {
                if (responseObject["data"] != null)
                {
                    JArray dataArray = (JArray)responseObject["data"];

                    List<treeData> treeDataList = new List<treeData>();

                    foreach (var item in dataArray)
                    {
                        treeData tree = new treeData
                        {
                            longitude = Convert.ToDouble(item["longitude"]),
                            latitude = Convert.ToDouble(item["latitude"]),
                            id = item["id"].ToString()
                        };

                        treeDataList.Add(tree);
                    }

                    return treeDataList;
                }
                else
                {
                    Console.WriteLine("No tree data found");
                    return new List<treeData>();
                }
            }
            else
            {
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
