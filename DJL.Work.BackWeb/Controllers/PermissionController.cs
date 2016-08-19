using DJL.Work.BackWeb.App_Start;
using DJL.Work.BackWeb.Common;
using DJL.Work.BackWeb.Models.Permission;
using DJL.Work.ICommonService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DJL.Work.Extend;
using DJL.Work.Enum;
using DJL.Work.BackWeb.Common.PermissionAttribute;
using DJL.Work.DataModel;

namespace DJL.Work.BackWeb.Controllers
{
    [CustomAuthorize]
    public class PermissionController : Controller
    {
        //
        // GET: /Permission/

        private static readonly IAdminInfoService _adminInfoService = UnityConfig.Instance.GetService<IAdminInfoService>();

        private static readonly IRoleInfoService _roleInfoService = UnityConfig.Instance.GetService<IRoleInfoService>();

        private static readonly IActionInfoService _actionInfoService = UnityConfig.Instance.GetService<IActionInfoService>();

        #region UserManager

        [HttpGet]
        public ActionResult UserManager()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult GetUserItems(PageModel model, string searchValue)
        {
            int total = 0;
            IEnumerable<AdminInfo> query;
            if (searchValue.IsNullOrWhiteSpace())
            {
                query = _adminInfoService.LoadPageEntities(model.rows, model.page, out total, x => true, x => x.ID, true);
            }
            else
            {
                query = _adminInfoService.LoadPageEntities(model.rows, model.page, out total, x => x.UserName.Contains(searchValue), x => x.ID, true);
            }
            var data = query.Select(x => new ShowUserModel()
            {
                Id = x.ID,
                CreateTime = x.CreateTime.ToString(),
                IsDeleted = x.DeleteFlag ? "是" : "否",
                Remark = x.Remark,
                UserEmail = x.Email,
                UserName = x.UserName
            });
            var result = new PageResultModel<ShowUserModel>()
            {
                rows = data,
                total = total
            };
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        [PermissionPostJson]
        public JsonResult SaveUser(SaveUserModel model)
        {
            var stand = new StandardResult();
            if (!string.Equals(model.UserNewPwd, model.UserConfirmPwd, StringComparison.CurrentCulture))
            {
                ModelState.AddModelError("key", new Exception("密码不一致"));
            }
            if (!model.UserRoles.Any())
            {
                ModelState.AddModelError("key1", new Exception("该用户没选择角色"));
            }
            if (ModelState.IsValid)
            {
                var pwdMd5 = Tool.MD5Helper.GetMD5(model.UserNewPwd + Tool.MD5Helper.GetMD5Salt());
                var isDeleted = model.Available == "on" ? false : true;
                //roles
                _adminInfoService.AddUserWithRoles(model.UserName, model.UserEmail, pwdMd5, isDeleted, model.UserRoles, model.Remark);
                stand.success = true;
                return Json(stand);
            }
            stand.message = ModelState.GetModelErrorMsgs().Aggregate((x, y) => x + "\r\n" + y);
            return Json(stand);
        }

        [HttpPost]
        public JsonResult GetAllRoles()
        {
            //get working roles
            var data = _roleInfoService.LoadEntities(x => x.DeleteFlag == false).Select(x => new
            {
                id = x.ID,
                text = x.RoleName
            });
            return Json(data);
        }

        [HttpPost]
        [PermissionPostJson]
        public JsonResult EditUser(int userId)
        {
            var stand = new StandardResult();
            if (userId <= 0)
            {
                stand.message = "改发送参数有意思么";
                return Json(stand);
            }
            try
            {
                _adminInfoService.UpdateStatus(userId);
            }
            catch (Exception ex)
            {
                stand.message = ex.Message;
                return Json(stand);
            }
            stand.success = true;
            return Json(stand);
        }

        [HttpPost]
        [PermissionPostJson]
        public ActionResult EnableUser(int userId)
        {
            var stand = new StandardResult();
            if (userId <= 0)
            {
                stand.message = "改发送参数有意思么";
                return Json(stand);
            }
            try
            {
                _adminInfoService.UpdateStatusEnable(userId);
            }
            catch (Exception ex)
            {
                stand.message = ex.Message;
                return Json(stand);
            }
            stand.success = true;
            return Json(stand);
        }

        [HttpPost]
        [PermissionPostJson]
        public JsonResult GetUserRoles(int userId)
        {
            var s = new StandardResult();
            try
            {
                int[] roleIds = _adminInfoService.GetUserRoles(userId);
                s.success = true;
                if (!roleIds.Any())
                {
                    s.message = string.Empty;
                    return Json(s);
                }
                s.message = roleIds.Select(x => x.ToString()).Aggregate((x, y) => x + "," + y);
                return Json(s);
            }
            catch (Exception ex)
            {
                s.message = ex.Message;
                return Json(s);
            }
        }

        [HttpPost]
        [PermissionPostJson]
        public JsonResult SetUserRoles(int userId, string rolesId)
        {
            var stan = new StandardResult();
            try
            {
                var roles = new int[] { };
                if (!string.IsNullOrEmpty(rolesId))
                {
                    roles = rolesId.Split(",".ToArray()).Select(int.Parse).ToArray();
                }
                _adminInfoService.SetUserRoles(userId, roles);
                stan.success = true;
                return Json(stan);
            }
            catch (Exception ex)
            {
                stan.message = ex.Message;
                return Json(stan);
            }
        }

        #endregion

        #region RoleManager

        [HttpGet]
        public ActionResult RoleManager()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult GetRoleItems(PageModel model, string searchValue)
        {
            int total = 0;
            IEnumerable<RoleInfo> query;
            if (searchValue.IsNullOrWhiteSpace())
            {
                query = _roleInfoService.LoadPageEntities(model.rows, model.page, out total, x => true, x => x.ID, true);
            }
            else
            {
                query = _roleInfoService.LoadPageEntities(model.rows, model.page, out total, x => x.RoleName.Contains(searchValue), x => x.ID, true);
            }
            var data = query.Select(x => new
            {
                Id = x.ID,
                RoleName = x.RoleName,
                IsDeleted = x.DeleteFlag ? "是" : "否",
                Remark = x.Remark,
                CreateTime = x.CreateTime.ToString()
            });
            var result = new PageResultModel<dynamic>()
            {
                rows = data,
                total = total
            };
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetAllActions()
        {
            //get wokring actions
            var data = _actionInfoService.LoadEntities(x => x.DeleteFlag == false).Select(x => new
            {
                id = x.ID,
                text = x.ActionName
            });
            return Json(data);
        }

        [HttpPost]
        [PermissionPostJson]
        public JsonResult GetRoleActions(int roleId)
        {
            var stand = new StandardResult();
            try
            {
                var actionIds = _roleInfoService.GetActionsByRoleId(roleId);
                stand.success = true;
                if (!actionIds.Any())
                {
                    stand.message = string.Empty;
                    return Json(stand);
                }
                stand.message = actionIds.Select(x => x.ToString()).Aggregate((x, y) => x + "," + y);
                return Json(stand);
            }
            catch (Exception ex)
            {
                stand.message = ex.Message;
                return Json(stand);
            }
        }

        [HttpPost]
        public JsonResult GetRoleUsers(int roleId)
        {
            var users = _roleInfoService.GetUsersByRoleId(roleId);
            return Json(users);
        }

        [HttpPost]
        [PermissionPostJson]
        public ActionResult SetRoleActions(int roleId, string actionsId)
        {
            var stan = new StandardResult();
            try
            {
                var actions = new int[] { };
                if (!string.IsNullOrEmpty(actionsId))
                {
                    actions = actionsId.Split(",".ToArray()).Select(int.Parse).ToArray();
                }
                _roleInfoService.SetRoleActions(roleId, actions);
                stan.success = true;
                return Json(stan);
            }
            catch (Exception ex)
            {
                stan.message = ex.Message;
                return Json(stan);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionPostJson]
        public ActionResult EditRole(EditRoleModel model)
        {
            var sd = new StandardResult();
            if (!model.RoleActions.Any())
            {
                ModelState.AddModelError("key", "角色不能没有权限");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _roleInfoService.EditRole(model.RoleId, model.Available == "on" ? false : true, model.Remark, model.RoleActions, model.RoleName);
                    sd.success = true;
                    return Json(sd);
                }
                catch (Exception ex)
                {
                    sd.message = ex.Message;
                    return Json(sd);
                }
            }
            sd.message = ModelState.GetModelErrorMsgs().Aggregate((x, y) => x + ";" + y);
            sd.success = false;
            return Json(sd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionPostJson]
        public ActionResult SaveRole(SaveRoleModel model)
        {
            var sd = new StandardResult();
            if (!model.RoleActions.Any())
            {
                ModelState.AddModelError("key", "角色至少有一个权限");
            }
            if (ModelState.IsValid)
            {
                _roleInfoService.AddRoleWithActions(model.RoleName, model.Remark, model.Available == "on" ? false : true, model.RoleActions);
                sd.success = true;
                return Json(sd);
            }
            sd.message = ModelState.GetModelErrorMsgs().Aggregate((x, y) => x + ";" + y);
            sd.success = false;
            return Json(sd);
        }

        #endregion

        #region UrlActionManager

        [HttpGet]
        public ActionResult UrlActionManager()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult GetActionItems(PageModel model, string searchValue)
        {
            int total = 0;
            IEnumerable<ActionInfo> query;
            if (searchValue.IsNullOrWhiteSpace())
            {
                query = _actionInfoService.LoadPageEntities(model.rows, model.page, out total, x => true, x => x.ID, true);
            }
            else
            {
                query = _actionInfoService.LoadPageEntities(model.rows, model.page, out total, x => x.ActionName.Contains(searchValue), x => x.ID, true);
            }
            var data = query.Select(x => new
            {
                Id = x.ID,
                ActionName = x.ActionName,
                ActionMethod = typeof(BaseEnum).GetEnumName(x.HttpMethod),
                ActionRequestUrl = x.RequestUrl,
                IsDeleted = x.DeleteFlag ? "是" : "否",
                Remark = x.Remark,
                CreateTime = x.CreateTime.ToString()
            });
            var result = new PageResultModel<dynamic>()
            {
                rows = data,
                total = total
            };
            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionPostJson]
        public JsonResult SaveAction(SaveActionModel model)
        {
            var std = new StandardResult();
            var enumRequestMethodValue = typeof(BaseEnum).GetEnumValues().Cast<byte>();
            if (!enumRequestMethodValue.Contains(model.ActionMethod))
            {
                ModelState.AddModelError("error_requestmethod", "请求模式参数错误!");
            }
            if (ModelState.IsValid)
            {
                _actionInfoService.Add(new DataModel.ActionInfo()
                {
                    ActionName = model.ActionName,
                    CreateTime = DateTime.Now,
                    DeleteFlag = model.Available == "on" ? false : true,
                    HttpMethod = model.ActionMethod,
                    Remark = model.Remark,
                    RequestUrl = model.ActionRequestUrl
                });
                std.success = true;
                return Json(std);
            }
            std.message = ModelState.GetModelErrorMsgs().Aggregate((x, y) => x + ";" + y);
            std.success = false;
            return Json(std);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionPostJson]
        public JsonResult EditAction(EditActionModel model)
        {
            var std = new StandardResult();
            var enumRequestMethodValue = typeof(BaseEnum).GetEnumValues().Cast<byte>();
            if (!enumRequestMethodValue.Contains(model.ActionMethod))
            {
                ModelState.AddModelError("error_requestmethod", "请求模式参数错误!");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _actionInfoService.Edit(model.ActionId, model.ActionMethod, model.ActionName, model.ActionRequestUrl, model.Available == "on" ? false : true, model.Remark);
                }
                catch (Exception ex)
                {
                    std.message = ex.Message;
                    return Json(std);
                }
                std.success = true;
                return Json(std);
            }
            std.message = ModelState.GetModelErrorMsgs().Aggregate((x, y) => x + ";" + y);
            std.success = false;
            return Json(std);
        }

        [HttpPost]
        public JsonResult GetActionRoles(int actionId)
        {
            var users = _actionInfoService.GetRolesByActionId(actionId);
            return Json(users);
        }

        #endregion

    }
}