using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
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
            MessageService messageService = new MessageService();

            for (int i = 0; i < steps.Count; i++)
            {
                if (responses.Count < 1)
                {
                    try
                    {
                        ServiceMessage serviceMessage = testStepsService.ExecuteStep(driver, steps[i], steps[i].StepParams);
                        responses.Add(serviceMessage);
                        
                        // set if pass or fail
                        StepItem stepItem = SetStepStatus(steps, i, serviceMessage);

                        messageService.SubmitMessage(JsonConvert.SerializeObject(stepItem));
                        // if step fails might aswell end test no point running remaiming steps as will likey fail aswell
                        if (serviceMessage.WasSuccess == false)
                            break;

                    }
                    catch (Exception e)
                    {
                        ServiceMessage s = new ServiceMessage()
                            {Message = "There was an error, the selector/url may be invalid ", WasSuccess = false};
                        responses.Add(s);
                        // set if pass or fail
                        StepItem stepItem = SetStepStatus(steps, i, s);
                        messageService.SubmitMessage(JsonConvert.SerializeObject(stepItem));
                        break;
                    }
                    
                }else if (responses[i - 1].WasSuccess == true)
                {
                    try
                    {
                        ServiceMessage serviceMessage = testStepsService.ExecuteStep(driver, steps[i], steps[i].StepParams);
                        responses.Add(serviceMessage);
                        // set if pass or fail
                        StepItem stepItem = SetStepStatus(steps, i, serviceMessage);
                        messageService.SubmitMessage(JsonConvert.SerializeObject(stepItem));

                        // if step fails might aswell end test no point running remaiming steps as will likey fail aswell
                        if(serviceMessage.WasSuccess ==false)
                            break;
                    }
                    catch (Exception e)
                    {
                        ServiceMessage s = new ServiceMessage()
                            {Message = "There was an error, the selector/url may be invalid ", WasSuccess = false};
                        responses.Add(s);
                        StepItem stepItem = SetStepStatus(steps, i, s);
                        messageService.SubmitMessage(JsonConvert.SerializeObject(stepItem));
                        break;
                    }
                    
                }
                //else
                //{
                //    responses.Add(new ServiceMessage(){Message = "There was an error, the selector/url may be invalid ", WasSuccess = false});
                //}
            }

            ClearDriver();

            return responses;

        }

        private static StepItem SetStepStatus(List<StepItem> steps, int i, ServiceMessage serviceMessage)
        {
            steps[i].StepIndexPos = i;

            if (serviceMessage.WasSuccess)
            {
                steps[i].DidFail = false;
                steps[i].DidPass = true;
              
            }
            else
            {
                steps[i].DidFail = true;
                steps[i].FailMessage = serviceMessage.Message;
            }

            return steps[i];
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
