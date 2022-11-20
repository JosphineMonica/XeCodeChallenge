using CsvHelper;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Globalization;
using System.IO;
using System.Threading;
using TechTalk.SpecFlow;
using WebDriverManager.DriverConfigs.Impl;
using XeCurrencyApp.ScenarioHooks;

namespace XeCurrencyApp.Utilities
{
    public class Utils
    {
        private static string ResultsFolderPath = string.Empty;

        private static string ScreenShotPath = String.Empty;

        private static string ObjRepPath = Path.Combine(Environment.CurrentDirectory, @"XeApp\ObjectRepository\ObjectRepository.csv");

        public static IWebDriver _driver = null;

        public void CreateResultsFolderPath()
        {
            ResultsFolderPath = Path.Combine(Environment.CurrentDirectory, @"Results\",
                ScenarioContext.Current.ScenarioInfo.Title + DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss"));
            if (!Directory.Exists(ResultsFolderPath))
                Directory.CreateDirectory(ResultsFolderPath);
        }
        public void Screenshot(string screenName)
        {
            ScreenShotPath = Path.Combine(ResultsFolderPath, @"Screenshots\");
            if (!Directory.Exists(ScreenShotPath))
                Directory.CreateDirectory(ScreenShotPath);
            ScreenShotPath = ScreenShotPath + screenName + ".Png";
            ((ITakesScreenshot)_driver).GetScreenshot().SaveAsFile(ScreenShotPath, ScreenshotImageFormat.Png);
        }

        public bool CheckIfElementExists(string ObjName)
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

        public string GetValue(string inputObjName)
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

        public void WebdriverWait(int timeSpan, string ObjName)
        {
            try
            {
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeSpan));
                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(GetValue(ObjName))));
                wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(GetValue(ObjName))));
            }
            catch (Exception e)
            {
                Assert.Fail("WebdriverWait: " + e.Message);
            }
        }

        public void ScrollBy()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            PageLoad(100);
            js.ExecuteScript("window.scrollBy(0,300)");
        }

        public void ExecutionStep(string ObjName, string Action, string Value = null, bool screenshot = false)
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
                        if (!CheckIfElementExists(ObjName))
                            new Actions(_driver).MoveToElement(_driver.FindElement(By.XPath(GetValue(ObjName))));
                        _driver.FindElement(By.XPath(GetValue(ObjName))).Click();
                        break;
                    default:
                        break;

                }
                Screenshot(Action + "_" + ObjName + "_" + DateTime.Now.ToString("HH-mm-ss"));
            }
            catch (Exception e)
            {
                Screenshot("Failed Steps_" + Action + "_" + ObjName + "_" + DateTime.Now.ToString("HH-mm-ss"));
                Assert.Fail("ExecutionStep:" + e.Message);
            }
        }

        public void PageRefresh()
        {
            _driver.Navigate().Refresh();
        }

        public void PageLoad(int timeMilliSeconds)
        {
            Thread.Sleep(timeMilliSeconds);
        }

        public void InitDriver(string browserName = "Chrome")
        {
            switch (browserName.ToUpper())
            {
                case "CHROME":
                    new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());
                    var chromeDriverService = ChromeDriverService.CreateDefaultService();
                    var chromeOptions = new ChromeOptions();
                    _driver = new ChromeDriver(chromeDriverService, chromeOptions);
                    break;
                case "FIREFOX":
                    new WebDriverManager.DriverManager().SetUpDriver(new FirefoxConfig());
                    _driver = new FirefoxDriver();
                    break;
                case "IE":
                    new WebDriverManager.DriverManager().SetUpDriver(new InternetExplorerConfig());
                    _driver = new InternetExplorerDriver();
                    break;
                default:
                    Console.WriteLine("Please give valid input: " + browserName);
                    break;
            }

            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            _driver.Manage().Window.Maximize();
        }

        public void CloseBrowser()
        {
            _driver.Quit();
        }
    }
}
