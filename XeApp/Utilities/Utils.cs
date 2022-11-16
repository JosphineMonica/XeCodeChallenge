using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using CsvHelper;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Globalization;
using System.IO;
using System.Threading;
using TechTalk.SpecFlow;
using XeCurrencyApp.ScenarioHooks;

namespace XeCurrencyApp.Utilities
{
    public class Utils
    {
        private static string ResultsFolderPath = string.Empty;

        private static string ScreenShotPath = String.Empty;

        private static string ObjRepPath = Path.Combine(Environment.CurrentDirectory, @"XeApp\ObjectRepository\ObjectRepository.csv");

        private static IWebDriver _driver = Hooks._driver;

       private static Actions actions = new Actions(_driver);


        public static void CreateResultsFolderPath()
        {
            ResultsFolderPath = Path.Combine(Environment.CurrentDirectory, @"Results\",
                ScenarioContext.Current.ScenarioInfo.Title + DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss"));
            if (!Directory.Exists(ResultsFolderPath))
                Directory.CreateDirectory(ResultsFolderPath);
        }
        public static void Screenshot(string screenName)
        {
            ScreenShotPath = Path.Combine(ResultsFolderPath, @"Screenshots\");
            if (!Directory.Exists(ScreenShotPath))
                Directory.CreateDirectory(ScreenShotPath);
            ScreenShotPath = ScreenShotPath + screenName + ".Png";
            ((ITakesScreenshot)_driver).GetScreenshot().SaveAsFile(ScreenShotPath, ScreenshotImageFormat.Png);
        }

        public static bool CheckIfElementExists(string ObjName)
        {
            try
            {
                return
                    (_driver.FindElement(By.XPath(GetValue(ObjName))).Displayed) ||
                    (_driver.FindElement(By.XPath(GetValue(ObjName))).Enabled);

            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string GetValue(string inputObjName)
        {
            try
            {
                using (var reader = new StreamReader(ObjRepPath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Read();
                    csv.ReadHeader();

                    while (csv.Read())
                    {
                        if (csv[csv.GetFieldIndex("ObjName")] == inputObjName)
                        {
                            return csv[csv.GetFieldIndex("ObjValue")];
                        }
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                Assert.Fail("GetValue: " + e.Message);
                return null;
            }
        }

        public static void WebdriverWait(int timeSpan, string ObjName)
        {
            try
            {
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeSpan));
                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(GetValue(ObjName))));
                wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(GetValue(ObjName))));
            }
            catch (Exception e)
            {
                Assert.Fail("WebdriverWait: "+ e.Message);
            }
        }

        public static void ScrollBy()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            PageLoad();
            js.ExecuteScript("window.scrollBy(0,300)");
        }

        public static void ExecutionStep(string ObjName, string Action, string Value = null, bool screenshot = false)
        {
            try
            {
                WebdriverWait(10, ObjName);

                switch (Action)
                {
                    case "Enter":
                        _driver.FindElement(By.XPath(GetValue(ObjName))).Click();
                        if (_driver.FindElement(By.XPath(GetValue(ObjName))).Text != null)
                            _driver.FindElement(By.XPath(GetValue(ObjName))).SendKeys(Keys.Control + "a" + Keys.Backspace);
                        _driver.FindElement(By.XPath(GetValue(ObjName))).SendKeys(Value);
                        break;
                    case "Clear":
                        _driver.FindElement(By.XPath(GetValue(ObjName))).Click();
                        _driver.FindElement(By.XPath(GetValue(ObjName))).SendKeys(Keys.Control + "a" + Keys.Backspace);
                        break;
                    case "Click":
                        if (!Utils.CheckIfElementExists(ObjName))
                            actions.MoveToElement(_driver.FindElement(By.XPath(GetValue(ObjName))));
                        _driver.FindElement(By.XPath(GetValue(ObjName))).Click();
                        break;
                    default:
                        break;

                }
                Screenshot(Action + "_" + ObjName + "_" +DateTime.Now.ToString("HH-mm-ss"));
            }
            catch (Exception e)
            {
                Screenshot("Failed Steps_"+ Action + "_" + ObjName + "_" + DateTime.Now.ToString("HH-mm-ss"));
                Assert.Fail("ExecutionStep:" + e.Message);
            }
        }

        public static void PageRefresh()
        {
            _driver.Navigate().Refresh();
        }

        public static void PageLoad()
        {
            Thread.Sleep(100);
        }
    }
}
