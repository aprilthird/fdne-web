using FDNE.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FDNE.PE.WEB.ADMIN.Dtos
{
    public class SeasonDto
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.REQUIRED)]
        [Display(Name = "Año")]
        public int Year { get; set; }

        public string Name => $"Temporada {Year}";

        [Required(ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.REQUIRED)]
        [Display(Name = "Inicia")]
        public string StartDate { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.REQUIRED)]
        [Display(Name = "Finaliza")]
        public string EndDate { get; set; }

        [Display(Name = "¿Está Activo?")]
        public bool IsActive { get; set; }

        [Display(Name = "¿Ha acabado?")]
        public bool IsFinished { get; set; }
    }
}