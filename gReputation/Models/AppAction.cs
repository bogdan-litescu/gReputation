using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gReputation.Models
{
    public class AppAction
    {
        public AppAction(string appName, string userId)
        {
        }

        public string AppName { get; set; }
        public string UserId { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }
    }

}
