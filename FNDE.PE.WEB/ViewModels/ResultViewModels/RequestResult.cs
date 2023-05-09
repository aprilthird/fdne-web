using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FNDE.PE.WEB.PORTAL.ViewModels.ResultViewModels
{
    public class RequestResult
    {
        public Guid DisciplineId { get; set; }

        public Guid SessonId { get; set; }

        public Guid? CategoryId { get; set; }

        public Guid? LevelId { get; set; }

        public string EventDate { get; set; }
    }
}