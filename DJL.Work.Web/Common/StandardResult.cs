using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DJL.Work.Web.Common
{
    [Serializable]
    public class StandardResult
    {
        public bool success;
        public string message;

        public StandardResult(bool state = false)
        {
            this.success = state;
        }
    }
}