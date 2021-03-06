﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DJL.Work.BackWeb.Models.WebSetting
{
    public class SaveCarouselModel
    {
        [Range(0, 10)]
        public int Order { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 1)]
        public string MusicName { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 1)]
        public string MusicDescription { get; set; }

        [Required]
        public HttpPostedFileBase ImageData { get; set; }

        [Required]
        public string Available { get; set; }
    }
}