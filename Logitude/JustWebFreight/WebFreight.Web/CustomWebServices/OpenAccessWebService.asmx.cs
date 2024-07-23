using Logitude.AmitalMessaging.Infrastructure;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.BL.StimulReport;
using Logitude.Customs.BL.StimulReport.Mapping;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Server.Tools.ExternalServices;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Services;
using Logitude.Customs.BL.Messaging.Customs.SignQueueBL;

namespace WebFreight.Web.CustomWebServices
{
    /// <summary>
    /// Summary description for OpenAccessWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class OpenAccessWebService : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public bool GetDeclarationPMByCustomFileNo(string customFileNo, int tenant, out DeclarationPM declarationPM, out DeclarationSReport declarationSReport, out List<DeclarationDocumentsFiling> declarationDocumentsFilingList, out string errMessage)
        {
            errMessage = "";
            declarationPM = null;
            declarationSReport = null;
            declarationDocumentsFilingList = new List<DeclarationDocumentsFiling>();

            var context = CustomContext.GetContext(tenant);
            var myQueryService = new DeclarationQueryService(context);

            var decId = myQueryService.GetIdByCustomFileNo(customFileNo, tenant);
            if (String.IsNullOrWhiteSpace(decId))
            {
                return false;
            }
            declarationPM = myQueryService.GetSingle(decId, true, false);
            if (declarationPM == null)
            {
                //throw new BusinessErrorException
                errMessage = ("Id is " + decId + " but Declaration not found");
                return false;
            }

            DeclarationSRMapping declarationSRMapping = new DeclarationSRMapping();
            declarationSReport = declarationSRMapping.Get(declarationPM);
            if (declarationSReport == null)
            {
                errMessage = ("Id is " + decId + " but DeclarationSReport not found");
                return false;
            }

            ///////////////                     ATTACHMENTS     =       DeclarationDocumentsFiling        /////////////////////////
            //Get Pointers for the Declaration
            var customsDocumentPointerQueryService = new CustomsDocumentPointerQueryService(tenant);
            List<CustomsDocumentPointerPM> customsDocumentPointerPMList = customsDocumentPointerQueryService.GetParentDocumentPointer(declarationPM.Id, "Declaration", tenant);
            if (customsDocumentPointerPMList != null && customsDocumentPointerPMList.Count() > 0)
            {
                //For each Pointer
                DeclarationDocumentsFiling declarationDocumentsFiling = new DeclarationDocumentsFiling();
                CustomsDocumentsTicketQueryService customsDocumentsTicketQueryService = new CustomsDocumentsTicketQueryService(tenant);
                List<string> documentsFilingIdList = new List<string>();
                CustomsDocumentQueryService customsDocumentQueryService = new CustomsDocumentQueryService(tenant);
                CustomsDocumentMetaDataValueQueryService customsDocumentMetaDataValueQueryService = new CustomsDocumentMetaDataValueQueryService(tenant);

                foreach (var customsDocumentPointerPM in customsDocumentPointerPMList)
                {
                    //Get the Ticket
                    declarationDocumentsFiling = new DeclarationDocumentsFiling();
                    declarationDocumentsFiling.customsDocumentsTicketPM = customsDocumentsTicketQueryService.GetSingle(customsDocumentPointerPM.CustomsDocumentsTicketId, true, false);
                    if (declarationDocumentsFiling.customsDocumentsTicketPM != null && !string.IsNullOrWhiteSpace(declarationDocumentsFiling.customsDocumentsTicketPM.DocumentsFilingId))
                    {
                        declarationDocumentsFiling.documentsFilingId = declarationDocumentsFiling.customsDocumentsTicketPM.DocumentsFilingId;
                        //For the Ticket - Get the DocumentsFiling
                        var documentsFilingQuery = new DocumentsFilingQuery(tenant);
                        declarationDocumentsFiling.documentsFilingPM = documentsFilingQuery.GetSinglePM(declarationDocumentsFiling.customsDocumentsTicketPM.DocumentsFilingId, tenant);
                        if (declarationDocumentsFiling.documentsFilingPM != null)
                        {
                            documentsFilingIdList.Clear();
                            documentsFilingIdList.Add(declarationDocumentsFiling.documentsFilingPM.Id);
                            List<CustomsDocumentPM> customsDocumentPMList = new List<CustomsDocumentPM>();
                            //For DocumentsFiling - Get the CustomsDocument
                            if (documentsFilingIdList != null && documentsFilingIdList.Count() > 0)
                            {
                                customsDocumentPMList = customsDocumentQueryService.GetCustomsDocumentList(documentsFilingIdList, tenant);
                                declarationDocumentsFiling.customsDocumentPM = customsDocumentPMList.FirstOrDefault();
                            }

                            //For DocumentsFiling - Get the DocumentMetaDataValue
                            declarationDocumentsFiling.customsDocumentMetaDataValuePMList = customsDocumentMetaDataValueQueryService.GetCustomsDocumentMetaDataValuesByCustomsDocumentFilingIds(declarationDocumentsFiling.documentsFilingPM.Id, tenant);

                            declarationDocumentsFiling.pointerChild1EntityCode = customsDocumentPointerPM.Child1EntityCode;
                            declarationDocumentsFiling.pointerChild1EntityId = customsDocumentPointerPM.Child1EntityId;
                        }
                    }
                    declarationDocumentsFilingList.Add(declarationDocumentsFiling);
                }
            }

            return true;
        }


