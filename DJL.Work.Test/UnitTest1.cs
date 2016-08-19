using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DJL.Work.DataModel;
using System.Linq;

namespace DJL.Work.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //初始化测试数据
            using (var db = new MusicOnlineDBContext())
            {
                //actions
                var action = new ActionInfo()
                {
                    ActionName = "新增管理员",
                    RequestUrl = "/Permission/SaveUser",
                    HttpMethod = 1,
                    Remark = "新增管理员",
                    DeleteFlag = false,
                    CreateTime = DateTime.Now
                };
                var role = new RoleInfo()
                {
                    RoleName = "administrator",
                    Remark = "超级角色",
                    DeleteFlag = false,
                    CreateTime = DateTime.Now
                };
                role.ActionInfos.Add(action);
                var admin = new AdminInfo()
                {
                    UserName = "administrator",
                    Password = "d651adde28622a8ecb52f545eb0027a4",
                    Email = "394922860@qq.com",
                    Remark = "超级管理员",
                    CreateTime = DateTime.Now,
                    DeleteFlag = false
                };
                admin.RoleInfos.Add(role);
                db.AdminInfos.Add(admin);
                db.Members.Add(new Member() { ActiveStatus = true, LoginEmail = "972417907@qq.com", NickName = "djlmusic", Password = "d651adde28622a8ecb52f545eb0027a4" });
                //111111djllovemusic
                db.SaveChanges();
            }
        }
    }
}
