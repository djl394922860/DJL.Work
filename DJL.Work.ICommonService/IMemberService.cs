using DJL.Work.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DJL.Work.ICommonService
{
    public interface IMemberService : IBaseService<Member>
    {
        void Register(string email, string nickName, string password);
        bool Active(string email);
        bool UpdatePwdByEmail(string email,string defaultPassword);
        bool CheckActiveOrExist(string email);

        Member Login(string p, string md5);
    }
}
