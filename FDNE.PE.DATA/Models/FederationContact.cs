using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDNE.PE.DATA.Models
{
   public class FederationContact
    {
        public FederationContact()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Address { get; set; }
 
        public double Longitude { get; set; }
 
        public double Latitude { get; set; }

    }
}
