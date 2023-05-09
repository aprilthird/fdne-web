using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace FNDE.PE.WEB.PORTAL.ViewModels.AboutUsViewModels
{
    public class HistoryViewModel
    {
        public string ImageUrl { get; set; }

        public List<HistoryPerYear> InformationPerYear { get; set;}
    }

    public class HistoryPerYear
    {
        public int Year { get; set; }

        public string Body { get; set; }

    }
}