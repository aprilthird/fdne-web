using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDNE.PE.DATA.Models
{
    public class HistoryPerYear
    {
        public HistoryPerYear()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public int Year { get; set; }

        public string Body { get; set; }

        public Guid FederationHistoryId { get; set; }

        public FederationHistory FederationHistory { get; set; }
    }
}
