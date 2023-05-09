using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDNE.PE.DATA.Models
{
    public class FederationLessonFile
    {
        public FederationLessonFile()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public string Nombre { get; set; }

        public string UrlArchivo { get; set; }

        public Guid FederationLessonId { get; set; }

        public FederationLesson FederationLesson { get; set; }
    }
}
