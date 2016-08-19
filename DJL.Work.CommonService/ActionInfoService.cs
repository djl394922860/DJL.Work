using DJL.Work.DataModel;
using DJL.Work.ICommonService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DJL.Work.CommonService
{
    public class ActionInfoService : BaseService<ActionInfo>, IActionInfoService
    {
        public void Edit(int id, byte method, string name, string requestUrl, bool isDeleted, string remark)
        {
            using (var db = new MusicOnlineDBContext())
            {
                var ac = db.ActionInfos.Find(id);
                if (ac == null) throw new Exception("参数ID错误");
                ac.HttpMethod = method;
                ac.ActionName = name;
                ac.RequestUrl = requestUrl;
                ac.DeleteFlag = isDeleted;
                ac.Remark = remark;
                db.Entry(ac).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }


        public object GetRolesByActionId(int actionId)
        {
            using (var db = new MusicOnlineDBContext())
            {
                var ac = db.ActionInfos.Find(actionId);
                if (ac == null) throw new Exception("找不到该角色");
                var data = ac.RoleInfos.Select(x => new { id = x.ID, text = x.RoleName }).ToArray();
                return data;
            }
        }
    }
}
