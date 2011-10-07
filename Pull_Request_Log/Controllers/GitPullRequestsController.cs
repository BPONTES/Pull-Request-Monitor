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
using GithubSharp_Wrapper;
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

        public ActionResult GetPullRequests(string Repo, string UserName, string UserPassword, DateTime FilterDate, string BranchName = "develop")
        {
            bool fetchNextBatch = true;
            List<PullRequest> totalPulls = new List<PullRequest>();

            List<Commit> totalCommits = new List<Commit>();
            int page = 1;
            while (fetchNextBatch)
            {
                var totalCommitsFetched = CommitsWrapper.GetCommitsRequest(Repo, UserName, UserPassword, BranchName, page);
                var tempList = totalCommitsFetched.Where(totalCommit => totalCommit.AuthoredDate > FilterDate || totalCommit.CommittedDate > FilterDate).ToList();
                if (tempList.Count <= 5) fetchNextBatch = false;
                totalCommits.AddRange(tempList);
                page++;
            }
            Thread.Sleep(20000);
            fetchNextBatch = true;

            List<PullRequest> totalPullsValid = new List<PullRequest>();
            page = 1;
            while (fetchNextBatch)
            {
                var totalPullsFetched = PullRequestsWrapper.GetPullRequests(Repo, UserName, UserPassword,page);
                var temptotalPullsValid = totalPullsFetched.Where(pull => pull.Created > FilterDate).ToList();
                if (temptotalPullsValid.Count <= totalPullsFetched.Count()) fetchNextBatch = false;
                totalPullsValid.AddRange(temptotalPullsValid);
                page++;
            }

            // Create the web request
            foreach (var pullRequest in totalPullsValid)
            {
                totalPulls.Add(PullRequestWrapper.GetPullRequest(Repo, UserName, UserPassword, pullRequest.Number));
                Thread.Sleep(900); 
            }

            Dictionary<Commit,PullRequest> CommitHash = new Dictionary<Commit, PullRequest>();
            foreach (var totalCommit in totalCommits)
            {
                string hash = totalCommit.Id;
                var matchingPull = totalPulls.FirstOrDefault(p => p.Discussion.Any(d => d.Type == "Commit" && d.Id == hash));
                CommitHash.Add(totalCommit, matchingPull);
            }
            ViewData["Response"] = CommitHash;

            return View("Index");
        }

        
    }
}
