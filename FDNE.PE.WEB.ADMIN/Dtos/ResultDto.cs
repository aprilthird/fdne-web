using FDNE.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FDNE.PE.WEB.ADMIN.Dtos
{
    public class ResultDto
    {
        [Required(ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.REQUIRED)]
        [Display(Name = "Jinete")]
        public string RiderId { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.REQUIRED)]
        [Display(Name = "Caballo")]
        public Guid HorseId { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.REQUIRED)]
        [Display(Name = "Club")]
        public Guid ClubId { get; set; }

        public RiderDto Rider { get; set; }

        public HorseDto Horse { get; set; }

        public ClubDto Club { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.REQUIRED)]
        [Display(Name = "Puntaje")]
        public double Score { get; set; }

        [Display(Name = "Porcentaje")]
        public double? Percent { get; set; }
    }
}