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
        public string Get(string appName, string subjectId, string action)
            //, eResponseFormat format = eResponseFormat.Json)
        {
            //var tbl = AzureTable.Get("reputation");
            //TableOperation insertOperation = TableOperation.Insert(new ReputationEntry(appName) {
            //    Subject = subjectId,
            //    Stat = "loyalty",
            //    Object = objectId
            //});
            //tbl.Execute(insertOperation);

            return "value";
        }

        public string Post(string appName, string subjectId, string verb, string objectId, [FromBody]string description)
        {
            var tbl = AzureTable.Get("reputation");
            TableOperation insertOperation = TableOperation.Insert(new ReputationEntry(appName) {
                Object = subjectId,
                Stat = "loyalty", // TODO: take this from some action => modifiers rules
                Description = description ?? "",
                // Object = objectId,
                Modifier = 1, // TODO: take this from some action => modifiers rules
                Total = 1 // TODO: total = prev total +/- modifier
            });
            tbl.Execute(insertOperation);

            return "";
        }

        //public string Get(string user, string appname, string stat, string format)
        ////, eResponseFormat format = eResponseFormat.Json)
        //{
        //    return "value";
        //}

        //// GET api/values
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/values/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/values
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //public void Delete(int id)
        //{
        //}
    }
}