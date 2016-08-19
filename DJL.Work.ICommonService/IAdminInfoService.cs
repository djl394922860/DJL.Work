using DJL.Work.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DJL.Work.ICommonService
{
    public interface IAdminInfoService : IBaseService<AdminInfo>
    {
        AdminInfo Login(string name, string pwd);

        void UpdateAdminInfo(int Id,string pname, string premark, string pemail, string poldpwd, string pnewpwd);

        void UpdateAdminInfo(int Id,string pname, string premark, string pemail);

        void AddUserWithRoles(string uName, string uEmail, string pwdMd5, bool isDeleted, int[] roleIds,string remark);

        void UpdateStatus(int userId);

        void UpdateStatusEnable(int userId);

        int[] GetUserRoles(int userId);

        void SetUserRoles(int userId, int[] roleIds);
    }
}
