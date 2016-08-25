using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DL.Web.ActionFilter
{
    public class CheckSessionAcitionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["Id"] == null || filterContext.HttpContext.Session["Account"] == null)
            {
                filterContext.Result = new RedirectResult("~/Account/Login");
                return;
            }
        }

        //public override void OnActionExecuted(ActionExecutedContext filterContext)
        //{

        //}

        //public override void OnResultExecuting(ResultExecutingContext filterContext)
        //{

        //}

        //public override void OnResultExecuted(ResultExecutedContext filterContext)
        //{

        //}
    }
}