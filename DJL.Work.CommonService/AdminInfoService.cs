using DJL.Work.DataModel;
using DJL.Work.ICommonService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DJL.Work.CommonService
{
    public class AdminInfoService : BaseService<AdminInfo>, IAdminInfoService
    {
        public AdminInfo Login(string name, string pwd)
        {
            using (var db = new MusicOnlineDBContext())
            {
                var admin = db.AdminInfos.FirstOrDefault(x => x.UserName == name && x.Password == pwd);
                if (admin == null) return null;
                db.Entry<AdminInfo>(admin).Collection<RoleInfo>(x => x.RoleInfos).Load();
                return admin;
            }
        }


        public void UpdateAdminInfo(int Id, string pname, string premark, string pemail, string poldpwd, string pnewpwd)
        {
            using (var db = new MusicOnlineDBContext())
            {
                var temp = db.AdminInfos.Find(Id);
                if (temp == null) throw new Exception("传递ID找不到对应管理员");
                if (!string.Equals(temp.Password, poldpwd, StringComparison.CurrentCultureIgnoreCase))
                {
                    throw new Exception("原密码错误");
                }
                temp.UserName = pname;
                temp.Remark = premark;
                temp.Email = pemail;
                temp.Password = pnewpwd;
                db.Entry(temp).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void UpdateAdminInfo(int Id, string pname, string premark, string pemail)
        {
            using (var db = new MusicOnlineDBContext())
            {
                var temp = db.AdminInfos.Find(Id);
                if (temp == null) throw new Exception("传递ID找不到对应管理员");
                temp.UserName = pname;
                temp.Remark = premark;
                temp.Email = pemail;
                db.Entry(temp).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }


        public void AddUserWithRoles(string uName, string uEmail, string pwdMd5, bool isDeleted, int[] roleIds, string remark)
        {
            using (var db = new MusicOnlineDBContext())
            {
                var roles = db.RoleInfos.Where(x => roleIds.Contains(x.ID)).ToArray();
                var newUser = new AdminInfo()
                {
                    UserName = uName,
                    Email = uEmail,
                    Password = pwdMd5,
                    Remark = remark,
                    DeleteFlag = isDeleted,
                    CreateTime = DateTime.Now
                };
                foreach (var item in roles)
                {
                    newUser.RoleInfos.Add(item);
                }
                db.AdminInfos.Add(newUser);
                db.SaveChanges();
            }
        }


        public void UpdateStatus(int userId)
        {
            using (var db = new MusicOnlineDBContext())
            {
                var temp = db.AdminInfos.Find(userId);
                if (temp == null) throw new Exception("找不到该管理员");
                temp.DeleteFlag = true;
                db.Entry(temp).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }


        public void UpdateStatusEnable(int userId)
        {
            using (var db = new MusicOnlineDBContext())
            {
                var temp = db.AdminInfos.Find(userId);
                if (temp == null) throw new Exception("找不到该管理员");
                temp.DeleteFlag = false;
                db.Entry(temp).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }


        public int[] GetUserRoles(int userId)
        {
            using (var db = new MusicOnlineDBContext())
            {
                var u = db.AdminInfos.Find(userId);
                if (u == null) throw new Exception("找不到管理员信息");
                return u.RoleInfos.Where(x => x.DeleteFlag == false).Select(x => x.ID).ToArray();
            }
        }


        public void SetUserRoles(int userId, int[] rolesId)
        {
            using (var db = new MusicOnlineDBContext())
            {
                var u = db.AdminInfos.Find(userId);
                if (u == null) throw new Exception("找不到管理员");
                foreach (var item in u.RoleInfos.ToArray())
                {
                    u.RoleInfos.Remove(item);
                }
                if (rolesId.Any())
                {
                    var roles = db.RoleInfos.Where(x => x.DeleteFlag == false).Where(x => rolesId.Contains(x.ID)).ToArray();
                    foreach (var item in roles)
                    {
                        u.RoleInfos.Add(item);
                    }
                }
                db.Entry(u).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}
