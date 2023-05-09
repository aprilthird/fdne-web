using FDNE.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FDNE.PE.WEB.ADMIN.Dtos
{
    public class HistoryPerYearDto
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.REQUIRED)]
        [Display(Name = "Año")]
        public int Year { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.REQUIRED)]
        [Display(Name = "Descripcion")]
        public string Body { get; set; }

        public Guid? FederationHistoryId { get; set; }

        public FederationHistoryDto FederationHistory { get; set; }
    }
}