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
            CourierMasterRepository MyCourierMasterRepository = new CourierMasterRepository(t.Tenant);
            CourierDeclarationRepository courierDeclarationRepository = new CourierDeclarationRepository(t.Tenant);
            DeclarationQueryService declarationQueryService = new DeclarationQueryService(t.Tenant);
            var OpenCourierMasters = MyCourierMasterRepository.GetAllOpenCourierMastersWithLandingDate(t.Tenant);
            foreach (var courierMaster in OpenCourierMasters)
            {
                var decIdsList = courierDeclarationRepository.GetDeclarationIdsByCourierMasterIDWithNoCourierCustomStatus(courierMaster.Id, t.Tenant);
                foreach (var dec in decIdsList)
                {
                    var decPM = declarationQueryService.GetSingleDeclarationById(dec, t.Tenant);
                    if (decPM != null)
                    {
                        SendDeclarationStatusRequest(decPM);
                    }
                }
                if (decIdsList != null)
                {
                    SendNatr(t.Tenant,"",courierMaster.UnifreightLeadingFile);
                }
            }


            /*CourierMasterQueryService courierMasterQueryService = new CourierMasterQueryService(t.Tenant);
            List<CourierMaster> courierMasters = courierMasterQueryService.GetAllCourierMastersToSendAutoManifest(t.Tenant);

            foreach (var courierMaster in courierMasters)
            {
                var messagingService = new DCAInUCB1170_MsgMessagingService();
                var sts = messagingService.CreateCRS(t.Tenant, null,
                    new SendALLCorrectRequestParams()
                    {
                        CourierMasterId = courierMaster.Id,
                        HAWB = courierMaster.HAWB,
                    // CourierDeclarationStatusCode = courierMaster.
                }

                    );
            }*/

            //CourierMasterQueryService courierMasterQueryService = new CourierMasterQueryService(t.Tenant);
            //List<CourierMasterPM> courierMasterPMList = courierMasterQueryService.GetAllCourierMastersForClosing(t.Tenant);
            //if(courierMasterPMList != null)
            //{
            //    LogMessagingUtil.Instance.AppendLine($"נמצאו " + courierMasterPMList.Count() + " טיסות פתוחות לסגירה " + "\n");
            //    ICustomContext dbContext = CustomContext.GetContext(t.Tenant);
            //    CourierMasterUpdateService CourierMasterUpdateService = new CourierMasterUpdateService(dbContext, new Dictionary<string, IContext>(), t.Tenant);
            //    foreach (CourierMasterPM courierMasterPMItem in courierMasterPMList)
            //    {
            //        try
            //        {
            //            courierMasterPMItem.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            //            courierMasterPMItem.IsOpen = false;
            //            CourierMasterUpdateService.Update(courierMasterPMItem, true);
            //            LogMessagingUtil.Instance.AppendLine($"נסגרה טיסה " + courierMasterPMItem.AirlinePrefix + "-" + courierMasterPMItem.MAWB + "\n");
            //        }
            //        catch
            //        {
            //            LogMessagingUtil.Instance.AppendLine($"לא נסגרה טיסה " + courierMasterPMItem.AirlinePrefix + "-" + courierMasterPMItem.MAWB + "\n");
            //        }
            //    }
            //}
            //else
            //{
            //    LogMessagingUtil.Instance.AppendLine($"לא נמצאו טיסות פתוחות לסגירה");
            //}
        }
        private void SendDeclarationStatusRequest(DeclarationPM declarationPM)
        {
            if (declarationPM.DeclarationNumber != "" && declarationPM.DeclarationNumber != null)
            {
                var loggedUserId = AuthenticationUtil.ResolveUserId(declarationPM.Tenant);
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
