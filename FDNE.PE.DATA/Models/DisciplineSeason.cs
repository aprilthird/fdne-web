using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDNE.PE.DATA.Models
{
    public class DisciplineSeason
    {
        public string Regulation { get; set; }

        [Key, Column(Order = 1)]
        public Guid SeasonId { get; set; }

        public Season Season { get; set; }

        [Key, Column(Order = 2)]
        public Guid DisciplineId { get; set; }

        public Discipline Discipline { get; set; }

        public ICollection<Ranking> Tournaments { get; set; }

        public ICollection<Event> Events { get; set; }
    }
}
