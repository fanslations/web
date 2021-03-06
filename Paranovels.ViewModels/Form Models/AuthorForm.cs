﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Paranovels.DataModels;
using Thi.Core;

namespace Paranovels.ViewModels
{
    public class AuthorForm : Author, IFormModel
    {
        public int ByUserID { get; set; }
        public string InlineEditProperty { get; set; }

        public IList<Aka> Akas { get; set; }
    }
}
