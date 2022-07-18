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
            var questionsList = _questionRepository.GetAllQuestions();

            ViewBag.QuestionList = questionsList;
            return View();
        }

        public IActionResult Question(string id)
        {
            //Get Question
            var question = _questionRepository.GetQuestion(Int32.Parse(id));
            ViewBag.Question = question;
            return View();
        }


        [HttpPost]
        public async void RunCode(CodeRunModel model)
        {
            model.QuestionNumber = "1";
            model.Language = "python";

            HttpClient client = new HttpClient();
            var json = JsonConvert.SerializeObject(model);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var runCodeResultModel = new RunCodeResultModel();

            if (ModelState.IsValid)
            {
                //TODO: Create Docker Container

                var result = await client.PostAsync("https://localhost:7278/CodeExecution", data);
                string resultContent = result.Content.ReadAsStringAsync().Result;
                runCodeResultModel = JsonConvert.DeserializeObject<RunCodeResultModel>(resultContent);

                //TODO: Teardown Docker Container
            }

            //TODO: Save Results

            ViewBag.Success = "True";
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