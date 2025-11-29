using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Helpers
{
    public  class ConnectionConfigHelper
    {
        private static readonly string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MinistopApp");
        private static readonly  string filePath = Path.Combine(folderPath, "connection.json");
        public static AppConfig Load()
        {
            if (!File.Exists(filePath))
            {
                return new AppConfig();
            }
            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<AppConfig>(json);
        }
        public static void Save(AppConfig config)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            string json = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }
}
