using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DJL.Work.BackWeb.Models.WebSetting
{
    public class ShowCarouseModel
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public string MusicName { get; set; }
        public string MusicDescription { get; set; }
        public string UpLoadTime { get; set; }
        public string ImageFormat { get; set; }
        public string UpdateTime { get; set; }
        public string IsDeleted { get; set; }
        //add
        public string NetworkRelativePath { get; set; }
        public int ImgSize { get; set; }
        public string PhysicalPath { get; set; }
    }
}