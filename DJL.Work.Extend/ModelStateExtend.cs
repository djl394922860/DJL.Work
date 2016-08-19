using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DJL.Work.Extend
{
    public static class ModelStateExtend
    {
        public static string[] GetModelErrorMsgs(this ModelStateDictionary modelStateDictionary)
        {
            var strs = new List<string>();
            foreach (var item in modelStateDictionary.Values)
            {
                if (item.Errors.Count > 0)
                {
                    foreach (var e in item.Errors)
                    {
                        strs.Add(e.ErrorMessage);
                    }
                }
            }
            return strs.ToArray();
        }
    }
}
