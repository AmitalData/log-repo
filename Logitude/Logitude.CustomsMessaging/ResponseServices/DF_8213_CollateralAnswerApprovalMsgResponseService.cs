using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Models;
using Logitude.Customs.Data;
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
using UnifreightIIG.Common.MessageLib.Collateral;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class DF_8213_CollateralAnswerApprovalMsgResponseService:
        ResponseServiceBase<INF_MSG_GenericResponseData, COLT_NG_8213_MSG10042_CollateralAnswerApprovalMsg, GenericRequestParams>
    {
        ICustomContext _CustomContext;
        CustomsCollateralPM _CustomsCollateralPM;

        public override void Update(COLT_NG_8213_MSG10042_CollateralAnswerApprovalMsg customResponse, GenericRequestParams requestParams)
        {
            //Analyze message 8213 - Collateral Answer Approval
            try
            {
                _CustomContext = CustomContext.GetContext(requestParams.Tenant);
                var customsCollateralQueryService = new CustomsCollateralQueryService(this._CustomContext);
                var customsCollateralUpdateService = new CustomsCollateralUpdateService(this._CustomContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), requestParams.Tenant);
                var declarationQueryService = new DeclarationQueryService(this._CustomContext);
                var CustomsCollateralsAnswerQueryService = new CustomsCollateralsAnswerQueryService(this._CustomContext);
                var CustomsCollateralsAnswerUpdateService = new CustomsCollateralsAnswerUpdateService(this._CustomContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), requestParams.Tenant);

                for (int collateralCounter = 0; collateralCounter < customResponse.AnswersApprovalList.LongCount(); collateralCounter++)
                {
                    this._CustomsCollateralPM = new CustomsCollateralPM();
                    var collateralRequestId = customsCollateralQueryService.GetIdByCollateralRequestNumber(customResponse.AnswersApprovalList[collateralCounter].collateralRequestNumber.ToString(), requestParams.Tenant);
                    if (String.IsNullOrWhiteSpace(collateralRequestId))
                    {
                        LogMessagingUtil.Instance.AppendLine("Can not found collateral request" + customResponse.AnswersApprovalList[collateralCounter].collateralRequestNumber);
                        this.MyResponseData = new INF_MSG_GenericResponseData();
                        this.MyResponseData.Succeeded = false;
                        this.MyResponseData.UserMessage = "Can not found collateral request" + requestParams.AppicationId;
                        return;
                    }
                    this._CustomsCollateralPM = customsCollateralQueryService.GetSingle(collateralRequestId, true, false);
                    var AnswerForCollateralRequestApprovalList = customResponse.AnswersApprovalList[collateralCounter].AnswerForCollateralRequestApproval.ToList();
                    var DBAnswers = this._CustomsCollateralPM.CustomsCollateralsAnswers.ToList();

                    if (AnswerForCollateralRequestApprovalList[0].TPGIdentifier == null)
                    {
                        this.MyResponseData = new INF_MSG_GenericResponseData();
                        this.MyResponseData.ApplicationID = _CustomsCollateralPM.Id;
                        this.MyResponseData.Succeeded = false;
                        this.MyResponseData.HasException = true;
                        this.MyResponseData.UserMessage = "Wrong collateral request, TPGIdentifier is missing";

                        this.MyRequestSheetParam = new RequestSheetParam();
                        this.MyRequestSheetParam.EntityId1 = _CustomsCollateralPM.Id;
                        this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.CustomsCollateral");
                        this.MyRequestSheetParam.RequestDescription = "אישור/דחייה מענה לבטוחה " + _CustomsCollateralPM.CollateralRequestNumber;
                        this.MyRequestSheetParam.ObjectTableId2 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                        this.MyRequestSheetParam.EntityId2 = _CustomsCollateralPM.DeclarationId;
                        return;
                    }
                    else
                    {
                        if (DBAnswers != null)
                        {
                            foreach (var answerItem in DBAnswers)
                            {
                                if (!string.IsNullOrWhiteSpace(answerItem.RequestFileTypeCode))
                                {
                                    answerItem.AnswerEntityTypeJoin = answerItem.RequestFileTypeCode;
                                }
                                else
                                {
                                    answerItem.AnswerEntityTypeJoin = answerItem.AnswerEntityTypeCode;
                                }
                            }
                        }

//                            //Join DB occurences with CustomResponse occurences
//                            var customsCollateralsAnswerJoin =
//                            (
//                            from customsCollateralsAnswerPM in DBAnswers
//                            join customMethodCustom in AnswerForCollateralRequestApprovalList

//                             on new
//                             {
//                                 //customsCollateralsAnswerPM.AnswerEntityTypeCode,
//                                 customsCollateralsAnswerPM.AnswerEntityTypeJoin,
//                             }
//                             equals
//                             new
//                             {
//                                 AnswerEntityTypeJoin = customMethodCustom.answerEntityType.ToString(),
//                             }

//                            select
//                            new { customsCollateralsAnswerPM, customMethodCustom }
//                             ).ToList();

//                        foreach (var joinItem in customsCollateralsAnswerJoin)
//                        {
//                            joinItem.customsCollateralsAnswerPM.ChangeSetOp = ChangeSetOperation.Update;
//                            joinItem.customsCollateralsAnswerPM.AnswerForCollateralStatusCode = joinItem.customMethodCustom.answerForCollateralStatus.ToString();
//                            string errorsList = null;
//                            if (joinItem.customMethodCustom.ErrorsForAnsware != null)
//                            {
//                                foreach (var errorItem in joinItem.customMethodCustom.ErrorsForAnsware)
//                                {
//                                    if (errorItem != null)
//                                    {
//                                        if (!string.IsNullOrWhiteSpace(errorsList))
//                                        {
//                                            errorsList = errorsList + @"

//";
//                                        }
//                                        errorsList = errorsList + errorItem.errorCode.ToString();
//                                        if (!string.IsNullOrWhiteSpace(errorItem.errorDescription))
//                                        {
//                                            errorsList = errorsList + " - " + errorItem.errorDescription;
//                                        }
//                                    }
//                                }
//                                if (!String.IsNullOrEmpty(errorsList))
//                                {
//                                    joinItem.customsCollateralsAnswerPM.Errors = errorsList;
//                                }
//                            }
//                            if (joinItem.customMethodCustom.TPGIdentifier != null)
//                            {
//                                joinItem.customsCollateralsAnswerPM.RequestedTapagFile = joinItem.customMethodCustom.TPGIdentifier.fileNumber;
//                                joinItem.customsCollateralsAnswerPM.RequestedTapagNumeral = joinItem.customMethodCustom.TPGIdentifier.numeral.ToString();
//                            }

//                            //this._CustomsCollateralPM.CustomsCollateralsAnswers.Add(joinItem.customsCollateralsAnswerPM);

//                            //Check if the CustomsCollateralsAnswer already exists in the DB
//                            CustomsCollateralsAnswerPM customsCollateralsAnswersToUpdate = this._CustomsCollateralPM.CustomsCollateralsAnswers.FirstOrDefault(rec => rec.CustomsCollateralId == joinItem.customsCollateralsAnswerPM.CustomsCollateralId && rec.Tenant == joinItem.customsCollateralsAnswerPM.Tenant && rec.LineNumber == joinItem.customsCollateralsAnswerPM.LineNumber);
//                            //If the CustomsCollateralsAnswer already exists in the DB - Update the item in the existing DB list
//                            if (customsCollateralsAnswersToUpdate != null)
//                            {
//                                foreach (var currentCustomsCollateralsAnswer in this._CustomsCollateralPM.CustomsCollateralsAnswers.Where(rec => rec.CustomsCollateralId == joinItem.customsCollateralsAnswerPM.CustomsCollateralId && rec.Tenant == joinItem.customsCollateralsAnswerPM.Tenant && rec.LineNumber == joinItem.customsCollateralsAnswerPM.LineNumber))
//                                {
//                                    currentCustomsCollateralsAnswer.ChangeSetOp = joinItem.customsCollateralsAnswerPM.ChangeSetOp;
//                                    currentCustomsCollateralsAnswer.AnswerForCollateralStatusCode = joinItem.customsCollateralsAnswerPM.AnswerForCollateralStatusCode;
//                                    currentCustomsCollateralsAnswer.RequestedTapagFile = joinItem.customsCollateralsAnswerPM.RequestedTapagFile;
//                                    currentCustomsCollateralsAnswer.RequestedTapagNumeral = joinItem.customsCollateralsAnswerPM.RequestedTapagNumeral;
//                                }
//                            }
//                            //If the CustomsCollateralsAnswer does NOT exists in the DB - Add it to the list
//                            else
//                            {
//                                this._CustomsCollateralPM.CustomsCollateralsAnswers.Add(joinItem.customsCollateralsAnswerPM);
//                            }
//                        }

                        foreach (var answerForCollateralItem in AnswerForCollateralRequestApprovalList)
                        {
                            string errorsList = null;
                            if (answerForCollateralItem.ErrorsForAnsware != null)
                            {
                                foreach (var errorItem in answerForCollateralItem.ErrorsForAnsware)
                                {
                                    if (errorItem != null)
                                    {
                                        if (!string.IsNullOrWhiteSpace(errorsList))
                                        {
                                            errorsList = errorsList + @"

";
                                        }
                                        errorsList = errorsList + errorItem.errorCode.ToString();
                                        if (!string.IsNullOrWhiteSpace(errorItem.errorDescription))
                                        {
                                            errorsList = errorsList + " - " + errorItem.errorDescription;
                                        }
                                    }
                                }
                            }

                            var customsCollateralsAnswerPMList = _CustomsCollateralPM.CustomsCollateralsAnswers.Where(d => d.AnswerEntityTypeJoin == answerForCollateralItem.answerEntityType.ToString()).ToList();
                            //If the CustomsCollateralsAnswer does NOT exists in the DB - Add it to the list
                            if (customsCollateralsAnswerPMList.Count == 0)
                            {
                                CustomsCollateralsAnswerPM customsCollateralsAnswerPM = new CustomsCollateralsAnswerPM();
                                customsCollateralsAnswerPM.ChangeSetOp = ChangeSetOperation.Insert;
                                customsCollateralsAnswerPM.CustomsCollateralId = _CustomsCollateralPM.Id;
                                customsCollateralsAnswerPM.Tenant = _CustomsCollateralPM.Tenant;
                                customsCollateralsAnswerPM.AnswerEntityTypeCode = answerForCollateralItem.answerEntityType.ToString();
                                customsCollateralsAnswerPM.AnswerForCollateralStatusCode = answerForCollateralItem.answerForCollateralStatus.ToString();
                                if (!String.IsNullOrEmpty(errorsList))
                                {
                                    customsCollateralsAnswerPM.Errors = errorsList;
                                }
                                if (answerForCollateralItem.TPGIdentifier != null)
                                {
                                    customsCollateralsAnswerPM.RequestedTapagFile = answerForCollateralItem.TPGIdentifier.fileNumber;
                                    customsCollateralsAnswerPM.RequestedTapagNumeral = answerForCollateralItem.TPGIdentifier.numeral.ToString();
                                }
                                this._CustomsCollateralPM.CustomsCollateralsAnswers.Add(customsCollateralsAnswerPM);
                                LogMessagingUtil.Instance.AppendLine("CustomsCollateralsAnswer does NOT exists in the DB - Add new to the list");
                            }
                            //If the CustomsCollateralsAnswer already exists in the DB - Update the item in the existing DB list
                            else if (customsCollateralsAnswerPMList.Count > 0)
                            {
                                var currentCustomsCollateralsAnswer = new CustomsCollateralsAnswerPM();
                                if (customsCollateralsAnswerPMList.Count == 1)
                                {
                                    currentCustomsCollateralsAnswer = customsCollateralsAnswerPMList.FirstOrDefault();
                                }
                                else // If there is more than one CustomsCollateralsAnswer with the same ‘allocatedAmount’ update CustomsCollateralsAnswer  with no 'answerForCollateralStatus'
                                {
                                    currentCustomsCollateralsAnswer = customsCollateralsAnswerPMList.FirstOrDefault(d => d.AnswerForCollateralStatusCode == null);
                                }
                                if (currentCustomsCollateralsAnswer != null)
                                {
                                    LogMessagingUtil.Instance.AppendLine("CustomsCollateralsAnswer already exists in the DB - Update the item");
                                    currentCustomsCollateralsAnswer.ChangeSetOp = ChangeSetOperation.Update;
                                    currentCustomsCollateralsAnswer.AnswerForCollateralStatusCode = answerForCollateralItem.answerForCollateralStatus.ToString();
                                    if (!String.IsNullOrEmpty(errorsList))
                                    {
                                        currentCustomsCollateralsAnswer.Errors = errorsList;
                                    }
                                    if (answerForCollateralItem.TPGIdentifier != null)
                                    {
                                        currentCustomsCollateralsAnswer.RequestedTapagFile = answerForCollateralItem.TPGIdentifier.fileNumber;
                                        currentCustomsCollateralsAnswer.RequestedTapagNumeral = answerForCollateralItem.TPGIdentifier.numeral.ToString();
                                    }
                                }
                                else
                                {
                                    LogMessagingUtil.Instance.AppendLine("Can NOT update CustomsCollateralsAnswer - all items has an answer already");
                                }
                            }
                        }
                        //}
                    }

                    this._CustomsCollateralPM.ChangeSetOp = ChangeSetOperation.Update;
                    this._CustomsCollateralPM.CollateralRequestStatusCode = customResponse.AnswersApprovalList[collateralCounter].collateralRequestStatus.ToString();
                    this._CustomsCollateralPM.RequestedCollateralTypeCode = customResponse.AnswersApprovalList[collateralCounter].collateralRequestType.ToString();
                    this._CustomsCollateralPM.Remarks = customResponse.AnswersApprovalList[collateralCounter].remarks;
                    var myInsertEventContextTagModel = new EventContextTagModel(); // Create Notification "Approval/Denial of Reply to Cllateral Request"
                    myInsertEventContextTagModel.CallProccessID = EventContextTagModel.ProccessEnum.DF_8213_CollateralAnswerApprovalMsgResponseServiceReply;
                    this._CustomsCollateralPM.CurrentContextTag = myInsertEventContextTagModel;
                    // moran 4.7.16 - Task 20790 -->
                    switch (this._CustomsCollateralPM.CollateralRequestStatusCode)
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
                    if (this._CustomsCollateralPM.CollateralRequestStatusCode != null)
                    {
                        CollateralRequestStatusQueryService collateralRequestStatusQueryService = new CollateralRequestStatusQueryService(this._CustomsCollateralPM.Tenant);
                        CollateralRequestStatusPM collateralRequestStatusPM = collateralRequestStatusQueryService.GetSingle(this._CustomsCollateralPM.CollateralRequestStatusCode, false, true);
                        this._CustomsCollateralPM.CollateralRequestStatusName = collateralRequestStatusPM.LocalName;
                    }
                    // moran 4.7.16 - Task 20790 <--
                    customsCollateralUpdateService.Update(this._CustomsCollateralPM, true);
                }

                this.MyResponseData = new INF_MSG_GenericResponseData();
                this.MyResponseData.ApplicationID = _CustomsCollateralPM.Id;
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.HasException = false;

                this.MyRequestSheetParam = new RequestSheetParam()
                {
                    EntityId1 = _CustomsCollateralPM.Id,
                    ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.CustomsCollateral"),
                    RequestDescription = "אישור/דחייה מענה לבטוחה " + _CustomsCollateralPM.CollateralRequestNumber,
                    ObjectTableId2 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration"),
                    EntityId2 = _CustomsCollateralPM.DeclarationId,
                };
            }

            catch (System.Exception ee)
            {
                throw;
            }
        }

        public override INF_MSG_GenericResponseData GetResponse(COLT_NG_8213_MSG10042_CollateralAnswerApprovalMsg customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }


    }
}
