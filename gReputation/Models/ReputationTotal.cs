using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gReputation.Models
{
    public class ReputationTotal : TableEntity
    {
        public ReputationTotal(string appName, string objectId)
        {
            this.PartitionKey = appName;
            this.RowKey = objectId;
            TotalQuality = 0;
            TotalQuantity = 0;
            TotalTrust = 0;
        }

        public ReputationTotal() 
        {
            TotalQuality = 0;
            TotalQuantity = 0;
            TotalTrust = 0;
        }

        public int TotalQuality { get; set; }
        public int TotalQuantity { get; set; }
        public int TotalTrust { get; set; }
    }

}
