using FDNE.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FDNE.PE.WEB.ADMIN.Dtos
{
    public class ClubDto
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.REQUIRED)]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.REQUIRED)]
        [Display(Name = "Acrónimo")]
        public string Acronym { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.REQUIRED)]
        [Display(Name = "Dirección")]
        public string Address { get; set; }

        [Display(Name = "Latitud")]
        public double Latitude { get; set; }

        [Display(Name = "Longitud")]
        public double Longitude { get; set; }

        [Display(Name = "Teléfono")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.NOT_VALID)]
        [Display(Name = "Correo Electrónico")]
        public string Email { get; set; }

        [Display(Name = "Imagen")]
        public string UrlPicture { get; set; }
        [Display(Name = "Imagen")]
        public string Base64Image { get; set; }

    }
}