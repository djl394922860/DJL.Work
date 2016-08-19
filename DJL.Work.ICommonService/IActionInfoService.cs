using DJL.Work.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DJL.Work.ICommonService
{
    public interface IActionInfoService : IBaseService<ActionInfo>
    {
        void Edit(int id, byte method, string name, string requestUrl, bool isDeleted, string remark);

        object GetRolesByActionId(int actionId);
    }
}
