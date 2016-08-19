using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJL.Work.Enum
{
    public enum MusicLanguage
    {
        [Description("中文")]
        Chinese = 1,
        [Description("英文")]
        English = 2,
        [Description("日本")]
        Japan = 3
    }
}
