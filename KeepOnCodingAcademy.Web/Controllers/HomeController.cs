using KeepOnCodingAcademy.DataAccess.Models;
using KeepOnCodingAcademy.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace KeepOnCodingAcademy.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IQuestionRepository _questionRepository;
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(ILogger<HomeController> logger, IQuestionRepository questionRepository, 
                                IHttpClientFactory httpClientFactory)
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
        public async Task<IActionResult> RunCode(CodeRunModel model)
        {
            model.QuestionNumber = "1";
            model.Language = "python";
            RunCodeResultModel runCodeResultModel;

            try
            {
                HttpClient client = new HttpClient();
                var json = JsonConvert.SerializeObject(model);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                runCodeResultModel = new RunCodeResultModel();

                if (ModelState.IsValid)
                {
                    //TODO: Create Docker Container
                    DockerClient dockerClient = new DockerClientConfiguration().CreateClient();

                    var createContainerResponse = await dockerClient.Containers.CreateContainerAsync(new CreateContainerParameters()
                        {
                            Image = "codeexecution",
                            HostConfig = new HostConfig
                            {
                                PortBindings = new Dictionary<string, IList<PortBinding>>
                                                {
                                                    {
                                                        "80" , new List<PortBinding>
                                                                    {
                                                                        new PortBinding { HostIP ="0.0.0.0" }
                                                                    }
                                                    }

                                                },
                            },
                            ExposedPorts = new Dictionary<string, EmptyStruct>
                                        {
                                                {
                                                    "80", new EmptyStruct()
                                                }
                                        }
                    });

                    string containerId = createContainerResponse.ID;

                    //Start Container
                    var started = await dockerClient.Containers.StartContainerAsync(
                        containerId,
                        new ContainerStartParameters()
                        );

                    //Get Port


                    Thread.Sleep(1000);

                    //Call To CodeExecution API
                    var result = await client.PostAsync("http://localhost:5000/CodeExecution", data);
                    string resultContent = await result.Content.ReadAsStringAsync();
                    runCodeResultModel = JsonConvert.DeserializeObject<RunCodeResultModel>(resultContent);


                    //Stop Container
                    await dockerClient.Containers.StopContainerAsync(
                        containerId,
                        new ContainerStopParameters
                        {
                            WaitBeforeKillSeconds = 15,
                        },
                        CancellationToken.None);

                    //Kill Container
                    //await dockerClient.Containers.KillContainerAsync(
                    //    containerId,
                    //    new ContainerKillParameters
                    //    {                   
                    //    },
                    //    CancellationToken.None);


                    //Delete Container
                    await dockerClient.Containers.RemoveContainerAsync(
                        containerId,
                        new ContainerRemoveParameters
                        {
                        },
                        CancellationToken.None);


                    //TODO: Save Results

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }


            ViewBag.Success = "True";

            return Ok();
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