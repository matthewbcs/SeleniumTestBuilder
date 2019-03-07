using System;
using System.Collections.Generic;
using System.Text;
using SeleniumTestRunner.Models.Dto;

namespace SeleniumTestRunner.Models.ViewModels
{
    public class TestBuilderViewModel
    {
        public List<StepItem> DefinedSteps { get; set; }
        public List<StepItem> Steps { get; set; }
    }
}
