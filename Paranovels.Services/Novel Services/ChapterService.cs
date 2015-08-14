using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Paranovels.Common;
using Paranovels.DataModels;
using Paranovels.ViewModels;
using Thi.Core;

namespace Paranovels.Services
{
    public class ChapterService : BaseService
    {
        public ChapterService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public int SaveChanges(ChapterForm chapterForm)
        {
            var tChapter = Table<Chapter>();

            var chapter = tChapter.GetOrAdd(w => w.ID == chapterForm.ID
                || (w.NovelID == chapterForm.NovelID && w.Number == chapterForm.Number));

            MapProperty(chapterForm, chapter, chapterForm.InlineEditProperty);
            UpdateAuditFields(chapter, chapterForm.ByUserID);
            // save
            SaveChanges();

            // save chapter content
            if (!string.IsNullOrWhiteSpace(chapterForm.Content))
            {
                var tContent = Table<Content>();

                var segments = chapterForm.Content.SegmentChapterContent();
                foreach (var segment in segments)
                {
                    var content = tContent.GetOrAdd(w => w.ChapterID == chapter.ID && w.RawHash == segment.Key);
                    if (content.ID == 0) // only add when the paragraph is new (does not exist)
                    {
                        UpdateAuditFields(content, chapterForm.ByUserID);
                        content.ChapterID = chapter.ID;
                        content.RawHash = segment.Key;
                        content.Final = segment.Value;

                        // save
                        SaveChanges();
                    }
                }
            }
            return chapter.ID;
        }

        public ChapterDetail Get(ChapterCriteria criteria)
        {
            var qChapter = Table<Chapter>().All();

            if (criteria.IDToInt > 0)
            {
                qChapter = qChapter.Where(w => w.ID == criteria.IDToInt);
            }

            var chapter = qChapter.FirstOrDefault();
            if (chapter == null) return null;

            var chapterDetail = new ChapterDetail();
            MapProperty(chapter, chapterDetail);

            return chapterDetail;
        }

        public int SaveChanges(ContentForm contentForm)
        {
            var tContent = Table<Content>();

            var content = tContent.GetOrAdd(w => w.ID == contentForm.ID);

            MapProperty(contentForm, content, contentForm.InlineEditProperty);
            UpdateAuditFields(content, contentForm.ByUserID);
            // save
            SaveChanges();

            return content.ID;
        }
    }
}
