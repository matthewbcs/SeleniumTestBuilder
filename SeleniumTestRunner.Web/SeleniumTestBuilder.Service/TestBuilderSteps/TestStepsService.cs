using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium.Chrome;
using SeleniumTestRunner.Models.Dto;
using SeleniumTestRunner.Models.Enums;

namespace SeleniumTestBuilder.Service.TestBuilderSteps
{
    public interface ITestStepsService
    {
        List<StepItem> GetDefinedSteps();
    }

    public class TestStepsService : ITestStepsService
    {
        public List<StepItem> GetDefinedSteps()
        {
            List<StepItem> steps = new List<StepItem>();

            // Given
            steps.Add(BuildStep(EStepType.Given, (int)EStepItemCode.ImOnUrl,"I am on this Url",CreateParams(new List<string> {"Url"})));


            //Whens
            steps.Add(BuildStep(EStepType.When, (int)EStepItemCode.ClickOnElement,"I click on the element",CreateParams(new List<string> {"CSS Selector"})));
            steps.Add(BuildStep(EStepType.When, (int)EStepItemCode.GoToUrl,"I go to the URL",CreateParams(new List<string> {"Url"})));
            steps.Add(BuildStep(EStepType.When, (int)EStepItemCode.SetDropDownValue,"I Set the dropdown element as value",CreateParams(new List<string> {"Dropdown Value","CSS Selector"})));
            steps.Add(BuildStep(EStepType.When, (int)EStepItemCode.SetTextBoxValue,"I Set the text box element value to",CreateParams(new List<string> {"Text box Value","CSS Selector"})));
            
            //Thens
            steps.Add(BuildStep(EStepType.Then, (int)EStepItemCode.UrlContainsString,"The Url Contains",CreateParams(new List<string> {"Url partial"})));
            steps.Add(BuildStep(EStepType.Then, (int)EStepItemCode.ElementIsVisible,"The element is visible",CreateParams(new List<string> {"CSS Selector"})));
            steps.Add(BuildStep(EStepType.Then, (int)EStepItemCode.ExpectedText,"the expected text is displayed",CreateParams(new List<string> {"CSS Selector","Expected Text"})));
            
            return steps;

        }

        private StepItem BuildStep(EStepType stepType, int stepItemCode, string stepLabel, List<StepParamDetail> stepParams)
        {
            StepItem step = new StepItem()
            {
                StepType = stepType,
                StepItemCode = stepItemCode,
                StepLabel = stepLabel,
                StepParams = stepParams
            };
            return step;
        }

        private List<StepParamDetail> CreateParams(List<string> strings)
        {
            List<StepParamDetail> p = new List<StepParamDetail>();

            foreach (var s in strings)
            {
                StepParamDetail d = new StepParamDetail();
                d.ParamLabel = s;
                p.Add(d); 
            }

            return p;

        }

