using System.Web;
using System.Web.Mvc;

namespace COP4331_RestaurantSystem_WebAPI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
