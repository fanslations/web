﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Paranovels.Common;
using Paranovels.DataAccess;
using Paranovels.DataModels;
using Paranovels.Services;
using Paranovels.ViewModels;
using Thi.Core;

namespace Paranovels.Facade
{
    public class ConnectorFacade : IFacade
    {

        public int AddConnector(ConnectorForm form)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new ConnectorService(uow);
                var id = service.SaveChanges(form);
                return id;
            }
        }
    }
}