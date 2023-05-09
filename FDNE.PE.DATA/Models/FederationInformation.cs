using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDNE.PE.DATA.Models
{
    public class FederationInformation
    {
        public FederationInformation()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Mision { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Vision { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string AboutUs { get; set; }
    }
}
