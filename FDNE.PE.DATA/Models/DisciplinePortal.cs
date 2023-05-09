using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDNE.PE.DATA.Models
{
    public class DisciplinePortal
    {
        public DisciplinePortal()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }

        public Guid DisciplineId { get; set; }

        public Discipline Discipline { get; set; }

        public string FirstTab { get; set; }

        public string FirstBody { get; set; }

        public string SecondTab { get; set; }

        public string SecondBody { get; set; }

        public string ThirdTab { get; set; }

        public string ThirdBody { get; set; }

        public string QuarterTab { get; set; }

        public string QuarterBody { get; set; }

        public string FifthTab { get; set; }

        public string FifthBody { get; set; }

        public string SixthTab { get; set; }

        public string SixthBody { get; set; }

        public string UrlImage { get; set; }
    }
}
