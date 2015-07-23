﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Paranovels.DataModels;
using Thi.Core;

namespace Paranovels.ViewModels
{
    public class ReleaseDetail: Release, IDetailModel
    {
        [NotMapped]
        public Series Series { get; set; }
        [NotMapped]
        public Group Group { get; set; }
        [NotMapped]
        public Summarize Summarize { get; set; }
    }
}