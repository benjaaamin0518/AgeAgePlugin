using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeAgePlugin.Properties
{


    internal sealed partial class Settings
    {
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public List<FormData> fd
        {
            get => (List<FormData>)this[nameof(fd)];
            set => this[nameof(fd)] = value;
        }
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public List<Customize> customize
        {
            get => (List<Customize>)this[nameof(customize)];
            set => this[nameof(customize)] = value;
        }
    }

}
