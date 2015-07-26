﻿using System;
using System.Linq;

namespace Paranovels.ViewModels
{
    public static class SortGridExtension
    {
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
                case "quality":
                    grids = grids.OrderByDescending(o => o.QualityCount == 0 ? 0 : o.QualityScore / o.QualityCount);
                    break;
                case "reads":
                    grids = grids.OrderByDescending(o => o.ViewCount);
                    break;
                case "top":
                    grids = grids.OrderByDescending(o => o.VoteUp);
                    break;
                case "votes":
                    grids = grids.OrderByDescending(o => o.VoteUp + o.VoteDown);
                    break;
                case "views":
                    grids = grids.OrderByDescending(o => o.ViewCount);
                    break;
                default:
                    grids = grids.OrderByDescending(o => o.UpdatedDate);
                    break;
            }
            return grids;
        }

        public static IQueryable<ReleaseGrid> Sort(this IQueryable<ReleaseGrid> grids, BaseCriteria criteria)
        {
            criteria.Sorted = criteria.Sorted ?? "new";
            switch (criteria.Sorted)
            {
                case "best":
                    grids = grids.OrderByDescending(o => o.VoteUp + o.VoteDown + o.ViewCount);
                    break;
                case "new":
                    grids = grids.OrderByDescending(o => o.Date);
                    break;
                case "old":
                    grids = grids.OrderBy(o => o.Date);
                    break;
                case "quality":
                    grids = grids.OrderByDescending(o => o.QualityCount == 0 ? 0 : o.QualityScore / o.QualityCount);
                    break;
                case "reads":
                    grids = grids.OrderByDescending(o => o.ViewCount);
                    break;
                case "top":
                    grids = grids.OrderByDescending(o => o.VoteUp);
                    break;
                case "votes":
                    grids = grids.OrderByDescending(o => o.VoteUp + o.VoteDown);
                    break;
                case "views":
                    grids = grids.OrderByDescending(o => o.ViewCount);
                    break;
                default:
                    grids = grids.OrderByDescending(o => o.Date);
                    break;
            }
            return grids;
        }

        public static IQueryable<CommentGrid> Sort(this IQueryable<CommentGrid> grids, BaseCriteria criteria)
        {
            criteria.Sorted = criteria.Sorted ?? "new";
            switch (criteria.Sorted)
            {
                case "best":
                    grids = grids.OrderByDescending(o => o.VoteUp + o.VoteDown + o.CommentCount);
                    break;
                case "comments":
                    grids = grids.OrderByDescending(o => o.CommentCount);
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
                case "top":
                    grids = grids.OrderByDescending(o => o.VoteUp);
                    break;
                case "votes":
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