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
using UnifreightIIG.Common.MessageLib.Deposit;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class DEPO_NG_5110_DepositRequestFulfillednfoMsgResponseService : ResponseServiceBase
        <INF_MSG_GenericResponseData,
        DEPO_NG_5110_MSG5_DepositRequestFulfillednfoMsg,
        GenericRequestParams>
    {
        public DepositPM _MyDepositPM;
        public override void Update(DEPO_NG_5110_MSG5_DepositRequestFulfillednfoMsg customResponse, GenericRequestParams requestParams)
        {
            //Analyzing Message 5110 - Notice of New Deposit File(DCA)
            ICustomContext myContext = CustomContext.GetContext(requestParams.Tenant);
            var depositQueryService = new DepositQueryService(myContext);
            var depositUpdateService = new DepositUpdateService(myContext, new Dictionary<string, IContext>(), requestParams.Tenant);
            var tapagQueryService = new TapagQueryService(myContext);
            var tapagUpdateService = new TapagUpdateService(myContext, new Dictionary<string, IContext>(), requestParams.Tenant);
            var tapagConnectionTableUpdateService = new TapagConnectionTableUpdateService(myContext, new Dictionary<string, IContext>(), requestParams.Tenant);
            var tapagConnectionTableQueryService = new TapagConnectionTableQueryService(myContext);

            string declarationId = null;
            bool isTapagConnected = false;

            TapagConnectionTablePM depositTapagConnectionTablePM = tapagConnectionTableQueryService.GetTapagConnectionByFileAndNumeral(customResponse.TapagIdentifier.fileNumber, customResponse.TapagIdentifier.numeral, requestParams.Tenant);
            if (depositTapagConnectionTablePM != null)
            {
                isTapagConnected = true;
            }
            foreach (var depositRequestItem in customResponse.DepositRequests)
            {
                //Check if Tapag file is exist
                //TapagConnectionTablePM tapagConnectionTablePM = tapagConnectionTableQueryService.GetTapagConnectionByFileAndNumeral(depositRequestItem.fileNumber, depositRequestItem.numeral, requestParams.Tenant);
                string requestFileNumber = string.Concat(depositRequestItem.fileNumber, "-", depositRequestItem.numeral);
                string tapagId = tapagConnectionTableQueryService.GetTapagIdByRequestFileNumber(requestFileNumber, requestParams.Tenant);
                if (string.IsNullOrWhiteSpace(tapagId))
                {
                    this.MyResponseData = new INF_MSG_GenericResponseData();
                    this.MyResponseData.Succeeded = true;
                    this.MyResponseData.HasException = true;
                    this.MyResponseData.UserMessage = "Can not find tapag file (not exist in connection table) " + "fileNumber=" + depositRequestItem.fileNumber + " Numeral=" + depositRequestItem.numeral;
                    LogMessagingUtil.Instance.AppendLine("Can not find tapag file (not exist in connection table) " + "fileNumber=" + depositRequestItem.fileNumber + " Numeral=" + depositRequestItem.numeral);
                    return;
                }
                TapagConnectionTablePM tapagConnectionTablePM = tapagConnectionTableQueryService.GetTapagConnectionByTapagId(tapagId, requestParams.Tenant).FirstOrDefault();
                if (tapagConnectionTablePM != null && !String.IsNullOrWhiteSpace(tapagConnectionTablePM.TapagId))
                {
                    var depositId = depositQueryService.GetDepositIdByTapagNumber(tapagConnectionTablePM.TapagId, requestParams.Tenant);
                    this._MyDepositPM = depositQueryService.GetSingle(depositId, true, false);
                    if (this._MyDepositPM == null)
                    {
                        this.MyResponseData = new INF_MSG_GenericResponseData();
                        this.MyResponseData.Succeeded = true;
                        this.MyResponseData.HasException = true;
                        this.MyResponseData.UserMessage = "Can not find tapag file (not exist in connection table) " + "fileNumber=" + depositRequestItem.fileNumber + " Numeral=" + depositRequestItem.numeral;
                        LogMessagingUtil.Instance.AppendLine("Can not find tapag file (not exist in connection table) " + "fileNumber=" + depositRequestItem.fileNumber + " Numeral=" + depositRequestItem.numeral);
                        return;
                    }
                    // Update Fields in Tapag + TapagConnection Tables
                    if (isTapagConnected)
                    {
                        var saveDeclaration = tapagConnectionTablePM.DeclarationId;
                        if (tapagConnectionTablePM.TapagId != depositTapagConnectionTablePM.TapagId && tapagConnectionTablePM.DeclarationId != depositTapagConnectionTablePM.DeclarationId)
                        {
                            //Delete tapag connection record
                            var saveTapag = tapagConnectionTablePM.TapagId;
                            tapagConnectionTablePM.ChangeSetOp = ChangeSetOperation.Delete;
                            tapagConnectionTableUpdateService.Update(tapagConnectionTablePM, true);

                            //Delete old tapag record (only tapag)
                            var tapagPM = tapagQueryService.GetSingle(saveTapag,false,false);
                            tapagPM.ChangeSetOp = ChangeSetOperation.Delete;
                            tapagUpdateService.Update(tapagPM,false);

                            //Conect deposit to other tapag record
                            var connectTapag = new TapagConnectionTablePM();
                            connectTapag.ChangeSetOp = ChangeSetOperation.Insert;
                            connectTapag.Tenant = requestParams.Tenant;
                            connectTapag.TapagId = depositTapagConnectionTablePM.TapagId;
                            connectTapag.DeclarationId = saveDeclaration;
                            connectTapag.CustomsTapagFile = customResponse.TapagIdentifier.fileNumber;
                            connectTapag.CustomsNumeral = customResponse.TapagIdentifier.numeral;
                            tapagConnectionTableUpdateService.Update(connectTapag, true);
                        }

                        this._MyDepositPM.TapagID = depositTapagConnectionTablePM.TapagId;
                        this._MyDepositPM.Remarks = "צירוף בקשת פיקדון " + depositRequestItem.fileNumber + "לתיק פיקדון " + customResponse.TapagIdentifier.fileNumber; 
                        declarationId = saveDeclaration;
                    }
                    else
                    {
                        tapagConnectionTablePM.ChangeSetOp = ChangeSetOperation.Update;
                        tapagConnectionTablePM.CustomsTapagFile = customResponse.TapagIdentifier.fileNumber;
                        tapagConnectionTablePM.CustomsNumeral = customResponse.TapagIdentifier.numeral;
                        tapagConnectionTableUpdateService.Update(tapagConnectionTablePM, true);

                        this._MyDepositPM.Remarks = "נפתח תיק פיקדון חדש שמספרו " + customResponse.TapagIdentifier.fileNumber + "לבקשת פיקדון " + depositRequestItem.fileNumber;
                        declarationId = tapagConnectionTablePM.DeclarationId;
                    }

                    //Update Fields in Tapag record
                    this._MyDepositPM.ChangeSetOp = ChangeSetOperation.Update;
                    this._MyDepositPM.CustomsBranchCode = customResponse.DepositRequestFulfillednfoMsg.customUnit.ToString();
                    this._MyDepositPM.ValidityDate = customResponse.DepositRequestFulfillednfoMsg.validityDay;
                    this._MyDepositPM.FollowDate = customResponse.DepositRequestFulfillednfoMsg.validityDay;
                    this._MyDepositPM.DepositAmount = customResponse.DepositRequestFulfillednfoMsg.depositAmount;
                    //this._MyDepositPM.DepositEssenceTypeCode = customResponse.DepositRequestFulfillednfoMsg.depositeEssenceType.ToString();
                    this._MyDepositPM.CustomsTapagFile = customResponse.TapagIdentifier.fileNumber;
                    this._MyDepositPM.CustomsNumeral = customResponse.TapagIdentifier.numeral;
                    if (declarationId != null)
                    {
                        this._MyDepositPM.ConnectedDeclarationId = declarationId;
                    }
                    this._MyDepositPM.TapagTypeCode = "2";//14564
                    
                    EventContextTagModel myInsertEventContextTagModel = new EventContextTagModel();
                    myInsertEventContextTagModel.CallProccessID = EventContextTagModel.ProccessEnum.DEPO_NG_5110_DepositRequestFulfillednfoMsg;
                    myInsertEventContextTagModel.EventCode = "DPN";
                    myInsertEventContextTagModel.EventRemarks = "Custom Despoit Notice";
                    this._MyDepositPM.CurrentContextTag = myInsertEventContextTagModel;

                    depositUpdateService.Update(this._MyDepositPM, true);
                }
                else
                {
                    this.MyResponseData = new INF_MSG_GenericResponseData();
                    this.MyResponseData.Succeeded = true;
                    this.MyResponseData.HasException = true;
                    this.MyResponseData.UserMessage = "Can not find tapag file (not exist in connection table) " + "fileNumber=" + depositRequestItem.fileNumber + " Numeral=" + depositRequestItem.numeral;
                    LogMessagingUtil.Instance.AppendLine("Can not find tapag file (not exist in connection table) " + "fileNumber=" + depositRequestItem.fileNumber + " Numeral=" + depositRequestItem.numeral);
                    return;
                }
            }


            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Deposit");
            this.MyRequestSheetParam.EntityId1 = this._MyDepositPM.Id;
            this.MyRequestSheetParam.RequestDescription = "ידוע על פתיחת פיקדון " + this._MyDepositPM.CustomsTapagFile;
            if (declarationId != null)
            {
                this.MyRequestSheetParam.ObjectTableId2 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                this.MyRequestSheetParam.EntityId2 = declarationId;
            }

            this.MyResponseData = new INF_MSG_GenericResponseData()
            {
                ApplicationID = this._MyDepositPM.Id,
                Succeeded = true,
                UserMessage = null,
            };
        }

        public override INF_MSG_GenericResponseData GetResponse(DEPO_NG_5110_MSG5_DepositRequestFulfillednfoMsg customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }
    }
}
