using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Paranovels.Facade;
using Paranovels.ViewModels;

namespace Paranovels.Tests.Paranovels.Facade
{
    [TestClass]
    public class ListFacade_Tests
    {
        [TestMethod]
        public void Get_list_that_has_new_releases_test()
        {
            var facade = new ListFacade();

            var list = facade.GetListHasReleases(new ListCriteria { ByUserID = 154});

            foreach (var item in list)
            {
                Console.WriteLine("{0} {1} {2} {3}", item.ID, item.Name, item.Type, item.UpdatedDate);
            }
        }
    }
}
