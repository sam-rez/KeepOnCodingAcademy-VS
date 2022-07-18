using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeepOnCodingAcademy.DataAccess.Models
{
    public class Question
    {
        public int Id { get; set; }

        [Required]
        public string? Prompt { get; set; }

        [Required]
        public string? Title { get; set; }

        [Required]
        public int Difficulty { get; set; }

        [Required]
        public int Category { get; set; }
    }
}
