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
                return service.Get(criteria);
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
                return service.Get(criteria);
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
