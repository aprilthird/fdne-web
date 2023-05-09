using System.Web;
using System.Web.Mvc;

namespace FDNE.PE.WEB.UIKIT
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
