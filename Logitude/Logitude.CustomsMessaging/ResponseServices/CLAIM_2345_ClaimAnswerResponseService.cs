using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.LogitudeClient.ClaimException;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools.Helpers;
using Newtonsoft.Json;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnifreightIIG.Common.ClaimAnswerServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class CLAIM_2345_ClaimAnswerResponseService : ResponseServiceBase<ClaimAnswerResponseData, CLAIM_MSG22_ClaimAnswer, CLAIM_2340_ClaimRequestRequestParams>
    {
        ClaimPM _MyClaimPM;

        public override void Update(CLAIM_MSG22_ClaimAnswer customResponse, CLAIM_2340_ClaimRequestRequestParams requestParams)
        {
            var context = CustomContext.GetContext(requestParams.Tenant);
            var myClaimQueryService = new ClaimQueryService(context);
            var myClaimUpdateService = new ClaimUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            string userMessage = "ניתוח מסר תביעה";

            this.MyResponseData = new ClaimAnswerResponseData();
            this.MyResponseData.Succeeded = true;
            this.MyResponseData.UserMessage = userMessage;

            if (string.IsNullOrWhiteSpace(requestParams.AppicationId))
            {
                LogMessagingUtil.Instance.AppendLine("Can not find claim: " + requestParams.AppicationId + " ExternalDeclarationNumber: " + customResponse.ClaimEntitySystemAnswer.FirstOrDefault().claimEntityID);
                this.MyResponseData.ResponseClaimXML = XmlGenericUtil<CLAIM_MSG22_ClaimAnswer>.SerializeObject(customResponse);
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = "Can not find claim: " + requestParams.AppicationId + " ExternalDeclarationNumber: " + customResponse.ClaimEntitySystemAnswer.FirstOrDefault().claimEntityID;
                return;
            }

            this._MyClaimPM = myClaimQueryService.GetSingle(requestParams.AppicationId, true, false);
            if (this._MyClaimPM == null)
            {
                LogMessagingUtil.Instance.AppendLine("Can not find claim" + requestParams.AppicationId);
                this.MyResponseData.ResponseClaimXML = XmlGenericUtil<CLAIM_MSG22_ClaimAnswer>.SerializeObject(customResponse);
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = "Can not find claim" + requestParams.AppicationId;
                return;
            }

            this._MyClaimPM.ChangeSetOp = ChangeSetOperation.Update;
            foreach (var claimsRelatedEntitiy in customResponse.ClaimEntitySystemAnswer)
            {
                ClaimsRelatedEntityPM claimsRelatedEntityPM = GetClaimsRelatedEntitiy(claimsRelatedEntitiy.claimEntity.ToString(), claimsRelatedEntitiy.claimEntityID);
                if (claimsRelatedEntityPM != null)
                {
                    claimsRelatedEntityPM.ChangeSetOp = ChangeSetOperation.Update;
                    claimsRelatedEntityPM.ContinuousMessagesTypeCode = claimsRelatedEntitiy.continuousMessagesTypecode.ToString();
                    if (!string.IsNullOrWhiteSpace(claimsRelatedEntityPM.ContinuousMessagesTypeCode))
                    {
                        string messageTypeName = GetMessageTypeName(claimsRelatedEntitiy.continuousMessagesTypecode);
                        userMessage = userMessage + "\n" + "סטטוס הישות " + claimsRelatedEntityPM.ClaimEntityNumber + "-" + messageTypeName;
                    }
                    if (claimsRelatedEntitiy.ClaimReferentialData != null)
                    {
                        claimsRelatedEntityPM.ClaimRequestNumber = claimsRelatedEntitiy.ClaimReferentialData.claimRequestNumber.ToString();
                        claimsRelatedEntityPM.CustomsExceptions = BuildCustomsExceptions(claimsRelatedEntitiy.ClaimReferentialData.note, null);
                        if (requestParams.IsAngularClient)
                        {
                            var doc = new XmlDocument();
                            doc.LoadXml(claimsRelatedEntityPM.CustomsExceptions);
                            claimsRelatedEntityPM.CustomsExceptions =JsonConvert.SerializeXmlNode(doc);
                            
                        }
                        
                        if (claimsRelatedEntitiy.ClaimReferentialData.customOfficeIDNumber != null && claimsRelatedEntitiy.ClaimReferentialData.customOfficeIDNumber > 0)
                        {
                            claimsRelatedEntityPM.CustomsBranchCode = claimsRelatedEntitiy.ClaimReferentialData.customOfficeIDNumber.ToString();
                        }
                        if (claimsRelatedEntitiy.ClaimReferentialData.TPGIdentifier != null)
                        {
                            claimsRelatedEntityPM.TapagNumber = claimsRelatedEntitiy.ClaimReferentialData.TPGIdentifier.fileNumber;
                            claimsRelatedEntityPM.Numeral = claimsRelatedEntitiy.ClaimReferentialData.TPGIdentifier.numeral;
                        }
                    }
                    
                    if (claimsRelatedEntitiy.Exceptions != null)
                    {
                        claimsRelatedEntityPM.CustomsExceptions = BuildCustomsExceptions(null, claimsRelatedEntitiy.Exceptions);
                        if (requestParams.IsAngularClient)
                        {
                            var doc = new XmlDocument();
                            doc.LoadXml(claimsRelatedEntityPM.CustomsExceptions);
                            claimsRelatedEntityPM.CustomsExceptions = JsonConvert.SerializeXmlNode(doc);

                        }
                        foreach (var exceptionItem in claimsRelatedEntitiy.Exceptions)
                        {
                            userMessage = userMessage + "\n" + exceptionItem.ExeptionDescription;
                        }
                    }
                }
            }
            myClaimUpdateService.Update(this._MyClaimPM, true);

            var xml = XmlGenericUtil<CLAIM_MSG22_ClaimAnswerClaimEntitySystemAnswer[]>.SerializeObject(customResponse.ClaimEntitySystemAnswer);
            this.MyResponseData.ResponseClaimXML = xml;
            this.MyResponseData.Succeeded = true;
            this.MyResponseData.UserMessage = userMessage;
            this.MyResponseData.HasException = false;

        }

        public override ClaimAnswerResponseData GetResponse(CLAIM_MSG22_ClaimAnswer customResponse, CLAIM_2340_ClaimRequestRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        private ClaimsRelatedEntityPM GetClaimsRelatedEntitiy(string claimEntity, string claimEntityID)
        {

            if (_MyClaimPM.ClaimsRelatedEntities == null || _MyClaimPM.ClaimsRelatedEntities.Count == 0)
            {
                return null;
            }

            List<ClaimsRelatedEntityPM> claimsRelatedEntityList = (from a in _MyClaimPM.ClaimsRelatedEntities
                                                                   where (a.ClaimEntityTypeCode == claimEntity && a.ClaimEntityNumber == claimEntityID)
                                                                   select a).ToList();

            if (claimsRelatedEntityList.Count > 0)
            {
                return claimsRelatedEntityList.FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        private string BuildCustomsExceptions(string notes, UnifreightIIG.Common.ClaimAnswerServiceReference.Exception[] exceptions)
        {
            ClaimCustomsExceptions ClaimCustomsExceptions = new ClaimCustomsExceptions();

            if (!string.IsNullOrWhiteSpace(notes))
            {
                ClaimCustomsExceptions.CustomsNotes = notes;
                if (ClaimCustomsExceptions.CustomsExceptions == null)
                {
                    ClaimCustomsExceptions.CustomsExceptions = new List<CustomsException>();
                }
                CustomsException noteException = new CustomsException();
                noteException.ExceptionType = "Note";
                noteException.ExceptionDescription = notes;
                ClaimCustomsExceptions.CustomsExceptions.Add(noteException);
            }

            if (exceptions != null)
            {
                if (ClaimCustomsExceptions.CustomsExceptions == null)
                {
                    ClaimCustomsExceptions.CustomsExceptions = new List<CustomsException>();
                } 
                foreach (var exceptionItem in exceptions)
                {
                    CustomsException customsException = new CustomsException();
                    customsException.ExceptionType = exceptionItem.ExeptionType.ToString();
                    customsException.ExceptionDescription = exceptionItem.ExeptionDescription;
                    ClaimCustomsExceptions.CustomsExceptions.Add(customsException);
                }
            }

            var myClaimCustomsExceptionsXml = XmlGenericUtil<ClaimCustomsExceptions>.SerializeObject(ClaimCustomsExceptions);
            return myClaimCustomsExceptionsXml;
        }

        private string GetMessageTypeName(int messageType)
        {
            string messageTypeName = null;
            if (messageType >= 0)
            {
                ContinuousMessagesTypeCodeQueryService continuousMessagesTypeCodeQueryService = new ContinuousMessagesTypeCodeQueryService(this._MyClaimPM.Tenant);
                ContinuousMessagesTypeCodePM continuousMessagesTypeCodePM = continuousMessagesTypeCodeQueryService.GetSingle(messageType.ToString(), false, true);
                if (continuousMessagesTypeCodePM != null)
                {
                    messageTypeName = continuousMessagesTypeCodePM.LocalName;
                }
            }
            return messageTypeName;
        }
    }
}
