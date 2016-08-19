using DJL.Work.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DJL.Work.ICommonService
{
    public interface IRoleInfoService : IBaseService<RoleInfo>
    {
        int[] GetActionsByRoleId(int roleId);

        object GetUsersByRoleId(int roleId);

        void SetRoleActions(int roleId, int[] actionsId);

        void EditRole(int p1, bool p2, string p3, int[] p4, string p5);

        void AddRoleWithActions(string p1, string p2, bool p3, int[] p4);

        IEnumerable<ActionInfo> GetActionsByRoleNames(string[] Roles);
    }
}
