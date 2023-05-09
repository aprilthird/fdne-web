using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace FDNE.PE.DATA.Models
{
    public class Ranking
    {
        public Ranking()
        {
            Id = Guid.NewGuid();
        }
        
        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        [ForeignKey("DisciplineSeason")]
        public Guid SeasonId { get; set; }

        public Season Season { get; set; }

        [ForeignKey("DisciplineSeason")]
        public Guid DisciplineId { get; set; }
        
        public Discipline Discipline { get; set; }

        public DisciplineSeason DisciplineSeason { get; set; }

        public Category Category { get; set; }

        public Guid? CategoryId { get; set; }

        public Guid? LevelId { get; set; }

        public Level Level { get; set; }

        public ICollection<Binomial> Binomials { get; set; }
    }
}
