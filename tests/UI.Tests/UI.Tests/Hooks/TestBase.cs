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
        try
        {
           
            string projectDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..");
            string reportPath = Path.Combine(projectDir, "Reports", "ExtentReport.html");
            var htmlReporter = new ExtentSparkReporter(reportPath);
            htmlReporter.Config.DocumentTitle = "Automation Report";
            htmlReporter.Config.ReportName = "UI Test Report";

            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing ExtentReports: {ex.Message}");
            throw;
        }
    }

    [SetUp]
    public virtual void SetUp()
    {
        try
        {
            Settings = TestSettings.Load();
            if (Driver == null)
            {
                Driver = DriverFactory.Create(Settings);
                Driver.Navigate().GoToUrl(Settings.BaseUrl);
            }
            test = extent.CreateTest(TestContext.CurrentContext.Test.Name);

        }
        catch (Exception ex)
        {
            if (extent != null)
                test = extent.CreateTest(TestContext.CurrentContext.Test.Name);
            test?.Fail($"SetUp failed: {ex.Message}");
            throw;
        }
    }


    [TearDown]
    public void TearDown()
    {
        try { 
            var stacktrace = TestContext.CurrentContext.Result.StackTrace;
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var message = TestContext.CurrentContext.Result.Message;

            if (Driver != null)
            {
                if (status == TestStatus.Failed)
                {

                   
                    string base64 = ScreenshotTaker.Take(Driver, TestContext.CurrentContext.Test.Name);
                    string screenshotTitle = $"Failed Screenshot {DateTime.Now:yyyyMMdd_HHmmss}";
                    if (!string.IsNullOrEmpty(base64))
                    {
                        
                        test?.Fail("Test Failed")
                            .AddScreenCaptureFromBase64String(base64, screenshotTitle);

                    }
                    if (!string.IsNullOrEmpty(stacktrace))
                        test?.Fail($"**Error Message:** {message}")
                        .Fail($"**StackTrace:** {stacktrace}");
                }
                else if (status == TestStatus.Passed)
                {
                    test?.Pass("Test passed");
                }
                
                if (this.GetType().Name.Contains("LoginTests"))
                {
                    try { Driver.Quit(); } catch { }
                    try { Driver.Dispose(); } catch { }
                    Driver = null!;
                }

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
        try
        {
            extent.Flush();
            if (Driver != null)
            {
                try { Driver.Quit(); } catch { }
                try { Driver.Dispose(); } catch { }
                Driver = null!;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in OneTimeTearDown: {ex.Message}");
        }
    }
}
