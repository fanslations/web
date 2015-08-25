using System;
using System.ServiceModel.Syndication;
using System.Web.Mvc;
using System.Xml;
using Thi.Core;

namespace Thi.Web
{
    public class FeedResult : ActionResult
    {
        SyndicationFeed feed;
        string type;

        /// <summary>
        /// Constructor to set up the action result feed.
        /// </summary>
        /// <param name="feed">Accepts a <see cref="SyndicationFeed"/>.</param>
        /// <param name="type"></param>
        public FeedResult(SyndicationFeed feed, string type = "rss")
        {
            this.feed = feed;
            this.type = type;
        }
        /// <summary>
        /// Executes the call to the ActionResult method and returns the created feed to the output response.
        /// </summary>
        /// <param name="context">Accepts the current <see cref="ControllerContext"/>.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            using (XmlWriter writer = XmlWriter.Create(context.HttpContext.Response.Output))
            {
                if (type == "atom10")
                {
                    context.HttpContext.Response.ContentType = "application/atom+xml";
                    var formatter = new Atom10FeedFormatter(this.feed);
                    formatter.WriteTo(writer);
                }
                if (type == "rss20")
                {
                    context.HttpContext.Response.ContentType = "application/rss+xml";
                    var formatter = new Rss20FeedFormatter(this.feed);
                    formatter.WriteTo(writer);

                }
            }
        }
    }
}