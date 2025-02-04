using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
