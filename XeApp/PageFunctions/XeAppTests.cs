using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Configuration;
using System.Linq;
using XeCurrencyApp.Utilities;

namespace XeCurrencyApp
{
    public class XeAppTests 
    {
        public IWebDriver _driver;

        public XeAppTests(IWebDriver driver)
        {
            _driver = driver;
        }

        public void LaunchAppforGivenURL(string PageURL)
        {
            try
            {
                _driver.Navigate().GoToUrl(ConfigurationManager.AppSettings[PageURL]);
                Utils.ExecutionStep("btn_CookiesAccept", "Click");

                if (PageURL.Equals("Convert"))
                    Utils.WebdriverWait(10, "div_OverlayWindow");

                Utils.PageRefresh();
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
            Utils.ExecutionStep("txt_Amount", "Enter", amount);

            Utils.ExecutionStep("txt_FromCurrencyDropdown", "Click");
            Utils.WebdriverWait(10, "list_FromCurrencyDropdown");
            _driver.FindElement(By.XPath(Utils.GetValue("list_FromCurrencyDropdown") + "/div[text()='" + fromCurrency + "']")).Click();

            Utils.ExecutionStep("txt_ToCurrencyDropdown", "Click");
            Utils.WebdriverWait(10, "list_ToCurrencyDropdown");
            _driver.FindElement(By.XPath(Utils.GetValue("list_ToCurrencyDropdown") + "/div[text()='" + toCurrency + "']")).Click();
            
            if (Utils.CheckIfElementExists("btn_Convert"))
                Utils.ExecutionStep("btn_Convert", "Click", "", true);
        }

        public void ValidateCurrencyConversion(string amount)
        {
            if (!Utils.CheckIfElementExists("div_ConversionUnitRate"))
                Utils.ScrollBy();

            Utils.Screenshot("Currency Validation");

            //Checking UnitRate
            var tex = _driver.FindElement(By.XPath(Utils.GetValue("div_ConversionUnitRate"))).Text;
            var unitRate = tex.Split(' ').ToList();
            double expectedVal = Math.Round(double.Parse(unitRate[3]) * double.Parse(amount), 2);

            //Checking ResultRate
            var act = _driver.FindElement(By.XPath(Utils.GetValue("ConversionResultRate"))).Text;
            var resultBigRate = act.Split(' ').ToList();
            double ActualVal = Math.Round(double.Parse(resultBigRate[0].Substring(0, 8)), 2);

            Console.WriteLine("Expected Rate: " + expectedVal +  " Actual Rate: " + ActualVal);

            //Comparing UnitRate calculation against ResultRate
            Assert.AreEqual(expectedVal, ActualVal);
        }

        public void ValidateCurrencyFieldErrors(string input, string ErrorMsg)
        {
                Utils.ExecutionStep("txt_Amount", "Enter", input, true);
                Assert.True(_driver.FindElement(By.XPath(Utils.GetValue("div_ErrorText"))).Text.Equals(ErrorMsg));
        }

        public async void SendMoney(string amount, string fromCurrency, string toCurrency)
        {
            Utils.ExecutionStep("txt_SendingCurrency", "Click");
            Utils.WebdriverWait(10, "list_SendingCurrency");
            _driver.FindElement(By.XPath(Utils.GetValue("list_SendingCurrency") + "/div[text()='" + fromCurrency + "']")).Click();

            Utils.ExecutionStep("txt_SendingAmount", "Enter", amount);

            Utils.ExecutionStep("txt_ReceivingCurrency", "Click");
            Utils.WebdriverWait(10, "list_ReceivingCurrency");
            _driver.FindElement(By.XPath(Utils.GetValue("list_ReceivingCurrency") + "/div[text()='" + toCurrency + "']")).Click();
            
            Utils.ScrollBy();

            if (Utils.CheckIfElementExists("div_ServiceUnavailableError"))
            {
                Utils.Screenshot("Temporarily Unavailable Error");
                Utils.PageLoad(300);               
            }
        }

        public void LoginUsingCredentials()
        {
            Utils.PageLoad(100);
            Utils.ExecutionStep("txt_Email", "Enter", ConfigurationManager.AppSettings["TestEmail"]);
            Utils.ExecutionStep("txt_Password", "Enter", ConfigurationManager.AppSettings["TestPassword"]);
            if (!Utils.CheckIfElementExists("btn_RegisterNow"))
                Utils.ScrollBy();
            Assert.True(_driver.FindElement(By.XPath(Utils.GetValue("btn_RegisterNow"))).Enabled);
            Utils.ScrollBy();
            Utils.Screenshot("RegisterNow");
        }

       
    }
}
