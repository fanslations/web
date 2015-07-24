using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Thi.Web
{
    public class StopwatchAttribute : ActionFilterAttribute
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _stopwatch.Restart();
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            _stopwatch.Stop();
            var elapsed = string.Format("{0:0.000}", _stopwatch.Elapsed.TotalSeconds);
            filterContext.HttpContext.Response.AddHeader("X-Stopwatch", elapsed);
            filterContext.HttpContext.Response.SetCookie(new HttpCookie("X-Stopwatch", elapsed));
        }
    }
}
