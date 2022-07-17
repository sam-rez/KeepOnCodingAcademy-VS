using System.ComponentModel.DataAnnotations;

namespace KeepOnCodingAcademy.Web.Models
{
    public class CodeRunModel
    {
        [Required]
        public string? Code { get; set; }

        //[Required]
        public string? Language { get; set; }

        //[Required]
        public string? QuestionNumber { get; set; }
    }
}
