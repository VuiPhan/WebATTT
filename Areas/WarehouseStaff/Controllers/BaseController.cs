using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models.Data;

namespace Web.Areas.WarehouseStaff.Controllers
{
    public class BaseController : Controller
    {
        // GET: WarehouseStaff/Base
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = (Member)Session["NVLK"];
            if (session == null)
            {
                filterContext.Result = new RedirectToRouteResult(new
                    System.Web.Routing.RouteValueDictionary(new { controller = "LoginSystem", action = "Index", Area = ""}));
            }
            base.OnActionExecuting(filterContext);
        }
    }
}