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
         <label id="repoLbl" for="Repo">Repository Name</label> <input type=text id="Repo" name="Repo"/>   
        </div>
        <div>
         <label id="userLbl" for="Repo">User Name</label> <input type=text id="UserName" name="UserName"/>   
        </div>
        <div>
         <label id="pwdLbl" for="Repo">Password</label> <input type=password id="UserPassword" name="UserPassword"/>   
        </div>
        <div>
         <label id="Label1" for="Repo">Start Pull Id</label> <input type=text id="StartId" name="StartId"/>   
        </div>
        <div>
         <label id="Label2" for="Repo">End Pull Id</label> <input type=text id="EndId" name="EndId"/>   
        </div>
        <input type="submit" id="GetPullRequests" />
    </form>
    <% if (ViewData["Response"] != null)
       {%>
       <table>
       <tr>
         <td>Commit Id</td>
         <td>Pull Id</td>
         <td>URL</td>
       </tr>
       <%  List<Tuple<string, int, string>> results = (List<Tuple<string, int, string>>)ViewData["Response"];
           foreach(Tuple<string, int, string> disc in results)
           {%>
               <tr>
                 <td><%= disc.Item1 %></td>
                 <td><%= disc.Item2 %></td>
                 <td><%= disc.Item3 %></td>
               </tr>
         <%}%>

       </table>
        <%}%>
</body>
</html>
