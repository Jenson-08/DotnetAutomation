using Framework.Config;
using Framework.Data_Driven;
using Framework.Drivers;
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
    [TestFixture, Order(2)]
    public class CourseTests : TestBase
    {

        private DashboardPage dashboard;
        private CoursePage coursePage;
        private LoginPage loginPage;

        [OneTimeSetUp]
        public void LoginOnce()
        {
            Settings = TestSettings.Load();
            Driver = DriverFactory.Create(Settings);
            AuthHelper.LoginAsDefaultUser(Driver,0);

            dashboard = new DashboardPage(Driver);
            coursePage = new CoursePage(Driver);
            loginPage = new LoginPage(Driver, Settings.BaseUrl);

            
        }

        [Test, TestCaseSource(typeof(CourseData), nameof(CourseData.ValidCourses)), Order(1)]
        public void CreateCourse_ValidData_ShouldAppearInList(string title, string description, string estimatedTime, string materialsNeeded)
        {
           
            coursePage.GoToNewCourse();
            coursePage.CreateCourse(title, description, estimatedTime, materialsNeeded);
          
            dashboard.WaitForCourseToAppear(title);
            Assert.That(dashboard.GetCoursesTitles(), Does.Contain(title));
        }

        [Test, TestCaseSource(typeof(CourseData), nameof(CourseData.InvalidCourses)), Order(2)]
        public void CreateCourse_InvalidData_ShouldShowError(string title, string description, string estimatedTime, string materialsNeeded, string errorMessage)
        {

            coursePage.GoToNewCourse();
            coursePage.CreateCourse(title, description, estimatedTime, materialsNeeded);
            var actualError = coursePage.GetErrorMessage();
         
            Assert.That(coursePage.GetErrorMessage(), Does.Contain(errorMessage));

            dashboard.GoToDashboard();

        }

        [Test, TestCaseSource(typeof(CourseData), nameof(CourseData.EditCourses)), Order(3)]
        public void EditCourse_ExistingCourse_ShouldUpdateTitle(string oldTitle, string newTitle, string description, string time, string materials)
        {
           
            dashboard.WaitForCourseToAppear(oldTitle);
          
            coursePage.GoToCourse(oldTitle);
            coursePage.WaitForCourseDetails();
            coursePage.ClickEditCourseButton();
            coursePage.EditCourse(newTitle,description,time,materials);    
            dashboard.WaitForCourseToAppear(newTitle);
            Assert.That(dashboard.GetCoursesTitles(), Does.Contain(newTitle));

        }

        [Test, TestCaseSource(typeof(CourseData), nameof(CourseData.DeleteCourse)), Order(4)]
        public void DeleteCourse_ExistingCourse_ShouldRemoveFromList(string courseName)
        {
            dashboard.WaitForCourseToAppear(courseName);
            dashboard.GoToCourse(courseName);
            coursePage.ConfirmDelete();                            
            dashboard.WaitForCourseToDisappear(courseName);
            Assert.That(dashboard.GetCoursesTitles(), Does.Not.Contain(courseName));
        }

        [OneTimeTearDown]
        public void LogoutAfterAllTests()
        {
            try
            {
                if (loginPage != null)
                    loginPage.Logout();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[WARNING] Logout failed: {ex.Message}");
            }
        }



    }
}
