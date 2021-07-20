using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.QuoteModel.APIDataContract.ApiV1;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.CommonDataModel;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Server.Tools.Helpers;
using Logitude.CRM.Data.Repsitories;
using Logitude.CRM.Data.EntityPOCOs;
namespace Logitude.BL.CommonDataModel.APIDataContract.ApiV1
{
    public partial class DocumentsFilingQueryService
    {
        public DocumentsFilingPM DocumentsFilingCustomDataMappingAndValidating(DocumentsFiling MyEntity, int Tenant, bool isNew, string ComputingPartnerCode = "")
        {

            if (MyEntity.DocumentType == null)
            {
                throw new ApplicationException("DocumentType is required!");
            }

            if (MyEntity.EntityNumber == null)
            {
                throw new ApplicationException("EntityNumber is required!");
            }

            ContactRepository contactRep = new ContactRepository(Tenant);
            var resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(Tenant);
            Simplog.Data.CommonDataModel.EntityPOCOs.Contact loggedContact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, Tenant);

            DocumentsFilingPM temp = DocumentsFilingDataMappingAndValidatin(MyEntity, Tenant);
            temp.Tenant = Tenant;
            temp.DirectionCode = "I";
            temp.HasFile = true;
            temp.ReceivedDate = TenantServerConfigration.GetCurrentDateTime(Tenant);
            temp.Received = true;
            temp.ReceivedByUserId = loggedContact.Id;
            temp.EntityId = GetEntityId(MyEntity, Tenant);

            ValidateEntityId(MyEntity, temp);
            return temp;

        }
        private void ValidateEntityId(DocumentsFiling MyEntity, DocumentsFilingPM temp)
        {
            if (!string.IsNullOrEmpty(MyEntity.EntityNumber) &&
                !string.IsNullOrEmpty(MyEntity.EntityType.Name) &&
                string.IsNullOrEmpty(temp.EntityId))
            {
                throw new ApplicationException("Entity with Number " + MyEntity.EntityNumber + " doesn't exist");
            }
        }

        public string GetEntityId(DocumentsFiling MyEntity, int Tenant)
        {
            if (string.IsNullOrEmpty(MyEntity.EntityNumber))
                return null;

            if (MyEntity.EntityType.Name == "Shipment")
            {
                ShipmentRepository shipmentRepository = new ShipmentRepository(Tenant);
                return shipmentRepository.GetShipmentIdByShipmentNumber(MyEntity.EntityNumber, Tenant);
            }
            if (MyEntity.EntityType.Name == "Ticket")
            {
                TicketRepository ticketRepository = new TicketRepository(Tenant);
                return ticketRepository.GetTicketId(MyEntity.EntityNumber, Tenant);
            }

            return null;
        }
    }
}
