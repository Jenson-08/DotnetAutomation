using Framework.Config;
using Framework.Data_Driven;
using Framework.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace UI.Tests.Hooks
{
    internal static class AuthHelper
    {
        public static void LoginAsDefaultUser(IWebDriver driver, int userIndex = 0)
        {
            var loginData = (object[])LoginData.ValidLogins[userIndex];
            string email = loginData[0].ToString();
            string password = loginData[1].ToString();
            string expectedMessage = loginData[2]!.ToString();

            var loginPage = new LoginPage(driver, TestSettings.Load().BaseUrl);
            loginPage.GoTo();
            loginPage.Login(email, password);

            // opcional: esperar a que cargue dashboard
            var dashboard = new DashboardPage(driver);
            Assert.That(dashboard.GetWelcomeMessage(), Does.Contain(expectedMessage));
        }
    }
}
