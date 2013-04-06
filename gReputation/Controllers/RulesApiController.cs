using gReputation.Helpers;
using gReputation.Models;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace gReputation.Controllers
{
    public class RulesApiController : ApiController
    {
        public IEnumerable<Rule> Get(string appName)
        {
            var tbl = AzureTable.Get("rules");

            // Create the table query.
            TableQuery<Rule> rangeQuery = new TableQuery<Rule>().Where(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, appName));

            return tbl.ExecuteQuery(rangeQuery);
        }

        public Rule Post(Rule rule)
        {
            string appName = HttpContext.Current.User.Identity.Name;
            string appKey = HttpContext.Current.Request.Params["appKey"];

            var tblApps = AzureTable.Get("apps");
            TableQuery<Rule> rangeQuery = new TableQuery<Rule>().Where(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, appName));

            var apps = tblApps.ExecuteQuery(rangeQuery);
            if (apps == null && apps.Count() == 0) {
                throw new UnauthorizedAccessException("Access Denied.");
            }

            var app = apps.First();

            var tbl = AzureTable.Get("rules");
            TableOperation op = null;

            if (!string.IsNullOrEmpty(rule.RowKey)) {
                TableOperation retrieveOperation = TableOperation.Retrieve<Rule>(appName, rule.RowKey);

                // Execute the retrieve operation.
                TableResult retrievedResult = tbl.Execute(retrieveOperation);

                // Print the phone number of the result.
                if (retrievedResult.Result != null) {
                    retrievedResult.Result.CopyFrom(rule);
                    op = TableOperation.Replace(rule);
                }
            }

            if (op == null) {
                rule.PartitionKey = appName;
                rule.RowKey = Guid.NewGuid().ToString().Replace("-", "");
                op = TableOperation.Insert(rule);
            }

            tbl.Execute(op);

            return rule;
        }

        public void Delete(string appName, string id)
        {
            var tbl = AzureTable.Get("rules");

            // Create a retrieve operation that expects a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<Rule>(appName, id);

            // Execute the operation.
            TableResult retrievedResult = tbl.Execute(retrieveOperation);

            // Assign the result to a CustomerEntity.
            Rule deleteEntity = (Rule)retrievedResult.Result;

            // Create the Delete TableOperation.
            if (deleteEntity != null) {
                TableOperation deleteOperation = TableOperation.Delete(deleteEntity);

                // Execute the operation.
                tbl.Execute(deleteOperation);
            }
        }
    }
}