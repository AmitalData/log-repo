                                                                //Yuval Chalup 23.06.2015 TASK-13278
using System;
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
using UnifreightIIG.Common.ConstraintApprovalRequestServiceReference;
using Logitude.AmitalMessaging.Customs.CustomFile;
using Logitude.AmitalMessaging.Utils;
using Unifreight.BL.EntityPMs.UGenerated;
using Unifreight.BL.EntityQueryServices;
using System.Collections;
using Unifreight.Data.AmitalModel;
using UnifreightIIG.Common.CreditQueryServiceReference;


namespace Logitude.CustomsMessaging.ResponseServices
{

    public class TSH_NG_8290_Web06_CreditInfoResponseService : ResponseServiceBase<
         CreditQueryResponseData, TSH_NG_8290_Web06_CreditInfo, CreditQueryRequestParams>
    {
        public UnifreightIIG.Common.CommonIIGInterface.IResponseHeaderOrFault _ResponseHeaderExeption;
        public bool _IsSubmitDeclarationResponse { get; set; }
        private Dictionary<string, IList> _MyLocalCache = new Dictionary<string, IList>();
        private AmitalContext _AmitalContext;

        public override void Update(TSH_NG_8290_Web06_CreditInfo customResponse, CreditQueryRequestParams requestParams)
        {
            var responseName = requestParams.ResponseName;
            var context = CustomContext.GetContext(requestParams.Tenant);
            DeclarationErrorPointerService mydDclarationErrorPointerService = new DeclarationErrorPointerService();

            this.MyResponseData = new CreditQueryResponseData();

            if (!string.IsNullOrWhiteSpace(requestParams.AgentID) || !string.IsNullOrWhiteSpace(requestParams.ExtertnalID))
            {
                if (this.MyRequestSheetParam == null)
                {
                    this.MyRequestSheetParam = new RequestSheetParam();
                }
            }

            //Checking foe Exceptions
            if (customResponse.ResponseContentHeader.Exception != null || _ResponseHeaderExeption != null)
            {
                if (customResponse.ResponseContentHeader.Exception != null)
                {
                    this.MyResponseData.UserMessage = customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription;
                }
                else
                {
                    this.MyResponseData.UserMessage = _ResponseHeaderExeption.ErrorDescription;
                }
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.HasException = true;
                this.MyResponseData.ResponseStatusXML = GetDummyXml(this.MyResponseData.UserMessage); 
                return;
            }

            if (customResponse.ResponseContentHeader != null)
            {
                {
                    if (customResponse.ResponseContentHeader.Exception != null)
                    {
                        if (String.IsNullOrWhiteSpace(customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription))
                        {
                            var errMess = "No Credit details in the Response for " + requestParams.AgentID + requestParams.ExtertnalID;
                            LogMessagingUtil.Instance.AppendLine(errMess);
                            this.MyResponseData.UserMessage = errMess;
                        }
                        else
                        {
                            var errMess = "No Credit details in the Response for " + customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription + ". For " + requestParams.AgentID + requestParams.ExtertnalID;
                            LogMessagingUtil.Instance.AppendLine(errMess);
                            this.MyResponseData.UserMessage = errMess;
                        }
                        this.MyResponseData.Succeeded = true;
                        this.MyResponseData.HasException = true;
                        this.MyResponseData.ResponseStatusXML = GetDummyXml(this.MyResponseData.UserMessage);
                        return;
                    }
                }
            }

            if (customResponse.BalanceDetails == null)
            {
                if (customResponse.ResponseContentHeader.Exception == null)
                {
                    if (customResponse.ResponseContentHeader.Exception != null)
                    {
                        if (String.IsNullOrWhiteSpace(customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription))
                        {
                            var errMess = "No Balance details in the Response for " + requestParams.AgentID + requestParams.ExtertnalID;
                            LogMessagingUtil.Instance.AppendLine(errMess);
                            this.MyResponseData.UserMessage = errMess;
                        }
                        else
                        {
                            var errMess = "No Balance details in the Response" + customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription + ". For " + requestParams.AgentID + requestParams.ExtertnalID;
                            LogMessagingUtil.Instance.AppendLine(errMess);
                            this.MyResponseData.UserMessage = errMess;
                        }
                        this.MyResponseData.Succeeded = true;
                        this.MyResponseData.HasException = true;
                    }
                }
                this.MyResponseData.ResponseStatusXML = GetDummyXml(this.MyResponseData.UserMessage);
                return;
            }

            if (customResponse.BalanceDetails != null)
            {
                {
                    MyResponseData.BalanceDetailsList = new List<CreditQueryResponseData.BalanceDetailsResult>();
                    {
                        CreditQueryResponseData.BalanceDetailsResult newBalanceDetailsResult = new CreditQueryResponseData.BalanceDetailsResult()
                        {
                            FreeBalance = String.Format("{0:N2}",customResponse.BalanceDetails.FreeBalance),
                            TemporaryCeiling = String.Format("{0:N2}",customResponse.BalanceDetails.TemporaryCeiling),
                            UsedBalance = String.Format("{0:N2}", customResponse.BalanceDetails.UsedBalance),
                        };
                        MyResponseData.BalanceDetailsList.Add(newBalanceDetailsResult);
                    }
                }
            }

            if (customResponse.BankAccountsList != null)
            {
                MyResponseData.BankAccountsList = new List<CreditQueryResponseData.BankAccountsResult>();

                foreach (var bankAccount in customResponse.BankAccountsList)
                {
                    CreditQueryResponseData.BankAccountsResult newBankAccountsResult = new CreditQueryResponseData.BankAccountsResult()
                    {
                        BankAccount = bankAccount.BankAccount,
                        UsedBalanceForBankAccount = String.Format("{0:N2}", bankAccount.UsedBalanceForBankAccount),
                    };
                    MyResponseData.BankAccountsList.Add(newBankAccountsResult);
                }
            }

         
            LogMessagingUtil.Instance.AppendLine("Analyze Credit Query response for " + requestParams.AgentID + requestParams.ExtertnalID);

            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;
            this.MyResponseData.UserMessage = "שליחת שאילתא לתקרת אשראי בוצעה בהצלחה";
        }

        public override CreditQueryResponseData GetResponse(TSH_NG_8290_Web06_CreditInfo customResponse, CreditQueryRequestParams requestParams)
        {
            return this.MyResponseData;
        }

         private string GetErrosXmlFromResponseHeaderExeption()
        {
            return _ResponseHeaderExeption.ErrorDescription;
        }

         private string GetDummyXml(string message)
         {
             var myDummyXml = new GeneralMessage() { Message = message };
             var xml = XmlGenericUtil<GeneralMessage>.SerializeObject(myDummyXml);
             return xml;
         }
         
        public class GeneralMessage
        {
            public string Message { get; set; }
            //public TSH_NG_8290_Web06_CreditInfo Response { get; set; }
        }
    }
    
}




