using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDNE.PE.DATA.Models
{
    public class Gallery
    {
        public Gallery()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public DateTime Date { get; set; }
    }
}
