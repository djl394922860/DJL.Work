using DJL.Work.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJL.Work.ICommonService
{
    public interface ICarouselCanfigService : IBaseService<CarouselCanfig>
    {
        void EditCarouse(int Id, string des, string name, int order, bool isDeleted);

        void EditCarouseWithImg(int Id, string des, string name, int order, int imgSize, string fileSuffix, bool isDeleted, string networkRelationPath, string physicalPath);
    }
}
