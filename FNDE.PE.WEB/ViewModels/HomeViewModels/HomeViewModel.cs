using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FNDE.PE.WEB.PORTAL.ViewModels.HomeViewModels
{
    public class HomeViewModel
    {
        public IList<DisciplineViewModel> Disciplines { get; set; }

        public IList<EventViewModel> Events { get; set; }
    }

    public class EventViewModel
    {
        public string Date { get; set; }

        public string Acronym { get; set; }

        public string Club { get; set; }
    }

    public class DisciplineViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}