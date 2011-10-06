using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using GithubSharp.Core.API;
using System.Runtime.Serialization.Json;
using GithubSharp.Core.Models;
using GithubSharp.Core.Models.Internal;
using PullRequest = GithubSharp.Core.Models.PullRequest;

namespace Pull_Request_Log.Controllers
{
    public class GitPullRequestsController : Controller
    {
        //
        // GET: /GitPullRequests/

        public ActionResult Index()
        {
            //var user = new User(new BasicCacher.BasicCacher(), new SimpleLogProvider());
            //var u = user.Get("rumpl");
            //Console.WriteLine(u.Blog);
            //u = user.Get("rumpl");
            //Console.WriteLine(u.Blog);

            //user.Authenticate(new GithubSharp.Core.Models.GithubUser { Name = "erikzaadi", APIToken = "851c28811a421ea23ba14a9b32f36631" });
            //try
            //{
            //    var privateuser = user.Get();
            //    if (privateuser == null)
            //        throw new Exception("Invalid user");
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //}


            //var issuesAPI = new Issues(new BasicCacher.BasicCacher(), new SimpleLogProvider());

            //var closedIssues = issuesAPI.List("GithubSharp", "erikzaadi", GithubSharp.Core.Models.IssueState.Closed);
            return View();
        }

        //
        // GET: /GitPullRequests/Details/5

        public ActionResult GetPullRequests(string Repo, string UserName, string UserPassword, int StartId, int EndId)
        {
            bool fetchNextBatch = true;
            List<PullRequest> totalPulls = new List<PullRequest>();
            int pullId = StartId;
            // Create the web request
            while (fetchNextBatch)
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
                    var serializer = new DataContractJsonSerializer(typeof(PullRequestContainer));
                    PullRequest pullRequests = null;
                    using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(responseTxt)))
                    {
                        pullRequests = ((PullRequestContainer)serializer.ReadObject(ms)).PullRequest;
                    }
                    requests = pullRequests;
                }
                totalPulls.Add(requests);
                fetchNextBatch = pullId < EndId;
                pullId++;
                Thread.Sleep(900); 
            }
            var commits = (from pull in totalPulls
                           from discussionEntry in pull.Discussion
                           where discussionEntry.Type == "Commit"
                           select new Tuple<string, int, string>(discussionEntry.Id, pull.Number, pull.DiffUrl)).ToList();

            ViewData["Response"] = commits.ToList();

            return View("Index");
        }
    }
}
