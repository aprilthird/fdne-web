using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDNE.PE.DATA.Models
{
    public class DocumentType
    {
        public DocumentType()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }
        
        [Required(AllowEmptyStrings = false)]
        public string Acronym { get; set; }

        public short Length { get; set; }

        public bool ExactLength { get; set; }

        public bool IsNumeric { get; set; }
    }
}
