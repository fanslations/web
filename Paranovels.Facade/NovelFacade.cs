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
                        glossaryForm.ByUserID = form.ByUserID;

                        var glossaryID = glossaryService.SaveChanges(glossaryForm);

                        // connect group to glossary
                        var connectorService = new ConnectorService(uow);
                        var connectorForm = new ConnectorForm()
                        {
                            ByUserID = form.ByUserID,
                            ConnectorType = R.ConnectorType.NOVEL_GLOSSARY,
                            SourceID = id,
                            TargetID = glossaryID
                        };
                        connectorService.SaveChanges(connectorForm);
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

                // chapter content
                var qContent = service.View<Content>().Where(w => w.ChapterID == detail.ID).Select(s => new ContentGrid
                {
                    ID = s.ID,
                    RawHash =  s.RawHash,
                    Final = s.Final,
                });

                var segments = detail.Content.SegmentChapterContent();

                int paragraph = 0;
                detail.Contents = qContent.ToList().Select(s =>
                {
                    s.Paragraph = paragraph++;
                    s.Raw = segments.ContainsKey(s.RawHash) ? segments[s.RawHash] : "Segment not found.";
                    return s;
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
