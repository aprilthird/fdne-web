﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDNE.PE.DATA.Models
{
    public class HorseClub
    {
        [Key, Column(Order = 1)]
        public Guid ClubId { get; set; }

        public Club Club { get; set; }

        [Key, Column(Order = 2)]
        public Guid HorseId { get; set; }

        public Horse Horse { get; set; }

        public ICollection<Binomial> Binomials { get; set; }
    }
}
