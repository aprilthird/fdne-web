using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDNE.PE.DATA.Models
{
    public class FederationGuidelines
    {
        public FederationGuidelines()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public string Body { get; set; }

        public string ImageUrl { get; set; }
    }
}
