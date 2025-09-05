using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Pages
{
    public class CoursePage
    {
        private readonly IWebDriver _driver;

        public CoursePage(IWebDriver driver) => _driver = driver;

        private IWebElement NewCourseButton => _driver.FindElement(By.XPath("//span[@class='course--add--title' and contains(text(),'New Course')]"));
        private IWebElement TitleInput => _driver.FindElement(By.XPath("//input[@id='courseTitle']"));
        private IWebElement DescriptionInput => _driver.FindElement(By.XPath("//textarea[@id='courseDescription']"));
        private IWebElement EstimatedTimeInput => _driver.FindElement(By.XPath("//input[@id='estimatedTime']"));
        private IWebElement MaterialsNeededInput => _driver.FindElement(By.XPath("//textarea[@id='materialsNeeded']"));
        private IWebElement SubmitButton => _driver.FindElement(By.XPath("//button[@class='button' and @type='submit']"));

        private IWebElement CourseDetails => _driver.FindElement(By.XPath("//h2[text()='Course Detail']"));

        private IWebElement UpdateCourseButton => _driver.FindElement(By.XPath("//a[text()='Update Course']"));


        private IWebElement ErrorMessage
        {
            get
            {
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
                return wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//div[@class='validation--errors']/h3")
                ));
            }
        }


        //navigation
        public void GoToNewCourse() => NewCourseButton.Click();

        public void GoToCourse(string courseTitle)
        {
            // Buscar el curso por título y hacer click en Edit
            var course = _driver.FindElement(By.XPath($"//h3[text()='{courseTitle}']/ancestor::a"));
            course.Click(); // abre detalle del curso

            //_driver.FindElement(By.XPath("//a[text()='Update Course']")).Click();
        }

        public IWebElement WaitForCourseDetails(int timeoutSec = 10)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutSec));
            return wait.Until(drv =>
            {
                try
                {
                    var element = CourseDetails;
                    return element.Displayed ? element : null;
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
            });
        }
      

        public void ClickEditCourseButton()
        {
            UpdateCourseButton.Click();
        }

        public void GoToDeleteCourse(string courseTitle)
        {
            // Buscar el curso por título y abrirlo
            var course = _driver.FindElement(By.XPath($"//h3[text()='{courseTitle}']/ancestor::a"));
            course.Click();
        }

        //actions
        public void CreateCourse(string title, string description, string estimatedTime, string materialsNeeded)
        {
            TitleInput.Clear();
            TitleInput.SendKeys(title);

            DescriptionInput.Clear();
            DescriptionInput.SendKeys(description);

            EstimatedTimeInput.Clear();
            EstimatedTimeInput.SendKeys(estimatedTime);

            MaterialsNeededInput.Clear();
            MaterialsNeededInput.SendKeys(materialsNeeded);

            SubmitButton.Click();
        }
        public void EditCourse(string newTitle, string newDescription, string newEstimatedTime = null, string newMaterials = null)
        {
           
            TitleInput.Clear();
            TitleInput.SendKeys(newTitle);

            DescriptionInput.Clear();
            DescriptionInput.SendKeys(newDescription);
            

            if (newEstimatedTime != null)
            {
                EstimatedTimeInput.Clear();
                EstimatedTimeInput.SendKeys(newEstimatedTime);
            }

            if (newMaterials != null)
            {
                MaterialsNeededInput.Clear();
                MaterialsNeededInput.SendKeys(newMaterials);
            }

            SubmitButton.Click();
        }
        public void ConfirmDelete()
        {
            _driver.FindElement(By.XPath("//button[text()='Delete Course']")).Click();
        }


        public string GetErrorMessage() => ErrorMessage.Text;
    }
}
