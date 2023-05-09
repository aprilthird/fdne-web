using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FNDE.PE.WEB.PORTAL.ViewModels.NewsViewModels
{
    public class NewsViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
        
        public string Day { get; set; }

        public string Month { get; set; }

        public string Year { get; set; }
    }
}