using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDNE.PE.DATA.Models
{
    public class FEIFile
    {
        public FEIFile()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Nanme { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string FileUrl { get; set; }

        public Guid FederationFEIId { get; set; }

        public FederationFEI FederationFEI { get; set; }
    }
}
