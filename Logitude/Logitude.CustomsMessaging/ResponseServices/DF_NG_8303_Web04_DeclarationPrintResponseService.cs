
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

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class DF_NG_8303_Web04_DeclarationPrintResponseService : ResponseServiceBase
        <DeclarationPrintResponseData,
        DF_NG_8303_Web04_DeclarationPrint_Response,
        DF_NG_8302_Web03_DeclarationPrintRequestParams>
    {
        public UnifreightIIG.Common.CommonIIGInterface.IResponseHeaderOrFault _ResponseHeaderExeption;
        private DeclarationPM _MyDeclarationPM;

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
                if (item.DeclarationPrintDetails!=null)
                {
                    if (item.DeclarationPrintDetails.DeclarationPrint!=null)
                    {
                        
                        var MD5Hash = MD5HashUtil.GetMD5Hash(item.DeclarationPrintDetails.DeclarationPrint.content);
                        item.DeclarationPrintDetails.DeclarationPrint.content = System.Text.UTF8Encoding.UTF8.GetBytes(MD5Hash);

                    }
                }
            }
             
                
        }



        public override void Update(DF_NG_8303_Web04_DeclarationPrint_Response customResponse, DF_NG_8302_Web03_DeclarationPrintRequestParams requestParams)
        {
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
                        if (declarationPrintAnswerItem.DeclarationPrintDetails.DeclarationType == "1") // Import Declaration
                        {
                            var myDeclarationId = myDeclarationQueryService.GetIdByDeclarationNumber(declarationPrintAnswerItem.DeclarationPrintDetails.DeclarationID, requestParams.Tenant);
                            _MyDeclarationPM = myDeclarationQueryService.GetSingle(myDeclarationId, false, false);
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
                    catch(System.Exception eeee)
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

        private void AnalyzePaymentDocument(Attachment attachment, DF_NG_8302_Web03_DeclarationPrintRequestParams requestParams,string MyDeclarationNumVersionId)
        {
            ICommonDataContext dataContext = CommonDataContext.GetContext(requestParams.Tenant);
            var documentsFilingService = new UnifreightDocumentsFilingService(dataContext, requestParams.Tenant,
                new CustomDocumentsFilingParams() { MainInterfaceCode = "8302" , IsCourier = IsCourier(requestParams.Tenant) }, MyDeclarationNumVersionId);
            var documentTypeQuery = new DocumentTypeQuery(requestParams.Tenant);
            var documentsFilingQuery = new DocumentsFilingQuery(requestParams.Tenant);
            DocumentsFilingPM documentsFilingPM = null;

            if (attachment == null | _MyDeclarationPM == null)
            {
                return;
            }


            //Check if file already exists
            var objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            var documentType = documentTypeQuery.GetSinglePMByCodeAndTenant("DEC", requestParams.Tenant);
            var documentsFilingPMList = documentsFilingQuery.GetDocumentsFilingPMsByEntityIdAndObjectTable(_MyDeclarationPM.Id, null, objectTableId, "I", requestParams.Tenant);
            foreach (var documentItem in documentsFilingPMList)
            {
                if (documentItem.DocumentTypeId == documentType.Id)
                {
                    documentsFilingPM = documentItem;
                    break;
                }
            }

            
            //DocumentsMetaDataTypePM verPm= 
            if (documentsFilingPM == null)
            {
              documentsFilingPM =CreatePaymentDocument(attachment, requestParams, this._MyDeclarationPM.DeclarationNumberandVersionId);
            }
            else
            {
                
                UpdatePaymentDocument(documentsFilingPM, attachment, requestParams, this._MyDeclarationPM.DeclarationNumberandVersionId);
            }
            //DocumentsFilingMetaDataValueQuery.UpSert(documentsFilingPM, "VER", this._MyDeclarationPM.VersionId);

        }



        private void UpdatePaymentDocument(DocumentsFilingPM documentsFilingPM, Attachment attachment, DF_NG_8302_Web03_DeclarationPrintRequestParams requestParams,string DeclarationNumVersionId)
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
            var documentsFilingService = new UnifreightDocumentsFilingService(dataContext, requestParams.Tenant, new CustomDocumentsFilingParams() { MainInterfaceCode = "8302" , IsCourier = IsCourier(requestParams.Tenant) }, DeclarationNumVersionId);
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
            documentsFilingPM.Description = "טופס הצהרה " + this._MyDeclarationPM.DeclarationNumber + "-" + this._MyDeclarationPM.VersionId;
            documentsFilingPM.ExternalEntityName = "CFIFILEM";
            documentsFilingPM.ExternalEntityReference = this._MyDeclarationPM.CustomFileNo;
            documentsFilingPM.FileExtension = "PDF";

            
            
            documentsFilingService.Create(documentsFilingPM, attachment.content, requestParams.LoggingUserId);
            LogMessagingUtil.Instance.AppendLine("Filed document " + documentsFilingPM.Code + "Created For declaration " + _MyDeclarationPM.DeclarationNumber);
            return documentsFilingPM;



        }

        private bool IsCourier(int tenant)
        {
            var pm = CustomsSettingQueryService.GetSettingByTenant(tenant);
            return pm?.CompanyType == "B";//Courier

        }
    }
}
