using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    public class JsonObjectConfig
    {
        
        public string html { get; set; }
        public List<string> js { get; set; }
        public List<string> css { get; set; }
        public List<string> required_params { get; set; }
    }
}
