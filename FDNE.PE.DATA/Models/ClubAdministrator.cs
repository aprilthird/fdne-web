using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDNE.PE.DATA.Models
{
    public class ClubAdministrator
    {
        [Key]
        [ForeignKey("User")]
        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public Guid ClubId { get; set; }

        public Club Club { get; set; }
    }
}
