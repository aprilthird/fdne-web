using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDNE.PE.DATA.Models
{
    public class Level
    {
        public Level()
        {
            Id = Guid.NewGuid();
        }
        
        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        public Guid DisciplineId { get; set; }

        public Discipline Discipline { get; set; }
    }
}
