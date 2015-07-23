using Paranovels.DataModels;
using Thi.Core;

namespace Paranovels.ViewModels
{
    public class GenericForm<T> : IFormModel where T : class, IDataModel, new ()
    {
        public int ByUserID { get; set; }
        public int ID { get; set; }
        public string InlineEditProperty { get; set; }

        public T DataModel { get; set; }

    }
}