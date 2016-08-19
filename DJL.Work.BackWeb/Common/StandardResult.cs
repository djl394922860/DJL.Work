using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DJL.Work.BackWeb.Common
{
    [Serializable]
    public class StandardResult
    {
        public bool success;
        public string message;

        public StandardResult(bool state = false, string message = "操作成功")
        {
            this.success = state;
            this.message = message;
        }
    }
}