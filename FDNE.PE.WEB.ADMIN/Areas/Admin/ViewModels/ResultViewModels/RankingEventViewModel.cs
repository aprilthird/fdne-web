using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FDNE.PE.WEB.ADMIN.Areas.Admin.ViewModels.ResultViewModels
{
    public class RankingEventViewModel
    {
        public string Discipline { get; set; }

        public int Season { get; set; }

        public string Category { get; set; }

        public string Level { get; set; }

        public Guid EventId { get; set; }

        public string EventName { get; set; }

        public bool IsDouble { get; set; }
    }
}