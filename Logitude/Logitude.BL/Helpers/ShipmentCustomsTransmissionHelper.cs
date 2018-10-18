using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.Helpers
{
    public class ShipmentCustomsTransmissionHelper
    {
        private int Tenant;
        public ShipmentCustomsTransmissionHelper(int tenant)
        {
            this.Tenant = tenant;
        }

        public void Run(ShipmentCustomsTransmissionArgs args)
        {
            string loggedUserEmail = AuthenticationUtil.GetAuthenticatedUser();
            ContactRepository contactRep = new ContactRepository(Tenant);
            Contact loggedContact = contactRep.GetSingleContactByEmail(loggedUserEmail, Tenant);

            ShipmentCustomsTransmissionRepository repository = new ShipmentCustomsTransmissionRepository(Tenant);
            ShipmentCustomsTransmission myEntity = repository.GetSingleCustomsTransmissionbyShipmentIdAndMessageType(args.ShipmentId, args.MessageType, Tenant);

            if (myEntity != null)
            {
                myEntity.LastSendDate = TenantServerConfigration.GetCurrentDateTime(Tenant);
                myEntity.SentByUserId = loggedContact.Id;
                myEntity.CommunicationLogId = args.CommunicationLogId;
                myEntity.Error = args.Error;
                myEntity.Status = args.Status;

                repository.Update(myEntity);
                repository.SubmitChanges();
            }

            else
            {
                myEntity = new ShipmentCustomsTransmission()
                {
                    Id = IdCounter.GetNumber("ShipmentCustomsTransmission", Tenant).ToString(),
                    Tenant = Tenant,
                    ShipmentId = args.ShipmentId,
                    LastSendDate = TenantServerConfigration.GetCurrentDateTime(Tenant),
                    SentByUserId = loggedContact.Id,
                    CommunicationLogId = args.CommunicationLogId,
                    Error = "",
                    MessageCode = args.MessageType,
                    Status = args.Status,
                };

                repository.Add(myEntity);
                repository.SubmitChanges();
            }
        }
    }

    public class ShipmentCustomsTransmissionArgs
    {
        public string ShipmentId { get; set; }
        public string MessageType { get; set; }
        public string Status { get; set; }
        public string CommunicationLogId { get; set; }
        public string Error { get; set; }
    }
}
