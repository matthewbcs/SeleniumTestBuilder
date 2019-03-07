using System;
using System.Collections.Generic;
using System.Text;
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
            steps.Add(BuildStep(EStepType.Given, 1,"I am on this Url",CreateParams(new List<string> {"URL"})));


            //Whens
            steps.Add(BuildStep(EStepType.When, 1,"I click on the element",CreateParams(new List<string> {"CSS Selector"})));
            steps.Add(BuildStep(EStepType.When, 1,"I go to the URL",CreateParams(new List<string> {"Url"})));
            steps.Add(BuildStep(EStepType.When, 1,"I Set the dropdown element as value",CreateParams(new List<string> {"Dropdown Value","CSS Selector"})));
            steps.Add(BuildStep(EStepType.When, 1,"I Set the text box element value to",CreateParams(new List<string> {"Text box Value","CSS Selector"})));
            
            //Thens
            steps.Add(BuildStep(EStepType.Then, 1,"The Url Contains",CreateParams(new List<string> {"Url partial"})));
            steps.Add(BuildStep(EStepType.Then, 1,"The element is visible",CreateParams(new List<string> {"CSS Selector"})));
            steps.Add(BuildStep(EStepType.Then, 1,"the expected text is displayed",CreateParams(new List<string> {"CSS Selector","Expected Text"})));
            
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
    }

   
}
