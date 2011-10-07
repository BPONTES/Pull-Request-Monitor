using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Runtime.Serialization;
using GithubSharp.Core.Models;

namespace GithubSharp_Wrapper
{
    public class PullRequestWrapper
    {
        public static PullRequest GetPullRequestFromJSON(string json)
        {
            var serializer = new DataContractJsonSerializer(typeof(PullRequestContainer));
            PullRequest pullRequests = null;
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                pullRequests = ((PullRequestContainer)serializer.ReadObject(ms)).PullRequest;
            }
            return pullRequests;
        }

        public static PullRequest GetPullRequest(string Repo, string UserName, string UserPassword, int pullId)
        {
            HttpWebRequest request
                    =
                    WebRequest.Create(string.Format("https://github.com/api/v2/json/pulls/mdsol/{0}/{1}", Repo, pullId)) as
                    HttpWebRequest;

            // Add authentication to request  
            string authInfo = UserName + ":" + UserPassword;
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            request.Headers["Authorization"] = "Basic " + authInfo;

            // Get response
            PullRequest requests = null;
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                // Get the response stream  
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string responseTxt = reader.ReadToEnd();

                requests = GetPullRequestFromJSON(responseTxt);
            }
            return requests;
        }

    }

    public class PullRequestsWrapper
    {
        public static IEnumerable<PullRequest> GetPullRequestsFromJSON(string json)
        {
            var serializer = new DataContractJsonSerializer(typeof(PullRequestCollection));
            PullRequestCollection pullRequests = null;
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                pullRequests = ((PullRequestCollection)serializer.ReadObject(ms));
            }
            return pullRequests.PullRequests;
        }

        public static IEnumerable<PullRequest> GetPullRequests(string Repo, string UserName, string UserPassword, int page)
        {
            HttpWebRequest request
                    =
                    WebRequest.Create(string.Format("https://github.com/api/v2/json/pulls/mdsol/{0}/closed?Page={1}", Repo,page)) as
                    HttpWebRequest;

            // Add authentication to request  
            string authInfo = UserName + ":" + UserPassword;
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            request.Headers["Authorization"] = "Basic " + authInfo;

            // Get response
            IEnumerable<PullRequest> requests = null;
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                // Get the response stream  
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string responseTxt = reader.ReadToEnd();

                requests = GetPullRequestsFromJSON(responseTxt);
            }
            return requests;
        }

    }

    [DataContract]
    public class PullRequestCollection
    {
        [DataMember(Name = "pulls")]
        public IEnumerable<PullRequest> PullRequests { get; set; }
    }

    [DataContract]
    public class PullRequestContainer
    {
        [DataMember(Name = "pull")]
        public PullRequest PullRequest { get; set; }
    }
}
