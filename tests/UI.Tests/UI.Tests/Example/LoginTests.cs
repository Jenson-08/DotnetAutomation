using Framework.Config;
using Framework.Data_Driven;
using Framework.Pages;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Tests.Hooks;

namespace UI.Tests.Example
{
    [TestFixture, Order(1)]
    public class LoginTests : TestBase
    {
        [Test, TestCaseSource(typeof(LoginData), nameof(LoginData.ValidLogins))]
        public void Login_WithValidCredentials_ShouldRedirectToDashboard(string email, string password, string expectedMessage)
        {
            var loginPage = new LoginPage(Driver, Settings.BaseUrl);
            loginPage.GoTo();
            loginPage.Login(email, password);

            var dashboard = new DashboardPage(Driver);

            Assert.That(dashboard.GetWelcomeMessage(), Does.Contain(expectedMessage));
            loginPage.Logout();
        }

        [Test, TestCaseSource(typeof(LoginData), nameof(LoginData.InvalidLogins))]
        public void Login_WithInvalidCredentials_ShouldShowErrorMessage(string email, string password, string expectedError)
        {
            var loginPage = new LoginPage(Driver, Settings.BaseUrl);
            loginPage.GoTo();
            loginPage.Login(email, password);
            loginPage.WaitForErrorMessage();
            Assert.That(loginPage.GetErrorMessage(), Does.Contain(expectedError));

        }
    }
}

