using System.Text.Json.Serialization;

namespace KeepOnCodingAcademy.Web.Models
{
    public class RunCodeResultModel
    {
        //[JsonPropertyName("input")]
        public string Input { get; set; }

        //[JsonPropertyName("output")]
        public string Output { get; set; }

        //[JsonPropertyName("expected")]
        public string Expected { get; set; }
    }
}
