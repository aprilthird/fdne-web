using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDNE.PE.DATA.Models
{
    public class Result
    {
        [Key, Column(Order = 1)]
        public Guid EventId { get; set; }

        public Event Event { get; set; }

        [Key, Column(Order = 2)]
        [ForeignKey("Binomial")]
        public Guid RankingId { get; set; }

        public Ranking Ranking { get; set; }

        [Key, Column(Order = 3)]
        [ForeignKey("Binomial")]
        public Guid ClubId { get; set; }

        [Key, Column(Order = 4)]
        [ForeignKey("Binomial")]
        public string RiderId { get; set; }

        public Rider Rider { get; set; }

        [Key, Column(Order = 5)]
        [ForeignKey("Binomial")]
        public Guid HorseId { get; set; }

        [Key, Column(Order = 6)]
        public DateTime Date { get; set; }

        public Horse Horse { get; set; }

        public Binomial Binomial { get; set; }

        public Club Club { get; set; }

        public double Score { get; set; }

        public double? Percent { get; set; }

        public bool Plays { get; set; } = false;
    }
}
