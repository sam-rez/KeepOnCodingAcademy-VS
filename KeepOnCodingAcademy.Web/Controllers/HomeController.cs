using KeepOnCodingAcademy.DataAccess.Models;
using KeepOnCodingAcademy.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace KeepOnCodingAcademy.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IQuestionRepository _questionRepository;

        public HomeController(ILogger<HomeController> logger, IQuestionRepository questionRepository)
        {
            _logger = logger;
            _questionRepository = questionRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RunCode(CodeRunModel model)
        {
            var qs = _questionRepository.GetAllQuestions();


            if (ModelState.IsValid)
            {

                //TODO: SubscribeUser(model.Code);

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
    }
}