        public ServiceMessage ExecuteStep(ChromeDriver driver, StepItem stepData, List<StepParamDetail> paramData)
        {
            // validate step data first
            if(stepData == null)
                return new ServiceMessage(){Message = "Step Data not specified",WasSuccess = false};

            if(stepData.StepItemCode == 0)
                return new ServiceMessage(){Message = "Step Code not specified",WasSuccess = false};
            StepParamDetail stepParamDetail = new StepParamDetail();

            // get step enum
            EStepItemCode eStepItemCode = (EStepItemCode) stepData.StepItemCode;

            // step definitions - well kind of todo; could try to improve this too many magic strings but quick and short  
            switch (eStepItemCode)
            {
                case EStepItemCode.ImOnUrl:
                    stepParamDetail = stepData.StepParams.FirstOrDefault(x => x.ParamLabel.ToLower() == "url");
                    if(string.IsNullOrEmpty(stepParamDetail.ParamValue))
                        return new ServiceMessage(){Message = "Url was not specified",WasSuccess = false};

                    return AreEqual(stepParamDetail.ParamValue, driver.Url, "Urls are equal",
                        "Urls did not match");
                    
                case EStepItemCode.ClickOnElement:
                    stepParamDetail = stepData.StepParams.FirstOrDefault(x => x.ParamLabel.ToLower() == "CSS Selector");
                    if(string.IsNullOrEmpty(stepParamDetail.ParamValue))
                        return new ServiceMessage(){Message = "selector was not specified",WasSuccess = false};
                    
                    driver.FindElementByCssSelector(stepParamDetail.ParamValue).Click();
                    return new ServiceMessage(){Message = "Success",WasSuccess = true};
               
                case EStepItemCode.GoToUrl:
                    stepParamDetail = stepData.StepParams.FirstOrDefault(x => x.ParamLabel.ToLower() == "url");
                    if(string.IsNullOrEmpty(stepParamDetail.ParamValue))
                        return new ServiceMessage(){Message = "Url was not specified",WasSuccess = false};

                    driver.Navigate().GoToUrl(stepParamDetail.ParamValue);
                    return new ServiceMessage(){Message = "Success",WasSuccess = true};

                case EStepItemCode.SetDropDownValue:
                    StepParamDetail stepParamDetail1 = stepData.StepParams.FirstOrDefault(x => x.ParamLabel.ToLower() == "Dropdown Value");
                    if(string.IsNullOrEmpty(stepParamDetail1.ParamValue))
                        return new ServiceMessage(){Message = "Dropdown value was not specified",WasSuccess = false};
                    StepParamDetail stepParamDetail2 = stepData.StepParams.FirstOrDefault(x => x.ParamLabel.ToLower() == "CSS Selector");
                    if(string.IsNullOrEmpty(stepParamDetail2.ParamValue))
                        return new ServiceMessage(){Message = "selector was not specified",WasSuccess = false};

                    driver.FindElementByCssSelector(stepParamDetail2.ParamValue).SendKeys(stepParamDetail1.ParamValue);
                    return new ServiceMessage(){Message = "Success",WasSuccess = true};
                case EStepItemCode.SetTextBoxValue:
                    StepParamDetail ptxtbox1 = stepData.StepParams.FirstOrDefault(x => x.ParamLabel.ToLower() == "Text box Value");
                    if(string.IsNullOrEmpty(ptxtbox1.ParamValue))
                        return new ServiceMessage(){Message = "Dropdown value was not specified",WasSuccess = false};

                    StepParamDetail selector2 = stepData.StepParams.FirstOrDefault(x => x.ParamLabel.ToLower() == "CSS Selector");
                    if(string.IsNullOrEmpty(selector2.ParamValue))
                        return new ServiceMessage(){Message = "selector was not specified",WasSuccess = false};

                    driver.FindElementByCssSelector(selector2.ParamValue).SendKeys(ptxtbox1.ParamValue);
                    return new ServiceMessage(){Message = "Success",WasSuccess = true};
                case EStepItemCode.UrlContainsString:
                    stepParamDetail = stepData.StepParams.FirstOrDefault(x => x.ParamLabel.ToLower() == "Url partial");

                    if(string.IsNullOrEmpty(stepParamDetail.ParamValue))
                        return new ServiceMessage(){Message = "Url was not specified",WasSuccess = false};

                    return DoesContain(stepParamDetail.ParamValue, driver.Url, "Success", "Url does not contain string");
                case EStepItemCode.ElementIsVisible:
                    stepParamDetail = stepData.StepParams.FirstOrDefault(x => x.ParamLabel.ToLower() == "CSS Selector");
                    if(string.IsNullOrEmpty(stepParamDetail.ParamValue))
                        return new ServiceMessage(){Message = "selector was not specified",WasSuccess = false};

                    bool isEnabled = driver.FindElementByCssSelector(stepParamDetail.ParamValue).Enabled;
                    return AreEqual("true", isEnabled.ToString(), "Success", "Element is disabled/ not visible");

                case EStepItemCode.ExpectedText:
                    StepParamDetail exptext = stepData.StepParams.FirstOrDefault(x => x.ParamLabel.ToLower() == "Expected Text");
                    if(string.IsNullOrEmpty(exptext.ParamValue))
                        return new ServiceMessage(){Message = "expected text value was not specified",WasSuccess = false};
                    StepParamDetail selector = stepData.StepParams.FirstOrDefault(x => x.ParamLabel.ToLower() == "CSS Selector");
                    if(string.IsNullOrEmpty(selector.ParamValue))
                        return new ServiceMessage(){Message = "selector was not specified",WasSuccess = false};

                    string actualText = driver.FindElementByCssSelector(selector.ParamValue).Text;
                    return AreEqual(exptext.ParamValue, actualText, "Success", "Text was not as expected");
                   
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private ServiceMessage AreEqual(string expected, string actual, string passMsg, string failMsg)
        {
            bool result = expected == actual;

            return result ? new ServiceMessage(){Message = passMsg,WasSuccess = true} : new ServiceMessage(){Message = failMsg,WasSuccess = false};
        }
        private ServiceMessage DoesContain(string partial, string fullstring, string passMsg, string failMsg)
        {
            bool result = fullstring.IndexOf(partial,StringComparison.Ordinal) >=0;
            

            return result ? new ServiceMessage(){Message = passMsg,WasSuccess = true} : new ServiceMessage(){Message = failMsg,WasSuccess = false};
        }
    }

   
}
