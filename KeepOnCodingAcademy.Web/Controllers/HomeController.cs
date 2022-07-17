using KeepOnCodingAcademy.DataAccess.Models;
using KeepOnCodingAcademy.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace KeepOnCodingAcademy.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IQuestionRepository _questionRepository;
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(ILogger<HomeController> logger, IQuestionRepository questionRepository, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _questionRepository = questionRepository;
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> RunCode(CodeRunModel model)
        {
            model.QuestionNumber = "1";
            model.Language = "python";
            
            //var qs = _questionRepository.GetAllQuestions();
            //await OnGet();

            HttpClient client = new HttpClient();
            var json = JsonConvert.SerializeObject(model);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            if (ModelState.IsValid)
            {
                var result = await client.PostAsync("https://localhost:7278/CodeExecution", data);
                string resultContent = result.Content.ReadAsStringAsync().Result;
                var runCodeResultModel = JsonConvert.DeserializeObject<RunCodeResultModel>(resultContent);
            }

            return View("Index", model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task OnGet()
        {
            var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Get,
                "https://api.github.com/repos/dotnet/AspNetCore.Docs/branches")
            {
                Headers =
            {
                { HeaderNames.Accept, "application/vnd.github.v3+json" },
                { HeaderNames.UserAgent, "HttpRequestsSample" }
            }
            };

            var httpClient = _httpClientFactory.CreateClient();
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream =
                    await httpResponseMessage.Content.ReadAsStreamAsync();
            }
        }
    }
}