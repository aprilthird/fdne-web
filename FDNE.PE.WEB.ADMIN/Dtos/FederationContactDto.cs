using FDNE.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FDNE.PE.WEB.ADMIN.Dtos
{
    public class FederationContactDto
    {
        [Required(ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.REQUIRED)]
        [Display(Name = "Dirección")]
        public string Address {get;set;}

        [Required(ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.REQUIRED)]
        [Display(Name = "Longitud")]
        public double Longitude {get;set;}

        [Required(ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.REQUIRED)]
        [Display(Name = "Latitud")]
        public double Latitude {get;set;}
    }
}