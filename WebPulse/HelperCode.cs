using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using WebPulse.models;

namespace WebPulse
{
    public class HelperCode
    {
        public string GetSetupJson()
        {
            return string.Join("\\", Directory.GetCurrentDirectory(), "SetupJson.json");
        }
        public string GetReleaseJson()
        {
            return string.Join("\\", Directory.GetCurrentDirectory(), "ReleaseJson.json");
        }
        
        public List<MyObject> GetSetupJsonObjects()
        {
            string path = GetSetupJson();
            if (!File.Exists(path))
            {
                using (File.Create(path)) { }
                File.WriteAllText(path, "[]");
                return new List<MyObject>();
            }
            Debug.WriteLine("Loading JSON configuration...");
            string existingJson = File.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(existingJson))
            {
                Debug.WriteLine("Warning: Config file is empty.");
                return new List<MyObject>();
            }
            var objects = JsonConvert.DeserializeObject<List<MyObject>>(existingJson);
            if (objects == null || objects.Count == 0)
            {
                Debug.WriteLine("Warning: No valid objects found in JSON.");
                return new List<MyObject>();
            }
            return objects;
        }
        
        public List<MyReleases> GetReleaseJsonObjects()
        {
            Debug.WriteLine("Requested Setup objects...");
            string path = GetReleaseJson();
            if (!File.Exists(path))
            {
                using (File.Create(path)) { }
                File.WriteAllText(path, "[]");
                return new List<MyReleases>();
            }
            string existingJson = File.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(existingJson))
            {
                Debug.WriteLine("Warning: Config file is empty.");
                return new List<MyReleases>();
            }
            var objects = JsonConvert.DeserializeObject<List<MyReleases>>(existingJson);
            if (objects == null || objects.Count == 0)
            {
                Debug.WriteLine("Warning: No valid objects found in JSON.");
                return new List<MyReleases>();
            }
            return objects;
        }
        
        public void UpdateSetupJsonValue(string targetName, string propertyName, string newValue)
        {
            var objects = GetSetupJsonObjects();
            if (objects == null)
                return;
            var obj = objects.FirstOrDefault(o => string.Equals(o.Name, targetName, StringComparison.OrdinalIgnoreCase));
            if (obj == null)
                return;
            var prop = typeof(MyObject).GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (prop != null && prop.CanWrite)
                prop.SetValue(obj, newValue);
            string newJson = JsonConvert.SerializeObject(objects, Formatting.Indented);
            File.WriteAllText(GetSetupJson(), newJson);
        }
        
        public void UpdateReleaseJsonValue(string targetName)
        {
            var objects = GetReleaseJsonObjects();
            if (objects == null)
                return;
            var obj = objects.FirstOrDefault(o => string.Equals(o.Name, targetName, StringComparison.OrdinalIgnoreCase));
            if (obj == null)
                return;
            var prop = typeof(MyReleases).GetProperty("Count", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (prop != null && prop.CanWrite)
                prop.SetValue(obj, "newValue");
            string newJson = JsonConvert.SerializeObject(objects, Formatting.Indented);
            File.WriteAllText(GetSetupJson(), newJson);
        }
        
        public void IncrementSetupJsonCount(string targetName)
        {
            var objects = GetSetupJsonObjects();
            if (objects == null)
                return;
            var obj = objects.FirstOrDefault(o => string.Equals(o.Name, targetName, StringComparison.OrdinalIgnoreCase));
            if (obj == null)
                return;
            var countProperty = typeof(MyObject).GetProperty("Count", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (countProperty != null && countProperty.CanWrite && countProperty.PropertyType == typeof(string))
            {
                string currentValueStr = (string)countProperty.GetValue(obj);
                int currentValue;
                if (!int.TryParse(currentValueStr, out currentValue))
                    currentValue = 0;
                countProperty.SetValue(obj, (currentValue + 1).ToString());
            }
            string newJson = JsonConvert.SerializeObject(objects, Formatting.Indented);
            File.WriteAllText(GetSetupJson(), newJson);
        }

        
    }
}
