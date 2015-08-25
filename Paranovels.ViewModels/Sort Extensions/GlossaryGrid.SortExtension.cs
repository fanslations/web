using System;
using System.Collections.Generic;
using System.Linq;

namespace Paranovels.ViewModels
{
    public static partial class SortGridExtension
    {
        public static IDictionary<string, string> SortOptions(this IQueryable<GlossaryGrid> grids)
        {
            return new Dictionary<string, string>
            {
                {"best", "best"},
                {"controversial", "controversial"},
                {"new", "new"},
                {"old", "old"},
                {"reply", "most replies"},
                {"score", "vote score"},
                {"vote", "most voted"},
            };
        }

        public static IQueryable<GlossaryGrid> Sort(this IQueryable<GlossaryGrid> grids, BaseCriteria criteria)
        {
            criteria.Sorted = criteria.Sorted ?? "new";
            switch (criteria.Sorted)
            {
                case "best":
                    grids = grids.OrderByDescending(o => o.VoteUp + o.VoteDown + o.CommentCount);
                    break;
                case "controversial":
                    grids = grids.OrderByDescending(o => (o.VoteUp + o.VoteDown) + (Math.Abs(o.VoteUp.Value + o.VoteDown.Value) * -1));
                    break;
                case "new":
                    grids = grids.OrderByDescending(o => o.InsertedDate);
                    break;
                case "old":
                    grids = grids.OrderBy(o => o.InsertedDate);
                    break;
                case "reply":
                    grids = grids.OrderByDescending(o => o.CommentCount);
                    break;
                case "score":
                    grids = grids.OrderByDescending(o => o.VoteUp - o.VoteDown);
                    break;
                case "vote":
                    grids = grids.OrderByDescending(o => o.VoteUp + o.VoteDown);
                    break;
                default:
                    grids = grids.OrderByDescending(o => o.InsertedDate);
                    break;
            }
            return grids;
        } 
    }
}