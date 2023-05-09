using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using FDNE.PE.CORE.Helpers;

namespace FDNE.PE.WEB.ADMIN.Dtos
{
    public class DisciplineDto
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.REQUIRED)]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.REQUIRED)]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Imagen")]
        public string Image { get; set; }

        [Display(Name = "Imagen")]
        public string base64Image { get; set; }
    }
}