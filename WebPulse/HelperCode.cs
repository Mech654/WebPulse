using System.IO;

namespace WebPulse
{
    public class HelperCode
    {
        public string GetJsonLocation()
        {
            return string.Join("\\", Directory.GetCurrentDirectory(), "SetupJson.json");
        }
    }
}
