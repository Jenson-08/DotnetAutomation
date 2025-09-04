using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using Framework.Config;
using Framework.Drivers;
using Framework.Utils;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;


namespace UI.Tests.Hooks;


[TestFixture]
[Parallelizable(ParallelScope.Fixtures)]
public abstract class TestBase
{
    protected IWebDriver Driver = null!;
    protected TestSettings Settings = null!;
    protected ExtentReports extent;
    protected ExtentTest test;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        // Crear reporte HTML
        string reportPath = Path.Combine(AppContext.BaseDirectory, "ExtentReport.html");
        var htmlReporter = new ExtentSparkReporter(reportPath);
        htmlReporter.Config.DocumentTitle = "Automation Report";
        htmlReporter.Config.ReportName = "UI Test Report";

        extent = new ExtentReports();
        extent.AttachReporter(htmlReporter);
    }

    [SetUp]
    public void SetUp()
    {
        try
        {
            Settings = TestSettings.Load();
            Driver = DriverFactory.Create(Settings);
            Driver.Navigate().GoToUrl(Settings.BaseUrl);
            test = extent.CreateTest(TestContext.CurrentContext.Test.Name);

        }
        catch (Exception ex)
        {
            if (extent != null)
                test = extent.CreateTest(TestContext.CurrentContext.Test.Name);
            test?.Fail($"SetUp failed: {ex.Message}");
            throw; // relanzar para que NUnit marque el test como fallido
        }
    }


    [TearDown]
    public void TearDown()
    {
        try { 
            var stacktrace = TestContext.CurrentContext.Result.StackTrace;
            var status = TestContext.CurrentContext.Result.Outcome.Status;

            if (Driver != null)
            {
                if (status == TestStatus.Failed)
                {

                    //var screenshot = ScreenshotTaker.Take(Driver, TestContext.CurrentContext.Test.Name);
                    //var screenshot = ((ITakesScreenshot)Driver).GetScreenshot();
                    string base64 = ScreenshotTaker.Take(Driver, TestContext.CurrentContext.Test.Name);
                    string screenshotTitle = $"Failed Screenshot {DateTime.Now:yyyyMMdd_HHmmss}";
                    if (!string.IsNullOrEmpty(base64))
                    {
                        //test.Fail("Test Failed").AddScreenCaptureFromPath(screenshot);
                        //string reportDir = AppContext.BaseDirectory;
                        //string relativePath = Path.Combine("Screenshots", Path.GetFileName(screenshot));
                        // Adjuntar screenshot al test en ExtentReports
                        test?.Fail("Test Failed")
                            .AddScreenCaptureFromBase64String(base64, screenshotTitle);

                    }
                    if (!string.IsNullOrEmpty(stacktrace))
                        test?.Fail(stacktrace);
                }
                else if (status == TestStatus.Passed)
                {
                    test.Pass("Test passed");
                }

                // Cerrar driver seguro
                try { Driver.Quit(); } catch { }
                try { Driver.Dispose(); } catch { }

            }
            else
            {
                test?.Warning("Driver was not initialized; skipping TearDown actions.");
            }

        }catch (Exception ex)
            {
                    test?.Warning($"Exception in TearDown: {ex.Message}");
            }
    }

    


    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        extent.Flush();
    }
}