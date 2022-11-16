Feature: XeAppTests

###To test Xe Currency Converter Application and to provide feedback

@Regression
Scenario: LaunchXeAppForConversion
	Given I launch Xe Currency Converter Web application 'Convert' URL
    When I send invalid amount 'test' and the errormessage 'Please enter a valid amount' should be displayed
	Then I check conversion for the amount '10' from 'GBP' to 'EUR'
	And I validate the results for the amount '10'
	When I re-check for another currency for the amount '100' from 'INR' to 'GBP'
	Then I validate the results for the amount '100'

@Regression
Scenario: LaunchXeAppToSendMoney
	Given I launch Xe Currency Converter Web application 'Send' URL
	Then I send the amount of '10' from 'GBP' to 'EUR'
	When I click on Signin and Send button that redirect to account creation Page
	Then I enter email and password for TestUser, Register Now button should be Enabled

@EndToEndTest
Scenario: LaunchXeAppToCheckAndSendMoney
	Given I launch Xe Currency Converter Web application 'Convert' URL
    When I send invalid amount '0' and the errormessage 'Please enter an amount greater than 0' should be displayed
	Then I check conversion for the amount '10' from 'EUR' to 'GBP'
	And I validate the results for the amount '10'
	When I click on View Transfer Quote and the application is redirected to send money section
	Then I send the amount of '10' from 'GBP' to 'EUR'
	When I click on Signin and Send button that redirect to account creation Page
	Then I enter email and password for TestUser, Register Now button should be Enabled