using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using VolgaAndroid.Models;

namespace VolgaAndroid.Helper
{
    public class GitHubApiMethod
    {
        public async Task<List<GitRepository>> GetRepositories()
        {

            List<GitRepository> repositories = new List<GitRepository>();
            string jsonRepositories = "";

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(@"https://Altarian0:ahmadag001@api.github.com/repositories");
            webRequest.Method = "GET";
            HttpWebResponse webResponse = (HttpWebResponse)await webRequest.GetResponseAsync();
            using (Stream stream = webResponse.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    jsonRepositories = reader.ReadToEnd();
                }
            }

            webResponse.Close();
            repositories = JsonConvert.DeserializeObject<List<GitRepository>>(jsonRepositories);
            return repositories;
        }

        public async Task<string> GetPostman()
        {
            string jsonResponse = "";

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(@"https://postman-echo.com/get?foo1=bar1&foo2=bar2");
            webRequest.Method = "GET";
            HttpWebResponse webResponse = (HttpWebResponse)await webRequest.GetResponseAsync();
            using (Stream stream = webResponse.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    jsonResponse = reader.ReadToEnd();
                }
            }

            webResponse.Close();
            return jsonResponse;
        }

    }
}