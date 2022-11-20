using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Configuration;
using System.Linq;
using XeCurrencyApp.Utilities;

namespace XeCurrencyApp
{
    public class XeAppTests : Utils
    {

        public void LaunchAppforGivenURL(string PageURL)
        {
            try
            {
                _driver.Navigate().GoToUrl(ConfigurationManager.AppSettings[PageURL]);
                ExecutionStep("btn_CookiesAccept", "Click");

                if (PageURL.Equals("Convert"))
                    WebdriverWait(10, "div_OverlayWindow");

                PageRefresh();
                if (_driver.Title.Contains("Xe Currency Converter"))
                    Console.WriteLine("Page launched successfully for the URL: " + ConfigurationManager.AppSettings[PageURL]);
            }
            catch (Exception e)
            {
                Assert.Fail("LaunchAppforGivenURL: " + e.Message);
            }
        }

        public void ConvertCurrencyforGivenAmount(string amount, string fromCurrency, string toCurrency)
        {
            ExecutionStep("txt_Amount", "Enter", amount);

            ExecutionStep("txt_FromCurrencyDropdown", "Click");
            WebdriverWait(10, "list_FromCurrencyDropdown");
            _driver.FindElement(By.XPath(GetValue("list_FromCurrencyDropdown") + "/div[text()='" + fromCurrency + "']")).Click();

            ExecutionStep("txt_ToCurrencyDropdown", "Click");
            WebdriverWait(10, "list_ToCurrencyDropdown");
            _driver.FindElement(By.XPath(GetValue("list_ToCurrencyDropdown") + "/div[text()='" + toCurrency + "']")).Click();

            if (CheckIfElementExists("btn_Convert"))
                ExecutionStep("btn_Convert", "Click", "", true);
        }

        public void ValidateCurrencyConversion(string amount)
        {
            if (!CheckIfElementExists("div_ConversionUnitRate"))
                ScrollBy();

            //Checking UnitRate
            var tex = _driver.FindElement(By.XPath(GetValue("div_ConversionUnitRate"))).Text;
            var unitRate = tex.Split(' ').ToList();
            double expectedVal = Math.Round(double.Parse(unitRate[3]) * double.Parse(amount), 2);

            //Checking ResultRate
            var act = _driver.FindElement(By.XPath(GetValue("ConversionResultRate"))).Text;
            var resultBigRate = act.Split(' ').ToList();
            double ActualVal = Math.Round(double.Parse(resultBigRate[0].Substring(0, 8)), 2);

            Console.WriteLine("Expected Rate: " + expectedVal + " Actual Rate: " + ActualVal);
            ScrollBy();
            Screenshot("Currency Validation");
            //Comparing UnitRate calculation against ResultRate
            Assert.AreEqual(expectedVal, ActualVal);
        }

        public void ValidateCurrencyFieldErrors(string input, string ErrorMsg)
        {
            ExecutionStep("txt_Amount", "Enter", input, true);
            Assert.True(_driver.FindElement(By.XPath(GetValue("div_ErrorText"))).Text.Equals(ErrorMsg));
        }

        public void SendMoney(string amount, string fromCurrency, string toCurrency)
        {
            ExecutionStep("txt_SendingCurrency", "Click");
            WebdriverWait(10, "list_SendingCurrency");
            _driver.FindElement(By.XPath(GetValue("list_SendingCurrency") + "/div[text()='" + fromCurrency + "']")).Click();

            ExecutionStep("txt_SendingAmount", "Enter", amount);

            ExecutionStep("txt_ReceivingCurrency", "Click");
            WebdriverWait(10, "list_ReceivingCurrency");
            _driver.FindElement(By.XPath(GetValue("list_ReceivingCurrency") + "/div[text()='" + toCurrency + "']")).Click();

            ScrollBy();

            if (CheckIfElementExists("div_ServiceUnavailableError"))
            {
                Screenshot("Temporarily Unavailable Error");
                PageLoad(100);
            }
        }

        public void ClickWebElement(string ObjName)
        {
            WebdriverWait(10, ObjName);
            ExecutionStep(ObjName, "Click");
        }


        public void LoginUsingCredentials()
        {
            ExecutionStep("txt_Email", "Enter", ConfigurationManager.AppSettings["TestEmail"]);
            ExecutionStep("txt_Password", "Enter", ConfigurationManager.AppSettings["TestPassword"]);
            if (!CheckIfElementExists("btn_RegisterNow"))
                ScrollBy();
            Assert.True(_driver.FindElement(By.XPath(GetValue("btn_RegisterNow"))).Enabled);
            ScrollBy();
            Screenshot("RegisterNow");
        }


    }
}
