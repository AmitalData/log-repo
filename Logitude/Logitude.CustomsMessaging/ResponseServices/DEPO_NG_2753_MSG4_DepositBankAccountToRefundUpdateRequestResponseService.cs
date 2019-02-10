using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.NotificationBL;
using Logitude.Customs.BL.Models;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib;
using UnifreightIIG.Common.MessageLib.Deposit;

namespace Logitude.CustomsMessaging.ResponseServices
{

    public class DEPO_NG_2753_MSG4_DepositBankAccountToRefundUpdateRequestResponseService : ResponseServiceBase<
        INF_MSG_GenericResponseData,
        DEPO_NG_2753_MSG4_DepositBankAccountToRefundUpdateRequest,
        GenericRequestParams>
    {
        public int _MyTenant { get; set; }
        public ICustomContext _MyContext;
        public TapagPM _MyTapagPM;
        public DepositPM _MyDepositPM;
        public DeclarationPM _MyDeclarationPM;
        private ICommonDataContext _CommonContext;

        public override void Update(DEPO_NG_2753_MSG4_DepositBankAccountToRefundUpdateRequest customResponse, GenericRequestParams requestParams)
        {
            //Analyze message 2753- Deposit Request (DCA)
            this._MyTenant = requestParams.Tenant;
            this._MyContext = CustomContext.GetContext(this._MyTenant);
            _CommonContext = CommonDataContext.GetContext(this._MyTenant);
            var depositQueryService = new DepositQueryService(this._MyContext);
            var depositUpdateService = new DepositUpdateService(this._MyContext, new Dictionary<string, IContext>(), requestParams.Tenant);
            var tapagQueryService = new TapagQueryService(this._MyContext);
            var tapagUpdateService = new TapagUpdateService(this._MyContext, new Dictionary<string, IContext>(), requestParams.Tenant);
            var tapagConnectionTableQueryService = new TapagConnectionTableQueryService(this._MyContext);

            string notificationDescription = "";
            string assigneToNotificationTypeCode = "I";

            //Check if Tapag file is already exist

            string requestFileNumber = string.Concat(customResponse.TapagIdentifier.fileNumber, "-", customResponse.TapagIdentifier.numeral);
            //string tapagId = tapagConnectionTableQueryService.GetTapagIdByRequestFileNumber(requestFileNumber, this._MyTenant);
            string tapagId = tapagConnectionTableQueryService.GetTapagIdByFileAndNumeral(customResponse.TapagIdentifier.fileNumber, customResponse.TapagIdentifier.numeral, this._MyTenant);

            this.MyRequestSheetParam = new RequestSheetParam();

            if (String.IsNullOrWhiteSpace(tapagId))
            {
                this.MyResponseData = new INF_MSG_GenericResponseData()
                {
                    Succeeded = true,
                    HasException = true,
                    UserMessage = "Can not find tapag file (not exist in connection table) " + "fileNumber=" + customResponse.TapagIdentifier.fileNumber + "Numeral=" + customResponse.TapagIdentifier.numeral,
                };
                LogMessagingUtil.Instance.AppendLine("Can not find tapag file (not exist in connection table) " + "fileNumber=" + customResponse.TapagIdentifier.fileNumber + "Numeral=" + customResponse.TapagIdentifier.numeral);
                return;
            }

            if (!String.IsNullOrWhiteSpace(tapagId))
            {
                notificationDescription = "בקשה להשלמת פרטי החזר פקדון תיק תפ\"ג: " + requestFileNumber.Replace("-"," מספר רץ ");
                var depositId = depositQueryService.GetDepositIdByTapagNumber(tapagId, this._MyTenant);
                if (depositId != null) this._MyDepositPM = depositQueryService.GetSingle(depositId, true, false);

                if (this._MyDepositPM == null)
                {
                    this.MyResponseData = new INF_MSG_GenericResponseData()
                    {
                        Succeeded = true,
                        HasException = true,
                        UserMessage = "Can not find deposit file (not exist in connection table) " + "fileNumber=" + customResponse.TapagIdentifier.fileNumber + "Numeral=" + customResponse.TapagIdentifier.numeral,
                    };
                    LogMessagingUtil.Instance.AppendLine("Can not find tapag file (not exist in connection table) " + "fileNumber=" + customResponse.TapagIdentifier.fileNumber + "Numeral=" + customResponse.TapagIdentifier.numeral);
                    return;
                }

                this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Deposit");
                this.MyRequestSheetParam.EntityId1 = depositId;
                this.MyRequestSheetParam.ObjectTableId2 = ObjectTableRepository.GetObjectTableByName("Customs.Tapag");
                this.MyRequestSheetParam.EntityId2 = tapagId;
                this.MyRequestSheetParam.RequestDescription = notificationDescription;

                string myDeclarationId = null;
                if (_MyDepositPM.EntityTypeCode == "1055")
                {
                    var myDeclarationQueryService = new DeclarationQueryService(this._MyTenant);
                    myDeclarationId = myDeclarationQueryService.GetIdByDeclarationNumber(_MyDepositPM.EntityNumber, this._MyTenant);
                    if (!string.IsNullOrWhiteSpace(myDeclarationId))
                    {
                        _MyDeclarationPM = myDeclarationQueryService.GetSingle(myDeclarationId, false, false);
                    }
                }
            }
            this.MyRequestSheetParam.RequestDescription = notificationDescription;
            if (_MyDeclarationPM != null)
            {
                this.MyRequestSheetParam.ObjectTableId2 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                this.MyRequestSheetParam.EntityId2 = _MyDeclarationPM.Id;
                this.MyRequestSheetParam.CustomFileNo = _MyDeclarationPM.CustomFileNo;
                if (this._MyDeclarationPM.IsConvertedDeclaration)
                {
                    MyRequestSheetParam.RequestDescription = string.Concat(MyRequestSheetParam.RequestDescription, "\n", this._MyDeclarationPM.UserNotes);
                }
                notificationDescription = notificationDescription + " (" + _MyDeclarationPM.CustomFileNo + ")";
            }

            LogMessagingUtil.Instance.AppendLine("Start Sending Notification... ");
            DoUpdateNotification("2753A", this._MyTenant, null, notificationDescription, assigneToNotificationTypeCode, _MyDeclarationPM);

            this.MyResponseData = new INF_MSG_GenericResponseData()
            {
                ApplicationID = this._MyDepositPM.Id,
                Succeeded = true,
                HasException = false,
                UserMessage = notificationDescription,
            };

        }


        private void DoUpdateNotification(string notificationDefinitionCode, int tenant, string responseToMessage, string description, string typeCode, DeclarationPM connectedDeclarationPM)
        {
            LogMessagingUtil.Instance.AppendLine("New Message To Agent Request Notification");

            ICustomContext dbContext = CustomContext.GetContext(tenant);
            var notificationUpdateService = new NotificationUpdateService(dbContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
            var notificationQueryService = new NotificationQueryService(dbContext);

            var newNotificationPM = new NotificationPM();
            newNotificationPM.ChangeSetOp = ChangeSetOperation.Insert;
            newNotificationPM.Tenant = tenant;
            newNotificationPM.NotificationDefinitionCode = notificationDefinitionCode;
            newNotificationPM.CreateDate = DateTime.Now;
            newNotificationPM.Description = description;
            newNotificationPM.Reference2Number = responseToMessage;
            newNotificationPM.DueDate = DateTime.Now;
            newNotificationPM.AssigneToNotificationTypeCode = typeCode;
            newNotificationPM.EntityId = _MyDepositPM.Id;
            newNotificationPM.ObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Deposit");
            if(!String.IsNullOrWhiteSpace(_MyDepositPM.TapagID)) newNotificationPM.Reference2Number = _MyDepositPM.TapagID;
            string customerId = null;
            string referentUserId = null;
            if (connectedDeclarationPM != null)
            {
                newNotificationPM.EntityId = connectedDeclarationPM.Id;
                newNotificationPM.ObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                newNotificationPM.Reference1Number = connectedDeclarationPM.CustomFileNo;
                newNotificationPM.DepartmentId = connectedDeclarationPM.DepartmentId;
                newNotificationPM.DeclarationOfficeCode = connectedDeclarationPM.DeclarationOfficeCode;
                customerId = connectedDeclarationPM.CustomerId;
                referentUserId = connectedDeclarationPM.ReferentUserId;
            }
            if (connectedDeclarationPM != null && !string.IsNullOrWhiteSpace(connectedDeclarationPM.CustomerId)) newNotificationPM.CustomerId = connectedDeclarationPM.CustomerId; // moran 20.6.16 - Task 20789

            newNotificationPM.AssigneToId =
               NotificationBase.
               CalcAssigneToId(newNotificationPM.Tenant, customerId, referentUserId, notificationDefinitionCode, "");

            if (connectedDeclarationPM != null && !string.IsNullOrWhiteSpace(newNotificationPM.DeclarationOfficeCode))
            {
                newNotificationPM.IsHandledByCustomOffice = true;
            }

            notificationUpdateService.Update(newNotificationPM, true);
        }

        public override INF_MSG_GenericResponseData GetResponse(DEPO_NG_2753_MSG4_DepositBankAccountToRefundUpdateRequest customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

    }
}
