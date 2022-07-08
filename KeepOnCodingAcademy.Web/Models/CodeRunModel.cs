using System.ComponentModel.DataAnnotations;

namespace KeepOnCodingAcademy.Web.Models
{
    public class CodeRunModel
    {
        [Required]
        public string? Code { get; set; }
    }
}
