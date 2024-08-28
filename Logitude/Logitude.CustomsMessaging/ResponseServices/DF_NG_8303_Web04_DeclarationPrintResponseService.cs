
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.Helpers;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnifreightIIG.Common.DeclarationPrintServiceReference;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;
using Logitude.Customs.BL.TraceEvents;
using Logitude.AmitalMessaging.Infrastructure.FuStatus;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Server.Tools.Utils;
using Logitude.BL.InfrastructureModel.Tools.Validating;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class DF_NG_8303_Web04_DeclarationPrintResponseService : ResponseServiceBase
        <DeclarationPrintResponseData,
        DF_NG_8303_Web04_DeclarationPrint_Response,
        DF_NG_8302_Web03_DeclarationPrintRequestParams>
    {
        public UnifreightIIG.Common.CommonIIGInterface.IResponseHeaderOrFault _ResponseHeaderExeption;
        private DeclarationPM _MyDeclarationPM;
        private DateTime _TransmitionDateTime;

        public override DeclarationPrintResponseData GetResponse(DF_NG_8303_Web04_DeclarationPrint_Response customResponse, DF_NG_8302_Web03_DeclarationPrintRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override Action<DF_NG_8303_Web04_DeclarationPrint_Response> GetActionShrinkCustomResponse()
        {
            return new Action<DF_NG_8303_Web04_DeclarationPrint_Response>(this.ShrinkCustomResponse);
        }
        void ShrinkCustomResponse(DF_NG_8303_Web04_DeclarationPrint_Response customResponse)
        {
           
            if (customResponse == null) return;
            if (customResponse.DeclarationPrintAnswer == null) return;
            foreach (var item in customResponse.DeclarationPrintAnswer)
            {
                if (item.DeclarationPrintDetails != null)
                {
                    if (item.DeclarationPrintDetails.DeclarationPrint != null)
                    {

                        var MD5Hash = MD5HashUtil.GetMD5Hash(item.DeclarationPrintDetails.DeclarationPrint.content);
                        item.DeclarationPrintDetails.DeclarationPrint.content = System.Text.UTF8Encoding.UTF8.GetBytes(MD5Hash);

                    }
                }
            }


        }

        public override void OnRequestFail(DF_NG_8303_Web04_DeclarationPrint_Response customResponse, DF_NG_8302_Web03_DeclarationPrintRequestParams requestParams)
        {
            var concurrentKiller = new ConcurrentKiller();
            string CRSKey = CustomsRequestsSheetDomainModelUtil.GetCRSVirtualKey(requestParams);
            concurrentKiller.FreeLock(CRSKey, requestParams.Tenant);
        }

        public override void Update(DF_NG_8303_Web04_DeclarationPrint_Response customResponse, DF_NG_8302_Web03_DeclarationPrintRequestParams requestParams)
        {
            try {
                _TransmitionDateTime = customResponse.ResponseContentHeader.TransmitionDateTime;
                //Analayze 8303- Declaration Print
                var myDeclarationQueryService = new DeclarationQueryService(requestParams.Tenant);
                this.MyResponseData = new DeclarationPrintResponseData();

                if (customResponse.ResponseContentHeader.Exception != null)
                {
                    this.MyResponseData.Succeeded = false;
                    this.MyResponseData.HasException = true;
                    this.MyResponseData.UserMessage = customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription;
                    return;
                }

                if (customResponse.Exception != null)
                {
                    this.MyResponseData.Succeeded = false;
                    this.MyResponseData.HasException = true;
                    this.MyResponseData.UserMessage = customResponse.Exception.ExeptionDescription;
                    return;
                }

                if (customResponse.DeclarationPrintAnswer == null)
                {
                    this.MyResponseData.Succeeded = false;
                    this.MyResponseData.HasException = true;
                    this.MyResponseData.UserMessage = "לא התקבלו נתונים מהמכס";
                    return;
                }

                this.MyResponseData.Succeeded = true;
                this.MyResponseData.HasException = false;
                this.MyResponseData.DeclarationPrintAnswer = new List<DeclarationPrintM>();

                foreach (var declarationPrintAnswerItem in customResponse.DeclarationPrintAnswer)
                {

                    if (!string.IsNullOrWhiteSpace(declarationPrintAnswerItem.ExceptionPerQuery))
                    {
                        var declarationPrintDetails = new DeclarationPrintM();
                        declarationPrintDetails.SequenceNumber = declarationPrintAnswerItem.SequenceNumber;
                        declarationPrintDetails.ErrorText = declarationPrintAnswerItem.ExceptionPerQuery;
                        declarationPrintDetails.IsFiled = false;
                        this.MyResponseData.DeclarationPrintAnswer.Add(declarationPrintDetails);
                        if (customResponse.DeclarationPrintAnswer.Count() == 1)
                        {
                            this.MyResponseData.Succeeded = true;
                            this.MyResponseData.HasException = true;
                            this.MyResponseData.UserMessage = declarationPrintAnswerItem.ExceptionPerQuery;
                            return;
                        }
                    }
                    else
                    {
                        try
                        {
                            List<String> declarationTypes = new List<string>() {
                            "1", // Import 
                            "2", // Export 
                            "3"//"Declaration of Claim"
                        };
                            if (!declarationTypes.Contains(declarationPrintAnswerItem.DeclarationPrintDetails.DeclarationType))
                            {
                                LogMessagingUtil.Instance.AppendLine("The DeclarationType must be one of Import, Export, or Declaration of Claim.");
                            }
                            //if (

                            //    (declarationPrintAnswerItem.DeclarationPrintDetails.DeclarationType == "1") // Import Declaration
                            //    ||
                            //    (declarationPrintAnswerItem.DeclarationPrintDetails.DeclarationType == "2") // Export Declaration
                            //    )
                            else
                            {
                                var myDeclarationId = myDeclarationQueryService.GetIdByDeclarationNumber(declarationPrintAnswerItem.DeclarationPrintDetails.DeclarationID, requestParams.Tenant);
                                _MyDeclarationPM = myDeclarationQueryService.GetSingle(myDeclarationId, true, false);
                                if (_MyDeclarationPM == null)
                                {
                                    var declarationPrintDetails = new DeclarationPrintM();
                                    declarationPrintDetails.SequenceNumber = declarationPrintAnswerItem.SequenceNumber;
                                    declarationPrintDetails.DeclarationNumber = declarationPrintAnswerItem.DeclarationPrintDetails.DeclarationID;
                                    declarationPrintDetails.ErrorText = "Can not find declaration" + declarationPrintAnswerItem.DeclarationPrintDetails.DeclarationID;
                                    declarationPrintDetails.IsFiled = false;
                                    this.MyResponseData.DeclarationPrintAnswer.Add(declarationPrintDetails);
                                    if (customResponse.DeclarationPrintAnswer.Count() == 1)
                                    {
                                        this.MyResponseData.Succeeded = true;
                                        this.MyResponseData.HasException = true;
                                        this.MyResponseData.UserMessage = "Can not find declaration" + declarationPrintAnswerItem.DeclarationPrintDetails.DeclarationID;
                                        return;
                                    }
                                }
                                else
                                {
                                    //Add Document- DeclarationPrint
                                    AnalyzePaymentDocument(declarationPrintAnswerItem.DeclarationPrintDetails.DeclarationPrint, requestParams, this._MyDeclarationPM.VersionId);
                                    var declarationPrintDetails = new DeclarationPrintM();
                                    declarationPrintDetails.SequenceNumber = declarationPrintAnswerItem.SequenceNumber;
                                    declarationPrintDetails.DeclarationNumber = _MyDeclarationPM.DeclarationNumber;
                                    declarationPrintDetails.CustomFileNo = _MyDeclarationPM.CustomFileNo;
                                    declarationPrintDetails.IsFiled = true;
                                    this.MyResponseData.DeclarationPrintAnswer.Add(declarationPrintDetails);
                                }
                            }
                        }
                        catch (System.Exception eeee)
                        {
                            throw;//20180222 -HOPE TILL NEXT PATCH - I WILL ABLE TO RESTORE THE PROBLEM (- AS EITAN ADVISE)
                            var declarationPrintDetails = new DeclarationPrintM();
                            declarationPrintDetails.SequenceNumber = declarationPrintAnswerItem.SequenceNumber;
                            declarationPrintDetails.DeclarationNumber = declarationPrintAnswerItem.DeclarationPrintDetails.DeclarationID;
                            declarationPrintDetails.ErrorText = "Can not add DeclarationPrint";
                            declarationPrintDetails.IsFiled = false;

                            this.MyResponseData.Succeeded = true;
                            this.MyResponseData.HasException = true;
                            this.MyResponseData.UserMessage = "Crash while trying to fill:" + eeee.ToString();
                            this.MyResponseData.DeclarationPrintAnswer.Add(declarationPrintDetails);
                        }
                    }
                }
                if (_MyDeclarationPM != null)
                {
                    this.MyRequestSheetParam = new RequestSheetParam();
                    this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                    this.MyRequestSheetParam.EntityId1 = _MyDeclarationPM.Id;
                    this.MyRequestSheetParam.RequestDescription = "בקשה לטופס הצהרה " + _MyDeclarationPM.DeclarationNumber;
                }
            }
              finally
            {
                var concurrentKiller = new ConcurrentKiller();
                string CRSKey = CustomsRequestsSheetDomainModelUtil.GetCRSVirtualKey(requestParams);
                concurrentKiller.FreeLock(CRSKey, requestParams.Tenant);
            }   
        }
        private static void RaiseEvent(DeclarationPM dirtyDeclarationPM, string loggingUserId, string status_id, DateTime? status_DateTime)
        {
            //primary_number = $"{dirtyDeclarationPM.CustomFileNo},{dirtyDeclarationPM.TransportModeId == "A" ? "EFIFILEM" : "MFIFILEM" }",
            string primary_number = $"{dirtyDeclarationPM.CustomFileNo},EFIFILEM";
            if (dirtyDeclarationPM.TransportModeId != "A")
            {
                primary_number = $"{dirtyDeclarationPM.CustomFileNo},MFIFILEM";
            }

            var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
            {
                Tenant = dirtyDeclarationPM.Tenant,
                objectTableName = "Customs.Declaration",
                EventCode = status_id,
                notes = "",
                CommunicationLoggingEntityReference = dirtyDeclarationPM.DeclarationNumber,
                EntityId = dirtyDeclarationPM.Id,
                UserId = loggingUserId,

                CommunicationSubject = "FU Status " + status_id + " from logitude",
                MyFUStatus = new AmitalEventTracerModel.FUStatus()
                {
                    entname = dirtyDeclarationPM.Direction == "E" ? "BFIFILE" : "CFIFILEM",
                    primary_number = primary_number,
                    status = "new",
                    xml_status = "new",
                    status_id = status_id,
                    status_DateTime = status_DateTime ?? DateTime.Now,
                    comments = "",
                }
            };

            AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel, suppress_RAISE_EVENT: true);


        }

        

       
        private void AnalyzePaymentDocument(Attachment attachment, DF_NG_8302_Web03_DeclarationPrintRequestParams requestParams, string MyDeclarationNumVersionId)
        {
            ICommonDataContext dataContext = CommonDataContext.GetContext(requestParams.Tenant);
            var documentsFilingService = new UnifreightDocumentsFilingService(dataContext, requestParams.Tenant,
                new CustomDocumentsFilingParams() { MainInterfaceCode = "8302", IsCourier = IsCourier(requestParams.Tenant) }, MyDeclarationNumVersionId);
            var documentTypeQuery = new DocumentTypeQuery(requestParams.Tenant);
            var documentsFilingQuery = new DocumentsFilingQuery(requestParams.Tenant);
            DocumentsFilingPM documentsFilingPM = null;

            if (attachment == null | _MyDeclarationPM == null)
            {
                return;
            }

            GDMFILINGQueryService uniGDMFILINGQueryService = null;
            if (CustomsSettingQueryService.GetSettingByTenant(requestParams.Tenant).IsConnectedToUniFreight)
            {
                uniGDMFILINGQueryService = new GDMFILINGQueryService(AmitalContext.GetContext(requestParams.Tenant));
            }
            //Check if file already exists
            var objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            var documentType = documentTypeQuery.GetSinglePMByCodeAndTenant("DEC", requestParams.Tenant);
            var declarationId = _MyDeclarationPM.Direction != "E" && _MyDeclarationPM.IsAmendment == true && !_MyDeclarationPM.AmendmentDontDisplayInList? _MyDeclarationPM.AmendmentOriginalDeclartation: _MyDeclarationPM.Id;
            var documentsFilingPMList = documentsFilingQuery.GetDocumentsFilingPMsByEntityIdAndObjectTable(declarationId, null, objectTableId, "I", requestParams.Tenant);
            foreach (var documentItem in documentsFilingPMList)
            {
                if (documentItem.DocumentTypeId == documentType?.Id)
                {
                    if (uniGDMFILINGQueryService != null)
                    {
                        var gdmfiling = uniGDMFILINGQueryService.GetSingle(documentItem.Id, true);
                        if (gdmfiling?.DELETED == "T")
                        {
                            // uniface deleted!!
                            continue;
                        }
                    }
                    documentsFilingPM = documentItem;
                    break;
                }
            }


            //DocumentsMetaDataTypePM verPm= 
            if (documentsFilingPM == null)
            {
                documentsFilingPM = CreatePaymentDocument(attachment, requestParams, this._MyDeclarationPM.DeclarationNumberandVersionId);

            }
            else
            {

                UpdatePaymentDocument(documentsFilingPM, attachment, requestParams, this._MyDeclarationPM.DeclarationNumberandVersionId);
            }
            var setting = CustomsSettingQueryService.GetSettingByTenant(this._MyDeclarationPM.Tenant);
            if ( setting.IsConnectedToUniFreight || AmitalEventTracer.UseHybrid_When_NotIsConnectedToUniFreight )
            {
                if (this._MyDeclarationPM.Direction == "E")
                {
                    RaiseEvent(this._MyDeclarationPM, null, status_id: "MRS", status_DateTime: _TransmitionDateTime);
                    var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                    {

                        Tenant = _MyDeclarationPM.Tenant,
                        objectTableName = "Customs.Declaration",
                        EventCode = null,
                        notes = "",
                        CommunicationLoggingEntityReference = _MyDeclarationPM.DeclarationNumber,
                        EntityId = _MyDeclarationPM.Id,
                        UserId = _MyDeclarationPM.CreatedByUserId,

                        CommunicationSubject = "עדכון תיק מכס",

                    };
                    Logitude.AmitalMessaging.Infrastructure.FuStatus.LOGICUSTFILE logistictFile = AmitalInsertToQueueEzer.setLogistictFile(_MyDeclarationPM, _TransmitionDateTime.ToString());
                    var amitalInsertToQueueService = new AmitalInsertToQueueService<Logitude.AmitalMessaging.Infrastructure.FuStatus.LOGICUSTFILE>(logistictFile);
                    amitalInsertToQueueService.InsertToQueue(myAmitalEventTracerModel, "UpdateExportCustomsFile");

                }
            }
            //DocumentsFilingMetaDataValueQuery.UpSert(documentsFilingPM, "VER", this._MyDeclarationPM.VersionId);

        }



        private void UpdatePaymentDocument(DocumentsFilingPM documentsFilingPM, Attachment attachment, DF_NG_8302_Web03_DeclarationPrintRequestParams requestParams, string DeclarationNumVersionId)
        {
            ICommonDataContext dataContext = CommonDataContext.GetContext(requestParams.Tenant);
            var documentsFilingService = new UnifreightDocumentsFilingService(dataContext, requestParams.Tenant, new CustomDocumentsFilingParams() { MainInterfaceCode = "8302", IsCourier = IsCourier(requestParams.Tenant) }, DeclarationNumVersionId);

            documentsFilingPM.Description = "טופס הצהרה " + this._MyDeclarationPM.DeclarationNumber + "-" + this._MyDeclarationPM.VersionId;
            documentsFilingPM.Name = "טופס הצהרה " + this._MyDeclarationPM.DeclarationNumber + "-" + this._MyDeclarationPM.VersionId;
            documentsFilingPM.UpdatedByUserId = requestParams.LoggingUserId;

            documentsFilingService.Update(documentsFilingPM, attachment.content, requestParams.LoggingUserId);
            LogMessagingUtil.Instance.AppendLine("File document " + documentsFilingPM.Code + " Updated For declaration " + _MyDeclarationPM.DeclarationNumber);



        }


        private DocumentsFilingPM CreatePaymentDocument(Attachment attachment, DF_NG_8302_Web03_DeclarationPrintRequestParams requestParams, string DeclarationNumVersionId)
        {
            ICommonDataContext dataContext = CommonDataContext.GetContext(requestParams.Tenant);
            var documentsFilingService = new UnifreightDocumentsFilingService(dataContext, requestParams.Tenant, new CustomDocumentsFilingParams() { MainInterfaceCode = "8302", IsCourier = IsCourier(requestParams.Tenant) }, DeclarationNumVersionId);
            var documentTypeQuery = new DocumentTypeQuery(requestParams.Tenant);

            var documentsFilingPM = new DocumentsFilingPM();
            documentsFilingPM.Tenant = requestParams.Tenant;
            var documentType = documentTypeQuery.GetSinglePMByCodeAndTenant("DEC", requestParams.Tenant);
            documentsFilingPM.DocumentTypeId = documentType.Id;
            documentsFilingPM.Name = "טופס הצהרה " + this._MyDeclarationPM.DeclarationNumber + "-" + this._MyDeclarationPM.VersionId;
            documentsFilingPM.EntityId = this._MyDeclarationPM.Id;
            documentsFilingPM.ObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            documentsFilingPM.CreatedByUserId = requestParams.LoggingUserId;
            documentsFilingPM.OwnerId = requestParams.LoggingUserId;
            documentsFilingPM.UpdatedByUserId = requestParams.LoggingUserId;
            documentsFilingPM.ReceivedByUserId = requestParams.LoggingUserId;
            documentsFilingPM.DirectionCode = "I";
            // I/O  - only  !!  -   documentsFilingPM.DirectionCode = this._MyDeclarationPM.Direction;
            documentsFilingPM.Description = "טופס הצהרה " + this._MyDeclarationPM.DeclarationNumber + "-" + this._MyDeclarationPM.VersionId;
            documentsFilingPM.ExternalEntityName = this._MyDeclarationPM.Direction == "E" ? "BFIFILE" : "CFIFILEM";
            documentsFilingPM.ExternalEntityReference = this._MyDeclarationPM.CustomFileNo;
            documentsFilingPM.FileExtension = "PDF";



            documentsFilingService.Create(documentsFilingPM, attachment.content, requestParams.LoggingUserId);
            LogMessagingUtil.Instance.AppendLine("Filed document " + documentsFilingPM.Code + "Created For declaration " + _MyDeclarationPM.DeclarationNumber + " documentsFilingPM.ID= " + documentsFilingPM.Id);



            return documentsFilingPM;



        }

        private bool IsCourier(int tenant)
        {
            var pm = CustomsSettingQueryService.GetSettingByTenant(tenant);
            return pm?.CompanyType == "B";//Courier

        }
    }
}
