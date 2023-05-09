using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FNDE.PE.WEB.PORTAL.ViewModels.EventViewModels
{
    public class EventoViewModel
    {
        public Guid Id { get; set; }
          
        public string Name { get; set; }
        
        public string Acronym { get; set; }

        public string StartDate { get; set; }
        
        public string EndDate { get; set; }

        public string StartDay { get; set; }

        public string EndDay { get; set; }
        
        public string Day { get; set; }

        public string Month { get; set; }

        public string Year { get; set; }
        
        public string Club { get; set; }
        
        public string LoadMap { get; set; }

    }
}