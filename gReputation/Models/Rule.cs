using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gReputation.Models
{
    public class Modifier
    {
        public eReputationType Type { get; set; }
        public int Amount { get; set; }
    }

    public enum eReputationReceiver
    { 
        Subject,
        Object
        // TODO: Subject of another action over the same object;
        // for example, auther of the article a user has just voted
    }

    public class Rule : TableEntity
    {
        public Rule(string appName)
        {
            this.PartitionKey = appName;
            this.RowKey = Guid.NewGuid().ToString().Replace("-", "");
        }

        public Rule()
        {
        }

        public string Action { get; set; }
        public string Receiver { get; set; }

        public string ModQuality { get; set; }
        public string ModQuantity { get; set; }
        public string ModTrust { get; set; }

        /// <summary>
        /// when object1 does action on object2
        /// </summary>
        /// <param name="object1"></param>
        /// <param name="action"></param>
        /// <param name="object2"></param>
        /// <returns></returns>
        public bool CheckAndExecute(string object1, string action, string object2)
        {
            throw new NotImplementedException();
        }
    }
}
