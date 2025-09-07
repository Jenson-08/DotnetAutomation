using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace Framework.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver _driver;
        private readonly string _baseUrl;
        private readonly int _defaultWaitSec = 10;

        public LoginPage(IWebDriver driver, string baseUrl)
        {
            _driver = driver;
            _baseUrl = baseUrl;
        }

        private IWebElement EmailInput => WaitAndFindVisible(By.XPath("//input[@id='emailAddress']"));
        private IWebElement PasswordInput => WaitAndFindVisible(By.XPath("//input[@id='password']"));
        private IWebElement SignInButton => WaitAndFindClickable(By.XPath("//button[text()='Sign In']"));
        private IWebElement SignUpButton => WaitAndFindClickable(By.XPath("//a[text()='Sign Out']"));
        private IWebElement ErrorMessage => WaitAndFindVisible(By.XPath("//div[@class='validation--errors']"));

        public void GoTo()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}signin");

            WaitAndFindVisible(By.XPath("//input[@id='emailAddress']"));
        }

        public void Login(string email, string password)
        {
            WaitAndSendKeys(EmailInput, email);
            WaitAndSendKeys(PasswordInput, password);
            SignInButton.Click();
        }

        public void Logout() => SignUpButton.Click();

        public string GetErrorMessage() => ErrorMessage.Text;

        public IWebElement WaitForErrorMessage(int timeoutSec = 10)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutSec));
            return wait.Until(drv =>
            {
                try
                {
                    var element = ErrorMessage;
                    return element.Displayed ? element : null;
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
            });
        }

        private IWebElement WaitAndFindVisible(By by, int timeoutSec = -1)
        {
            var waitTime = timeoutSec > 0 ? timeoutSec : _defaultWaitSec;
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(waitTime));
            return wait.Until(ExpectedConditions.ElementIsVisible(by));
        }

        private IWebElement WaitAndFindClickable(By by, int timeoutSec = -1)
        {
            var waitTime = timeoutSec > 0 ? timeoutSec : _defaultWaitSec;
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(waitTime));
            return wait.Until(ExpectedConditions.ElementToBeClickable(by));
        }

        private void WaitAndSendKeys(IWebElement element, string text)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(_defaultWaitSec));
            wait.Until(drv => element.Displayed && element.Enabled);
            element.Clear();
            element.SendKeys(text);
        }
    }
}
