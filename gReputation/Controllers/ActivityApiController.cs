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
    public class ActivityApiController : ApiController
    {
        public string Get(string appName, string objectId, string actionName)
        //, eResponseFormat format = eResponseFormat.Json)
        {
            HttpContext.Current.Response.Cache.SetNoStore();

            return "value";
        }

        public string Post(string appName, string subjectId, string actionName, string objectId, [FromBody]string description)
        {
            // iterate all rules for this action
            var rules = GetAllRules(appName, actionName);
            if (rules == null || rules.Count() == 0)
                return "";

            foreach (var rule in rules) {
                var receiver = rule.Receiver == "Subject" ? subjectId : objectId;
                ApplyReputationChange(appName, receiver, "Quality", rule.ModQuality, description);
                ApplyReputationChange(appName, receiver, "Quantity", rule.ModQuantity, description);
                ApplyReputationChange(appName, receiver, "Trust", rule.ModTrust, description);
            }

            return "";
        }

        void ApplyReputationChange(string appName, string objectId, string stat, string modifier, string description)
        {
            int modVal;
            if (!int.TryParse(modifier.Trim('+'), out modVal) || modVal == 0)
                return;

            // get previous total for this stat
            var tblTotal = AzureTable.Get("reputationtotals");
            TableOperation retrieveOperation = TableOperation.Retrieve<ReputationTotal>(appName, objectId);
            TableResult retrievedResult = tblTotal.Execute(retrieveOperation);

            // Print the phone number of the result.
            if (retrievedResult.Result != null) {
                AddStat((retrievedResult.Result as ReputationTotal), stat, modVal);
                tblTotal.Execute(TableOperation.Replace(retrievedResult.Result as ReputationTotal));
            } else {
                var total = new ReputationTotal(appName, objectId);
                AddStat(total, stat, modVal);
                tblTotal.Execute(TableOperation.Insert(total));
            }

            var tblChanges = AzureTable.Get("reputationchanges");
            TableOperation insertOperation = TableOperation.Insert(new ReputationEntry(appName) {
                Object = objectId,
                Stat = stat,
                Description = description ?? "",
                Modifier = modVal,
            });
            tblChanges.Execute(insertOperation);
        }

        void AddStat(ReputationTotal total, string stat, int val)
        {
            switch (stat) {
                case "Quality":
                    total.TotalQuality += val;
                    break;
                case "Quantity":
                    total.TotalQuantity += val;
                    break;
                case "Trust":
                    total.TotalTrust += val;
                    break;
            }
        }

        IEnumerable<Rule> GetAllRules(string appName, string actionName)
        {
            var tbl = AzureTable.Get("rules");

            // Create the table query.
            TableQuery<Rule> rangeQuery = new TableQuery<Rule>().Where(
                TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, appName),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("Action", QueryComparisons.Equal, actionName)));

            return tbl.ExecuteQuery(rangeQuery);
        }

    }
}