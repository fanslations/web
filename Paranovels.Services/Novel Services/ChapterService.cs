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

            var chapter = tChapter.GetOrAdd(w => w.ChapterID == chapterForm.ChapterID
                || (w.NovelID == chapterForm.NovelID && w.Number == chapterForm.Number));

            MapProperty(chapterForm, chapter, chapterForm.InlineEditProperty);
            UpdateAuditFields(chapter, chapterForm.ByUserID);
            // save
            SaveChanges();

            // save chapter content
            if (!string.IsNullOrWhiteSpace(chapterForm.Content))
            {
                var tContent = Table<Content>();

                var contents = tContent.Where(w => w.ChapterID == chapter.ChapterID);
                // delete old contents if there any
                foreach (var content in contents)
                {
                    content.IsDeleted = true;
                    UpdateAuditFields(content, chapterForm.ByUserID);
                }

                var paragraphs = chapterForm.Content.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
 
                for (int line = 1; line <= paragraphs.Length; line++)
                {
                    var paragraph = paragraphs[line - 1].Trim();
                    var hashCode = paragraph.GetIntHash();
                    var content = tContent.GetOrAdd(w => w.ChapterID == chapter.ChapterID && w.HashCode == hashCode);
                    UpdateAuditFields(content, chapterForm.ByUserID);
                    content.HashCode = hashCode;
                    content.ChapterID = chapter.ChapterID;
                    content.Paragraph = line;
                    content.Raw = paragraph;
                    content.Final = paragraph;

                    // save
                    SaveChanges();
                }
            }
            return chapter.ChapterID;
        }

        public ChapterDetail Get(ChapterCriteria criteria)
        {
            var qChapter = Table<Chapter>().All();

            if (criteria.IDToInt > 0)
            {
                qChapter = qChapter.Where(w => w.ChapterID == criteria.IDToInt);
            }

            var chapter = qChapter.FirstOrDefault();
            if (chapter == null) return null;

            var chapterDetail = new ChapterDetail();
            MapProperty(chapter, chapterDetail);

            // chapter content
            var qContent = Table<Content>().All();

            qContent = qContent.Where(w => w.ChapterID == chapter.ChapterID);

            chapterDetail.Contents = qContent.ToList();

            return chapterDetail;
        }

        public int SaveChanges(ContentForm contentForm)
        {
            var tContent = Table<Content>();

            var content = tContent.GetOrAdd(w => w.ContentID == contentForm.ContentID);

            MapProperty(contentForm, content, contentForm.InlineEditProperty);
            UpdateAuditFields(content, contentForm.ByUserID);
            // save
            SaveChanges();

            return content.ID;
        }
    }
}
