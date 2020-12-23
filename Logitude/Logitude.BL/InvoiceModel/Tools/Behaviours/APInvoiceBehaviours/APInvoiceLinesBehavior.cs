using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.Tools.Initializers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.Behaviours.APInvoiceBehaviours
{
    public class APInvoiceLinesBehavior : IServiceBehaviour
    {
        private APInvoicePM entityPM;

        private APInvoiceServiceInitializer initializer;

        public void Handle(IServiceInitializer initializer)
        {
            this.initializer = (APInvoiceServiceInitializer)initializer;
            this.entityPM = this.initializer.EntityPM;
            this.HandleBehaviour();
        }

        private void HandleBehaviour()
        {
            FillEmptyObjectTableId();

            if (initializer.EntityPM.TotalVATOnly)
            {
                foreach (APInvoiceLinePM item in initializer.EntityPM.InvoiceLines)
                {
                    if (item.VatTypeId != null)
                    {
                        item.VatTypeId = null;
                        item.VatPercentage = null;

                        if (item.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.None)
                        {
                            item.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                        }
                    }
                }
            }
        }

        private void FillEmptyObjectTableId()
        {
            //if (entityPM.InvoiceLines.Where(d => d.ObjectTableId == null).Any())
            //{
            //    List<ObjectTable> tables = new ObjectTableRepository(initializer.Tenant).context.ObjectTables.Where(d => (d.Tenant == 0 || d.Tenant == initializer.Tenant) && (d.Name == "Shipment" || d.Name == "Master")).ToList();

            //    foreach (APInvoiceLinePM item in entityPM.InvoiceLines.Where(d => d.ObjectTableId == null))
            //    {
            //        string shipmentLevelCode = shipmentRepository.GetShipmentLevelCode(item.EntityId);

            //        ObjectTable table = (shipmentLevelCode == "C") ? tables.Where(d => d.Name == "Master").FirstOrDefault() : tables.Where(d => d.Name == "Shipment").FirstOrDefault();
            //        if (table != null)
            //        {
            //            item.ObjectTableId = table.Id;
            //        }
            //    }
            //}
        }
    }
}
