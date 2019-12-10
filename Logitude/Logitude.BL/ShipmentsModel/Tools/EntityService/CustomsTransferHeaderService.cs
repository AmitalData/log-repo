using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Logitude.BL.ShipmentsModel.EntityOtherServices;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.DataMapping;
using Logitude.BL.ShipmentsModel.Tools.TraceEvents;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Logitude.BL.ShipmentsModel.Tools.EntityService
{
    public class CustomsTransferHeaderService
    {
        public CustomsTransferHeader entityPoco { get; set; }
        private int tenant;
        private bool isNewEntity;
        private Tenant loggedTenant;
        private string loggedContactId;
        private CustomsTransferHeaderPM entityPM;
        private IShipmentsContext objectContext;
        private CustomsTransferHeaderRepository entityRepository;
        private CustomsTransferLineRepository CustomsTransferLineRepository;
        
        private List<ShipmentDataView> shipments;
        private ShipmentRepository shipmentRepository;
        public CustomsTransferHeaderService(IShipmentsContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new CustomsTransferHeaderRepository(objectContext);
            this.CustomsTransferLineRepository = new CustomsTransferLineRepository(objectContext);
            this.loggedTenant = TenantRepository.GetSingleTenant(tenant, true);

            this.shipments = new List<ShipmentDataView>();
            this.shipmentRepository = new ShipmentRepository(objectContext);

            this.GetLoggedContact();
        }

        private void GetLoggedContact()
        {
            ContactRepository contactRepository = new ContactRepository(tenant);

            string email = HttpContext.Current.User.Identity.Name;

            if (email != null)
            {
                Contact loggedContact = contactRepository.GetSingleContactByEmail(email, tenant);
                this.loggedContactId = loggedContact.Id;
            }

            else
            {
                ContactPM loggedContact = new ContactQuery(tenant).GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), tenant, true);
                this.loggedContactId = loggedContact.Id;
            }
        }

        private void GetAllEntities()
        {
            List<string> ids = entityPM.CustomsTransferLines.Select(s => s.ShipmentId).ToList();

            if (ids.Count > 0)
            {
                this.shipments = shipmentRepository.GetShipmentsFromIdList(ids, tenant);

                switch (entityPM.CustomsTransferTypeCode)
                {
                    case "AMAS":
                        {
                            this.shipments = this.shipments.Where(d => d.TransportModeId == "A").ToList();
                            break;
                        }

                    case "AMOS":
                        {
                            this.shipments = this.shipments.Where(d => d.TransportModeId == "O").ToList();
                            break;
                        }
                }
            }
        }

        private List<CustomsTransferLinePM> transferLinesChangeSet;
        public void SetChangeSet(List<CustomsTransferLinePM> transferLinesChangeSet)
        {
            this.transferLinesChangeSet = transferLinesChangeSet;
        }

        public void Create(CustomsTransferHeaderPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPM = entityPM;
            this.GetAllEntities();
            this.entityPM.Id = IdCounter.GetNumber("CustomsTransferHeader", tenant).ToString();

            this.entityPoco = new CustomsTransferHeader()
            {
                Id = entityPM.Id,
                Tenant = tenant,
                CustomsTransferTypeCode = entityPM.CustomsTransferTypeCode,
            };

            this.InitializeComponent();
            this.UpdateTransferLines();

            CustomsTransferHeaderTracing.Trace(entityPM, entityPoco, isNewEntity);
            CustomsTransferHeaderMapping.MapEntity(entityPM, entityPoco, isNewEntity);

            entityRepository.Add(entityPoco);
            entityRepository.SubmitChanges();

            this.TransferData();
        }

        public void Update(CustomsTransferHeaderPM entityPM, bool mapComposition = false)
        {
            this.isNewEntity = false;
            this.entityPM = entityPM;
            this.GetAllEntities();
            this.entityPoco = entityRepository.GetSingleEntity(entityPM.Id, tenant);

            if (mapComposition)
            {
                this.transferLinesChangeSet = this.entityPM.CustomsTransferLines;
            }

            this.InitializeComponent();
            this.UpdateTransferLines();

            CustomsTransferHeaderMapping.MapEntity(entityPM, entityPoco, isNewEntity);

            entityRepository.Update(entityPoco);
            entityRepository.SubmitChanges();
        }

        private void InitializeComponent()
        {
            if (isNewEntity)
            {
                if (entityPM.TransferDate == null)
                {
                    entityPM.TransferDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                }

                if (string.IsNullOrEmpty(entityPM.TransferNumber))
                {
                    int counter = CodeCounter.GetNumber("CustomsTransferHeader", tenant);
                    entityPM.TransferNumber = counter.ToString();
                }

                this.SetFileName();
            }

            entityPM.CreatedByUserId = loggedContactId;
        }

        private void SetFileName()
        {
            if (isNewEntity)
            {
                switch (entityPM.CustomsTransferTypeCode)
                {
                    case "AMAS":
                        {
                            entityPM.FileName = "Air Shipments" + entityPM.TransferNumber + ".xls";
                            break;
                        }

                    case "AMOS":
                        {
                            entityPM.FileName = "Ocean Shipments" + entityPM.TransferNumber + ".xls";
                            break;
                        }
                }
            }
        }

        private void UpdateTransferLines()
        {
            if (isNewEntity)
            {
                foreach (CustomsTransferLinePM itemPM in entityPM.CustomsTransferLines)
                {
                    this.CreateCustomsTransferLine(itemPM);
                }
            }

            else
            {
                if (transferLinesChangeSet != null)
                {
                    foreach (CustomsTransferLinePM itemPM in transferLinesChangeSet)
                    {
                        switch (itemPM.ChangeSetOp)
                        {
                            case ChangeSetOperation.Insert:
                                {
                                    this.CreateCustomsTransferLine(itemPM);
                                    break;
                                }

                            case ChangeSetOperation.Update:
                                {
                                    this.UpdateCustomsTransferLine(itemPM);
                                    break;
                                }

                            case ChangeSetOperation.Delete:
                                {
                                    this.DeleteCustomsTransferLine(itemPM);
                                    break;
                                }

                            default: { break; }
                        }
                    }
                }
            }
        }

        private void CreateCustomsTransferLine(CustomsTransferLinePM itemPM)
        {
            itemPM.Id = IdCounter.GetNumber("CustomsTransferLine", tenant).ToString();
            itemPM.CustomsTransferHeaderId = entityPM.Id;

            CustomsTransferLine itemPoco = new CustomsTransferLine()
            {
                Id = itemPM.Id,
                Tenant = tenant,
                CustomsTransferHeaderId = itemPM.CustomsTransferHeaderId,
            };

            CustomsTransferHeaderMapping.MapLine(itemPM, itemPoco, true);
            CustomsTransferLineRepository.Add(itemPoco);

            this.UpdateShipment(itemPM.ShipmentId);           
        }
        private void UpdateCustomsTransferLine(CustomsTransferLinePM itemPM)
        {
            CustomsTransferLine itemPoco = CustomsTransferLineRepository.GetSingleEntity(itemPM.Id);
            if (itemPoco != null)
            {
                CustomsTransferHeaderMapping.MapLine(itemPM, itemPoco, false);
                CustomsTransferLineRepository.Update(itemPoco);
            }
        }
        private void DeleteCustomsTransferLine(CustomsTransferLinePM itemPM)
        {
            CustomsTransferLine itemPoco = CustomsTransferLineRepository.GetSingleEntity(itemPM.Id);
            if (itemPoco != null)
            {
                CustomsTransferLineRepository.Remove(itemPoco);
            }
        }

        private void UpdateShipment(string myEntityId)
        {
            Shipment shipment = shipmentRepository.GetSingleShipment(myEntityId, tenant);
            if (shipment != null)
            {
                shipment.LocalCustomsTransmissionsStatusCode = "SENT";
                shipment.LocalCustomsTransmissionsStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                shipment.LocalCustomsSentByUserId = loggedContactId;
                shipment.LocalCustomsTransmissionsStatusError = null;
                shipmentRepository.Update(shipment);
            }
        }

        private void TransferData()
        {
            if (isNewEntity)
            {
                CustomsTransferService customsTransferService = new CustomsTransferService(shipments, entityPM.FileName, entityPM.CustomsTransferTypeCode, tenant);
                customsTransferService.Transfer();
            }
        }
    }
}
