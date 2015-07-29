using System;
using System.Collections.Generic;
using System.Linq;

namespace Paranovels.ViewModels
{
    public static partial class SortGridExtension
    {
        public static IDictionary<string, string> SortOptions(this IQueryable<SeriesGrid> grids)
        {
            return new Dictionary<string, string>
            {
                {"best", "best"},
                {"new", "new"},
                {"old", "old"},
                {"rating", "average rating"},
                {"read", "most read"},
                {"score", "vote score"},
                {"title", "title"},
                {"vote", "most voted"},
                {"view", "most viewed"},
            };
        }
        public static IQueryable<SeriesGrid> Sort(this IQueryable<SeriesGrid> grids, BaseCriteria criteria)
        {
            criteria.Sorted = criteria.Sorted ?? "new";
            switch (criteria.Sorted)
            {
                case "best":
                    grids = grids.OrderByDescending(o => o.VoteUp + o.VoteDown + o.ViewCount);
                    break;
                case "new":
                    grids = grids.OrderByDescending(o => o.UpdatedDate);
                    break;
                case "old":
                    grids = grids.OrderBy(o => o.UpdatedDate);
                    break;
                case "rating":
                    grids = grids.OrderByDescending(o => o.QualityCount == 0 ? 0 : o.QualityScore / o.QualityCount);
                    break;
                case "read":
                    grids = grids.OrderByDescending(o => o.ViewCount);
                    break;
                case "score":
                    grids = grids.OrderByDescending(o => o.VoteUp - o.VoteDown);
                    break;
                case "title":
                    grids = grids.OrderBy(o => o.Title);
                    break;
                case "vote":
                    grids = grids.OrderByDescending(o => o.VoteUp + o.VoteDown);
                    break;
                case "view":
                    grids = grids.OrderByDescending(o => o.ViewCount);
                    break;
                default:
                    grids = grids.OrderByDescending(o => o.UpdatedDate);
                    break;
            }
            return grids;
        }
        
    }
}