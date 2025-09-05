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

            // Inicializar Page Objects
            dashboard = new DashboardPage(Driver);
            coursePage = new CoursePage(Driver);
            loginPage = new LoginPage(Driver, Settings.BaseUrl);

            
        }

        [Test, TestCaseSource(typeof(CourseData), nameof(CourseData.ValidCourses)), Order(1)]
        public void CreateCourse_ValidData_ShouldAppearInList(string title, string description, string estimatedTime, string materialsNeeded)
        {
            //var dashboard = new DashboardPage(Driver);
            //var coursePage = new CoursePage(Driver);

            //dashboard.GoTo(); // Si hace falta ir al dashboard
            coursePage.GoToNewCourse();
            coursePage.CreateCourse(title, description, estimatedTime, materialsNeeded);
            //Thread.Sleep(5000);
            // Validar que el curso apareció en la lista
            dashboard.WaitForCourseToAppear(title);
            Assert.That(dashboard.GetCoursesTitles(), Does.Contain(title));
        }

        [Test, TestCaseSource(typeof(CourseData), nameof(CourseData.InvalidCourses)), Order(2)]
        public void CreateCourse_InvalidData_ShouldShowError(string title, string description, string estimatedTime, string materialsNeeded, string errorMessage)
        {
            //var coursePage = new CoursePage(Driver);

            coursePage.GoToNewCourse();
            coursePage.CreateCourse(title, description, estimatedTime, materialsNeeded);
            var actualError = coursePage.GetErrorMessage();
            Console.WriteLine($"[DEBUG] ErrorMessage real: '{actualError}'");
            Console.WriteLine($"[DEBUG] ErrorMessage esperado: '{errorMessage}'");

            Assert.That(coursePage.GetErrorMessage(), Does.Contain(errorMessage));
            //Thread.Sleep(5000);
            dashboard.GoToDashboard(); // Volver al dashboard para el siguiente test

        }

        [Test, TestCaseSource(typeof(CourseData), nameof(CourseData.EditCourses)), Order(3)]
        public void EditCourse_ExistingCourse_ShouldUpdateTitle(string oldTitle, string newTitle, string description, string time, string materials)
        {
            //var dashboard = new DashboardPage(Driver);
            //var coursePage = new CoursePage(Driver);
            dashboard.WaitForCourseToAppear(oldTitle);
            //dashboard.GoToDashboard();
            coursePage.GoToCourse(oldTitle); // Método que debes implementar en Page
            coursePage.WaitForCourseDetails();
            coursePage.ClickEditCourseButton();
            coursePage.EditCourse(newTitle,description,time,materials);     // Método que debes implementar en Page
            dashboard.WaitForCourseToAppear(newTitle);
            // Validar que el curso actualizado aparece
            Assert.That(dashboard.GetCoursesTitles(), Does.Contain(newTitle));
            //Thread.Sleep(5000);
        }

        [Test, TestCaseSource(typeof(CourseData), nameof(CourseData.DeleteCourse)), Order(4)]
        public void DeleteCourse_ExistingCourse_ShouldRemoveFromList(string courseName)
        {
            //var dashboard = new DashboardPage(Driver);
            //var coursePage = new CoursePage(Driver);

            dashboard.WaitForCourseToAppear(courseName);
            //coursePage.GoToDeleteCourse(courseToDelete); // Método en Page
            dashboard.GoToCourse(courseName);
            coursePage.ConfirmDelete();                   // Método en Page
            //Thread.Sleep(5000);
            dashboard.WaitForCourseToDisappear(courseName);
            Assert.That(dashboard.GetCoursesTitles(), Does.Not.Contain(courseName));
            //loginPage.Logout(); // Cerrar sesión al final de las pruebas
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
