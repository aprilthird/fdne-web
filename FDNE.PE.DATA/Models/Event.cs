using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDNE.PE.DATA.Models
{
    public class Event
    {
        public Event()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        [ForeignKey("DisciplineSeason")]
        public Guid SeasonId { get; set; }

        [ForeignKey("DisciplineSeason")]
        public Guid DisciplineId { get; set; }

        public DisciplineSeason DisciplineSeason { get; set; }

        public Season Season { get; set; }

        public Discipline Discipline { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public Guid ClubId { get; set; }

        public Club Club { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Acronym { get; set; }
        
        public bool HasTwoDates { get; set; } = false;

        public bool DoublesScore { get; set; }

        public bool HasResults { get; set; } = false;

        public ICollection<Result> Results { get; set; }
    }
}
