using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using GithubSharp.Core.Models;

namespace GithubSharp_Wrapper
{
    public class CommitsWrapper
    {
        public static IEnumerable<Commit> GetCommitsFromJSON(string json)
        {
            var serializer = new DataContractJsonSerializer(typeof(CommitListContainer));
            CommitListContainer commitsRequests = null;
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                commitsRequests = ((CommitListContainer)serializer.ReadObject(ms));
            }
            return commitsRequests.Commits;
        }

        public static IEnumerable<Commit> GetCommitsRequest(string Repo, string UserName, string UserPassword, string branch, int page)
        {
            HttpWebRequest request
                    =
                    WebRequest.Create(string.Format("http://github.com/api/v2/json/commits/list/mdsol/{0}/{1}?page={2}", Repo, branch, page)) as
                    HttpWebRequest;
            // Add authentication to request  
            string authInfo = UserName + ":" + UserPassword;
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            request.Headers["Authorization"] = "Basic " + authInfo;

            // Get response
            IEnumerable<Commit> requests = null;
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                // Get the response stream  
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string responseTxt = reader.ReadToEnd();

                requests = GetCommitsFromJSON(responseTxt);
            }
            return requests;
        }

    }

    [DataContract]
    internal class CommitListContainer
    {
        [DataMember(Name = "commits")]
        public IEnumerable<Commit> Commits { get; set; }
    }

    [DataContract]
    internal class SingleFileCommitContainer
    {
        [DataMember(Name = "commit")]
        public SingleFileCommit Commit { get; set; }
    }
}
