﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Paranovels.DataModels;
using Thi.Core;

namespace Paranovels.ViewModels
{
    public class ListDetail: UserList, IDetailModel
    {
        public IList<Connector> Connectors { get; set; } 
        public IList<SeriesGrid> Series { get; set; }

        public IList<Release> Releases { get; set; }

        public IList<UserRead> Reads { get; set; } 
    }
}
