using System;
using System.Security.Cryptography.X509Certificates;

namespace Thi.Core
{
    public interface IFeed
    {
        DateTime InsertedDate { get; set; }
        DateTime UpdatedDate { get; set; }
        string Title { get; set; }
        string Content { get; set; }
        string Url { get; set; }
    }
}