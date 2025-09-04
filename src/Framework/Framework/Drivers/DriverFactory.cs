using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using Framework.Config;
namespace Framework.Drivers
{
    public static class DriverFactory
    {
        public static IWebDriver Create(TestSettings s)
        {
            IWebDriver driver = s.Browser.ToLowerInvariant() switch
            {
                "edge" => CreateEdge(s),
                "firefox" => CreateFirefox(s),
                _ => CreateChrome(s)
            };


            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(s.ImplicitWaitMs);
            driver.Manage().Window.Size = new System.Drawing.Size(1920, 1080);
            return driver;
        }


        private static IWebDriver CreateChrome(TestSettings s)
        {
            var options = new ChromeOptions();
            if (s.Headless) options.AddArgument("--headless=new");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--window-size=1920,1080");
            return new ChromeDriver(options); // Selenium Manager resuelve el driver
        }


        private static IWebDriver CreateEdge(TestSettings s)
        {
            var options = new EdgeOptions();
            if (s.Headless) options.AddArgument("--headless=new");
            options.AddArgument("--window-size=1920,1080");
            return new EdgeDriver(options);
        }


        private static IWebDriver CreateFirefox(TestSettings s)
        {
            var options = new FirefoxOptions();
            if (s.Headless) options.AddArgument("-headless");
            return new FirefoxDriver(options);
        }
    }
}
