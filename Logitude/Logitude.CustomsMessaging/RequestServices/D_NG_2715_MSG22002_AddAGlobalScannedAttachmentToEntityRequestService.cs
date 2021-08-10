using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityKeys;
using System.Data.Common;
using System.Data.SqlClient;
using Logitude.Server.Tools.Helpers;
using UnifreightIIG.Common.GlobalScannedAttachmentToEntityServiceReference;
using Logitude.Server.Tools.Models;
using Logitude.Customs.BL.Messaging.Customs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.CustomsMessaging.MessagingServices;
using Unifreight.Data.AmitalModel.Repsitories;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntityRequestService : RequestServiceBase<D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntity, D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntityRequestParam>
    {
        private ICustomContext _Context;
        CustomsDocumentPM _CustomsDocumentPM;
        CustomsDocumentsTicketPM _CustomsDocumentsTicketPM;
        bool _IsSendAnywayWithoutAttachment = false;

        public override void ManipulateRequestParams(D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntityRequestParam requestParams)
        {
            //var repo = new GDMFILEVERRepository(requestParams.Tenant);
            //var list =repo.GetList(requestParams.DocumentsFilingId);
            //var lastVer = list.Max(r => r.VERSION);
            //var lastGDMFILEVER= list.First(r => r.VERSION == lastVer);
            ////9558452
            ////7000000
            //if (lastGDMFILEVER.FILESIZE> 3000000)
            //{
                
            //    requestParams.RequestVIAChangeDue = "lastGDMFILEVER.FILESIZE> 3000000";
            //    LogMessagingUtil.Instance.AppendLine("ManipulateRequestParams:SendRequestVIA.DCABatch" + requestParams.RequestVIAChangeDue + ":FILESIZE=" + lastGDMFILEVER.FILESIZE.GetValueOrDefault().ToString());
            //    requestParams.RequestVIA = SendRequestVIA.DCABatch;

            //}
            base.ManipulateRequestParams(requestParams);
        }

        public override D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntity GetRequest(D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntityRequestParam requestParams)
        {
            var req = new D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntity();
            this._Context = CustomContext.GetContext(requestParams.Tenant);
            var customsDocumentQueryService = new CustomsDocumentQueryService(_Context);
            var customsDocumentsTicketQueryService = new CustomsDocumentsTicketQueryService(_Context);
            _CustomsDocumentsTicketPM = null;

            _CustomsDocumentPM = customsDocumentQueryService.GetSingle(requestParams.DocumentsFilingId, true, false);
            if (_CustomsDocumentPM == null)
            {
                LogMessagingUtil.Instance.AppendLine("No customs Document for ID " + requestParams.DocumentsFilingId + " ,Document Ticket Id: " + requestParams.DocumentsTicketId);
                throw new BusinessErrorException("No customs Document for ID " + requestParams.DocumentsFilingId);
            }

            if (!string.IsNullOrWhiteSpace(requestParams.DocumentsTicketId))
            {
                _CustomsDocumentsTicketPM = customsDocumentsTicketQueryService.GetSingle(requestParams.DocumentsTicketId, true, false);
                CustomsDocumentPointerPM customsDocumentPointerPM = _CustomsDocumentsTicketPM.CustomsDocumentPointers.FirstOrDefault();
                if (!String.IsNullOrWhiteSpace(_CustomsDocumentPM.CustomsDocId))
                {
                    if (customsDocumentPointerPM.ParentEntityCode == "CustomsCollateral")
                    {
                        _IsSendAnywayWithoutAttachment = true;
                        LogMessagingUtil.Instance.AppendLine("Customs Document already sent to Customs").AppendLine("_IsSendAnywayWithoutAttachment = true;");
                    }
                }
            }

            if (!String.IsNullOrWhiteSpace(_CustomsDocumentPM.CustomsDocId) && !_IsSendAnywayWithoutAttachment)
            {
                ToCancelSheetAfterGetRequest = true;
                LogMessagingUtil.Instance.AppendLine("Customs Document already sent to Customs").AppendLine("_ToCancelSheetAfterGetRequest = true;");
                return new D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntity();
                //throw  new BusinessErrorException("Customs Document already sent to Customs");
            }

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.CustomsDocument");
            this.MyRequestSheetParam.EntityId1 = requestParams.DocumentsFilingId;

            string requestDescription = "שליחת צרופה " + _CustomsDocumentPM.ExternalAttachmentId;

            //Get document details
            req.Attachment = GetAttachment();

            // If the document is required by customs- get connected entity details (By getting document pointer details)
            // If the document is required it will be linked to only one pointer
            req.documentID = 0;
            req.documentIDSpecified = false;
            if (!string.IsNullOrWhiteSpace(requestParams.DocumentsTicketId))
            {
                //_CustomsDocumentsTicketPM = customsDocumentsTicketQueryService.GetSingle(requestParams.DocumentsTicketId, true, false);
                if (_CustomsDocumentsTicketPM != null && _CustomsDocumentsTicketPM.RequestedCustomsDocId != null)
                {
                    int requiredDocID = 0;
                    int.TryParse(_CustomsDocumentsTicketPM.RequestedCustomsDocId, out requiredDocID);
                    req.documentID = requiredDocID;
                    req.documentIDSpecified = true;
                    req.RelatedEntity = GetRelatedEntity();
                    requestDescription = "שליחת צרופה " + _CustomsDocumentPM.ExternalAttachmentId + "- מענה לדרישה " + _CustomsDocumentsTicketPM.RequestedCustomsDocId;
                }
                else if(_CustomsDocumentsTicketPM != null)
                {
                    req.RelatedEntity = GetRelatedCollateralEntity();
                }
                if(req.RelatedEntity != null && req.RelatedEntity.entityIdKey1 == null)
                {
                    req.RelatedEntity = null;
                }
            }
            _Context = null;

            this.MyRequestSheetParam.RequestDescription = requestDescription;
            //this.MyRequestSheetParam = D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntityMessagingService.GetReqSheetParam(requestParams.DocumentsFilingId, requestParams.DeclaretionId, requestDescription);

            return req;
        }

        private ConnectedEntity GetRelatedEntity()
        {
            var relatedEntity = new ConnectedEntity();
            var myDeclarationQueryService = new DeclarationQueryService(_Context);

            CustomsDocumentPointerPM customsDocumentPointerPM = _CustomsDocumentsTicketPM.CustomsDocumentPointers.FirstOrDefault();
            if (customsDocumentPointerPM != null)
            {
                if (customsDocumentPointerPM.ParentEntityCode == "Declaration")
                {
                    var myDeclarationPM = myDeclarationQueryService.GetSingle(customsDocumentPointerPM.ParentEntityId, true, false);
                    if (myDeclarationPM != null)
                    {
                        this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                        this.MyRequestSheetParam.EntityId1 = myDeclarationPM.Id;

                        relatedEntity.entityType = 1055;
                        relatedEntity.entityIdKey1 = myDeclarationPM.DeclarationNumber;
                        if (customsDocumentPointerPM.Child1EntityCode == "SupplierInvoice" && customsDocumentPointerPM.Child1EntityId != null)
                        {
                            int child1EntityId = 0;
                            int.TryParse(customsDocumentPointerPM.Child1EntityId, out child1EntityId);
                            var supplierInvoicePM = myDeclarationPM.SupplierInvoices.FirstOrDefault(si => si.InvoiceCounterKey == child1EntityId);
                            relatedEntity.entityIdKey2 = supplierInvoicePM.SequenceNumeric.ToString();

                            if (customsDocumentPointerPM.Child2EntityCode == "SupplierInvoiceItem" && customsDocumentPointerPM.Child2EntityId != null)
                            {
                                int child2EntityId = 0;
                                int.TryParse(customsDocumentPointerPM.Child2EntityId, out child2EntityId);
                                var supplierInvoiceItemPM = supplierInvoicePM.SupplierInvoiceItems.FirstOrDefault(si => si.LineNumber == child2EntityId);
                                relatedEntity.entityIdKey3 = supplierInvoiceItemPM.SequenceNumeric.ToString();
                            }
                        }
                        else if (customsDocumentPointerPM.Child1EntityCode != null && customsDocumentPointerPM.Child1EntityId != null)
                        {
                            relatedEntity.entityIdKey2 = customsDocumentPointerPM.Child1EntityId;
                        }
                    }
                }
                else if (customsDocumentPointerPM.ParentEntityCode == "Claim")
                {
                    var myClaimQueryService = new ClaimQueryService(_Context);
                    var myClaimPM = myClaimQueryService.GetSingle(customsDocumentPointerPM.ParentEntityId, true, false);
                    if (myClaimPM != null)
                    {
                        this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Claim");
                        this.MyRequestSheetParam.EntityId1 = myClaimPM.Id;

                        //relatedEntity.entityType = 12383;
//                        relatedEntity.entityType = 1008;
                        if (customsDocumentPointerPM.Child1EntityCode == "ClaimsRelatedEntity" && customsDocumentPointerPM.Child1EntityId != null)
                        {
                            int child1EntityId = 0;
                            int.TryParse(customsDocumentPointerPM.Child1EntityId, out child1EntityId);
                            ClaimsRelatedEntityPM claimsRelatedEntityPM = myClaimPM.ClaimsRelatedEntities.FirstOrDefault(si => si.EntityCounterKey == child1EntityId);
                            if (!string.IsNullOrEmpty(claimsRelatedEntityPM.TapagNumber))
                            {
                                relatedEntity.entityType = 1008;
                                relatedEntity.entityIdKey1 = claimsRelatedEntityPM.TapagNumber;
                                if (claimsRelatedEntityPM.Numeral != null)
                                {
                                    relatedEntity.entityIdKey2 = claimsRelatedEntityPM.Numeral.ToString();
                                }
                            }
                        }
                    }
                }
                else if (customsDocumentPointerPM.ParentEntityCode == "CustomsCollateral")
                {
                    var myCustomsCollateralQueryService = new CustomsCollateralQueryService(_Context);
                    var myCustomsCollateralPM = myCustomsCollateralQueryService.GetSingle(customsDocumentPointerPM.ParentEntityId, true, false);
                    if (myCustomsCollateralPM != null)
                    {
                        relatedEntity.entityType = 12234;
                        relatedEntity.entityIdKey1 = myCustomsCollateralPM.CollateralRequestNumber;
                    }
                }
                else if (customsDocumentPointerPM.ParentEntityCode == "Vehicle")
                {
                    this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Vehicle");
                    this.MyRequestSheetParam.EntityId1 = customsDocumentPointerPM.ParentEntityId;
                }
                if(!string.IsNullOrEmpty(customsDocumentPointerPM.OriginEntity))
                {
                    relatedEntity.entityType = Convert.ToInt32(customsDocumentPointerPM.OriginEntity);
                }
            }

            return relatedEntity;
        }

        private ConnectedEntity GetRelatedCollateralEntity()
        {
            var relatedEntity = new ConnectedEntity();
            var myDeclarationQueryService = new DeclarationQueryService(_Context);

            CustomsDocumentPointerPM customsDocumentPointerPM = _CustomsDocumentsTicketPM.CustomsDocumentPointers.FirstOrDefault();
            if (customsDocumentPointerPM != null)
            {
                if (customsDocumentPointerPM.ParentEntityCode == "CustomsCollateral")
                {
                    var myCustomsCollateralQueryService = new CustomsCollateralQueryService(_Context);
                    var myCustomsCollateralPM = myCustomsCollateralQueryService.GetSingle(customsDocumentPointerPM.ParentEntityId, true, false);
                    if (myCustomsCollateralPM != null)
                    {
                        relatedEntity.entityType = 12234;
                        relatedEntity.entityIdKey1 = myCustomsCollateralPM.CollateralRequestNumber;
                    }
                }
                else if (customsDocumentPointerPM.ParentEntityCode == "Claim")
                {
                    var myClaimQueryService = new ClaimQueryService(_Context);
                    var myClaimPM = myClaimQueryService.GetSingle(customsDocumentPointerPM.ParentEntityId, true, false);
                    if (myClaimPM != null)
                    {
                        this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Claim");
                        this.MyRequestSheetParam.EntityId1 = myClaimPM.Id;
                        if (customsDocumentPointerPM.Child1EntityCode == "ClaimsRelatedEntity" && customsDocumentPointerPM.Child1EntityId != null)
                        {
                            int child1EntityId = 0;
                            int.TryParse(customsDocumentPointerPM.Child1EntityId, out child1EntityId);
                            ClaimsRelatedEntityPM claimsRelatedEntityPM = myClaimPM.ClaimsRelatedEntities.FirstOrDefault(si => si.EntityCounterKey == child1EntityId);
                            if (!string.IsNullOrEmpty(claimsRelatedEntityPM.TapagNumber))
                            {
                                relatedEntity.entityType = 1008;
                                relatedEntity.entityIdKey1 = claimsRelatedEntityPM.TapagNumber;
                                if (claimsRelatedEntityPM.Numeral != null)
                                {
                                    relatedEntity.entityIdKey2 = claimsRelatedEntityPM.Numeral.ToString();
                                }
                            }
                        }
                    }
                }
                else if (customsDocumentPointerPM.ParentEntityCode == "Vehicle")
                {
                    this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Vehicle");
                    this.MyRequestSheetParam.EntityId1 = customsDocumentPointerPM.ParentEntityId;
                }
            }

            return relatedEntity;
        }

        private Attachment GetAttachment()
        {
            byte[] byteArray = null;
            var documentrepository = new DocumentRepository(_CustomsDocumentPM.Tenant);
            var document = documentrepository.GetSingleDocument(_CustomsDocumentPM.Tenant,
                //_CustomsDocumentPM.DocumentsFilingId
                _CustomsDocumentPM.DocumentId
                );
            if (document == null)
            {
                throw new BusinessErrorException(" CustomsDocument.DocumentId is missing ");
            }
            if (!CustomsRequestsSheetDomainModelService<D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntityRequestParam>.GetBlob(document.Tenant, document, out byteArray))
            {
                throw new BusinessErrorException("Unable to get Bolb Of " + _CustomsDocumentPM.DocumentsFilingId);
            }

            if (!String.IsNullOrWhiteSpace(_CustomsDocumentPM.CustomsDocId))
            {

                var attachmentOnly = new Attachment();
                attachmentOnly.externalAttachmentID = _CustomsDocumentPM.ExternalAttachmentId;
                attachmentOnly.IsAttachment = "false";
                return attachmentOnly;
            }

            var attachment = new Attachment();
            attachment.AdditionalData = GetAttachmentAdditionalData(_CustomsDocumentPM.CustomsDocumentMetaDataValues);
            attachment.attachmentID = "false";
            attachment.content = byteArray;
            attachment.externalAttachmentID = _CustomsDocumentPM.ExternalAttachmentId;
            attachment.Remark = _CustomsDocumentPM.DocumentRemarks;
            attachment.documentType = _CustomsDocumentPM.DocumentTypeCode;
            var bolbName = document.GetBlobUrl("");
            if (!String.IsNullOrWhiteSpace(bolbName))
            {
                bolbName = System.IO.Path.GetFileName(bolbName);
            }
            attachment.fileName = bolbName;
            attachment.IsAttachment = "true";
            return attachment;
        }

        private AttachmentAdditionalData[] GetAttachmentAdditionalData(List<CustomsDocumentMetaDataValuePM> customsDocumentMetaDataList)
        {
            var AdditionalDataList = new List<AttachmentAdditionalData>();
            foreach (var customsDocumentMetaData in customsDocumentMetaDataList)
            {
                var AdditionalData = new AttachmentAdditionalData();
                int fieldId;
                if (int.TryParse(customsDocumentMetaData.MetaDataTypeCode, out fieldId))
                {
                    if (!String.IsNullOrWhiteSpace(customsDocumentMetaData.MetaDataValue))
                    {
                        AdditionalData.fieldID = fieldId;
                        //if (customsDocumentMetaData.MetaDataTypeCode == "55")
                        //{
                        //    AdditionalData.fieldData = DateExt.GetToDay();
                        //}
                        //else
                        //{
                        //    AdditionalData.fieldData = customsDocumentMetaData.MetaDataValue;
                        //}
                        AdditionalData.fieldData = customsDocumentMetaData.MetaDataValue;
                        AdditionalDataList.Add(AdditionalData);
                    }
                }

            }

            return AdditionalDataList.ToArray();
        }

        public static byte[] stringToBase64ByteArray(String input)
        {
            byte[] ret = System.Text.Encoding.Unicode.GetBytes(input);
            string s = Convert.ToBase64String(ret);
            ret = System.Text.Encoding.Unicode.GetBytes(s);
            return ret;
        }
        void ShrinkCustomRequest(D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntity customRequest)
        {
            if (customRequest.Attachment == null || customRequest.Attachment.IsAttachment == "false") return;
            var MD5Hash = MD5HashUtil.GetMD5Hash(customRequest.Attachment.content);
            customRequest.Attachment.content = System.Text.UTF8Encoding.UTF8.GetBytes(MD5Hash);

        }
        public override Action<D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntity> GetActionShrinkCustomRequest()
        {
            return ShrinkCustomRequest;
        }
    }

    public static class DateExt
    {
        public  static string GetToDay()
        {
            var today =
                ("00" + DateTime.Now.Day.ToString()).GetLast(2) + "." +
                ("00" + DateTime.Now.Month.ToString()).GetLast(2) + "." +
                ("00" + DateTime.Now.Year.ToString()).GetLast(2)
                ;

            return today;
        }
        public static string GetLast(this string source, int tail_length)
        {
            if (tail_length >= source.Length)
                return source;
            return source.Substring(source.Length - tail_length);
        }
    }
}
