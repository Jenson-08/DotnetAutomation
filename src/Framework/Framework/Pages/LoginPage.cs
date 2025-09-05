using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Framework.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver _driver;
        private readonly string _baseUrl;

        public LoginPage(IWebDriver driver, string baseUrl)
        {
            _driver = driver;
            _baseUrl = baseUrl;
        }

        private IWebElement EmailInput => _driver.FindElement(By.XPath("//input[@id='emailAddress']"));
        private IWebElement PasswordInput => _driver.FindElement(By.XPath("//input[@id='password']"));
        private IWebElement SignInButton => _driver.FindElement(By.XPath("//button[text()='Sign In']"));

        private IWebElement SignUpButton => _driver.FindElement(By.XPath("//a[text()='Sign Out']"));
        private IWebElement ErrorMessage => _driver.FindElement(By.XPath("//div[@class='validation--errors']"));

        public void GoTo() => _driver.Navigate().GoToUrl($"{_baseUrl}signin");

        public void Login(string email, string password)
        {
            EmailInput.Clear();
            EmailInput.SendKeys(email);

            PasswordInput.Clear();
            PasswordInput.SendKeys(password);

            SignInButton.Click();
        }
        public void Logout(){ SignUpButton.Click(); }
        

        public string GetErrorMessage() => ErrorMessage.Text;
    }
}