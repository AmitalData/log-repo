using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer;
using Logitude.Customs.BL.NotificationBL;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.ResponseServices.DeclarationErrorPointer;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.Storage;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using UnifreightIIG.Common.MessageLib.Docs;
using Logitude.AmitalMessaging.Customs.CustomFile;
using Logitude.AmitalMessaging.Utils;
using Unifreight.BL.EntityPMs.UGenerated;
using Unifreight.BL.EntityQueryServices;
using System.Collections;
using Unifreight.Data.AmitalModel;
using UnifreightIIG.Common.DeclarationFilterParamServiceReference;
using Newtonsoft.Json;

namespace Logitude.CustomsMessaging.ResponseServices
{

    public class TPG_NG_8249_Web10_TPGDeclarationDetailResponseService : ResponseServiceBase<
         DeclarationFilterResponseData, TPG_NG_8249_Web10_TPGDeclarationDetail, DeclarationFilterRequestParams>
    {
        public UnifreightIIG.Common.CommonIIGInterface.IResponseHeaderOrFault _ResponseHeaderExeption;
        public bool _IsSubmitDeclarationResponse { get; set; }
        DeclarationPM _MyDeclarationPM;
        ConsignmentPM _MyConsignmentPM;
        private GTRTRANQueryService _GTRTRANQueryService;
        private Dictionary<string, IList> _MyLocalCache = new Dictionary<string, IList>();
        private AmitalContext _AmitalContext;

        public override void Update(TPG_NG_8249_Web10_TPGDeclarationDetail customResponse, DeclarationFilterRequestParams requestParams)
        {
            //
            string xml = null;
            var responseName = requestParams.ResponseName;
            var context = CustomContext.GetContext(requestParams.Tenant);
            var myDeclarationQueryService = new DeclarationQueryService(context);
            var myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            DeclarationErrorPointerService mydDclarationErrorPointerService = new DeclarationErrorPointerService();

            this.MyResponseData = new DeclarationFilterResponseData();

            if (!string.IsNullOrWhiteSpace(requestParams.DeclarationId))
            {
                if (this.MyRequestSheetParam == null)
                {
                    this.MyRequestSheetParam = new RequestSheetParam();
                }
                this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                this.MyRequestSheetParam.EntityId1 = requestParams.DeclarationId;
            }

            //Checking foe Exceptions
            if (customResponse.ResponseContentHeader.Exception != null || customResponse.Exception != null || _ResponseHeaderExeption != null)
            {
                if (customResponse.ResponseContentHeader.Exception != null)
                {
                    this.MyResponseData.UserMessage = customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription;
                }
                else
                {
                    if (customResponse.Exception != null)
                    {
                        this.MyResponseData.UserMessage = customResponse.Exception.ExeptionDescription;
                    }
                    else
                    {
                        this.MyResponseData.UserMessage = _ResponseHeaderExeption.ErrorDescription;
                    }
                }
                this.MyResponseData.HasException = true;
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.ResponseStatusXML = GetDummyXml(this.MyResponseData.UserMessage, requestParams.IsAngularClient);
                return;
            }

            if (customResponse.ResponseContentHeader != null)
            {
                //if (customResponse.ResponseContentHeader.ApplicationID == 0)
                {
                    if (customResponse.ResponseContentHeader.Exception != null)
                    {
                        if (String.IsNullOrWhiteSpace(customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription))
                        {
                            var errMess = "No Declaration details in the Response " + requestParams.DeclarationNumber;
                            LogMessagingUtil.Instance.AppendLine(errMess);
                            this.MyResponseData.UserMessage = errMess;
                        }
                        else
                        {
                            var errMess = "No Declaration details in the Response " + customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription + ". " + requestParams.DeclarationNumber;
                            LogMessagingUtil.Instance.AppendLine(errMess);
                            this.MyResponseData.UserMessage = errMess;
                        }
                        this.MyResponseData.Succeeded = true;
                        this.MyResponseData.HasException = true;
                        this.MyResponseData.ResponseStatusXML = GetDummyXml(this.MyResponseData.UserMessage, requestParams.IsAngularClient);
                        return;
                    }
                }
            }

            if (customResponse.Claim == null && customResponse.Deficit == null && customResponse.GeneralDetails == null && customResponse.Guarantee == null)
            {
                if (customResponse.ResponseContentHeader.Exception == null)
                {
                    if (customResponse.ResponseContentHeader.Exception != null)
                    {
                        if (String.IsNullOrWhiteSpace(customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription))
                        {
                            var errMess = "No details in the Response " + requestParams.DeclarationNumber;
                            LogMessagingUtil.Instance.AppendLine(errMess);
                            this.MyResponseData.UserMessage = errMess;
                        }
                        else
                        {
                            var errMess = "No details in the Response " + customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription + ". " + requestParams.DeclarationNumber;
                            LogMessagingUtil.Instance.AppendLine(errMess);
                            this.MyResponseData.UserMessage = errMess;
                        }
                        this.MyResponseData.Succeeded = true;
                        this.MyResponseData.HasException = true;
                    }
                }
                return;
            }

            LogMessagingUtil.Instance.AppendLine("Analyze Manifest Status Query response " + requestParams.DeclarationNumber);

            if (string.IsNullOrWhiteSpace(requestParams.DeclarationId))
            {
                var text = "Can not find declaration";
                if (!string.IsNullOrWhiteSpace(text))
                {
                    LogMessagingUtil.Instance.AppendLine(text);

                    LogMessagingUtil.Instance.AppendLine(text);
                    this.MyResponseData.Succeeded = true;
                    this.MyResponseData.HasException = false;
                    this.MyResponseData.ResponseStatusXML = "";// GetDummyXml(text, customResponse.Cargo);
                    return;
                }
            }
            else
            {
                this._MyDeclarationPM = myDeclarationQueryService.GetSingle(requestParams.DeclarationId, true, false);

                if (this._MyDeclarationPM == null)
                {
                    var text = "Can not find declaration";

                    LogMessagingUtil.Instance.AppendLine(text);

                    LogMessagingUtil.Instance.AppendLine(text);
                    this.MyResponseData.DeclarationID = requestParams.DeclarationId;
                    this.MyResponseData.Succeeded = true;
                    this.MyResponseData.HasException = false;
                    this.MyResponseData.ResponseStatusXML = "";// GetDummyXml(text, customResponse.Cargo);
                    return;
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(this._MyDeclarationPM.Id))
                    {
                        if (this.MyRequestSheetParam == null)
                        {
                            this.MyRequestSheetParam = new RequestSheetParam();
                        }
                        this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                        this.MyRequestSheetParam.EntityId1 = this._MyDeclarationPM.Id;
                    }

                    LogMessagingUtil.Instance.AppendLine("Analyze Manifest response " + requestParams.DeclarationNumber);

                }
            }


            /////////////////////////////////////////////////////////////////////////////////// FILL DATA FOR TESTING
            //customResponse.GeneralDetails = new TPG_NG_8249_Web10_TPGDeclarationDetailGeneralDetails()
            //{
            //    customOfficeName = "customOfficeName",
            //    customOfficeNumber = 11111,
            //    declerationStatus = 22222,
            //    externalID = 33333,
            //    name = "name",
            //    statusName = "statusName",
            //};

            //List<TPG_NG_8249_Web10_TPGDeclarationDetailFileList> claimList = new List<TPG_NG_8249_Web10_TPGDeclarationDetailFileList>();
            //TPG_NG_8249_Web10_TPGDeclarationDetailFileList claim1 = new TPG_NG_8249_Web10_TPGDeclarationDetailFileList()
            //{
            //    agentExternalID = 11111,
            //    agentName = "agentName",
            //    claimAmount = 2222,
            //    closeDate = DateTime.Now,
            //    createDate = DateTime.Now,
            //    displayFileNumber = "displayFileNumber",
            //    fileNumber = "fileNumber",
            //    Numeral = 1,
            //    status = 55555,
            //    statusName = "statusName",
            //    totalRefundAmount = 66666,
            //};
            //claimList.Add(claim1);
            //claim1 = new TPG_NG_8249_Web10_TPGDeclarationDetailFileList()
            //{
            //    agentExternalID = 55,
            //    agentName = "agentName",
            //    claimAmount = 66,
            //    closeDate = DateTime.Now,
            //    createDate = DateTime.Now,
            //    displayFileNumber = "displayFileNumber",
            //    fileNumber = "fileNumber",
            //    Numeral = 1,
            //    status = 55555,
            //    statusName = "statusName",
            //    totalRefundAmount = 66666,
            //};
            //claimList.Add(claim1);
            //customResponse.Claim = claimList.ToArray();

            //List<TPG_NG_8249_Web10_TPGDeclarationDetailFileList1> deficitList = new List<TPG_NG_8249_Web10_TPGDeclarationDetailFileList1>();
            //TPG_NG_8249_Web10_TPGDeclarationDetailFileList1 deficit1 = new TPG_NG_8249_Web10_TPGDeclarationDetailFileList1()
            //{
            //    agentExternalID = 11111,
            //    agentName = "agentName",
            //    closeDate = DateTime.Now,
            //    displayFileNumber = "displayFileNumber",
            //    estimatedBalance = 222222,
            //    fileNumber = "fileNumber",
            //    Numeral = 3,
            //    productionDate = DateTime.Now,
            //    status = 44,
            //    statusName = "statusName",
            //    totalRefundAmount = 555,
            //};
            //deficitList.Add(deficit1);
            //customResponse.Deficit = deficitList.ToArray();


            //List<TPG_NG_8249_Web10_TPGDeclarationDetailFileList2> guaranteeList = new List<TPG_NG_8249_Web10_TPGDeclarationDetailFileList2>();
            //TPG_NG_8249_Web10_TPGDeclarationDetailFileList2 guarantee1 = new TPG_NG_8249_Web10_TPGDeclarationDetailFileList2()
            //{
            //    agentExternalID = 11111,
            //    agentName = "agentName",
            //    Amount = 22222,
            //    displayFileNumber = "displayFileNumber",
            //    fileNumber = "fileNumber",
            //    fileType = 33,
            //    fileTypeName = "fileTypeName",
            //    guaranteeStatus = 44,
            //    guaranteeStatusName = "guaranteeStatusName",
            //    Numeral = 5,
            //    validity = DateTime.Now,

            //};
            //guaranteeList.Add(guarantee1);
            //customResponse.Guarantee = guaranteeList.ToArray();
            /////////////////////////////////////////////////////////////////////////////////// FILL DATA FOR TESTING
            
            
            
            if (customResponse.GeneralDetails != null)
            {
                MyResponseData.GeneralDetailsData = new DeclarationFilterResponseData.GeneralDetails()
                {
                    customOfficeName = customResponse.GeneralDetails.customOfficeName,
                    customOfficeNumber = customResponse.GeneralDetails.customOfficeNumber,
                    declerationStatus = customResponse.GeneralDetails.declerationStatus,
                    externalID = customResponse.GeneralDetails.externalID,
                    name = customResponse.GeneralDetails.name,
                    statusName = customResponse.GeneralDetails.statusName,
                };
            }

            if (customResponse.Claim != null)
            {
                if (customResponse.Claim.Count() > 0)
                {
                    var myClaimList = new List<DeclarationFilterResponseData.Claim>();
                    foreach (var claim in customResponse.Claim)
                    {
                        DeclarationFilterResponseData.Claim newClaim = new DeclarationFilterResponseData.Claim()
                        {
                            agentExternalID = claim.agentExternalID,
                            agentName = claim.agentName,
                            claimAmount = claim.claimAmount,
                            closeDate = claim.closeDate,
                            createDate = claim.createDate,
                            displayFileNumber = claim.displayFileNumber,
                            fileNumber = claim.fileNumber,
                            Numeral = claim.Numeral,
                            status = claim.status,
                            statusName = claim.statusName,
                            totalRefundAmount = claim.totalRefundAmount,
                        };
                        myClaimList.Add(newClaim);
                    }
                    MyResponseData.ClaimList = new List<DeclarationFilterResponseData.Claim>(myClaimList.OrderBy(rec => rec.Numeral));
                }
            }
            if (customResponse.Deficit != null)
            {
                if (customResponse.Deficit.Count() > 0)
                {
                    var myDeficitList = new List<DeclarationFilterResponseData.Deficit>();
                    foreach (var deficit in customResponse.Deficit)
                    {
                        DeclarationFilterResponseData.Deficit newDeficit = new DeclarationFilterResponseData.Deficit()
                        {
                            agentExternalID = deficit.agentExternalID,
                            agentName = deficit.agentName,
                            closeDate = deficit.closeDate,
                            displayFileNumber = deficit.displayFileNumber,
                            estimatedBalance = deficit.estimatedBalance,
                            fileNumber = deficit.fileNumber,
                            Numeral = deficit.Numeral,
                            productionDate = deficit.productionDate,
                            status = deficit.status,
                            statusName = deficit.statusName,
                            totalRefundAmount = deficit.totalRefundAmount,
                        };
                        myDeficitList.Add(newDeficit);
                    }
                    MyResponseData.DeficitList = new List<DeclarationFilterResponseData.Deficit>(myDeficitList.OrderBy(rec => rec.Numeral));
                }
            }
            if (customResponse.Guarantee != null)
            {
                if (customResponse.Guarantee.Count() > 0)
                {
                    var myGuaranteeList = new List<DeclarationFilterResponseData.Guarantee>();
                    foreach (var guarantee in customResponse.Guarantee)
                    {
                        DeclarationFilterResponseData.Guarantee newGuarantee = new DeclarationFilterResponseData.Guarantee()
                        {
                            agentExternalID = guarantee.agentExternalID,
                            agentName = guarantee.agentName,
                            Amount = guarantee.Amount,
                            displayFileNumber = guarantee.displayFileNumber,
                            fileNumber = guarantee.fileNumber,
                            fileType = guarantee.fileType,
                            fileTypeName = guarantee.fileTypeName,
                            guaranteeStatus = guarantee.guaranteeStatus,
                            guaranteeStatusName = guarantee.guaranteeStatusName,
                            Numeral = guarantee.Numeral,
                            validity = guarantee.validity,

                        };
                        myGuaranteeList.Add(newGuarantee);
                    }
                    MyResponseData.GuarenteeList = new List<DeclarationFilterResponseData.Guarantee>(myGuaranteeList.OrderBy(rec => rec.Numeral));
                }
            }

            //xml = XmlGenericUtil<TPG_NG_8249_Web10_TPGDeclarationDetail>.SerializeObject(customResponse);
            //MyResponseData.ResponseStatusXML = xml;

            if (!string.IsNullOrWhiteSpace(requestParams.DeclarationId))
            {
                MyResponseData.DeclarationID = requestParams.DeclarationId;
            }
            MyResponseData.Succeeded = true;
        }

