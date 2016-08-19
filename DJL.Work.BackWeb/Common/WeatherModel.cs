using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DJL.Work.BackWeb.Common
{
    public class WeatherModel
    {
        public int error { get; set; }
        public string status { get; set; }
        public string date { get; set; }
        public List<BaiduResult> results { get; set; }
    }

    public class BaiduResult
    {
        public string currentCity { get; set; }
        public string pm25 { get; set; }
        public List<BaiduIndex> index { get; set; }
        public List<BaiDuWeaterData> weather_data { get; set; }
    }

    public class BaiduIndex
    {
        public string title { get; set; }
        public string zs { get; set; }
        public string tipt { get; set; }
        public string des { get; set; }
    }

    public class BaiDuWeaterData
    {
        public string date { get; set; }
        public string dayPictureUrl { get; set; }
        public string nightPictureUrl { get; set; }
        public string weather { get; set; }
        public string wind { get; set; }
        public string temperature { get; set; }
    }
}