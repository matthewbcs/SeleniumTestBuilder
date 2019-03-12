using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using SeleniumTestBuilder.Service.TestBuilderSteps;
using SeleniumTestBuilder.Service.TestRunner;
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
            model.SampleTest1Steps = _testStepsService.SampleTest1();
            return View(model);
        }

        [HttpPost]
        public ActionResult RunSeleniumTest(List<StepItem> StepsList)
        {
            List<ServiceMessage> serviceMessage = new TestRunnerService().RunTest(StepsList);

            return Json(serviceMessage);
        }
        [HttpPost]
        public ActionResult RunSingleStep(StepItem Step)
        {
            Thread.Sleep(4000);
            ServiceMessage serviceMessage = new TestRunnerService().RunTest(Step);
            
            //TestRunnerService t = new TestRunnerService();
            //ServiceMessage serviceMessage = t.Run(Step);

            return Json(serviceMessage);
        }
        [HttpPost]
        public ActionResult ClearChromeDriver()
        {
            new TestRunnerService().ClearDriver();
            return Json(true);
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