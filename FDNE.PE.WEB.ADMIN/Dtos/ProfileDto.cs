using FDNE.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FDNE.PE.WEB.ADMIN.Dtos
{
    public class ProfileDto
    {
        public Guid? Id { get; set;}

        [Required(ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.REQUIRED)]
        [Display(Name = "Nombre:")]
        public string Name { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.REQUIRED)]
        [Display(Name = "Apellido Paterno:")]
        public string PaternalSurname { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.REQUIRED)]
        [Display(Name = "Apellido Materno:")]
        public string MaternalSurname { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.REQUIRED)]
        [Display(Name = "Tipo de Documento:")]
        public Guid DocumentTypeId { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.REQUIRED)]
        [Display(Name = "Documento:")]
        public string Document { get; set; }

        public DocumentTypeDto DocumentType { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.REQUIRED)]
        [Display(Name = "Email:")]
        [EmailAddress(ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.EMAIL_ADDRESS)]
        public string Email { get; set; }

        [Display(Name = "Imagen")]
        public string ImageUrl { get; set; }

        [Display(Name = "Imagen:")]
        public string Base64Image { get; set; }

    }
}