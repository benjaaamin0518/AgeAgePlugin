using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AgeAgePlugin
{
    public class CustomizeJsonData
    {
        public string app { get; set; }
        public string scope { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public JsonObjectCustom desktop { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public JsonObjectCustom mobile { get; set; }
    }
}
