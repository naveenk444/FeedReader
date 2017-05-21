using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Web.Script.Serialization;
using System.Linq;
using System.Globalization;
using System.IO;
using System.Net;

namespace MarketingCloud.AutomationStudio.FeedReader.Core.Services
{
    public class FeedService : Interfaces.IFeedService
    {
        public IEnumerable<Entities.Feed> GetFeeds(int noOfFeeds, string strUserName)
        {
            if(ValidateConfigurationEntries())
            {
                return GetTwitterFeeds(noOfFeeds, strUserName);
            }
            else
            {
                throw new Exception("Some of the configuration entries required for the application are missing. Please check them and try again");
            }
        }
        public IEnumerable<Entities.Feed> GetTwitterFeeds(int noOfFeeds, string strUserName)
        {
            string accessToken = GetAccessToken();
            var reader = new AppSettingsReader();
            string apiUrl = reader.GetValue("APIURL", typeof(string)).ToString();
            StringBuilder sbRequestUri = new StringBuilder();
            sbRequestUri.Append(apiUrl);
            sbRequestUri.Append(String.Format("?screen_name={0}&exclude_replies=1&count={1}", strUserName, noOfFeeds));

            HttpWebRequest timeLineRequest = (HttpWebRequest)WebRequest.Create(sbRequestUri.ToString());
            timeLineRequest.Headers.Add("Authorization", "Bearer " + accessToken);
            timeLineRequest.Method = "Get";
            WebResponse timeLineResponse = timeLineRequest.GetResponse();
            string timeLineJson;
            using (timeLineResponse)
            {
                using (var streamReader = new StreamReader(timeLineResponse.GetResponseStream()))
                {
                    timeLineJson = streamReader.ReadToEnd();
                }
            }
            var serializer = new JavaScriptSerializer();
            var feedsJson = (object[])serializer.Deserialize<object>(timeLineJson);

            if (feedsJson == null)
            {
                return null;
            }

            return feedsJson.Cast<Dictionary<string, object>>().Select(feed =>
             {
                 var user = ((Dictionary<string, object>)feed["user"]);
                 string mediaUrl = string.Empty;
                 dynamic entities = ((Dictionary<string, object>)feed["entities"]);
                 if (entities.ContainsKey("media"))
                 {
                     dynamic media = entities["media"];
                     if (media != null)
                     {
                         mediaUrl = media[0]["media_url"].ToString();
                         
                     }
                 }
                 
                 var date = DateTime.ParseExact(feed["created_at"].ToString(),
                     "ddd MMM dd HH:mm:ss zz00 yyyy",
                     CultureInfo.InvariantCulture).ToLocalTime();

                 return new Entities.Feed
                 {
                     UserName = (string)user["name"],
                     UserScreenName = (string)user["screen_name"],
                     UserProfileImageUrl = (string)user["profile_image_url"],
                     MediaUrl = mediaUrl,
                     Content = (string)feed["text"],
                     PostedDate = date,
                     RetweetCount = Convert.ToInt32(feed["retweet_count"])
                 };

             }).ToArray();
 
        }
        public string GetAccessToken()
        {
            var reader = new AppSettingsReader();
            var oAuthConsumerSecret = reader.GetValue("OAuthConsumerSecret", typeof(string));
            var oAuthConsumerKey = reader.GetValue("OAuthConsumerKey", typeof(string));
            string tokenUrl = reader.GetValue("TokenURL", typeof(string)).ToString();

            var apiCredentials = Convert.ToBase64String(new UTF8Encoding().GetBytes(oAuthConsumerKey + ":" + oAuthConsumerSecret));
            var postBody = "grant_type=client_credentials";

            HttpWebRequest authRequest = (HttpWebRequest)WebRequest.Create(tokenUrl);
            authRequest.Headers.Add("Authorization", "Basic " + apiCredentials);
            authRequest.Method = "POST";
            authRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            authRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (Stream stream = authRequest.GetRequestStream())
            {
                byte[] content = Encoding.ASCII.GetBytes(postBody);
                stream.Write(content, 0, content.Length);
            }

            authRequest.Headers.Add("Accept-Encoding", "gzip");

            WebResponse authResponse = authRequest.GetResponse();
            // deserialize into an object
            using (authResponse)
            {
                using (var streamReader = new StreamReader(authResponse.GetResponseStream()))
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    var json = streamReader.ReadToEnd();
                    dynamic item = serializer.Deserialize<object>(json);
                    return item["access_token"];
                }
            }

            //using (var httpClient = new HttpClient())
            //{
            //    var request = new HttpRequestMessage(HttpMethod.Post, tokenUrl);
            //    var apiCredentials = Convert.ToBase64String(new UTF8Encoding().GetBytes(oAuthConsumerKey + ":" + oAuthConsumerSecret));
            //    request.Headers.Add("Authorization", "Basic " + apiCredentials);
            //    request.Content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

            //    HttpResponseMessage response = await httpClient.SendAsync(request).ConfigureAwait(false);


            //    string json = response.Content.ReadAsStringAsync().Result;
            //    var serializer = new JavaScriptSerializer();
            //    dynamic item = serializer.Deserialize<object>(json);
            //    return item["access_token"];
            //}
        }

        public bool ValidateConfigurationEntries()
        {
            bool configValid = true;
            var reader = new AppSettingsReader();

            if (reader.GetValue("OAuthConsumerSecret", typeof(string)) == null ||
                reader.GetValue("OAuthConsumerKey", typeof(string)) == null ||
                reader.GetValue("TokenURL", typeof(string)) == null ||
                reader.GetValue("APIURL", typeof(string)) ==null)
            {
                configValid = false;
            }
            return configValid;
        }
    }
}
