using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FDNE.PE.DATA.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string PaternalSurname { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string MaternalSurname { get; set; }

        public DocumentType DocumentType { get; set; }

        public Guid DocumentTypeId { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Document { get; set; }

        [NotMapped]
        public string FullName => $"{PaternalSurname} {MaternalSurname}, {Name}";
        
        [Required]
        public byte Sex { get; set; }

        public DateTime? BirthDate { get; set; }

        public string UrlPicture { get; set; }

        public bool IsActive { get; set; } = false;
    }
}
