using DJL.Work.DataModel;
using DJL.Work.ICommonService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DJL.Work.CommonService
{
    public class RoleInfoService : BaseService<RoleInfo>, IRoleInfoService
    {
        public int[] GetActionsByRoleId(int roleId)
        {
            using (var db = new MusicOnlineDBContext())
            {
                var r = db.RoleInfos.Find(roleId);
                if (r == null) throw new Exception("找不到该角色");
                var arr = r.ActionInfos.Where(x => x.DeleteFlag == false).Select(x => x.ID).ToArray();
                return arr;
            }
        }


        public object GetUsersByRoleId(int roleId)
        {
            using (var db = new MusicOnlineDBContext())
            {
                var r = db.RoleInfos.Find(roleId);
                if (r == null) throw new Exception("找不到该角色");
                var data = r.AdminInfos.Select(x => new { id = x.ID, text = x.UserName }).ToArray();
                return data;
            }
        }


        public void SetRoleActions(int roleId, int[] actionsId)
        {
            using (var db = new MusicOnlineDBContext())
            {
                var r = db.RoleInfos.Find(roleId);
                if (r == null) throw new Exception("找不到该角色");
                foreach (var item in r.ActionInfos.ToArray())
                {
                    r.ActionInfos.Remove(item);
                }
                if (actionsId.Any())
                {
                    var actins = db.ActionInfos.Where(x => x.DeleteFlag == false).Where(x => actionsId.Contains(x.ID)).ToArray();
                    foreach (var item in actins)
                    {
                        r.ActionInfos.Add(item);
                    }
                }
                db.Entry(r).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }


        public void EditRole(int id, bool isdeleted, string remark, int[] actions, string name)
        {
            using (var db = new MusicOnlineDBContext())
            {
                var r = db.RoleInfos.Find(id);
                if (r == null) throw new Exception("fuck,角色不见了");
                foreach (var item in r.ActionInfos.ToArray())
                {
                    r.ActionInfos.Remove(item);
                }
                foreach (var item in db.ActionInfos.Where(x => x.DeleteFlag == false).Where(x => actions.Contains(x.ID)).ToArray())
                {
                    r.ActionInfos.Add(item);
                }
                r.DeleteFlag = isdeleted;
                r.Remark = remark;
                r.RoleName = name;
                db.Entry(r).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }


        public void AddRoleWithActions(string name, string remark, bool isdeleted, int[] acs)
        {
            using (var db = new MusicOnlineDBContext())
            {
                var actions = db.ActionInfos.Where(x => x.DeleteFlag == false).Where(x => acs.Contains(x.ID)).ToArray();
                var temp = new RoleInfo()
                {
                    Remark = remark,
                    RoleName = name,
                    DeleteFlag = isdeleted,
                    CreateTime = DateTime.Now
                };
                foreach (var item in actions)
                {
                    temp.ActionInfos.Add(item);
                }
                db.RoleInfos.Add(temp);
                db.SaveChanges();
            }
        }


        public IEnumerable<ActionInfo> GetActionsByRoleNames(string[] Roles)
        {
            using (var db = new MusicOnlineDBContext())
            {
                var roles = db.RoleInfos.Where(x => Roles.Contains(x.RoleName) && x.DeleteFlag == false).ToArray();
                var actions = new List<ActionInfo>();
                foreach (var item in roles)
                {
                    actions.AddRange(item.ActionInfos.Where(x => x.DeleteFlag == false));
                }
                var ids = actions.Select(x => x.ID).Distinct();
                actions = actions.Where(x => ids.Contains(x.ID)).ToList();
                return actions;
            }
        }
    }
}
