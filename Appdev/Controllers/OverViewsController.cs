using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Appdev.Controllers
{
    [Authorize(Roles = "Staff")]
    public class OverViewsController : Controller
    {
        // GET: OverViews
        public ActionResult Index()
        {
            return View();
        }
    }
}