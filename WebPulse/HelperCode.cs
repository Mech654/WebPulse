using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace WebPulse
{
    public class HelperCode
    {
        public string GetJsonLocation()
        {
            return string.Join("\\", Directory.GetCurrentDirectory(), "SetupJson.json");
        }

        public List<MyObject> GetSetupJsonObjects()
        {
            string path = GetJsonLocation();
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

        public void UpdateJsonValue(string targetName, string propertyName, string newValue)
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
            File.WriteAllText(GetJsonLocation(), newJson);
        }

    }

}
