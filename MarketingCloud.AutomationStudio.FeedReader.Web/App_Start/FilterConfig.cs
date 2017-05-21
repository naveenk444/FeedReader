using System.Web;
using System.Web.Mvc;

namespace MarketingCloud.AutomationStudio.FeedReader.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
