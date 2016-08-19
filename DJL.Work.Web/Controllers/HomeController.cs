using DJL.Work.ICommonService;
using DJL.Work.Web.App_Start;
using DJL.Work.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DJL.Work.Extend;
using DJL.Work.Web.Common;
using DJL.Work.Tool;
using System.Threading.Tasks;
using System.Configuration;
using System.Text.RegularExpressions;

namespace DJL.Work.Web.Controllers
{
    public class HomeController : Controller
    {
        private static readonly IMemberService _memberService = UnityConfig.Instance.GetService<IMemberService>();

        private static readonly ICarouselCanfigService _carouselCanfigService = UnityConfig.Instance.GetService<ICarouselCanfigService>();

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            var form = HttpContext.User.Identity as FormsIdentity;
            var user = new UserSessionModel();
            if (form != null && form.IsAuthenticated)
            {
                user.NickName = form.Ticket.UserData;
                var result = 0;
                if (int.TryParse(form.Ticket.Name, out result))
                {
                    user.UserId = result;
                }
            }
            var items = _carouselCanfigService.LoadEntities(x => x.IsDeleted == false).OrderBy(x => x.Order).ToList();
            if (!items.Any()) throw new ArgumentNullException("Carousels");
            ViewBag.AllCarousels = items;
            return View(user);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult NotFind()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Search(string searchMusic)
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(true)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            //todo
            var standarResult = new StandardResult();
            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(model.Vcode))
                {
                    standarResult.message = "验证码不能为空!";
                    return Json(standarResult);
                }
                if (model.Vcode.ToLower() != Session["ValidataCode"].ToString().ToLower())
                {
                    Session.Remove("ValidataCode");
                    standarResult.message = "验证码错误!";
                    return Json(standarResult);
                }
                if (!string.Equals(model.Password, model.Password2, StringComparison.CurrentCulture))
                {
                    standarResult.message = "密码不一致!请注意大小写";
                    return Json(standarResult);
                }
                //check data
                var anys = _memberService.LoadEntities(x => x.LoginEmail == model.Email || x.NickName == model.Name);
                if (anys.Any())
                {
                    standarResult.message = "邮箱或者用户名已经被注册!";
                    return Json(standarResult);
                }
                Session.Remove("ValidataCode");
                //register to db
                _memberService.Register(model.Email, model.Name, MD5Helper.GetMD5(model.Password + MD5Helper.GetMD5Salt()));
                //send email
                var activeCode = Guid.NewGuid().ToString();
                Session.Add("ActiveCode", activeCode);
                Session.Add("SendEmailTime", DateTime.Now);
                //taks
                await Task.Run(() =>
                {
                    //todo 获取服务地址，邮件模版
                    var mainBody = string.Format(@"<a href='http://localhost:33839/Home/Active?loginEmail={0}&activeCode={1}'>请单击激活账号，注意请使用同一浏览器激活连接，请在24小时候之内激活，不然链接就会失效，谢谢</a>", model.Email, activeCode);//设置激活连接地址
                    //读取 邮件模板
                    EmailHelper.Send(model.Email, "注册djl-music-online激活", mainBody, new string[] { "" });
                });
                standarResult.success = true;
                standarResult.message = "/";
                return Json(standarResult);
            }
            var strs = ModelState.GetModelErrorMsgs().Aggregate((x, y) => x + "\r\n" + y);
            standarResult.message = strs;
            return Json(standarResult, JsonRequestBehavior.DenyGet);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Active(string loginEmail, string activeCode)
        {
            var ac = HttpContext.Session["ActiveCode"] as string;
            if (string.IsNullOrEmpty(loginEmail) || string.IsNullOrEmpty(activeCode))
            {
                return Content("请不要乱搞");
            }
            TimeSpan ts = DateTime.Now - ((DateTime)HttpContext.Session["SendEmailTime"]);
            Session.Remove("ActiveCode");
            Session.Remove("SendEmailTime");
            if (string.Equals(ac, activeCode, StringComparison.CurrentCultureIgnoreCase) && ts.Hours < 24)
            {
                var b = _memberService.Active(loginEmail);
                if (!b) return Content("激活失败,请确认您的注册邮箱是否已经注册!");
                return RedirectToAction("Login", "Home");
            }
            return Content("激活码错误或者已经过期,请重新激活!");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ActiveAgain()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ActiveAgain(string email)
        {
            var stand = new StandardResult() { success = false };
            if (string.IsNullOrEmpty(email))
            {
                stand.message = "请填写邮箱";
                return Json(stand, JsonRequestBehavior.DenyGet);
            }
            Regex regex = new Regex(@"^(\w)+(\.\w+)*@(\w)+((\.\w+)+)$", RegexOptions.IgnoreCase);
            if (!regex.IsMatch(email))
            {
                stand.message = "乱填好玩么？";
                return Json(stand, JsonRequestBehavior.DenyGet);
            }
            //check already active or exist
            var b = _memberService.CheckActiveOrExist(email);
            if (!b)
            {
                stand.message = "对不起，您邮箱已经激活或者没有注册，请确认!";
                return Json(stand);
            }

            //send email
            var activeCode = Guid.NewGuid().ToString();
            Session.Add("ActiveCode", activeCode);
            Session.Add("SendEmailTime", DateTime.Now);
            await Task.Run(() =>
            {
                //todo 获取服务地址
                var mainBody = string.Format(@"<a href='http://localhost:53472/Home/Active?loginEmail={0}&activeCode={1}'>请单击激活账号，注意请使用同一浏览器激活连接，请在24小时候之内激活，不然链接就会失效，谢谢</a>", email, activeCode);//设置激活连接地址
                //读取 邮件模板
                EmailHelper.Send(email, "注册djl-music-online激活", mainBody, new string[] { "" });
            });
            stand.success = true;
            stand.message = "/";
            return Json(stand);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult CheckUserName(string name)
        {
            if (_memberService.LoadEntities(x => x.NickName == name).Any())
            {
                return Json(new { valid = false });
            }
            return Json(new { valid = true });
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult CheckUserEmail(string email)
        {
            if (_memberService.LoadEntities(x => x.LoginEmail == email).Any())
            {
                return Json(new { valid = false });
            }
            return Json(new { valid = true });
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(true)]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            var standarResult = new StandardResult();
            if (ModelState.IsValid)
            {
                var md5 = MD5Helper.GetMD5(model.Password + MD5Helper.GetMD5Salt());
                //Check
                var member = _memberService.Login(model.Email, md5);
                if (member == null)
                {
                    standarResult.message = "账号或者密码错误!";
                    return Json(standarResult);
                }
                if (!member.ActiveStatus)
                {
                    standarResult.message = "╮(╯_╰)╭,请激活邮箱才能登陆!";
                    return Json(standarResult);
                }
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(2, member.Id.ToString(), DateTime.Now, DateTime.Now.AddHours(1), model.Remember == "on" ? false : true, member.NickName);
                var auth = FormsAuthentication.Encrypt(ticket);
                Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, auth));
                //Test Session
                Session["User_Session"] = new UserSessionModel
                {
                    NickName = member.NickName,
                    UserId = member.Id
                };
                standarResult.success = true;
                standarResult.message = "/";
                return Json(standarResult, JsonRequestBehavior.DenyGet);
            }
            var strs = ModelState.GetModelErrorMsgs().Aggregate((x, y) => x + "\r\n" + y);
            standarResult.message = strs;
            return Json(standarResult, JsonRequestBehavior.DenyGet);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Forget()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Forget(string email)
        {
            var stand = new StandardResult() { success = false };

            if (string.IsNullOrEmpty(email))
            {
                stand.message = "不填好玩么？";
                return Json(stand, JsonRequestBehavior.DenyGet);
            }
            Regex regex = new Regex(@"^(\w)+(\.\w+)*@(\w)+((\.\w+)+)$", RegexOptions.IgnoreCase);
            if (!regex.IsMatch(email))
            {
                stand.message = "乱填好玩么？";
                return Json(stand, JsonRequestBehavior.DenyGet);
            }

            var pwd = ConfigurationManager.AppSettings.Get("DefaultPassword");
            var password = MD5Helper.GetMD5(pwd + MD5Helper.GetMD5Salt());
            var success = _memberService.UpdatePwdByEmail(email, password);
            if (!success)
            {
                stand.message = "该邮箱没有注册本网站，请确认";
                return Json(stand);
            }
            await Task.Run(() =>
            {
                var body = string.Format(@"您的账户:{0}</br>您的新密码:{1}</br><p>建议您尽快修改密码(>^ω^<)喵</p>", email, pwd);
                var subject = "djl-music-online忘记密码";
                EmailHelper.Send(email, subject, body, new string[] { "" });
            });
            //send email && update pwd      
            stand.success = true;
            stand.message = Url.Action("Login", "Home");
            return Json(stand);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("Login", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ValidataCode()
        {
            var code = ValidataImageHelper.CreateValidateCode(4);
            Session["ValidataCode"] = code;
            var img = ValidataImageHelper.CreateValidateGraphic(code);
            return File(img, @"image/jpeg");
        }
    }
}