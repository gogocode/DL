using DL.Web.ViewModels.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DL.Web.Controllers.Test
{
    public class TestController : Controller
    {
        [HttpGet]
        public ActionResult ValidateFrontEnd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ValidateFrontEnd(TestValidateFrontEnd model)
        {
            TestValidateFrontEnd newMode = model;

            return View();
        }
    }
}