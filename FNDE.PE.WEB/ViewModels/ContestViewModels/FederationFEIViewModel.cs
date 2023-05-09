using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FNDE.PE.WEB.PORTAL.ViewModels.ContestViewModels
{
    public class FederationFEIViewModel
    {
        public string Title { get; set; }

        public string SubTitle { get; set; }

        public string Body { get; set; }

        public string ImageUrl { get; set; }

        public string FileUrl { get; set; }

        public List<FeiFilesViewModel> FeiFiles { get; set; }
    }

    public class FeiFilesViewModel
    {
        public string Name { get; set; }

        public string UrlFile { get; set; }
    }
}