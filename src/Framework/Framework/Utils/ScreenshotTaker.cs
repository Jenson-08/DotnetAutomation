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
                //var dir = Path.Combine(AppContext.BaseDirectory, "Screenshots");
                //Directory.CreateDirectory(dir);
                //var file = Path.Combine(dir, $"{Sanitize(testName)}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                //ss.SaveAsFile(file);
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
