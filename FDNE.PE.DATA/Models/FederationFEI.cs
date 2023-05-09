using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDNE.PE.DATA.Models
{
    public class FederationFEI
    {
        public FederationFEI()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string SubTitle { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Body { get; set; }

        public string ImageUrl { get; set; }

        public string FileUrl { get; set; }
    }
}
