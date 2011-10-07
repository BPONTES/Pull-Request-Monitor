<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="GithubSharp.Core.Models" %>

<!DOCTYPE html>

<html>
<head runat="server">
          <script type="text/javascript" src="/scripts/jquery-1.5.1.min.js"></script>
      <script type="text/javascript" src="/scripts/jquery-1.5.1.js"></script>
      <script type="text/javascript" src="/scripts/jquery.json-2.3.js"></script>
    <title>Index</title>
</head>
<body>
    <form method="post" action="/GitPullRequests/GetPullRequests">
        <div>
         <label id="repoLbl" for="Repo">Repository Name</label> <%= Html.TextBox("Repo")%> 
        </div>
        <div>
         <label id="Label1" for="Repo">Branch Name</label> <%= Html.TextBox("BranchName")%>
        </div>
        <div>
         <label id="userLbl" for="Repo">User Name</label> <%= Html.TextBox("UserName")%>   
        </div>
        <div>
         <label id="pwdLbl" for="Repo">Password</label> <input type=password id="UserPassword" name="UserPassword"/>   
        </div>
        <div>
         <label id="Label3" for="Repo">Filter Date</label> <%= Html.TextBox("FilterDate")%>   
        </div>
        <input type="submit" id="GetPullRequests" />
    </form>
    <% if (ViewData["Response"] != null)
       {%>
       <table cellspacing="0" cellpadding="5" width="100%" border="0" style="border-width:1px;width:100%;border-collapse:collapse;">
       <tr style="background-color:Gainsboro;">
         <td>Commit Id</td>
         <td>Pull Id</td>
         <td>Author</td>
         <td>Authored Date</td>
         <td>Message</td>
       </tr>
       <%  Dictionary<Commit, PullRequest> CommitHash = (Dictionary<Commit, PullRequest>)ViewData["Response"];
           foreach (var pullRequest in CommitHash)
           {%>
               <tr>
                 <td><a href="<%= pullRequest.Key.URL%>"><%= pullRequest.Key.Id%></a></td>
                 <td>
                    <%
                        string pullId = "None";
                        bool found = false;
                        if (pullRequest.Value != null)
                        {
                            pullId = pullRequest.Value.Number.ToString();
                            found = true;
                        }
                        else if (pullRequest.Key.Message.Contains("Merge pull request #"))
                        {
                            pullId = "Pull";
                            found = true;  
                        }%>
                   <span style="color:<%= found ? "Green" : "Red" %>;" ><%=pullId %></span>
                 </td>
                 <td><%= pullRequest.Key.Author.Name%></td>
                 <td><%= pullRequest.Key.AuthoredDate.ToShortDateString()%></td>
                 <td><%= pullRequest.Key.Message%></td>
               </tr>
         <%}%>

       </table>
        <%}%>
</body>
</html>
