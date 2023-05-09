using FDNE.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FDNE.PE.WEB.ADMIN.Dtos
{
    public class FeiFilesDto
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.REQUIRED)]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.REQUIRED)]
        [Display(Name = "Extension")]
        public string Extension { get; set; }

        [Display(Name = "Archivo")]
        public string FileUrl { get; set; }

        [Display(Name = "Archivo")]
        public string Base64File { get; set; }

        public Guid? FederationFEIDtoId { get; set; }

        public FederationFEIDto FederationFEIDto { get; set; }


    }
}