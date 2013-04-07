using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gReputation.Controllers
{
    public class AdminController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Logout()
        {
            System.Web.Security.FormsAuthentication.SignOut();
            return Redirect("/");
        }
    }
}
