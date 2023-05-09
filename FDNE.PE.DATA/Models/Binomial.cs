using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDNE.PE.DATA.Models
{
    public class Binomial
    {
        [Key, Column(Order = 1)]
        public Guid RankingId { get; set; }

        public Ranking Ranking { get; set; }

        [Key, Column(Order = 2)]
        public Guid ClubId { get; set; }

        public Club Club { get; set; }

        [Key, Column(Order = 3)]
        public string RiderId { get; set; }

        public Rider Rider { get; set; }

        [Key, Column(Order = 4)]
        public Guid HorseId { get; set; }
        
        public Horse Horse { get; set; }

        public HorseClub HorseClub { get; set; }

        public RiderClub RiderClub { get; set; }

        public ICollection<Result> Results { get; set; }
    }
}
