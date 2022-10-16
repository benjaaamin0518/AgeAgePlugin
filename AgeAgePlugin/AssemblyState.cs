using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeAgePlugin
{
    internal static class AssemblyState
    {
        public const bool IsDebug =
            #if DEBUG
                 true;
            #else
                false;
            #endif
    }
}
