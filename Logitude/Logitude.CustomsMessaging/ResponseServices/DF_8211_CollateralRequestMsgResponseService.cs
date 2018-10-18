using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Models;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.AgentPaymentRequestServiceReference;
using UnifreightIIG.Common.MessageLib.Collateral;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class DF_8211_CollateralRequestMsgResponseService:
        ResponseServiceBase<INF_MSG_GenericResponseData, COLT_NG_8211_MSG10040_CollateralRequestMsg, GenericRequestParams>
    {
        ICustomContext _CustomContext;
        CustomsCollateralPM _CustomsCollateralPM;
        string _DeclarationId;

        public override void Update(COLT_NG_8211_MSG10040_CollateralRequestMsg customResponse, GenericRequestParams requestParams)
        {
            //Analyze message 8211 - Collateral Request
            try
            {
                this._CustomContext = CustomContext.GetContext(requestParams.Tenant);
                var customsCollateralQueryService = new CustomsCollateralQueryService(this._CustomContext);
                var customsCollateralUpdateService = new CustomsCollateralUpdateService(this._CustomContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), requestParams.Tenant);
                var declarationConstraintQueryService = new DeclarationConstraintQueryService(this._CustomContext);
                var declarationConstraintUpdateService = new DeclarationConstraintUpdateService(this._CustomContext, new Dictionary<string, IContext>(), requestParams.Tenant);
                EventContextTagModel myInsertEventContextTagModel = new EventContextTagModel();
                int changedPropertiesCount = 0;

                foreach (var collateralRequestItem in customResponse.CollateralRequestDetails)
                {
                    this._CustomsCollateralPM = new CustomsCollateralPM();
                    var collateralRequestId = customsCollateralQueryService.GetIdByCollateralRequestNumber(collateralRequestItem.collateralRequestNumber.ToString(), requestParams.Tenant);
                    if (!String.IsNullOrWhiteSpace(collateralRequestId))
                    {
                        _CustomsCollateralPM = customsCollateralQueryService.GetSingle(collateralRequestId, true, false);
                        changedPropertiesCount = _CustomsCollateralPM.ChangedProperties.Count();
                        /*if (_CustomsCollateralPM.CustomsCollateralsAnswers.Any())
                        {
                            this.MyRequestSheetParam = new RequestSheetParam()
                            {
                                EntityId1 = _CustomsCollateralPM.Id,
                                ObjectTableId1 = ObjectTabelRepository.GetObjectTableByName("Customs.CustomsCollateral"),
                                RequestDescription = "דרישה לבטוחה - " + _CustomsCollateralPM.CollateralRequestNumber,
                            };

                            this.MyResponseData = new INF_MSG_GenericResponseData();
                            this.MyResponseData.ApplicationID = this._CustomsCollateralPM.Id;
                            this.MyResponseData.Succeeded = false;
                            this.MyResponseData.HasException = true;
                            this.MyResponseData.ExceptionMessage = "קיימת תשובה לבטוחה" + _CustomsCollateralPM.CollateralRequestNumber;

                            return; // Do not update if an answer to the request was sent.
                        }*/
                    }
                    if (String.IsNullOrWhiteSpace(collateralRequestId) || (!String.IsNullOrWhiteSpace(collateralRequestId) && (_CustomsCollateralPM.CollateralRequestStatusCode == "4" || _CustomsCollateralPM.CollateralRequestStatusCode == "1")))
                    {
                        if (String.IsNullOrWhiteSpace(collateralRequestId))
                        {
                            _CustomsCollateralPM.ChangeSetOp = ChangeSetOperation.Insert;
                            myInsertEventContextTagModel.CallProccessID = EventContextTagModel.ProccessEnum.DF_8211_CollateralRequestMsgInsert;
                            _CustomsCollateralPM.CurrentContextTag = myInsertEventContextTagModel;
                            _CustomsCollateralPM.CreateDateTime = DateTime.Today;
                            _CustomsCollateralPM.Tenant = requestParams.Tenant;
                        }
                        else
                        {
                            DeleteCollateralCondition();
                            _CustomsCollateralPM.ChangeSetOp = ChangeSetOperation.Update;
                            myInsertEventContextTagModel.CallProccessID = EventContextTagModel.ProccessEnum.DF_8211_CollateralRequestMsgUpdate;
                            _CustomsCollateralPM.CurrentContextTag = myInsertEventContextTagModel;
                        }

                        GatCustomsCollateralsDetails(collateralRequestItem, requestParams.Tenant);
                        if (changedPropertiesCount == _CustomsCollateralPM.ChangedProperties.Count() && _CustomsCollateralPM.DeletedCustomsCollateralsConditions.Count() == collateralRequestItem.CollateralConditioning.Count())
                        {
                            myInsertEventContextTagModel.CallProccessID = EventContextTagModel.ProccessEnum.None;
                        }

                        if (collateralRequestItem.PaymentOrderReply != null) // Create PaymentOrderReply
                        {
                            LogMessagingUtil.Instance.AppendLine("ConstraintApprovalDecision: Create PaymentOrderReply");
                            var headerXml = XmlGenericUtil<UnifreightIIG.Common.MessageLib.Collateral.ResponseContentHeader>
                                .SerializeObject(customResponse.ResponseContentHeader);
                            var header = XmlGenericUtil<UnifreightIIG.Common.AgentPaymentRequestServiceReference.ResponseContentHeader>.DeSerializeObject(headerXml);
                            var requestXml = XmlGenericUtil<UnifreightIIG.Common.MessageLib.Collateral.PaymentOrderReply>
                                .SerializeObject(collateralRequestItem.PaymentOrderReply);
                            var paymentOrderReply = XmlGenericUtil<UnifreightIIG.Common.AgentPaymentRequestServiceReference.PaymentOrderReply>.DeSerializeObject(requestXml);

                            TSH_MSG2_PaymentOrderReply myTSH_MSG2_PaymentOrderReply = new TSH_MSG2_PaymentOrderReply();
                            myTSH_MSG2_PaymentOrderReply.ResponseContentHeader = header;
                            myTSH_MSG2_PaymentOrderReply.PaymentOrderReply = paymentOrderReply;
                            var xml = XmlGenericUtil<UnifreightIIG.Common.AgentPaymentRequestServiceReference.TSH_MSG2_PaymentOrderReply>
                                .SerializeObject(myTSH_MSG2_PaymentOrderReply);

                            var ser = XmlGenericUtil<UnifreightIIG.Common.AgentPaymentRequestServiceReference.TSH_MSG2_PaymentOrderReply>.DeSerializeObject(xml);
                            var myTSH_MSG2_3050_PaymentOrderReplyResponseService = new TSH_MSG2_3050_PaymentOrderReplyResponseService();
                            NewPaymentRequestParams myrequestParams = new NewPaymentRequestParams();
                            myrequestParams.Tenant = requestParams.Tenant;
                            myrequestParams.LoggingUserId = requestParams.LoggingUserId;
                            myrequestParams.RequestParamsVersion = 1;
                            myTSH_MSG2_3050_PaymentOrderReplyResponseService.Update(ser, myrequestParams);

                            var paymentOrderQueryService = new PaymentOrderQueryService(this._CustomContext);
                            var paymentOrderId = paymentOrderQueryService.GetIdByPaymentNumber(ser.PaymentOrderReply.PaymentDetails.paymentID.ToString(), requestParams.Tenant);
                            if (!string.IsNullOrWhiteSpace(paymentOrderId))
                            {
                                var customsCollateralsAnswerQueryService = new CustomsCollateralsAnswerQueryService(this._CustomContext);
                                CustomsCollateralsAnswerPM newCustomsCollateralsAnswerPM = _CustomsCollateralPM.CustomsCollateralsAnswers.Where(rec => rec.PaymentOrderId == paymentOrderId).FirstOrDefault();
                                if (newCustomsCollateralsAnswerPM == null || string.IsNullOrWhiteSpace(newCustomsCollateralsAnswerPM.CustomsCollateralId))
                                {
                                    //Create CustomsCollateralsAnswer
                                    var myCustomsCollateralsAnswerUpdateService = new CustomsCollateralsAnswerUpdateService(this._CustomContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), requestParams.Tenant);
                                    newCustomsCollateralsAnswerPM = new CustomsCollateralsAnswerPM()
                                    {       
                                        IsClosed = true,
                                        NewFileRequest = true,
                                        RequestFileTypeCode = "3", //()Deposit request
                                        RequestFileAmount = collateralRequestItem.PaymentOrderReply.paymentOrderTotalSumToPay,
                                        PaymentOrderId = paymentOrderId,
                                        Tenant = requestParams.Tenant,
                                        ChangeSetOp = ChangeSetOperation.Insert,
                                    };

                                    if (collateralRequestItem.PaymentOrderReply.ConnectedEntity.entityType == 11118) //Deposit Request
                                    {
                                        newCustomsCollateralsAnswerPM.RequestedTapagFile = collateralRequestItem.PaymentOrderReply.ConnectedEntity.entityIdKey1;
                                        newCustomsCollateralsAnswerPM.RequestedTapagNumeral = collateralRequestItem.PaymentOrderReply.ConnectedEntity.entityIdKey2;
                                    }

                                    this._CustomsCollateralPM.CustomsCollateralsAnswers.Add(newCustomsCollateralsAnswerPM);
                                }
                            }
                        }

                        customsCollateralUpdateService.Update(this._CustomsCollateralPM, true);

                        if (this._CustomsCollateralPM.CustomsEntityTypeCode == "12240") // Update Contraint
                        {
                            string customsCollateralId = null;
                            List<CustomsCollateralPM> customsCollateralPMList = customsCollateralQueryService.GetCollateralsListByDeclarationConstraint("12240", this._CustomsCollateralPM.EntityIdKey1, this._CustomsCollateralPM.EntityIdKey2, this._CustomsCollateralPM.Tenant);
                            if (customsCollateralPMList != null && customsCollateralPMList.Count > 0)
                            {
                                if (customsCollateralPMList.Count == 1)
                                {
                                    customsCollateralId = customsCollateralPMList.FirstOrDefault().Id;
                                }
                                else if (customsCollateralPMList.Count > 1)
                                {
                                    foreach (var customsCollateralPMItem in customsCollateralPMList)
                                    {
                                        if(!customsCollateralPMItem.IsClosed)
                                        {
                                            customsCollateralId = customsCollateralPMItem.Id;
                                            break;
                                        }
                                    }
                                    if(customsCollateralId == null)
                                    {
                                        customsCollateralId = customsCollateralPMList.FirstOrDefault().Id;
                                    }
                                }
                            }
                            else
                            {
                                customsCollateralId = this._CustomsCollateralPM.Id;
                            }

                            DeclarationConstraintPM declarationConstraintPM = declarationConstraintQueryService.GetSingle(this._DeclarationId, this._CustomsCollateralPM.EntityIdKey2, true, false);
                            if (declarationConstraintPM != null)
                            {
                                declarationConstraintPM.ChangeSetOp = ChangeSetOperation.Update;
                                declarationConstraintPM.CustomsCollateralId = customsCollateralId;
                                declarationConstraintUpdateService.Update(declarationConstraintPM, true);
                            }
                        } 
                    }
                }


                this.MyResponseData = new INF_MSG_GenericResponseData();
                this.MyResponseData.ApplicationID = this._CustomsCollateralPM.Id;
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.HasException = false;

                this.MyRequestSheetParam = new RequestSheetParam()
                {
                    EntityId1 = _CustomsCollateralPM.Id,
                    ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.CustomsCollateral"),
                    EntityId2 = this._CustomsCollateralPM.DeclarationId,
                    ObjectTableId2 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration"),
                    RequestDescription = "דרישה לבטוחה " + this._CustomsCollateralPM.CollateralRequestNumber + "-" + _CustomsCollateralPM.CollateralRequestStatusName, 
                };
            }
            catch (System.Exception ee)
            {
                throw;
            }
        }

        private void DeleteCollateralCondition() // Delete old Collateral conditions
        {
            foreach (var collateralsConditionItem in _CustomsCollateralPM.CustomsCollateralsConditions)
            {
                collateralsConditionItem.ChangeSetOp = ChangeSetOperation.Delete;
                _CustomsCollateralPM.DeletedCustomsCollateralsConditions.Add(collateralsConditionItem);
            }
        }

        private void GatCustomsCollateralsDetails(CollateralRequestDetails collateralRequestDetails, int tenant)
        {
            this._CustomsCollateralPM.Tenant = tenant;
            this._CustomsCollateralPM.CollateralRequestNumber = collateralRequestDetails.collateralRequestNumber.ToString();
            this._CustomsCollateralPM.RequestValidityDate = collateralRequestDetails.requestValidityDate;
            this._CustomsCollateralPM.CollateralValidityDate = collateralRequestDetails.collateralValidityDate;
            this._CustomsCollateralPM.CollateralRequestStatusCode = collateralRequestDetails.collateralRequestStatus.ToString();
            switch (collateralRequestDetails.collateralRequestStatus.ToString())
            {
                case "1":
                    _CustomsCollateralPM.IsClosed = false;
                    break;
                case "2":
                case "3":
                case "5":
                case "7":
                    _CustomsCollateralPM.IsClosed = true;
                    break;
            }
            if (collateralRequestDetails.collateralRequestStatus != null)
            {
                CollateralRequestStatusQueryService collateralRequestStatusQueryService = new CollateralRequestStatusQueryService(tenant);
                CollateralRequestStatusPM collateralRequestStatusPM = collateralRequestStatusQueryService.GetSingle(collateralRequestDetails.collateralRequestStatus.ToString(), false, true);
                if (collateralRequestStatusPM != null)
                {
                    this._CustomsCollateralPM.CollateralRequestStatusName = collateralRequestStatusPM.LocalName;
                }
            }
            this._CustomsCollateralPM.RequestedCollateralTypeCode = collateralRequestDetails.requestedCollateralType.ToString();
            this._CustomsCollateralPM.OrganizationUnitTypeCode = collateralRequestDetails.Worker.organizationUnitType.ToString();
            this._CustomsCollateralPM.CustomsHouseTypeCode = collateralRequestDetails.Worker.customsHouse.ToString();
            this._CustomsCollateralPM.WorkerName = collateralRequestDetails.Worker.workerName;
            this._CustomsCollateralPM.Remarks = collateralRequestDetails.remarks;
            this._CustomsCollateralPM.IncludingThirdPartyGuarantee = collateralRequestDetails.IncludingThirdPartyGuarantee;

            //Get Connected Entity
            if (collateralRequestDetails.RelatedEntity != null)
            {
                var declarationQueryService = new DeclarationQueryService(this._CustomContext);
                this._DeclarationId = declarationQueryService.GetIdByDeclarationNumber(collateralRequestDetails.RelatedEntity.FirstOrDefault().entityIdKey1, tenant);
                if (String.IsNullOrWhiteSpace(this._DeclarationId))
                {
                    LogMessagingUtil.Instance.AppendLine("CustomsCollateral.declarationNumber = " + collateralRequestDetails.RelatedEntity.FirstOrDefault().entityIdKey1 + " But declarationRep.GetIdByDeclarationNumber return null ");
                }
                else
                {
                    var myDeclarationPM = declarationQueryService.GetSingle(this._DeclarationId, false, false);
                    this._CustomsCollateralPM.FileNo = myDeclarationPM.CustomFileNo;
                    this._CustomsCollateralPM.DeclarationId = this._DeclarationId;
                    this._CustomsCollateralPM.CustomerId = myDeclarationPM.CustomerId; //Yuval Chalup 04.12.2016 TASK-24908
                } 
            
                if (!string.IsNullOrWhiteSpace(collateralRequestDetails.RelatedEntity.FirstOrDefault().entityIdExternalReferenceID))
                {
                    this._CustomsCollateralPM.FileNo = collateralRequestDetails.RelatedEntity.FirstOrDefault().entityIdExternalReferenceID;
                }
                this._CustomsCollateralPM.CustomsEntityTypeCode = collateralRequestDetails.RelatedEntity.FirstOrDefault().entityType.ToString();
                this._CustomsCollateralPM.EntityIdKey1 = collateralRequestDetails.RelatedEntity.FirstOrDefault().entityIdKey1;
                this._CustomsCollateralPM.EntityIdKey2 = collateralRequestDetails.RelatedEntity.FirstOrDefault().entityIdKey2;
                this._CustomsCollateralPM.EntityIdKey3 = collateralRequestDetails.RelatedEntity.FirstOrDefault().entityIdKey3;
            }

            foreach (var collateralsConditionItem in collateralRequestDetails.CollateralConditioning)
            {
                CustomsCollateralsConditionPM myCustomsCollateralsCondition = new CustomsCollateralsConditionPM();
                myCustomsCollateralsCondition.ChangeSetOp = ChangeSetOperation.Insert;
                myCustomsCollateralsCondition.Tenant = tenant;
                myCustomsCollateralsCondition.ConditionCode = collateralsConditionItem.conditionCode.ToString();
                myCustomsCollateralsCondition.RequestedAmount = collateralsConditionItem.requestedAmount;

                this._CustomsCollateralPM.CustomsCollateralsConditions.Add(myCustomsCollateralsCondition);
            }
        }

        public override INF_MSG_GenericResponseData GetResponse(COLT_NG_8211_MSG10040_CollateralRequestMsg customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }
    }
}