        private string GetDummyXml(string message, bool toJson)
        {
            var myDummyXml = new GeneralMessage() { Message = message };

            if (toJson)
            {
                var wraper = new { GeneralMessage = myDummyXml };
                return JsonConvert.SerializeObject(wraper);
            }

            var xml = XmlGenericUtil<GeneralMessage>.SerializeObject(myDummyXml);
            return xml;
        }

        private string GetDummyXml(string message, TPG_NG_8249_Web10_TPGDeclarationDetail response)
        {
            var myDummyXml = new GeneralMessage() { Message = message };
            if (response != null)
            {
                myDummyXml.Response = new TPG_NG_8249_Web10_TPGDeclarationDetail();
                myDummyXml.Response = response;
            }
            var xml = XmlGenericUtil<GeneralMessage>.SerializeObject(myDummyXml);
            return xml;
        }


        private string GetTranslationP2L(string partnerID, string tableID, string partnerCode)
        {
            if (partnerID == null || tableID == null || partnerCode == null)
            {
                return ("");
            }

            if (_GTRTRANQueryService == null)
            {
                _GTRTRANQueryService = new GTRTRANQueryService(_AmitalContext);
            }
            return GetTranslationP2LFromCache(partnerID, tableID, partnerCode);
        }

        private string GetTranslationP2LFromCache(string partnerID, string tableID, string partnerCode)
        {
            var key = partnerID + "'," + tableID;
            if (!_MyLocalCache.ContainsKey(key))
            {
                _MyLocalCache[key] = _GTRTRANQueryService.GetMulti(partnerID, tableID) ?? new List<GTRTRANPM>();
            }
            var myList = _MyLocalCache[key] as List<GTRTRANPM>;
            var recordTR = myList.FirstOrDefault(rec => rec.PARTNERID == partnerID && rec.TABLEID == tableID && rec.PARTNERCODE == partnerCode);
            if (recordTR == null)
            {
                return "";
            }
            return recordTR.LOCALCODE;
        }

        public override DeclarationFilterResponseData GetResponse(TPG_NG_8249_Web10_TPGDeclarationDetail customResponse, DeclarationFilterRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        private string GetErrosXmlFromResponseHeaderExeption()
        {
            return _ResponseHeaderExeption.ErrorDescription;
        }

        public class GeneralMessage
        {
            public string Message { get; set; }
            public TPG_NG_8249_Web10_TPGDeclarationDetail Response { get; set; }
        }
    }

}




