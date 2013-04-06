using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gReputation.Models
{
    public class App : TableEntity
    {
        public App(string name)
        {
            this.PartitionKey = name;
            this.RowKey = Guid.NewGuid().ToString().Replace("-", "");
        }

        public App()
        {
        }

        public string Email { get; set; }
    }
}
