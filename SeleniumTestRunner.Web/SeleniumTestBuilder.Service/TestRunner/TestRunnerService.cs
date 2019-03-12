using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SeleniumTestBuilder.Service.TestBuilderSteps;
using SeleniumTestRunner.Models.Dto;

namespace SeleniumTestBuilder.Service.TestRunner
{
    public class TestRunnerService
    {
        private IWebDriver driver;

        public TestRunnerService()
        {
            driver = new ChromeDriver();
        }

        public void ClearDriver()
        {
            driver.Dispose();
        }
        public List<ServiceMessage> RunTest(List<StepItem> steps)
        {
            TestStepsService testStepsService = new TestStepsService();
            List<ServiceMessage> responses = new List<ServiceMessage>();

            for (int i = 0; i < steps.Count; i++)
            {
                if (responses.Count < 1)
                {
                    try
                    {
                        ServiceMessage serviceMessage = testStepsService.ExecuteStep(driver, steps[i], steps[i].StepParams);
                        responses.Add(serviceMessage);
                    }
                    catch (Exception e)
                    {
                        responses.Add(new ServiceMessage(){Message = "There was an error, the selector/url may be invalid ", WasSuccess = false});
                    }
                    
                }else if (responses[i - 1].WasSuccess == true)
                {
                    try
                    {
                        ServiceMessage serviceMessage = testStepsService.ExecuteStep(driver, steps[i], steps[i].StepParams);
                        responses.Add(serviceMessage);
                    }
                    catch (Exception e)
                    {
                        responses.Add(new ServiceMessage(){Message = "There was an error, the selector/url may be invalid ", WasSuccess = false});
                    }
                    
                }
            }

            ClearDriver();

            return responses;

        }

        public ServiceMessage RunTest(StepItem step)
        {
            TestStepsService testStepsService = new TestStepsService();

            try
            {
                ServiceMessage serviceMessage = testStepsService.ExecuteStep(driver, step, step.StepParams);
                return serviceMessage;
            }
            catch (Exception e)
            {
                return new ServiceMessage(){Message = "There was an error ", WasSuccess = false};
            }
            

        }
        public  ServiceMessage Run(StepItem step)
        {
            IWebDriver drivert = new ChromeDriver();
            TestStepsService testStepsService = new TestStepsService();
           
            ServiceMessage serviceMessage = testStepsService.ExecuteStep(drivert, step, step.StepParams);
            return serviceMessage;

        }
    }
}
