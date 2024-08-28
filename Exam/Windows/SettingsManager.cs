using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
namespace Exam.Windows
{
    public class ApplicationSettings
    {
        public Dictionary<string, WindowSettings> Windows { get; set; } = new Dictionary<string, WindowSettings>();
    }

    public class WindowSettings
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public double Top { get; set; }
        public double Left { get; set; }
        public bool IsMaximized { get; set; }
    }
    public static class SettingsManager
    {
        private static readonly string SettingsFilePath = "WindowSettings.json";

        public static ApplicationSettings LoadSettings()
        {
            if (File.Exists(SettingsFilePath))
            {
                try
                {
                    string json = File.ReadAllText(SettingsFilePath);
                    return JsonConvert.DeserializeObject<ApplicationSettings>(json);
                }
                catch (Exception)
                {
                    // мяу
                }
            }
            return new ApplicationSettings();
        }

        public static void SaveSettings(ApplicationSettings settings)
        {
            try
            {
                string json = JsonConvert.SerializeObject(settings, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(SettingsFilePath, json);
            }
            catch (Exception ex)
            {
                // мяу
            }
        }
    }
}
