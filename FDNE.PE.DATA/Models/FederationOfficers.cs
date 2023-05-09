using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDNE.PE.DATA.Models
{
    public class FederationOfficers
    {
        public FederationOfficers()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public string UrlImage { get; set; }
    }
}
