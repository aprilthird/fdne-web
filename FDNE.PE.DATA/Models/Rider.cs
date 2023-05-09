using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDNE.PE.DATA.Models
{
    public class Rider
    {
        [Key]
        [ForeignKey("User")]
        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<Binomial> Binomials { get; set; }

        public ICollection<Result> Results { get; set; }

        public ICollection<RiderClub> RiderClubs { get; set; }
    }
}
