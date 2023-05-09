using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FNDE.PE.WEB.PORTAL.ViewModels.AboutUsViewModels
{
    public class OfficersViewModel
    {
        public string ImageUrl { get; set; }

        public IEnumerable<ChildOfficersViewModel> LstOfficers { get; set; }
    }

    public class ChildOfficersViewModel
    {
        public string Name { get; set; }

        public string CategoryName { get; set; }

        public string DisciplineName{ get; set; }
    }
}