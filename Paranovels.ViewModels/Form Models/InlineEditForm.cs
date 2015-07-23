using Thi.Core;

namespace Paranovels.ViewModels
{
    public class InlineEditForm<T> : IFormModel where T : class , IDetailModel
    {
        public int ByUserID { get; set; }
        public string InlineEditProperty { get; set; }

        public int ID { get; set; }
        public T Model { get; set; }
    }
}