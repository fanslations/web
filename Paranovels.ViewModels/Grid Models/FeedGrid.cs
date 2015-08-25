using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Paranovels.DataModels;
using Thi.Core;

namespace Paranovels.ViewModels
{
    public class FeedGrid : Feed, IFeed
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
