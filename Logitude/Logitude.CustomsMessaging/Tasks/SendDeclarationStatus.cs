using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Def.EntityQueryServicesExt;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Server.Tools.Contracts;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.CustomsMessaging.Tasks
{
    public class SendDeclarationStatus : ICustomsSendDeclarationStatus
    {
        public void StartRun(string taskId, int seedDefaultTenant)
        {

            var customsSettingQueryService = new CustomsSettingQueryService(seedDefaultTenant);
            var allCustomsSetting = customsSettingQueryService.GetAll();
            allCustomsSetting.ForEach(t => RunPerTenant(t));


        }

        private void RunPerTenant(CustomsSettingPM t)
        {
            LogMessagingUtil.Instance.AppendLine($"RunPerTenant({t.Tenant})");
            CourierMasterQueryService courierMasterQueryService = new CourierMasterQueryService(t.Tenant);
            CourierDeclarationRepository courierDeclarationRepository = new CourierDeclarationRepository(t.Tenant);
            DeclarationQueryService declarationQueryService = new DeclarationQueryService(t.Tenant);
            var OpenCourierMasters = courierMasterQueryService.GetAllOpenCourierMastersWithLandingDate(t.Tenant);
            foreach (var courierMaster in OpenCourierMasters)
            {
                bool isEventNATR = false;
                var loggedUserId = AuthenticationUtil.ResolveUserId(courierMaster.Tenant);
                var _CustomContext = CustomContext.GetContext(t.Tenant);
                var myDeclarationUpdateService = new DeclarationUpdateService(_CustomContext, new Dictionary<string, IContext>(), t.Tenant);
                try
                {
                    isEventNATR = myDeclarationUpdateService.CheckLeadingFileEvent(courierMaster, loggedUserId, "NATR");
                }
                catch (Exception e)
                {
                    LogMessagingUtil.Instance.AppendLine("Exception was thrown while checking if NATR exist in the couriermaster " + courierMaster.Id + Environment.NewLine + e.Message);
                }
                if (!isEventNATR)
                {
                    var decIdsList = courierDeclarationRepository.GetDeclarationIdsByCourierMasterIDWithNoCourierCustomStatus(courierMaster.Id, t.Tenant);
                    foreach (var dec in decIdsList)
                    {
                        var decPM = declarationQueryService.GetSingleDeclarationById(dec, t.Tenant);
                        if (decPM != null)
                        {
                            SendDeclarationStatusRequest(decPM, loggedUserId);
                        }
                    }
                    if (decIdsList != null)
                    {
                        SendNatr(t.Tenant, "", courierMaster.UnifreightLeadingFile);
                    }
                }
            }


           
        }
        private void SendDeclarationStatusRequest(DeclarationPM declarationPM,string loggedUserId)
        {
            if (declarationPM.DeclarationNumber != "" && declarationPM.DeclarationNumber != null)
            {
                var requestParams = new DeclarationStatusRequestParams()
                {
                    LoggingEnabled = true,
                    IsFakeResponse = true,
                    InterfaceTypeCode = "8250",
                    CustomFileNo = declarationPM.CustomFileNo,
                    DeclarationNumber = declarationPM.DeclarationNumber,
                    Tenant = declarationPM.Tenant,
                    RequestName = "Declaration Status Search",
                    ResponseName = "Declaration Status Search",
                    CargoRadio = false,
                    DeclarationRadio = true,
                    OldReshimonRadio = false,
                    OldReshimonNumber = null,
                    LoggingEntityId = declarationPM.Id,
                    RequestVIA = SendRequestVIA.WebServiceBatch,
                    SuppressSplitWR = true
                };
                try
                {
                    SBQMessageService.CreateSheetSBQMessage<DeclarationStatusRequestParams>(requestParams
                        , false, DateTime.Now
                        );
                }
                catch (CustomsRequestsSheetDomainModelServiceException myCustomsRequestsSheetServiceException)
                {
                    if (myCustomsRequestsSheetServiceException.Where == CustomsRequestsSheetDomainModelServiceException.WhereEnum.SameRequestInProgress)
                    {
                        Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("8520 RequestInProgress stop create a new one !! ");
                    }
                    throw;
                }
            }
        }
        public void SendNatr(int Tenant, string remarks, string UnifreightLeadingFile)
        {
            string loggedContactId = null;
            ContactRepository contactRepository = new ContactRepository(Tenant);
            var loggedContact = contactRepository.GetSingleContactByEmail(AuthenticationUtil.ResolveLoggingUserId(Tenant), Tenant);
            if (loggedContact != null)
            {
                loggedContactId = loggedContact.Id;
            }

            string unifrieghtEvent = "NATR";
            string eventRemarks = remarks;
            var MyUnifreightEventParam = new UnifreightEventParam()
            {
                Code = unifrieghtEvent,
                Mode = UnifreightEventMode.@new,
                EventDateTime = DateTime.Now,
                Entname = "CFIFILEM",
                PrimaryNum = UnifreightLeadingFile,
                EventRemarks = eventRemarks,
            };
            LogMessagingUtil.Instance.AppendLine("MyUnifreightEventParam = " + MyUnifreightEventParam ?? "NULL");
            var myOpenUnifreighTask = new UnifreightEventTaskService();
            myOpenUnifreighTask.UpsertEventLE2U(
                Tenant,
                loggedContactId,
                MyUnifreightEventParam);
        }
    }
}
