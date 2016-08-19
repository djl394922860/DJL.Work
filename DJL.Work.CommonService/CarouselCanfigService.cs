using DJL.Work.DataModel;
using DJL.Work.ICommonService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJL.Work.CommonService
{
    public class CarouselCanfigService : BaseService<CarouselCanfig>, ICarouselCanfigService
    {
        public void EditCarouse(int Id, string des, string name, int order, bool isDeleted)
        {
            using (var db = new MusicOnlineDBContext())
            {
                var item = db.CarouselCanfigs.Find(Id);
                if (item == null) throw new Exception("Id错误");
                item.MusicDescription = des;
                item.MusicName = name;
                item.Order = order;
                item.UpdateTime = DateTime.Now;
                item.IsDeleted = isDeleted;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void EditCarouseWithImg(int Id, string des, string name, int order, int imgSize, string fileSuffix, bool isDeleted, string networkRelationPath, string physicalPath)
        {
            using (var db = new MusicOnlineDBContext())
            {
                var item = db.CarouselCanfigs.Find(Id);
                if (item == null) throw new Exception("Id错误");
                item.MusicDescription = des;
                item.MusicName = name;
                item.Order = order;
                item.ImageFormat = fileSuffix;
                item.ImageDataSize = imgSize;
                item.UpdateTime = DateTime.Now;
                item.IsDeleted = isDeleted;
                item.NetworkRelativePath = networkRelationPath;
                item.PhysicalPath = physicalPath;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}