        [WebMethod]
        public bool SendSignedDeclaration(string customFileNo, int tenant, string user, string personId, out string errMessage)
        {
            errMessage = "";

            var context = CustomContext.GetContext(tenant);
            var myQueryService = new DeclarationQueryService(context);

            if (String.IsNullOrWhiteSpace(user))
            {
                errMessage = ("user is null");
                return false;
            }
            var repo = new UserRepository(tenant);
            var pocoUser=repo.GetSingleUserByCode(user, tenant, false);
            if (pocoUser == null)
            {
                errMessage = ("UserByCode  not found");
                return false;
            }

            personId = pocoUser.PersonalId;
            if (String.IsNullOrWhiteSpace(personId))
            {
                errMessage = ("UserByCode  not found");
                return false;

            }
            var decId = myQueryService.GetIdByCustomFileNo(customFileNo, tenant);
            if (String.IsNullOrWhiteSpace(decId))
            {
                errMessage = ("CustomFileNo not found");
                return false;
            }
            DeclarationPM declarationPM = myQueryService.GetSingle(decId, true, false);
            if (declarationPM == null)
            {
                //throw new BusinessErrorException
                errMessage = ("Id is " + decId + " but Declaration not found");
                return false;
            }

            var dec = new Dictionary<string, string>();
            dec.Add("FromMevaker", "YES");
            var unifreightListOnServerOnly = UnifreightListsUtil.Serialize(dec);
                
            GenericRequestParams genericRequestParams = new GenericRequestParams()
            {
                Tenant = tenant,
                LoggingEntityId = declarationPM.Id,
                AppicationId = declarationPM.Id,
                LoggingUserId = pocoUser.Id,
                RequestVIA = SendRequestVIA.WebServiceBatch,
                LoggingEntityReference = declarationPM.DeclarationNumber,
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration"),
                LoggingEnabled = true,
                ForcePersonalSign = true,
                UnifreightListOnServerOnly = unifreightListOnServerOnly,
            };
            var signQueueHSMService = new SignQueueHSMService();
            var dbSignQueueService = new SignQueueHybridDbService();
            SignMethodByQueueEnum signMethodByQueueEnum = SignMethodByQueueEnum.None;
            string availableSignServer = null;

            if (signQueueHSMService.IsHSMSign_IsOn(tenant))
            {
                (availableSignServer, signMethodByQueueEnum) = dbSignQueueService
                    .GetAvailableSignServer(tenant, SignQueueByType.SignQueueByPersonId, personId);
            }
            if (string.IsNullOrEmpty(availableSignServer))
            {
                if (!CheckSignServerOn(tenant, personId))
                {
                    errMessage = ("Sign server not available");
                    return false;
                }
                            
            }
            INF_MSG_GenericResponseData responseData;
            var myDF_MSG10000_ImportDeclarationMessagingService = new DF_MSG10000_ImportDeclarationMessagingService();
            responseData = myDF_MSG10000_ImportDeclarationMessagingService.Send(genericRequestParams);

            if (!responseData.Succeeded)
            {
                errMessage = responseData.UserMessage;
                return false;
            }

            return true;
        }

        private bool CheckSignServerOn(int tenant, string personId)
        {
            //var availableSignServer = SignQueue.Instance.GetAvailableSignServer(tenant, Server.Tools.ExternalServices.SignQueueByType.SignQueueByPersonId, personId);
            var availableSignServer = SignQueue.Instance.GetAvailableSignServer(tenant, SignQueueByType.SignQueueByPersonId, personId);
            return !String.IsNullOrWhiteSpace(availableSignServer);
        }

    }
}
