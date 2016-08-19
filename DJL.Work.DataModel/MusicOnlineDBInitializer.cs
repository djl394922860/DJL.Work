using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using DJL.Work.DataModel;

namespace DJL.Work.DataModel
{
    /// <summary>
    /// 自定义数据库数据初始化
    /// </summary>
    public class MusicOnlineDBInitializer : CreateDatabaseIfNotExists<MusicOnlineDBContext>
    {
        protected override void Seed(MusicOnlineDBContext context)
        {
            //do something for example : add admin
            base.Seed(context);
        }
    }

    public class MusicOnlineDBInitializerDropCreate : DropCreateDatabaseIfModelChanges<MusicOnlineDBContext>
    {
        protected override void Seed(MusicOnlineDBContext context)
        {
            //do something
            base.Seed(context);
        }
    }
}
