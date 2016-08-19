using DJL.Work.BackWeb.App_Start;
using DJL.Work.BackWeb.Common;
using DJL.Work.BackWeb.Models.WebSetting;
using DJL.Work.DataModel;
using DJL.Work.ICommonService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DJL.Work.Extend;
using System.IO;
using DJL.Work.Tool;

namespace DJL.Work.BackWeb.Controllers
{
    [Authorize]
    public class WebSiteSettingController : Controller
    {
        //
        // GET: /WebSiteSetting/
        private static readonly ICarouselCanfigService _carouselCanfigService = UnityConfig.Instance.GetService<ICarouselCanfigService>();

        #region Carousel

        [HttpGet]
        public ActionResult CarouselCanfig()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult GetCarouselItems(PageModel model)
        {
            int total = 0;
            var data = _carouselCanfigService.LoadPageEntities(model.rows, model.page, out total, x => true, x => x.Id, true).Select(x => new ShowCarouseModel()
            {
                Id = x.Id,
                ImageFormat = x.ImageFormat,
                MusicDescription = x.MusicDescription,
                MusicName = x.MusicName,
                Order = x.Order,
                UpLoadTime = x.UpLoadTime.ToString(),
                UpdateTime = x.UpdateTime.ToString(),
                IsDeleted = x.IsDeleted ? "是" : "否",
                NetworkRelativePath = x.NetworkRelativePath,
                ImgSize = x.ImageDataSize,
                PhysicalPath = x.PhysicalPath
            });
            var result = new PageResultModel<ShowCarouseModel>()
            {
                rows = data,
                total = total
            };
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [ValidateInput(true)]
        [ValidateAntiForgeryToken]
        public ActionResult SaveCarousel(SaveCarouselModel model)
        {
            var stand = new StandardResult();
            if (ModelState.IsValid)
            {
                if (model.ImageData.ContentLength <= 0 || model.ImageData == null)
                {
                    stand.message = "请选择上传图片";
                    return Json(stand);
                }
                if (model.ImageData.ContentLength >= 1024 * 1024)
                {
                    stand.message = "图片不能大于1M";
                    return Json(stand);
                }
                var imgs = new string[] { "jpg", "png", "gif", "jpeg" };
                var filePath = model.ImageData.FileName;
                var fileSuffix = filePath.Split(".".ToArray()).Last();
                if (!imgs.Contains(fileSuffix))
                {
                    stand.message = "自己说，上传的什么图片格式!";
                    return Json(stand);
                }
                var imgSize = model.ImageData.ContentLength / 1024;
                //check Directory save file
                var savePath = Server.MapPath("~/Content/images/carousel/");
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }
                var saveName = string.Format("{0}.{1}", MD5Helper.GetStreamMD5(model.ImageData.InputStream), fileSuffix);
                var physicalPath = Path.Combine(savePath, saveName);
                if (!System.IO.File.Exists(physicalPath))
                {
                    model.ImageData.SaveAs(physicalPath);
                }
                var host = HttpContext.Request.UrlReferrer.AbsoluteUri.TrimEnd('/');
                var networkPath = Path.Combine("/Content/images/carousel/", saveName);
                var fullPath = string.Format("{0}{1}?v={2}", host, networkPath, DateTime.Now.Ticks);
                var entity = new CarouselCanfig()
                {
                    ImageDataSize = imgSize,
                    ImageFormat = fileSuffix,
                    MusicDescription = model.MusicDescription,
                    Order = model.Order,
                    MusicName = model.MusicName,
                    UpLoadTime = DateTime.Now,
                    IsDeleted = model.Available == "on" ? false : true,
                    UpdateTime = DateTime.Now,
                    NetworkRelativePath = fullPath,
                    PhysicalPath = physicalPath
                };
                _carouselCanfigService.Add(entity);
                stand.success = true;
                stand.message = "操作成功";
                return Json(stand);
            }
            stand.message = ModelState.GetModelErrorMsgs().Aggregate((x, y) => x + "\r\n" + y);
            return Json(stand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public ActionResult EditCarousel(EditCarouselModel model)
        {
            var stand = new StandardResult();
            if (ModelState.IsValid)
            {
                if (model.ImageData == null || model.ImageData.ContentLength <= 0)
                {
                    //不修改图片
                    _carouselCanfigService.EditCarouse(model.Id, model.MusicDescription, model.MusicName, model.Order, model.Available == "on" ? false : true);
                    stand.success = true;
                    return Json(stand);
                }
                if (model.ImageData.ContentLength >= 1024 * 1024)
                {
                    stand.message = "图片不能大于1M";
                    return Json(stand);
                }
                var imgs = new string[] { "jpg", "png", "gif", "jpeg" };
                var filePath = model.ImageData.FileName;
                var fileSuffix = filePath.Split(".".ToArray()).Last();
                if (!imgs.Contains(fileSuffix))
                {
                    stand.message = "自己说，上传的什么图片格式!";
                    return Json(stand);
                }
                var imgSize = model.ImageData.ContentLength / 1024;
                //save new file img
                var savePath = Server.MapPath("~/Content/images/carousel/");
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }
                var saveName = string.Format("{0}.{1}", MD5Helper.GetStreamMD5(model.ImageData.InputStream), fileSuffix);
                var physicalPath = Path.Combine(savePath, saveName);
                if (!System.IO.File.Exists(physicalPath))
                {
                    model.ImageData.SaveAs(physicalPath);
                }
                var host = HttpContext.Request.UrlReferrer.AbsoluteUri.TrimEnd('/');
                var networkPath = Path.Combine("/Content/images/carousel/", saveName);
                var fullPath = string.Format("{0}{1}?v={2}", host, networkPath, DateTime.Now.Ticks);
                //保存修改图片
                _carouselCanfigService.EditCarouseWithImg(model.Id, model.MusicDescription, model.MusicName, model.Order, imgSize, fileSuffix, model.Available == "on" ? false : true, fullPath, physicalPath);
                stand.success = true;
                return Json(stand);
            }
            stand.message = ModelState.GetModelErrorMsgs().Aggregate((x, y) => x + "\r\n" + y);
            return Json(stand);
        }

        #endregion

        #region BaseConfig

        [HttpGet]
        public ActionResult BaseConfig()
        {
            return PartialView();
        }
        #endregion

        #region LogManager
        [HttpGet]
        public ActionResult LogManager()
        {
            return PartialView();
        }
        #endregion

        #region BaseStatusLook
        [HttpGet]
        public ActionResult BaseStatusLook()
        {
            return PartialView();
        }
        #endregion

    }
}