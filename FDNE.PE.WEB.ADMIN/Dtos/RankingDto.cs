using FDNE.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FDNE.PE.WEB.ADMIN.Dtos
{
    public class RankingDto
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.REQUIRED)]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.REQUIRED)]
        [Display(Name = "Temporada")]
        public Guid SeasonId { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGE.VALIDATION.REQUIRED)]
        [Display(Name = "Disciplina")]
        public Guid DisciplineId { get; set; }

        [Display(Name = "Categoría")]
        public Guid? CategoryId { get; set; }

        [Display(Name = "Nivel")]
        public Guid? LevelId { get; set; }

        public DisciplineDto Discipline { get; set; }

        public SeasonDto Season { get; set; }

        public CategoryDto Category { get; set; }

        public LevelDto Level { get; set; }
    }
}