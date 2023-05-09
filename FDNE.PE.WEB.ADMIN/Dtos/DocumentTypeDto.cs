using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FDNE.PE.WEB.ADMIN.Dtos
{
    public class DocumentTypeDto
    {
        [Display(Name = "Id")]
        public Guid Id { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; }
    }
}