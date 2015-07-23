using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thi.Core;

namespace Paranovels.ViewModels
{
    public class TranslatorCriteria : BaseCriteria
    {
        public int By { get; set; }
        public string Text { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public int Glossary { get; set; }
    }
}
