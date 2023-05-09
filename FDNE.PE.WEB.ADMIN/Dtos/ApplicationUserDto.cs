using FDNE.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace FDNE.PE.WEB.ADMIN.Dtos
{
    public class ApplicationUserDto
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.REQUIRED)]
        [Display(Name = "Nombre de Cuenta")]
        public string UserName { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.REQUIRED)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&._-]).{6,}$", ErrorMessage = "La contraseña debe contener al menos un dígito, una mayúscula, minúscula, un caracter especial y ser de 6 caracteres de largo como mínimo.")]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.REQUIRED)]
        [DataType(DataType.EmailAddress, ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.NOT_VALID)]
        [Display(Name = "Correo Electrónico")]
        public string Email { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.REQUIRED)]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.REQUIRED)]
        [Display(Name = "Apellido Paterno")]
        public string PaternalSurname { get; set; }
        
        [Required(ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.REQUIRED)]
        [Display(Name = "Apellido Materno")]
        public string MaternalSurname { get; set; }

        public string FullName => $"{PaternalSurname} {MaternalSurname}, {Name}";

        [Required(ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.REQUIRED)]
        [Display(Name = "Tipo de Documento")]
        public Guid DocumentTypeId { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.REQUIRED)]
        [Display(Name = "Documento")]
        public string Document { get; set; }

        public DocumentTypeDto DocumentType { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.REQUIRED)]
        [Display(Name = "Telefono")]
        public string PhoneNumber { get; set; }
        
        [Required(ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.REQUIRED)]
        [Display(Name = "Sexo")]
        public byte Sex { get; set; }

        [Display(Name = "Activo")]
        public bool IsActive { get; set; }

        [Display(Name = "Fecha de Nacimiento")]
        public string BirthDate { get; set; }

        [Display(Name = "Imagen")]
        public string ImageUrl { get; set; }

        [Display(Name = "Imagen")]
        public string Base64Image { get; set; }
    }
}