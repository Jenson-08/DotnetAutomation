using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
namespace Framework.Utils
{
    public static class ScreenshotTaker
    {
        public static string Take(IWebDriver driver, string testName)
        {
            try
            {
                var ss = ((ITakesScreenshot)driver).GetScreenshot();
             
                return ss.AsBase64EncodedString;
            }
            catch
            {
                return string.Empty;
            }
        }


        private static string Sanitize(string s)
        => string.Concat(s.Where(c => !Path.GetInvalidFileNameChars().Contains(c)));
    }
}
