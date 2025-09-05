using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
namespace Framework.Config
{
    public class TestSettings
    {
        public string BaseUrl { get; set; } = "http://localhost:3000/";
        public string Browser { get; set; } = "chrome"; // chrome | edge | firefox
        public bool Headless { get; set; } = false;
        public int ExplicitWaitSec { get; set; } = 10;
        public int ImplicitWaitMs { get; set; } = 0;


        public static TestSettings Load(string? path = null)
        {
            var envBrowser = Environment.GetEnvironmentVariable("BROWSER");
            var envHeadless = Environment.GetEnvironmentVariable("HEADLESS");
            var envBaseUrl = Environment.GetEnvironmentVariable("BASEURL");


            path ??= Path.Combine(AppContext.BaseDirectory, "TestSettings.json");
            var settings = new TestSettings();


            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                var fileSettings = JsonSerializer.Deserialize<TestSettings>(json);
                if (fileSettings is not null) settings = fileSettings;
            }


            if (!string.IsNullOrWhiteSpace(envBrowser)) settings.Browser = envBrowser;
            if (!string.IsNullOrWhiteSpace(envHeadless) && bool.TryParse(envHeadless, out var h)) settings.Headless = h;
            if (!string.IsNullOrWhiteSpace(envBaseUrl)) settings.BaseUrl = envBaseUrl;


            return settings;
        }
    }
}
