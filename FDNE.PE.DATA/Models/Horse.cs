using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDNE.PE.DATA.Models
{
    public class Horse
    {
        public Horse()
        {
            Id = Guid.NewGuid();
        }
        
        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        [Required]
        public byte Sex { get; set; }

        public bool IsActive { get; set; } = true;

        public bool BelongsToUs { get; set; } = false;

        public string UrlToImage { get; set; }

        public ICollection<HorseClub> HorseClubs { get; set; }

        public ICollection<Binomial> Binomials { get; set; }

        public ICollection<Result> Results { get; set; }
    }
}
