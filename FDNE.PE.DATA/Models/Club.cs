using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDNE.PE.DATA.Models
{
    public class Club
    {
        public Club()
        {
            Id = Guid.NewGuid();
        }
        
        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Acronym { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Address { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string UrlPicture { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<RiderClub> RiderClubs { get; set; }

        public ICollection<HorseClub> HorseClubs { get; set; }

        public ICollection<Binomial> Binomials { get; set; }

        public ICollection<Result> Results { get; set; }
    }
}
