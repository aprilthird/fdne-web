using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDNE.PE.DATA.Models
{
    public class Season
    {
        public Season()
        {
            Id = Guid.NewGuid();
        }
        
        public Guid Id { get; set; }

        public int Year { get; set; }

        public string Name => $"Temporada {Year}";

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsActive => StartDate <= DateTime.UtcNow && EndDate > DateTime.UtcNow;

        public bool IsFinished => EndDate <= DateTime.UtcNow;
    }
}
