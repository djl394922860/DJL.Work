using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DJL.Work.BackWeb.Common
{
    public class PageResultModel<T> where T : class,new()
    {
        public IEnumerable<T> rows;
        public int total;
    }
}