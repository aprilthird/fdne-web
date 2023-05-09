using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDNE.PE.DATA.Models
{
    public class CoachingSystem
    {
        public CoachingSystem()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string FirstTab { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string FirstBody { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string SecondTab { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string SecondBody { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string ThirdTab { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string ThirdBody { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string QuarterTab { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string QuarterBody { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string FifthTab { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string FifthBody { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string SixthTab { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string SixthBody { get; set; }

        public string UrlImage { get; set; }
    }
}
