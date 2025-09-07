using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Pages
{
    public class DashboardPage
    {
        private readonly IWebDriver _driver;
        private readonly int _explicitWaitSec;

        public DashboardPage(IWebDriver driver, int explicitWaitSec = 10)
        {
            _driver = driver;
            _explicitWaitSec = explicitWaitSec;
        }


        private IWebElement DashboardButton => _driver.FindElement(By.XPath("//a[text()='Courses']"));
        private IWebElement WelcomeMessage => _driver.FindElement(By.XPath("//ul[@class='header--signedin']/li[contains(text(),'Welcome')]"));

        public void GoToDashboard() => DashboardButton.Click();


        public List<string> GetCoursesTitles()
        {
            List<string> courseTitles = new List<string>();

            
            var courses = _driver.FindElements(By.XPath("//a[contains(@class,'course--module') and not(contains(@class,'course--add--module'))]"));

            foreach (var course in courses)
            {
                
                var title = course.FindElement(By.XPath(".//h3[@class='course--title']")).Text;
                courseTitles.Add(title);
            }

            return courseTitles;
        }

        public void WaitForCourseToAppear(string courseTitle, int timeoutSec = 5)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutSec));
            wait.Until(d =>
            {
                try
                {
                    return GetCoursesTitles().Contains(courseTitle);
                }
                catch
                {
                    return false;
                }
            });
        }

        public void WaitForCourseToDisappear(string courseTitle, int timeoutSec = 5)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutSec));
            wait.Until(d =>
            {
                try
                {
                    return !GetCoursesTitles().Contains(courseTitle);
                }
                catch
                {
                    return true; 
                }
            });
        }

        public void GoToCourse(string courseTitle)
        {
           
            var courseLink = _driver.FindElement(By.XPath($"//h3[text()='{courseTitle}']/ancestor::a[contains(@class,'course--module')]"));
            courseLink.Click();
        }

        public string GetWelcomeMessage(bool waitForLoad = true)
        {
            if (waitForLoad)
            {
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(_explicitWaitSec));
                wait.Until(d =>
                {
                    try
                    {
                        return !string.IsNullOrEmpty(WelcomeMessage.Text);
                    }
                    catch (NoSuchElementException)
                    {
                        return false;
                    }
                });
            }

            return WelcomeMessage.Text;
        }

    }
}
