using DJL.Work.BackWeb.App_Start;
using DJL.Work.BackWeb.Common;
using DJL.Work.BackWeb.Models;
using DJL.Work.ICommonService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DJL.Work.Extend;
using System.Text;
using DJL.Work.Tool;
using DJL.Work.BackWeb.Models.Common;
using System.Net.Http;
using System.Threading.Tasks;

namespace DJL.Work.BackWeb.Controllers
{
    public class HomeController : Controller
    {
        private static readonly IAdminInfoService _adminInfoService = UnityConfig.Instance.GetService<IAdminInfoService>();

        [HttpGet]
        [Authorize]
        public async Task<ViewResult> Index()
        {
            var indexModel = new HomeIndexModel();
            var str = string.Empty;
            var weather = new WeatherModel();
            if (HttpContext.Cache.Get("weather_info") != null)
            {
                weather = HttpContext.Cache.Get("weather_info") as WeatherModel;
            }
            else
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = new TimeSpan(0, 0, 2);
                    str = await client.GetStringAsync(@"http://api.map.baidu.com/telematics/v3/weather?location=重庆&output=json&ak=hXWAgbsCC9UTkBO5V5Qg1WZ9");
                }
                if (!string.IsNullOrEmpty(str))
                {
                    weather = Tool.JsonHelper.Deserialize<WeatherModel>(str);
                    if (weather.status != "302")
                    {
                        HttpContext.Cache.Insert("weather_info", weather, null, DateTime.Now.AddHours(6), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                }
            }
            if (weather.date.IsNullOrEmpty())
            {
                weather.date = DateTime.Now.ToString();
            }
            if (weather.results == null || !weather.results.Any())
            {
                weather.results = new List<BaiduResult>()
                {
                    new BaiduResult()
                    {
                         pm25="80", currentCity="重庆", weather_data=new List<BaiDuWeaterData>()
                         {
                              new BaiDuWeaterData()
                              {
                               weather="天气晴", temperature="20℃",  dayPictureUrl="/content/images/duoyun_0.png",
                                nightPictureUrl="/content/images/duoyun_1.png"
                              }
                         } 
                    }
                };
            }
            indexModel.WeatherModel = weather;
            indexModel.ManagerName = _adminInfoService.GetEntityByPrimaryKey(int.Parse(HttpContext.User.Identity.Name)).UserName;
            return View(indexModel);
        }

        [HttpGet]
        [Authorize]
        public RedirectToRouteResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("Login", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public ViewResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(true)]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public JsonResult Login(LoginModel model)
        {
            var stander = new StandardResult();
            if (ModelState.IsValid)
            {
                var sessionCode = Session["AdminValidataCode"] as string;
                if (string.IsNullOrEmpty(sessionCode))
                {
                    stander.message = "请刷新验证码重试!";
                    return Json(stander, JsonRequestBehavior.DenyGet);
                }
                if (!string.Equals(sessionCode, model.ValidataCode, StringComparison.CurrentCultureIgnoreCase))
                {
                    Session.Remove("AdminValidataCode");
                    stander.message = "验证码错误!请重试";
                    return Json(stander, JsonRequestBehavior.DenyGet);
                }
                var md5Pwd = MD5Helper.GetMD5(model.AdminPwd + MD5Helper.GetMD5Salt());
                //checked db data
                var admin = _adminInfoService.Login(model.AdminName, md5Pwd);
                if (admin == null)
                {
                    stander.message = "帐号或者密码错误！";
                    return Json(stander, JsonRequestBehavior.DenyGet);
                }
                if (admin.DeleteFlag)
                {
                    stander.message = "你的账号被强制关闭了！请联系超级管理员";
                    return Json(stander, JsonRequestBehavior.DenyGet);
                }
                var roles = admin.RoleInfos.Select(x => x.RoleName).ToArray();
                var strRoles = string.Empty;
                if (roles.Any())
                {
                    var sb = new StringBuilder();
                    foreach (var item in roles)
                    {
                        sb.Append(item + "|");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    strRoles = sb.ToString();
                }
                //save 票据
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(2, admin.ID.ToString(), DateTime.Now, DateTime.Now.AddDays(7), false, strRoles, "/");
                var authStr = FormsAuthentication.Encrypt(ticket);
                Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, authStr));
                //save to session
                Session.Add("Admin_Session", new AdminSessionModel()
                {
                    AdminId = admin.ID,
                    AdminName = admin.UserName
                });
                stander.success = true;
                stander.message = Url.Action("Index", "Home");
                return Json(stander, JsonRequestBehavior.DenyGet);
            }
            // modelsate error
            var result = ModelState.GetModelErrorMsgs();
            var str = new StringBuilder();
            foreach (var item in result)
            {
                str.AppendLine(item);
            }
            stander.message = str.ToString();
            return Json(stander, JsonRequestBehavior.DenyGet);
        }

        [HttpGet]
        [AllowAnonymous]
        public FileResult GetCodeImg()
        {
            var code = ValidataImageHelper.CreateValidateCode(4);
            Session["AdminValidataCode"] = code;
            var img = ValidataImageHelper.CreateValidateGraphic(code);
            return File(img, @"image/jpeg");
        }

        [HttpGet]
        public ViewResult NotFind()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public JsonResult GetAdminInfo()
        {
            var stand = new StandardResult();
            var adminId = int.Parse(HttpContext.User.Identity.Name);
            var admin = _adminInfoService.GetEntityByPrimaryKey(adminId);
            if (admin == null)
            {
                stand.message = "找不到用户信息";
                return Json(stand, JsonRequestBehavior.AllowGet);
            }
            stand.success = true;
            stand.message = Tool.JsonHelper.Serialize(new { UserName = admin.UserName, Email = admin.Email, Remark = admin.Remark, Id = admin.ID });
            return Json(stand, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public JsonResult UpdateAdminInfo(UpdateAdminInfoModel model)
        {
            var stand = new StandardResult();
            if (model.IsUpdatedPwd)
            {
                if (string.IsNullOrEmpty(model.NewPwd) || string.IsNullOrEmpty(model.OldPwd) || string.IsNullOrEmpty(model.ConfirmPwd))
                {
                    ModelState.AddModelError("key", new Exception("请填写完整消息"));
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.IsUpdatedPwd)
                    {
                        var solt = MD5Helper.GetMD5Salt();
                        _adminInfoService.UpdateAdminInfo(model.Id, model.Name, model.Remark, model.Email, MD5Helper.GetMD5(model.OldPwd + solt), MD5Helper.GetMD5(model.NewPwd + solt));
                    }
                    else
                    {
                        _adminInfoService.UpdateAdminInfo(model.Id, model.Name, model.Remark, model.Email);
                    }
                }
                catch (Exception ex)
                {
                    stand.message = ex.Message;
                    stand.success = false;
                    return Json(stand);
                }
                stand.success = true;
                stand.message = model.Name;
                return Json(stand);
            }
            var error = ModelState.GetModelErrorMsgs().Aggregate((x, y) => x + "\r\n" + y);
            stand.message = error;
            return Json(stand, JsonRequestBehavior.DenyGet);
        }
    }
}