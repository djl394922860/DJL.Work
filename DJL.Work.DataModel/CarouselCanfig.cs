using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJL.Work.DataModel
{
    public partial class CarouselCanfig
    {
        public CarouselCanfig()
        {
            UpLoadTime = DateTime.Now;
            IsDeleted = false;
            UpdateTime = DateTime.Now;
        }
        public int Id { get; set; }
        public int Order { get; set; }
        public int ImageDataSize { get; set; }
        public string MusicName { get; set; }
        public string MusicDescription { get; set; }
        public DateTime UpLoadTime { get; set; }
        public string ImageFormat { get; set; }
        //add
        public bool IsDeleted { get; set; }
        public DateTime UpdateTime { get; set; }
        //add
        public string NetworkRelativePath { get; set; }
        public string PhysicalPath { get; set; }
    }
}
