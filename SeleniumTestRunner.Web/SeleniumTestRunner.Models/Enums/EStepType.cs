using System;
using System.Collections.Generic;
using System.Text;

namespace SeleniumTestRunner.Models.Enums
{
    public enum EStepType
    {
        Given,
        When,
        Then,
    }

    public enum EStepItemCode
    {
        ImOnUrl = 1,
        ClickOnElement = 2,
        GoToUrl = 3,
        SetDropDownValue = 4,
        SetTextBoxValue = 5,
        UrlContainsString = 6,
        ElementIsVisible = 7,
        ExpectedText = 8,
        WaitForSeconds =9,

    }
}
