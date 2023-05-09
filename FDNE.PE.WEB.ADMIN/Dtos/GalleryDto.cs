using FDNE.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FDNE.PE.WEB.ADMIN.Dtos
{
    public class GalleryDto
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.REQUIRED)]
        [Display(Name = "Título")]
        public string Title { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.REQUIRED)]
        [Display(Name = "Descripcion")]
        public string Description { get; set; }

        [Display(Name = "Imagen")]
        public string ImageUrl { get; set; }

        [Display(Name = "Imagen")]
        public string Base64Image { get; set; }

        public string Date { get; set; }
    }
}