
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.BankAccountToRefundUpdateReplayServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class TPG_NG_2018_BankAccountToRefundUpdateReplayRequestService : RequestServiceBase
        <TPG_NG_2018_MSG4_BankAccountToRefundUpdateReplay, BankAccountToRefundRequestParams>
    {
        public override TPG_NG_2018_MSG4_BankAccountToRefundUpdateReplay GetRequest(BankAccountToRefundRequestParams requestParams)
        {
            //Build request 2018 - Message Request For Refund
            var myTPG_NG_2018_MSG4_BankAccountToRefundUpdateReplay = new TPG_NG_2018_MSG4_BankAccountToRefundUpdateReplay();
            ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);

            myTPG_NG_2018_MSG4_BankAccountToRefundUpdateReplay.RequestContentHeader = new RequestContentHeader() { Convertor = "1", RecieverID = new int[] { 1 } };
            myTPG_NG_2018_MSG4_BankAccountToRefundUpdateReplay.FileIdentifier = new TPG_NG_2018_MSG4_BankAccountToRefundUpdateReplayFileIdentifier();
            int fileType;
            int.TryParse(requestParams.FileType, out fileType);
            myTPG_NG_2018_MSG4_BankAccountToRefundUpdateReplay.FileIdentifier.FileType = fileType;

            CustomsSettingPM customsSetting = CustomsSettingQueryService.GetSettingByTenant(requestParams.Tenant);
            int customsAgentId;
            int.TryParse(customsSetting.CustomsAgentId, out customsAgentId);
            myTPG_NG_2018_MSG4_BankAccountToRefundUpdateReplay.FileIdentifier.agentID = customsAgentId;
            myTPG_NG_2018_MSG4_BankAccountToRefundUpdateReplay.FileIdentifier.TPGIdentifier = new TPGIdentifier();
            myTPG_NG_2018_MSG4_BankAccountToRefundUpdateReplay.FileIdentifier.TPGIdentifier.fileNumber = requestParams.FileNumber;
            myTPG_NG_2018_MSG4_BankAccountToRefundUpdateReplay.FileIdentifier.TPGIdentifier.numeral = requestParams.Numeral;

            myTPG_NG_2018_MSG4_BankAccountToRefundUpdateReplay.ClaimRefundMethod = new TPG_NG_2018_MSG4_BankAccountToRefundUpdateReplayClaimRefundMethod();
            int typeOfFactorCode;
            int.TryParse(requestParams.IdentifierType, out typeOfFactorCode);
            myTPG_NG_2018_MSG4_BankAccountToRefundUpdateReplay.ClaimRefundMethod.typeOfFactorCode = typeOfFactorCode;
            int typeOfFactorCodeID;
            int.TryParse(requestParams.IdentifierCode, out typeOfFactorCodeID);
            myTPG_NG_2018_MSG4_BankAccountToRefundUpdateReplay.ClaimRefundMethod.typeOfFactorCodeID = typeOfFactorCodeID;

            myTPG_NG_2018_MSG4_BankAccountToRefundUpdateReplay.ClaimRefundMethod.accountCountry = requestParams.CountryCode;//accountCountry;
            if (requestParams.CountryCode == "IL")
            {         
                myTPG_NG_2018_MSG4_BankAccountToRefundUpdateReplay.ClaimRefundMethod.AccountDetails = new TPG_NG_2018_MSG4_BankAccountToRefundUpdateReplayClaimRefundMethodAccountDetails();
                int codeBank;
                int.TryParse(requestParams.BankCode, out codeBank);
                myTPG_NG_2018_MSG4_BankAccountToRefundUpdateReplay.ClaimRefundMethod.AccountDetails.codeBank = codeBank;
                if (!string.IsNullOrWhiteSpace(requestParams.AccountBranch))
                {
                    int branch = 0;
                    if (requestParams.AccountBranch.Contains(","))
                    {
                        string[] branchArray = requestParams.AccountBranch.Split(',');
                        requestParams.AccountBranch = branchArray[0];
                    }
                    int.TryParse(requestParams.AccountBranch, out branch);
                    myTPG_NG_2018_MSG4_BankAccountToRefundUpdateReplay.ClaimRefundMethod.AccountDetails.accountBranch = branch;
                }
                myTPG_NG_2018_MSG4_BankAccountToRefundUpdateReplay.ClaimRefundMethod.AccountDetails.accountNumber = requestParams.AccountNumber;
            }
            else
            {
                myTPG_NG_2018_MSG4_BankAccountToRefundUpdateReplay.ClaimRefundMethod.ForeignAccountDetails = new TPG_NG_2018_MSG4_BankAccountToRefundUpdateReplayClaimRefundMethodForeignAccountDetails();
                int codeBank;
                int.TryParse(requestParams.BankCode, out codeBank);
                myTPG_NG_2018_MSG4_BankAccountToRefundUpdateReplay.ClaimRefundMethod.ForeignAccountDetails.foreignBank = codeBank;
                int bankBranch;
                int.TryParse(requestParams.AccountBranch, out bankBranch);
                myTPG_NG_2018_MSG4_BankAccountToRefundUpdateReplay.ClaimRefundMethod.ForeignAccountDetails.foreignBranch = bankBranch;
                myTPG_NG_2018_MSG4_BankAccountToRefundUpdateReplay.ClaimRefundMethod.ForeignAccountDetails.accountNumber = requestParams.AccountNumber;
                myTPG_NG_2018_MSG4_BankAccountToRefundUpdateReplay.ClaimRefundMethod.ForeignAccountDetails.accountCurrency = requestParams.AccountCurrency;
            }

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = "בקשה להחזר פיקדון  " + requestParams.FileNumber;
            this.MyRequestSheetParam.ObjectTableId2 = ObjectTableRepository.GetObjectTableByName("Customs.Deposit");
            if (requestParams.LoggingObjectTableId == "Customs.Declaration" && !string.IsNullOrEmpty(requestParams.LoggingEntityId))
            {
                this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                this.MyRequestSheetParam.EntityId1 = requestParams.LoggingEntityId;
            }

            return myTPG_NG_2018_MSG4_BankAccountToRefundUpdateReplay;

        }
    }
}
