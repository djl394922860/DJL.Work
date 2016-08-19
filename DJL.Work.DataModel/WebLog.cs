using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DJL.Work.DataModel
{
    public partial class WebLog
    {
        public WebLog()
        {
            CreateTime = DateTime.Now;
        }
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public string Message { get; set; }
    }
}
