using DJL.Work.DataModel;
using DJL.Work.ICommonService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DJL.Work.CommonService
{
    public class MemberService : BaseService<Member>, IMemberService
    {
        public void Register(string email, string nickName, string password)
        {
            using (var db = new MusicOnlineDBContext())
            {
                db.Members.Add(new Member()
                {
                    LoginEmail = email,
                    NickName = nickName,
                    Password = password
                });
                db.SaveChanges();
            }
        }


        public bool Active(string email)
        {
            using (var db = new MusicOnlineDBContext())
            {
                var mem = db.Members.FirstOrDefault(x => x.LoginEmail == email);
                if (mem == null) return false;
                mem.ActiveStatus = true;
                db.Entry<Member>(mem).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return true;
            }
        }


        public bool UpdatePwdByEmail(string email, string defaultPassword)
        {
            using (var db = new MusicOnlineDBContext())
            {
                var mem = db.Members.SingleOrDefault(x => x.LoginEmail == email);
                if (mem == null) return false;
                mem.Password = defaultPassword;
                db.Entry<Member>(mem).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return true;
            }
        }

        public bool CheckActiveOrExist(string email)
        {
            using (var db = new MusicOnlineDBContext())
            {
                var mem = db.Members.SingleOrDefault(x => x.LoginEmail == email);
                if (mem == null) return false;
                if (mem.ActiveStatus) return false;
                return true;
            }
        }


        public Member Login(string p, string md5)
        {
            using (var db = new MusicOnlineDBContext())
            {
                return db.Members.FirstOrDefault(x => x.LoginEmail == p && x.Password == md5);
            }
        }
    }
}
