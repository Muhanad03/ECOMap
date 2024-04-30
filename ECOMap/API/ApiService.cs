using Newtonsoft.Json;
using System.Net.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections.Specialized;

namespace ECOMap.API
{
    public class ApiService
    {
        private HttpClient _httpClient;
        private string TreeEndPoint = "treePoints/";
        private string UserEndPoint = "userHandler/";


        public ApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://ct503824grp5-ct5038.uogs.co.uk/ECOMapAPI/");

        }

        public async Task<List<treeData>> GetTreeDataAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{TreeEndPoint}read.php");
            try
            {
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
                                id = (int)item["Tree_ID"],
                                addedByUser_ID = (int)item["AddedByUser_ID"],
                                guardian_ID = (int?)item["Guardian_ID"],
                                longitude = Convert.ToDouble(item["Longitude"]),
                                latitude = Convert.ToDouble(item["Latitude"]),
                                height = Convert.ToDouble(item["Height"]),
                                circumference = Convert.ToDouble(item["Circumference"]),
                                plant_Age = item["Plant_Age"].ToString(),
                                planter_Name = item["Planter_Name"].ToString()

                            };

                            treeDataList.Add(tree);
                        }

                        return treeDataList;
                    }
                    else
                    {
                        Debug.WriteLine("No tree data found");
                        return new List<treeData>();
                    }
                }
                else
                {
                    Debug.WriteLine("API returned an error");
                    return new List<treeData>();
                }
            }catch(HttpRequestException ex)
            {

                return new List<treeData>();
            }
            

            
        }

        public async Task<string> AddTreeDataAsync(treeData tree)
        {
            try
            {
                // Serialize the treeData object to JSON
                string jsonTree = JsonConvert.SerializeObject(tree);
                Debug.WriteLine(jsonTree);

                // Create the HTTP content with the JSON data
                var content = new StringContent(jsonTree, Encoding.UTF8, "application/json");

                // Send the POST request to the API endpoint
                HttpResponseMessage response = await _httpClient.PostAsync(TreeEndPoint+"create.php", content);

                // Check if the request was successful
                response.EnsureSuccessStatusCode();

                // Read the response body
                string responseBody = await response.Content.ReadAsStringAsync();

                // Log response for debugging
                Console.WriteLine($"Response from server: {responseBody}");

                // Return the response body or other relevant information
                return responseBody;
            }
            catch (HttpRequestException ex)
            {
                // Handle HTTP request-related errors
                return $"HTTP request error: {ex.Message}";
            }
            catch (Exception ex)
            {
                // Handle other types of exceptions
                return $"Error adding tree data: {ex.Message}";
            }
        }



        public async Task<string> checkLogin(userLoginDetails user)
        {

            try
            {
                string jsonTree = JsonConvert.SerializeObject(user);
                Debug.WriteLine(jsonTree);

                var content = new StringContent(jsonTree, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync($"{UserEndPoint}login.php", content);

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Response from server: {responseBody}");

                return responseBody;
            }
            catch (HttpRequestException ex)
            {
                return $"HTTP request error: {ex.Message}";
            }
            catch (Exception ex)
            {
                return $"Error checking user data: {ex.Message}";
            }
        }


        //public async Task<string> PostImage(imageData image)
        //{
        //    try
        //    {
        //        string jsonTree = JsonConvert.SerializeObject(image);
        //        Debug.WriteLine(jsonTree);

        //        var content = new StringContent(jsonTree, Encoding.UTF8, "application/json");

        //        HttpResponseMessage response = await _httpClient.PostAsync("imageupload.php", content);

        //        response.EnsureSuccessStatusCode();

        //        string responseBody = await response.Content.ReadAsStringAsync();

        //        Console.WriteLine($"Response from server: {responseBody}");

        //        return responseBody;
        //    }
        //    catch (HttpRequestException ex)
        //    {
        //        return $"HTTP request error: {ex.Message}";
        //    }
        //    catch (Exception ex)
        //    {
        //        return $"Error adding tree data: {ex.Message}";
        //    }
        //}




      




    }

    public class treeData
    {
        [JsonProperty("Tree_ID")]
        public int id { get; set; }


        [JsonProperty("Guardian_ID")]
        public int? guardian_ID { get; set; }

        [JsonProperty("AddedByUser_ID")]
        public int addedByUser_ID { get; set; }

        [JsonProperty("Longitude")]
        public double longitude { get; set; }

        [JsonProperty("Latitude")]
        public double latitude { get; set; }

        [JsonProperty("Height")]
        public double height { get; set; }

        [JsonProperty("Circumference")]
        public double circumference { get; set; }

        [JsonProperty("Plant_Age")]
        public string plant_Age { get; set; }

        [JsonProperty("Planter_Name")]
        public string planter_Name { get; set; }

    }


    public class userData
    {
        [JsonProperty("User_ID")]
        public int id { get; set; }


        [JsonProperty("Email")]
        public string email { get; set; }

        [JsonProperty("First_Name")]
        public string first_Name { get; set; }

        [JsonProperty("Last_Name")]
        public string last_Name { get; set; }

        [JsonProperty("User_Type")]
        public char user_Type { get; set; }

        [JsonProperty("Point_Total")]
        public int total_Points { get; set; }

        [JsonProperty("Password")]
        public string password { get; set; }







    }

    public class userLoginDetails
    {
        [JsonProperty("email")]
        public string email { get; set; }

        [JsonProperty("password")]
        public string password { get; set; }
    }

    public class imageData
    {
        [JsonProperty("Image_ID")]
        public int id { get; set; }
        [JsonProperty("Tree_ID")]
        public int tree_id { get; set; }
        [JsonProperty("User_ID")]
        public int user_id { get; set; }
        [JsonProperty("Base64")]
        public string base64 { get; set; }





    }
}
