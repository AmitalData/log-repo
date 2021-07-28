using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using System.Data;
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class ClientUpdateService : EntityUpdateService<Client, ClientPM, EntityPM>
    {
        protected override void OnCreating(ClientPM entityPM, EntityPM entityParentPM)
        {
            entityPM.Id = IdCounter.GetNumber("Customs.Client", entityPM.Tenant);
            if (string.IsNullOrEmpty(entityPM.FullName))
            {
                entityPM.FullName = "Empty";
            }
            if (string.IsNullOrEmpty(entityPM.Code))
            {
                entityPM.Code = "Empty";
            }
        }
        protected override void OnUpdating(ClientPM entityPM)
        {
            if (entityPM.ClientPoas == null || entityPM.ClientPoas.Count() == 0)
                entityPM.IsExportPoaActive = null;
            else
                entityPM.IsExportPoaActive =
                        entityPM.ClientPoas.Any(x => x.PoaAuthorizationType == "200" && DateTime.Now >= x.StartDate && DateTime.Now <= x.EndDate);
        }
        protected override void UpdateComposition(ClientPM entityPM)
        {
            ClientAddressUpdateService clientAddressUpdateService = new ClientAddressUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            clientAddressUpdateService.UpdateMulti(entityPM.ClientAddresses, entityPM.DeletedClientAddresses, entityPM, false);

            ClientDrivingLicenseUpdateService clientDrivingLicenseUpdateService = new ClientDrivingLicenseUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            clientDrivingLicenseUpdateService.UpdateMulti(entityPM.ClientDrivingLicenses, entityPM.DeletedClientDrivingLicenses, entityPM, false);

            ClientsPoaUpdateService clientsPoaUpdateService = new ClientsPoaUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            clientsPoaUpdateService.UpdateMulti(entityPM.ClientPoas, entityPM.DeletedClientPoas, entityPM, false);

            base.UpdateComposition(entityPM);
        }

        protected override void CheckConcurrency(ClientPM entityPM, Client entityPOCO)
        {
            //if (!entityPM.ConcurrencyGUID.Equals(entityPOCO.ConcurrencyGUID) && !entityPM.NewConcurrencyGUID.Equals(entityPOCO.ConcurrencyGUID))
            //if (entityPM.ConcurrencyGUID != entityPOCO.ConcurrencyGUID && entityPM.NewConcurrencyGUID != entityPOCO.ConcurrencyGUID)
            //{
            //    string msg = TranslateTextsClass.Translate("General.M.CantUpdateRecord", entityPM.Tenant);
            //    throw new OptimisticConcurrencyException(msg);
            //}

        }

        public void InsertNewClientOnlyByCode(ClientPM entityPM, bool commit)
        {

            ClientPM newClientPM = new ClientPM();
            newClientPM.ChangeSetOp = ChangeSetOperation.Insert;
            newClientPM.Tenant = entityPM.Tenant;

            if (!string.IsNullOrWhiteSpace(entityPM.Code))
            {
                newClientPM.Code = GetExternalID(entityPM.Code);
            }
            if (string.IsNullOrWhiteSpace(entityPM.Code) && !string.IsNullOrWhiteSpace(entityPM.PassportNumber))
            {
                newClientPM.Code = entityPM.PassportNumber;
            }
            newClientPM.PassportNumber = entityPM.PassportNumber;
            newClientPM.PassportTypeCode = entityPM.PassportTypeCode;
            newClientPM.PassportCountryCode = entityPM.PassportCountryCode;

            newClientPM.FullName = string.Concat(newClientPM.Code, " יש לשלוף לקוח");
            newClientPM.LocalCorporationName = string.Concat(newClientPM.Code, " יש לשלוף לקוח");
            newClientPM.LocalFirstName = "יש לשלוף לקוח";
            newClientPM.IsActive = true;

            this.Update(newClientPM, commit);
        }

        private string GetExternalID(string ExternalId)
        {
            while (ExternalId.Length < 9)
            {
                ExternalId = ExternalId.Insert(0, "0");
            }
            return ExternalId;
        }

        
        }
    }
