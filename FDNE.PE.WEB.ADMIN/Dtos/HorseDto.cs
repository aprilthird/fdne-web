﻿using FDNE.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FDNE.PE.WEB.ADMIN.Dtos
{
    public class HorseDto
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.REQUIRED)]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Clubes")]
        public IEnumerable<Guid> ClubIds { get; set; }

        public IEnumerable<ClubDto> Clubs { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.REQUIRED)]
        [Display(Name = "Sexo")]
        public byte Sex { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.REQUIRED)]
        [Display(Name = "Activo")]
        public bool IsActive { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.REQUIRED)]
        [Display(Name = "Nos pertenece?")]
        public bool BelongsToUs { get; set; }

        [Display(Name = "Imagen")]
        public string UrlToImage { get; set; }

        [Display(Name = "Imagen")]
        public string Base64Image { get; set; }
    }
}