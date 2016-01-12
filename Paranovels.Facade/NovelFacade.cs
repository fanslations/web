using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Paranovels.Common;
using Paranovels.DataModels;
using Thi.Core;
using Paranovels.Services;
using Paranovels.ViewModels;

using Paranovels.DataAccess;

namespace Paranovels.Facade
{
    public class NovelFacade : IFacade
    {
        public int AddNovel(NovelForm form)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new NovelService(uow);
                var id = service.SaveChanges(form);

                if (form.Glossaries != null && form.InlineEditProperty == form.PropertyName(m => m.Glossaries))
                {
                    foreach (var glossary in form.Glossaries)
                    {
                        var glossaryService = new GlossaryService(uow);
                        var glossaryForm = new GlossaryForm();
                        new PropertyMapper<Glossary, GlossaryForm>(glossary, glossaryForm).Map();
                        glossaryForm.SourceID = id;
                        glossaryForm.SourceTable = R.SourceTable.NOVEL;
                        glossaryForm.ByUserID = form.ByUserID;

                        var glossaryID = glossaryService.SaveChanges(glossaryForm);

                    }
                }
                return id;
            }
        }

        public NovelDetail GetNovel(NovelCriteria criteria)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new NovelService(uow);
                var detail = service.Get(criteria);

                // author
                var qTag = service.View<Tag>().All();
                var qConnector = service.View<Connector>().All();

                var qAuthor = service.View<Author>().All();
                var authors = qConnector.Where(w => w.ConnectorType == R.ConnectorType.NOVEL_AUTHOR)
                    .Where(w => w.SourceID == detail.ID).Select(s => s.TargetID).ToList();
                detail.Authors = qAuthor.Where(w => authors.Contains(w.ID)).ToList();

                // chapter
                var qChapter = service.View<Chapter>().All();

                qChapter = qChapter.Where(w => w.NovelID == detail.ID);

                detail.Chapters = qChapter.ToList();
                // glossary
                detail.Glossaries = service.View<Glossary>().Where(w => w.SourceTable == R.SourceTable.NOVEL && w.SourceID == detail.ID).ToList();
                // summarize
                detail.Summarize = service.View<Summarize>().Where(w => w.SourceTable == R.SourceTable.NOVEL && w.SourceID == detail.ID).SingleOrDefault() ?? new Summarize();
                return detail;
            }
        }

        public int AddChapter(ChapterForm chapterForm)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new ChapterService(uow);
                return service.SaveChanges(chapterForm);
            }
        }

        public ChapterDetail GetChapter(ChapterCriteria criteria)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new ChapterService(uow);
                var detail = service.Get(criteria);

                // novel 
                detail.Novel = service.View<Novel>().Where(w => w.ID == detail.NovelID).Single();
                // chapter content
                var contents = service.View<Content>().Where(w => w.ChapterID == detail.ID).Select(s => new ContentGrid
                {
                    ID = s.ID,
                    RawHash =  s.RawHash,
                    Final = s.Final,
                }).ToList();
                
                // summarize
                detail.Summarize = service.View<Summarize>().Where(w => w.SourceTable == R.SourceTable.CHAPTER && w.SourceID == detail.ID).SingleOrDefault() ?? new Summarize();
                // glossary
                detail.Glossaries = service.View<Glossary>().Where(w => w.SourceTable == R.SourceTable.NOVEL && w.SourceID == detail.NovelID).ToList();
                // checked
                var contentIDs = contents.Select(s => s.ID).ToList();
                detail.Checks = service.View<Check>().Where(w=> w.IsDeleted == false && contentIDs.Contains(w.SourceID) && w.SourceTable == R.SourceTable.CONTENT).ToList();
                // segments (paragraphs)
                var paragraphs = detail.Content.ToParagraphs().Select((s, i) => new { Index = i, Raw = s, RawHash = s.GetIntHash() });

                detail.Contents = paragraphs.Join(contents, p=>p.RawHash, c=>c.RawHash, (p,c) => new ContentGrid
                {
                    ID = c.ID,
                    Final = c.Final,
                    Paragraph = p.Index,
                    RawHash = p.RawHash,
                    Raw = p.Raw,
                    IsTranslated = detail.Checks.Any(w => w.Type == R.CheckType.TRANSLATING && w.SourceID == c.ID),
                    IsEdited = detail.Checks.Any(w => w.Type == R.CheckType.EDITING && w.SourceID == c.ID),
                    IsProofread = detail.Checks.Any(w => w.Type == R.CheckType.PROOFREADING && w.SourceID == c.ID),
                }).ToList();

                return detail;
            }
        }

        public int AddContent(ContentForm contentForm)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new ChapterService(uow);
                return service.SaveChanges(contentForm);
            }
        }
    }
}
