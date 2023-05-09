using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDNE.PE.DATA.Models
{
    public class FederationLesson
    {
        public FederationLesson()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
