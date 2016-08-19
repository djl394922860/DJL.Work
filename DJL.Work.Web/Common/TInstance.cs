using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DJL.Work.Web.Common
{
    public class TInstance<T> where T : class,new()
    {
        private static readonly T instance = new T();

        public static T Instance
        {
            get { return instance; }
        }
    }
}