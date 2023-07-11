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
            string[] InActiveStatues  = { "40", "50", "60" };
            bool isExportPoaActive_before = entityPM.IsExportPoaActive.GetValueOrDefault();
            if (entityPM.ClientPoas == null || entityPM.ClientPoas.Count() == 0)
                entityPM.IsExportPoaActive = null;
            else
                entityPM.IsExportPoaActive =
                        entityPM.ClientPoas.Any(x => x.PoaAuthorizationType == "200" && DateTime.Now >= x.StartDate && DateTime.Now <= x.EndDate && !InActiveStatues.Contains(x.PoaStatus) && x.Tenant == entityPM.Tenant);
            if (entityPM.IsExportPoaActive == true && isExportPoaActive_before == false && entityPM.IsPOAExpireReminderSent == true)
                entityPM.IsPOAExpireReminderSent = false;
        }
        protected override void UpdateComposition(ClientPM entityPM)
        {
            ClientAddressUpdateService clientAddressUpdateService = new ClientAddressUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            clientAddressUpdateService.UpdateMulti(entityPM.ClientAddresses, entityPM.DeletedClientAddresses, entityPM, false);

            ClientDrivingLicenseUpdateService clientDrivingLicenseUpdateService = new ClientDrivingLicenseUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            clientDrivingLicenseUpdateService.UpdateMulti(entityPM.ClientDrivingLicenses, entityPM.DeletedClientDrivingLicenses, entityPM, false);

            ClientsPoaUpdateService clientsPoaUpdateService = new ClientsPoaUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            clientsPoaUpdateService.UpdateMulti(entityPM.ClientPoas, entityPM.DeletedClientPoas, entityPM, false);


            ClientsTapagUpdateService clientsTapagUpdateService = new ClientsTapagUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            clientsTapagUpdateService.UpdateMulti(entityPM.ClientsTapags, entityPM.DeletedClientsTapags, entityPM, false);

            ClientIndicationUpdateService clientIndicationUpdateService = new ClientIndicationUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            clientIndicationUpdateService.UpdateMulti(entityPM.ClientIndications, entityPM.DeletedClientIndications, entityPM, false);

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
            LogMessagingUtil.Instance.AppendLine("InsertNewClientOnlyByCode");
            ClientPM newClientPM = new ClientPM();
            newClientPM.ChangeSetOp = ChangeSetOperation.Insert;
            newClientPM.Tenant = entityPM.Tenant;
            LogMessagingUtil.Instance.AppendLine("InsertNewClientOnlyByCode ,entityPM.Code" + entityPM.Code);
            if (!string.IsNullOrWhiteSpace(entityPM.Code))
            {
                newClientPM.Code = GetExternalID(entityPM.Code);
            }
            if (string.IsNullOrWhiteSpace(entityPM.Code) && !string.IsNullOrWhiteSpace(entityPM.PassportNumber))
            {
                newClientPM.Code = entityPM.PassportNumber;
            }
            LogMessagingUtil.Instance.AppendLine("InsertNewClientOnlyByCode ,newClientPM.Code"+ newClientPM.Code);
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
