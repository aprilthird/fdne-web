using FDNE.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FDNE.PE.WEB.ADMIN.Dtos
{
    public class RiderDto
    {
        public string Id { get; set; }

        public ApplicationUserDto User { get; set; }

        [Display(Name = "Clubes")]
        public IEnumerable<Guid> ClubIds { get; set; }
        
        public IEnumerable<ClubDto> Clubs { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.REQUIRED)]
        [Display(Name = "Activo")]
        public bool IsActive { get; set; }

        [Display(Name = "Imagen")]
        public string ImageUrl { get; set; }

        [Display(Name = "Imagen")]
        public string Base64Image { get; set; }
    }
}