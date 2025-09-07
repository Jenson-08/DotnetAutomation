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
            return driver;
        }
        private static IWebDriver CreateChrome(TestSettings s)
        {
            var options = new ChromeOptions();
            if (s.Headless)
            {
                options.AddArgument("--headless=new");
                options.AddArgument("--window-size=1920,1080");
            }
            else
            {
                options.AddArgument("--disable-gpu");
                options.AddArgument("--no-sandbox");
            }

            var driver = new ChromeDriver(options);
            if (!s.Headless) driver.Manage().Window.Maximize(); 
            return driver;
        }

        private static IWebDriver CreateEdge(TestSettings s)
        {
            var options = new EdgeOptions();
            if (s.Headless)
            {
                options.AddArgument("--headless=new");
                options.AddArgument("--window-size=1920,1080");
            }

            var driver = new EdgeDriver(options);
            if (!s.Headless) driver.Manage().Window.Maximize();
            return driver;
        }

        private static IWebDriver CreateFirefox(TestSettings s)
        {
            var options = new FirefoxOptions();
            if (s.Headless)
            {
                options.AddArgument("-headless");
                options.AddArgument("--width=1920");
                options.AddArgument("--height=1080");
            }

            var driver = new FirefoxDriver(options);
            if (!s.Headless) driver.Manage().Window.Maximize();
            return driver;
        }
    }
}
