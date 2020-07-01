using System.Web.Mvc;

namespace Web.Areas.WarehouseStaff
{
    public class WarehouseStaffAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "WarehouseStaff";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "WarehouseStaff_default",
                "WarehouseStaff/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                 new[] { "Web.Areas.WarehouseStaff.Controllers" }
            );
        }
    }
}