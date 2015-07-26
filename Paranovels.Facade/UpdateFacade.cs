using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Paranovels.DataAccess;
using Paranovels.ViewModels;
using Thi.Core;

namespace Paranovels.Facade
{
    public class UpdateFacade : IFacade
    {
        public int SaveChanges(object formModel)
        {
            if (formModel.GetType() == typeof(UserForm))
                return new UserFacade().AddUser(formModel as UserForm);

            if(formModel.GetType() == typeof(NovelForm))
                return new NovelFacade().AddNovel(formModel as NovelForm);

            if (formModel.GetType() == typeof(ChapterForm))
                return new NovelFacade().AddChapter(formModel as ChapterForm);

            if (formModel.GetType() == typeof(ContentForm))
                return new NovelFacade().AddContent(formModel as ContentForm);

            if (formModel.GetType() == typeof (SeriesForm))
                return new SeriesFacade().AddSeries(formModel as SeriesForm);
            
            if (formModel.GetType() == typeof(TagForm))
                return new TagFacade().AddTag(formModel as TagForm);

            if (formModel.GetType() == typeof(GroupForm))
                return new GroupFacade().AddGroup(formModel as GroupForm);

            if (formModel.GetType() == typeof(ReleaseForm))
                return new SeriesFacade().AddRelease(formModel as ReleaseForm);

            if (formModel.GetType() == typeof(GlossaryForm))
                return new GlossaryFacade().AddGlossary(formModel as GlossaryForm);

            if (formModel.GetType() == typeof(CommentForm))
                return new CommentFacade().AddComment(formModel as CommentForm);

            if (formModel.GetType() == typeof(ListForm))
                return new ListFacade().AddList(formModel as ListForm);

            if (formModel.GetType() == typeof(PreferenceForm))
                return new UserFacade().AddPreference(formModel as PreferenceForm);

            if (formModel.GetType() == typeof(SpamReportForm))
                return new SpamReportFacade().AddSpamReport(formModel as SpamReportForm);

            return -1;
        }
    }
}
