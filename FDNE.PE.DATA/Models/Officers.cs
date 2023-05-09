using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDNE.PE.DATA.Models
{
    public class Officers
    {
        public Officers()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Category { get; set; }

        public bool Discipline { get; set; }

        public Guid FederationOfficersId { get; set; }

        public FederationOfficers FederationOfficers { get; set; }
    }
}
