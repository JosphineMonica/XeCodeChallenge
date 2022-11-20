# XeCodeChallenge
Objective: To create automation framework from the scratch and perform end to end tests for Xe.com application.

> **Prerequisites**:
> - Visual Studio [Version 2022 is used here]
> - SpecFlow plugin for [Visual Studio](https://docs.specflow.org/projects/getting-started/en/latest/GettingStarted/Step1.html)

About Framework

> - BDD Feature files are placed under Feature Folder
> - External csv files is created to hold object repository. Thus, modifying object properties becomes easier.
> - Scenario Hooks and Utilies Folder are created to improve reusability, maintainability of the scripts.
> - App.Config file holds the URL and Credential information for security reasons.
> - Runtime Results folder is created for every single run and screenshots is stored within Results folder.

Areas to Implement
> - Word documents can be generated using the screenshots placed in the runtime order to reduce manual documentation efforts.
> - HTML Reporting either own or Extent Reports can be implemented to capture each step execution.

**Steps to Execute Test cases**

1. Please clone the repository and download the solution.
2. Add following plugins from Nuget Package Manager
   > - NUnit3TestAdapter
   > - Csv Helper
   > - DotNetSeleniumExtras.WaitHelpers
   > - SpecFlow
   > - SpecFlow.NUnit
   > - Selenium.WebDriver.ChromeDriver
   > - Selenium.Support
3. Build the solution and check for any package errors. If so, kindly install appropriate references.
4. Once the build is successful, click on Test-> Test Explorer
5. There are three test cases available. Click on each test case to run. 
   > - Scenario 1: This Scenario covers Conversion section testing by supplying invalid and valid inputs.
   > - Scenario 2: This scenario covers the Send money Section.
   > - Scenario 3. This is a combined scenario of Conversion followed by send money.
7. Once the test case run is successful, I have created Results folder where the Screenshots is stored for each testcase.

About Postman
1. Create a collection of tests and send GET, POST request independently.
2. Written scripts within Tests section to validate the given scenarios.
3. Screenshots are placed in the Postman Testcase word document.

## Observations
> - In Convert section, after clicking the "convert" button, "View Transfer Quote" button appears and clicking on the "View Transfer Quote" button displays no quotation values but redirects to fresh "Send" Money Section.
> - When the naviagtion occurs from "Convert" to "Send" section, the values entered in "Convert" section is not sent to "Send" section which lets user to again enter the currency details.
> - In Convert section, whenever new currency is changed either is From or To currency Field, "Convert" or "View Transfer Quote" button disappears from second search.
> - When the dropdown values are being selected, it takes few seconds to load and hence wait time is set to avoid failures but it makes the execution slow.
> - In Send Section, when receive currency dropdown is selected and temporarily unavailable message appears till the field gets loaded. This is also handled in the code.


