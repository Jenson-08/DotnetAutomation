using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using UI.Tests.Hooks;
using Framework.Pages;
using Framework.Data_Driven;

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

            AuthHelper.LoginAsDefaultUser(Driver,0);

            // Inicializar Page Objects
            dashboard = new DashboardPage(Driver);
            coursePage = new CoursePage(Driver);
            loginPage = new LoginPage(Driver, Settings.BaseUrl);

            
        }

        [Test, TestCaseSource(typeof(CourseData), nameof(CourseData.ValidCourses))]
        public void CreateCourse_ValidData_ShouldAppearInList(string title, string description, string estimatedTime, string materialsNeeded)
        {
            //var dashboard = new DashboardPage(Driver);
            //var coursePage = new CoursePage(Driver);

            //dashboard.GoTo(); // Si hace falta ir al dashboard
            coursePage.GoToNewCourse();
            coursePage.CreateCourse(title, description, estimatedTime, materialsNeeded);

            // Validar que el curso apareció en la lista
            Assert.That(dashboard.GetCoursesTitles(), Does.Contain(title));
        }

        [Test, TestCaseSource(typeof(CourseData), nameof(CourseData.InvalidCourses))]
        public void CreateCourse_InvalidData_ShouldShowError(string title, string description, string estimatedTime, string materialsNeeded)
        {
            //var coursePage = new CoursePage(Driver);

            coursePage.GoToNewCourse();
            coursePage.CreateCourse(title, description, estimatedTime, materialsNeeded);

            Assert.That(coursePage.GetErrorMessage(), Is.Not.Null.And.Not.Empty);
            dashboard.GoToDashboard(); // Volver al dashboard para el siguiente test
        }

        [Test, TestCaseSource(typeof(CourseData), nameof(CourseData.EditCourses))]
        public void EditCourse_ExistingCourse_ShouldUpdateTitle(string oldTitle, string newTitle, string description, string time, string materials)
        {
            //var dashboard = new DashboardPage(Driver);
            //var coursePage = new CoursePage(Driver);

            //dashboard.GoToDashboard();
            coursePage.GoToEditCourse(oldTitle); // Método que debes implementar en Page
            coursePage.EditCourse(newTitle,description,time,materials);     // Método que debes implementar en Page

            // Validar que el curso actualizado aparece
            Assert.That(dashboard.GetCoursesTitles(), Does.Contain(newTitle));
        }

        [Test, TestCaseSource(typeof(CourseData), nameof(CourseData.DeleteCourse))]
        public void DeleteCourse_ExistingCourse_ShouldRemoveFromList(string courseName)
        {
            //var dashboard = new DashboardPage(Driver);
            //var coursePage = new CoursePage(Driver);

            
            //coursePage.GoToDeleteCourse(courseToDelete); // Método en Page
            dashboard.GoToCourse(courseName);
            coursePage.ConfirmDelete();                   // Método en Page

            Assert.That(dashboard.GetCoursesTitles(), Does.Not.Contain(courseName));
            loginPage.Logout(); // Cerrar sesión al final de las pruebas
        }
    }
}
