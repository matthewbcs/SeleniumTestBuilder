using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SeleniumTestBuilder.Service.TestBuilderSteps;
using SeleniumTestRunner.Models.Dto;
using SeleniumTestRunner.Models.ViewModels;

namespace SeleniumTestRunner.Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly ITestStepsService _testStepsService;
        public HomeController()
        {
            _testStepsService = new TestStepsService();
        }
        [HttpGet]
        public ActionResult Index()
        {
            TestBuilderViewModel model = new TestBuilderViewModel();
            model.DefinedSteps = _testStepsService.GetDefinedSteps();
            return View(model);
        }

        [HttpPost]
        public ActionResult RunSeleniumTest(List<StepItem> StepsList)
        {
            
          
            return Json(null);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}