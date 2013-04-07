using gReputation.Helpers;
using gReputation.Models;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gReputation.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(string appName, string email)
        {
            var tbl = AzureTable.Get("apps");

            TableQuery<Rule> rangeQuery = new TableQuery<Rule>().Where(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, appName));

            var apps = tbl.ExecuteQuery(rangeQuery);
            if (apps != null && apps.Count() > 0) {
                ViewBag.Error = "App already exists";
                return View("Index");
            }

            // generate key and insert new app
            var app = new App(appName) {
                Email = email
            };

            // Create the TableOperation that inserts the customer entity.
            TableOperation insertOperation = TableOperation.Insert(app);

            // Execute the insert operation.
            tbl.Execute(insertOperation);
            System.Web.Security.FormsAuthentication.SetAuthCookie(app.PartitionKey, true);
            Session["appKey"] = app.RowKey;

            return View("RegisterSuccess", app);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string appName, string email)
        {
            var tbl = AzureTable.Get("apps");

            TableQuery<Rule> rangeQuery = new TableQuery<Rule>().Where(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, appName));

            var apps = tbl.ExecuteQuery(rangeQuery);
            if (apps == null && apps.Count() == 0) {
                ViewBag.Error = "Invalid app";
                return View("Index");
            }

            var app = apps.FirstOrDefault();
            System.Web.Security.FormsAuthentication.SetAuthCookie(app.PartitionKey, true);
            Session["appKey"] = app.RowKey;

            return Redirect("/admin");
        }
    }
}
