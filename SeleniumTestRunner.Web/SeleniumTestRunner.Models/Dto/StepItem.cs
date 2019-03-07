using System;
using System.Collections.Generic;
using System.Text;
using SeleniumTestRunner.Models.Enums;

namespace SeleniumTestRunner.Models.Dto
{
    public class StepItem
    {
        public EStepType StepType { get; set; }
        public string StepTypeLabel => StepType.ToString();
        public string StepTypeLabelFull => StepType.ToString() + " " +  StepLabel;

        public string StepLabel { get; set; }
        public int StepItemCode { get; set; }
        public List<StepParamDetail> StepParams { get; set; }
    }
}
