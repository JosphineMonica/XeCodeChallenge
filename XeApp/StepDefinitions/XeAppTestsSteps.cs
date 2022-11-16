using TechTalk.SpecFlow;
using XeCurrencyApp.ScenarioHooks;
using XeCurrencyApp.Utilities;

namespace XeCurrencyApp
{
    [Binding]
    public class XeAppTestsSteps
    {
        XeAppTests xeApp = new XeAppTests(Hooks._driver);

        [Given(@"I launch Xe Currency Converter Web application '([^']*)' URL")]
        public void GivenILaunchXeCurrencyConverterWebApplicationURL(string Page)
        {
            Utils.CreateResultsFolderPath();
            xeApp.LaunchAppforGivenURL(Page);
        }

        [Then(@"I check conversion for the amount '([^']*)' from '([^']*)' to '([^']*)'")]
        public void ThenICheckConversionForTheAmountFromTo(string amount, string fromCurrency, string ToCurrency)
        {
            xeApp.ConvertCurrencyforGivenAmount(amount, fromCurrency, ToCurrency);
        }

        [When(@"I re-check for another currency for the amount '([^']*)' from '([^']*)' to '([^']*)'")]
        public void WhenIRe_CheckForAnotherCurrencyForTheAmountFromTo(string amount, string fromCurrency, string ToCurrency)
        {
            xeApp.ConvertCurrencyforGivenAmount(amount, fromCurrency, ToCurrency);
        }


        [Then(@"I validate the results for the amount '([^']*)'")]
        public void ThenIValidateTheResultsForTheAmount(string amount)
        {
            xeApp.ValidateCurrencyConversion(amount);
        }


        [When(@"I send invalid amount '([^']*)' and the errormessage '([^']*)' should be displayed")]
        public void WhenISendInvalidAmountAndTheErrormessageShouldBeDisplayed(string errorData, string errorMessage)
        {
            xeApp.ValidateCurrencyFieldErrors(errorData, errorMessage);
        }

        [Then(@"I send the amount of '([^']*)' from '([^']*)' to '([^']*)'")]
        public void ThenISendTheAmountOfFromTo(string amount, string fromCurrency, string toCurrency)
        {
            xeApp.SendMoney(amount, fromCurrency, toCurrency);
        }

        [When(@"I click on Signin and Send button that redirect to account creation Page")]
        public void WhenIClickOnSigninAndSendButtonThatRedirectToAccountCreationPage()
        {
            Utils.ExecutionStep("btn_SigninAndSend", "Click");
        }

        [Then(@"I enter email and password for TestUser, Register Now button should be Enabled")]
        public void ThenIEnterEmailAndPasswordForTestUserRegisterNowButtonShouldBeEnabled()
        {
            xeApp.LoginUsingCredentials();
        }

        [When(@"I click on View Transfer Quote and the application is redirected to send money section")]
        public void WhenIClickOnViewTransferQuoteAndTheApplicationIsRedirectedToSendMoneySection()
        {
            Utils.ExecutionStep("lnk_ViewTransferQuote","Click");
            Utils.ScrollBy();
            Utils.Screenshot("lnk_ViewTransferQuote");
        }



    }
}
