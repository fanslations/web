using System;
using System.Collections.Generic;
using System.Linq;
using Thi.Core;

namespace Paranovels.ViewModels
{
    public class BaseCriteria : ICriteriaModel
    {
        public int ByUserID { get; set; }

        public object ID { get; set; }
        public int IDToInt
        {
            get
            {
                return ID == null ? 0 : char.IsDigit(IDToStr[0]) ? Convert.ToInt32(ID) : 0;
            }
        }
        public string IDToStr
        {
            get
            {
                return Convert.ToString(ID);
            }
        }

        public List<int> IDs { get; set; }

        public List<int> NotIDs { get; set; }

        public string Query { get; set; }

        public string Sorted { get; set; }

        public string Alt { get; set; }

        public string SearchType { get; set; }
        
        public Dictionary<string, string> HiddenFields { get; set; } 
    }
}