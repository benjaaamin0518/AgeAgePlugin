using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    public class ManifestJsonData
    {
        public int manifest_version { get; set; }
        public decimal version { get; set; }
        public string type { get; set; }
        public JsonObjectLanguage name { get; set; }
        public JsonObjectLanguage description { get; set; }
        public string icon { get; set; }
        public JsonObjectLanguage homepage_url { get; set; }
        public JsonObjectCustom desktop { get; set; }
        public JsonObjectCustom mobile { get; set; }
        public JsonObjectConfig config { get; set; }
    }
}
