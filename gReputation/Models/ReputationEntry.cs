using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gReputation.Models
{
    //public enum eResponseFormat
    //{
    //    Json,
    //    Xml
    //}

    public enum eReputationType
    {
        Trust, // good and bad behavior
        Quality, // ratings by other users
        Quantity // amount of content generated
    }

    public class ReputationEntry : TableEntity
    {
        public ReputationEntry(string appName)
        {
            this.PartitionKey = appName;
            this.RowKey = Guid.NewGuid().ToString().Replace('-', ' ');
        }

        public ReputationEntry() 
        {
        }

        public string Object { get; set; }
        public string Stat { get; set; }
        public string Description { get; set; }
        public int Modifier { get; set; }
        public int Total { get; set; }
    }

}
