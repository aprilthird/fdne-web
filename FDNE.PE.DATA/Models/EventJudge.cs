using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDNE.PE.DATA.Models
{
    public class EventJudge
    {
        [Key, Column(Order = 1)]
        public Guid EventId { get; set; }

        public Event Event { get; set; }

        [Key, Column(Order = 2)]
        [Required(AllowEmptyStrings = false)]
        public string JudgeId { get; set; }
        
        public ApplicationUser Judge { get; set; }
    }
}
