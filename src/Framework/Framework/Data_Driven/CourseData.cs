using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Framework.Data_Driven
{
    public static class CourseData
    {
        // Positives cases for creating courses
        public static object[] ValidCourses =
        {
            new object[] { "Selenium course", "Automation with Selenium", "12.5h", "Computer and internet access" },
            new object[] { "C# course", "C# foundations and good practices", "8h", "Computer and internet access" }
        };

        // Negative cases for creating courses
        public static object[] InvalidCourses =
        {
            new object[] { "", "Untitled description","","" },
            new object[] { "Title without description", "" , "", ""}
        };

        // Editing courses
        public static object[] EditCourses =
        {
            new object[] { "Selenium course","Selenium II" ,"Advanced Selenium Course", "24.5h", "Computer,internet access and Selenium foundations" },
            new object[] { "C# course", "C# Course II","Intermediate C# Course", "18h", "Computer,internet access and C# foundations" }
        };

        // Delete course
        public static object[] DeleteCourse =
        {
            new object[] { "Selenium II" }
        };
    }
}
