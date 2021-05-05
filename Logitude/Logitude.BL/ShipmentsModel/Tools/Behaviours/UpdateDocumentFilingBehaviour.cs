using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.Initializers;
using Logitude.BL.ShipmentsModel.Tools.TraceEvents;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.Behaviours
{
    public class UpdateDocumentFilingBehaviour : IServiceBehaviour
    {

        private ShipmentPM entityPM;
        private ShipmentServiceInitializer initializer;

        public void Handle(IServiceInitializer initializer)
        {
            this.initializer = (ShipmentServiceInitializer)initializer;
            this.entityPM = this.initializer.EntityPM;

            this.HandleBehaviour();
        }

        private void HandleBehaviour()
        {
            if (initializer.IsNewEntity)
            {
                ConnectDocumentFilingsToShipment();
            }

        }


        private void ConnectDocumentFilingsToShipment()
        {
            if (!string.IsNullOrEmpty(entityPM.DocumentFilingIds))
            {
                List<string> documentFilingIdLists = entityPM.DocumentFilingIds.Split(',').ToList();
                if (documentFilingIdLists.Count > 0)
                {
                    DocumentsFilingRepository documentsFilingRepository = new DocumentsFilingRepository(this.entityPM.Tenant); 
                    List<DocumentsFiling> documentsFilingLists = documentsFilingRepository.GetAll(this.entityPM.Tenant).Where(d => documentFilingIdLists.Contains(d.Id)).ToList();
                    foreach (DocumentsFiling item in documentsFilingLists)
                    {
                        item.EntityId = entityPM.Id;
                        documentsFilingRepository.Update(item);
                    }
                    documentsFilingRepository.SubmitChanges();
                }
            }
            entityPM.DocumentFilingIds = null;
        }






    }
}
