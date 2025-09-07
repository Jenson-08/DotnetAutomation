using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace Framework.Pages
{
    public class CoursePage
    {
        private readonly IWebDriver _driver;
        private readonly int _defaultWaitSec = 10;

        public CoursePage(IWebDriver driver) => _driver = driver;

        private IWebElement NewCourseButton => WaitAndFindClickable(By.XPath("//span[@class='course--add--title' and contains(text(),'New Course')]"));
        private IWebElement TitleInput => WaitAndFindVisible(By.XPath("//input[@id='courseTitle']"));
        private IWebElement DescriptionInput => WaitAndFindVisible(By.XPath("//textarea[@id='courseDescription']"));
        private IWebElement EstimatedTimeInput => WaitAndFindVisible(By.XPath("//input[@id='estimatedTime']"));
        private IWebElement MaterialsNeededInput => WaitAndFindVisible(By.XPath("//textarea[@id='materialsNeeded']"));
        private IWebElement SubmitButton => WaitAndFindClickable(By.XPath("//button[@class='button' and @type='submit']"));

        private IWebElement CourseDetails => WaitAndFindVisible(By.XPath("//h2[text()='Course Detail']"));
        private IWebElement UpdateCourseButton => WaitAndFindClickable(By.XPath("//a[text()='Update Course']"));

        private IWebElement ErrorMessage => WaitAndFindVisible(By.XPath("//div[@class='validation--errors']/h3"), 5);

     
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

   
        public void GoToNewCourse() => NewCourseButton.Click();

        public void GoToCourse(string courseTitle)
        {
            var course = WaitAndFindClickable(By.XPath($"//h3[text()='{courseTitle}']/ancestor::a"));
            course.Click(); 
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

        public void ClickEditCourseButton() => UpdateCourseButton.Click();

        public void GoToDeleteCourse(string courseTitle)
        {
            var course = WaitAndFindClickable(By.XPath($"//h3[text()='{courseTitle}']/ancestor::a"));
            course.Click();
        }

        public void CreateCourse(string title, string description, string estimatedTime, string materialsNeeded)
        {
            WaitAndSendKeys(TitleInput, title);
            WaitAndSendKeys(DescriptionInput, description);
            WaitAndSendKeys(EstimatedTimeInput, estimatedTime);
            WaitAndSendKeys(MaterialsNeededInput, materialsNeeded);

            SubmitButton.Click();
        }

        public void EditCourse(string newTitle, string newDescription, string newEstimatedTime = null, string newMaterials = null)
        {
            WaitAndSendKeys(TitleInput, newTitle);
            WaitAndSendKeys(DescriptionInput, newDescription);

            if (newEstimatedTime != null)
                WaitAndSendKeys(EstimatedTimeInput, newEstimatedTime);

            if (newMaterials != null)
                WaitAndSendKeys(MaterialsNeededInput, newMaterials);

            SubmitButton.Click();
        }

        public void ConfirmDelete()
        {
            var deleteButton = WaitAndFindClickable(By.XPath("//button[text()='Delete Course']"));
            deleteButton.Click();
        }

        public string GetErrorMessage() => ErrorMessage.Text;
        private void WaitAndSendKeys(IWebElement element, string text)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(_defaultWaitSec));
            wait.Until(drv => element.Displayed && element.Enabled);
            element.Clear();
            element.SendKeys(text);
        }
    }
}
