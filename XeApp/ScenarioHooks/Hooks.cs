using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using TechTalk.SpecFlow;
using XeCurrencyApp.Utilities;

namespace XeCurrencyApp.ScenarioHooks
{
    [Binding]
    public sealed class Hooks
    {
        readonly Utils utils = new Utils();

        [BeforeScenario("@tag1")]
        public void BeforeScenarioWithTag()
        {
            // Example of filtering hooks using tags. (in this case, this 'before scenario' hook will execute if the feature/scenario contains the tag '@tag1')
            // See https://docs.specflow.org/projects/specflow/en/latest/Bindings/Hooks.html?highlight=hooks#tag-scoping

            //TODO: implement logic that has to run before executing each scenario
        }

        [BeforeScenario]
        public void FirstBeforeScenario()
        {
            // Example of ordering the execution of hooks
            // See https://docs.specflow.org/projects/specflow/en/latest/Bindings/Hooks.html?highlight=order#hook-execution-order

            //TODO: implement logic that has to run before executing each scenario
        }

        [BeforeScenario(Order = 1)]
        public void BeforeScenario()
        {
            utils.InitDriver();
            utils.CreateResultsFolderPath();
        }

        [AfterScenario]
        public void AfterScenario()
        {
            utils.CloseBrowser();
        }
    }
}