using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDNE.PE.DATA.Models
{
    public class News
    {
        public News()
        {
            Id = Guid.NewGuid();
        }

        public  Guid Id { get; set; }
        
        public int NumericIdentifier { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Title { get; set; }
        
        public string Body { get; set; }

        public string ImageUrl { get; set; }

        public DateTime Date { get; set; }
    }
}
