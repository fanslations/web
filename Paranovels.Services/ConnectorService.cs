using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Paranovels.Common;
using Paranovels.DataModels;
using Paranovels.ViewModels;
using Thi.Core;

namespace Paranovels.Services
{
    public class ConnectorService : BaseService
    {
        public ConnectorService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public int SaveChanges(ConnectorForm form)
        {
            var tConnector = Table<Connector>();

            var connector = tConnector.GetOrAdd(w => w.ConnectorID == form.ConnectorID || 
                (form.ConnectorID == 0 && w.ConnectorType == form.ConnectorType && w.SourceID == form.SourceID && w.TargetID == form.TargetID));

            MapProperty(form, connector, form.InlineEditProperty); 
            UpdateAuditFields(connector, form.ByUserID);
            // save
            SaveChanges();

            return connector.ConnectorID;
        }

        public void Connect(GenericForm<Connector> form, IList<int> targetIDs)
        {
            var tConnector = Table<Connector>();

            // delete all exist connector for connectorType and sourceID
            var connectors = tConnector.Where(w => w.IsDeleted == false && w.ConnectorType == form.DataModel.ConnectorType && w.SourceID == form.DataModel.SourceID);
            foreach (var connector in connectors)
            {
                if (targetIDs.Contains(connector.TargetID))
                {
                    targetIDs.Remove(connector.TargetID);
                }
                else
                {
                    UpdateAuditFields(connector, form.ByUserID);
                    connector.IsDeleted = true;
                }
            }
            // add new one or set delete flag to false
            foreach (var targetID in targetIDs)
            {
                form.DataModel.TargetID = targetID;
                var connector = tConnector.GetOrAdd(w => w.ConnectorType == form.DataModel.ConnectorType && w.SourceID == form.DataModel.SourceID && w.TargetID == form.DataModel.TargetID);
                UpdateAuditFields(connector, form.ByUserID);
                MapProperty(form.DataModel, connector);
                connector.IsDeleted = false;
            }
            // save
            SaveChanges();
        }

        public ConnectorDetail Get(ConnectorCriteria criteria)
        {
            var qConnector = View<Connector>().All();

            if (criteria.IDToInt > 0)
            {
                qConnector = qConnector.Where(w => w.ConnectorID == criteria.IDToInt);
            }

            var connector = qConnector.SingleOrDefault();
            if (connector == null) return null;

            var detail = new ConnectorDetail();
            MapProperty(connector, detail);

            return detail;
        }
    }
}
