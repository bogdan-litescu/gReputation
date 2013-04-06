using gReputation.Helpers;
using gReputation.Models;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace gReputation.Controllers
{
    public class ReputationApiController : ApiController
    {
        public ReputationTotal Get(string appName, string objectId)
        //, eResponseFormat format = eResponseFormat.Json)
        {
            var tblTotal = AzureTable.Get("reputationtotals");
            TableOperation retrieveOperation = TableOperation.Retrieve<ReputationTotal>(appName, objectId);
            TableResult retrievedResult = tblTotal.Execute(retrieveOperation);

            return retrievedResult.Result as ReputationTotal;
        }

        public int Get(string appName, string objectId, string stat)
        //, eResponseFormat format = eResponseFormat.Json)
        {
            var tblTotal = AzureTable.Get("reputationtotals");
            TableOperation retrieveOperation = TableOperation.Retrieve<ReputationTotal>(appName, objectId);
            TableResult retrievedResult = tblTotal.Execute(retrieveOperation);

            if (retrievedResult.Result == null)
                return 0;

            switch (stat) {
                case "Quality":
                    return (retrievedResult.Result as ReputationTotal).TotalQuality;
                case "Quantity":
                    return (retrievedResult.Result as ReputationTotal).TotalQuantity;
                case "Trust":
                    return (retrievedResult.Result as ReputationTotal).TotalTrust;
            }

            return 0;
        }

        /// <summary>
        /// Let's just do the global reputation an average of local reputation 
        /// What would be best is to normalize the reputation based on other users in each app
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public object Get(string objectId)
        //, eResponseFormat format = eResponseFormat.Json)
        {
                var tblTotal = AzureTable.Get("reputationtotals");

                TableQuery<ReputationTotal> rangeQuery = new TableQuery<ReputationTotal>().Where(
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, objectId));

            // Loop through the results, displaying information about the entity.
            var entries = tblTotal.ExecuteQuery(rangeQuery);
            if (entries == null || entries.Count() == 0)
                return null;

            var toltalQuality = 0;
            var toltalQuantity = 0;
            var toltalTrust = 0;
            foreach (ReputationTotal entry in entries) {
                toltalQuality += entry.TotalQuality;
                toltalQuantity += entry.TotalQuantity;
                toltalTrust += entry.TotalTrust;
            }

            return new {
                TotalQuality = toltalQuality / entries.Count(),
                TotalQuantity = toltalQuantity / entries.Count(),
                TotalTrust = toltalTrust / entries.Count()
            };
        }

        //public int Get(string objectId, string stat)
        ////, eResponseFormat format = eResponseFormat.Json)
        //{
        //    var tblTotal = AzureTable.Get("reputationtotals");
        //    TableOperation retrieveOperation = TableOperation.Retrieve<ReputationTotal>(appName, objectId);
        //    TableResult retrievedResult = tblTotal.Execute(retrieveOperation);

        //    if (retrievedResult.Result == null)
        //        return 0;

        //    switch (stat) {
        //        case "Quality":
        //            return (retrievedResult.Result as ReputationTotal).TotalQuality;
        //        case "Quantity":
        //            return (retrievedResult.Result as ReputationTotal).TotalQuantity;
        //        case "Trust":
        //            return (retrievedResult.Result as ReputationTotal).TotalTrust;
        //    }

        //    return 0;
        //}

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