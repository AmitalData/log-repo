using Logitude.AmitalMessaging.Infrastructure;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Messaging.L2U.CustomFile;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
//using System.Exception;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnifreightIIG.Common.MasavPaymentsToAgentServiceReference;
using Logitude.Server.Tools.Helpers;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class TSH_8356_MasavPaymentsToAgentResponseService : ResponseServiceBase<MasavPaymentsToAgentResponseData, TSH_NG_8356_MSG33_MasavPaymentsToAgent, MasavPaymentsToAgentRequestParams>
    {
        List<AgentMasavPaymentResult> _MyAgentMasavPaymentResultList;
        decimal _TotalForBankAccount;
        ICustomContext _MyDbContext;

        public override void Update(TSH_NG_8356_MSG33_MasavPaymentsToAgent customResponse, MasavPaymentsToAgentRequestParams requestParams)
        {
            this._MyDbContext = CustomContext.GetContext(requestParams.Tenant);
            DeclarationQueryService declarationQueryService = new DeclarationQueryService(this._MyDbContext);

            //Analyze message 8356- Masav Payments To Agent
            if (customResponse.ResponseContentHeader.Exception != null)
            {
                this.MyResponseData = new MasavPaymentsToAgentResponseData();
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription;
                return;
            }

            if (customResponse.AgentMasavPayment == null)
            {
                this.MyResponseData = new MasavPaymentsToAgentResponseData();
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = "אין נתונים להצגה לתאריך המבוקש";
                return;
            }

            _MyAgentMasavPaymentResultList = new List<AgentMasavPaymentResult>();

            foreach (var agentMasavPaymentItem in customResponse.AgentMasavPayment)
            {
                AgentMasavPaymentResult myAgentMasavPaymentResult = new AgentMasavPaymentResult();
                myAgentMasavPaymentResult.PaymentProcess = agentMasavPaymentItem.paymentProcess.ToString();
                myAgentMasavPaymentResult.PaymentProcessName = GetPaymentProcessName(agentMasavPaymentItem.paymentProcess.ToString());
                //myAgentMasavPaymentResult.PaymentID = agentMasavPaymentItem.paymentID.ToString();
                if (agentMasavPaymentItem.amount != agentMasavPaymentItem.PaymentMethodAmount)
                {
                    myAgentMasavPaymentResult.PaymentID = agentMasavPaymentItem.paymentID.ToString() + "-1";
                }
                else
                {
                    myAgentMasavPaymentResult.PaymentID = agentMasavPaymentItem.paymentID.ToString();
                }
                myAgentMasavPaymentResult.Amount = String.Format("{0:N2}", agentMasavPaymentItem.amount);
                myAgentMasavPaymentResult.PaymentType = agentMasavPaymentItem.PaymentType.ToString();
                myAgentMasavPaymentResult.PaymentTypeName = GetPaymentTypeName(agentMasavPaymentItem.PaymentType.ToString());
                myAgentMasavPaymentResult.ExternalID = agentMasavPaymentItem.ExternalID.ToString();
                myAgentMasavPaymentResult.ExternalName = GetExternalName(agentMasavPaymentItem.ExternalID.ToString(), requestParams.Tenant);
                myAgentMasavPaymentResult.CustomsUnit = agentMasavPaymentItem.CustomsUnit.ToString();
                myAgentMasavPaymentResult.PaymentMethodAmount = String.Format("{0:N2}", agentMasavPaymentItem.PaymentMethodAmount);
                if (agentMasavPaymentItem.agentAccountPosession == 1)
                {
                    myAgentMasavPaymentResult.AgentAccountPosessionV = "Visible";
                    myAgentMasavPaymentResult.AgentAccountPosessionX = "Collapsed";
                }
                else
                {
                    myAgentMasavPaymentResult.AgentAccountPosessionX = "Visible";
                    myAgentMasavPaymentResult.AgentAccountPosessionV = "Collapsed";
                }
                myAgentMasavPaymentResult.Bank = GetBankName(agentMasavPaymentItem.bank.ToString());
                myAgentMasavPaymentResult.Branch = agentMasavPaymentItem.branch.ToString();
                myAgentMasavPaymentResult.AccountNumber = agentMasavPaymentItem.accountNumber;
                myAgentMasavPaymentResult.BankCode = agentMasavPaymentItem.bank.ToString(); // moran 3.11.15 - Task 16978

                //<--- Yuval Chalup 29.02.2016 TASK-19978
                TSH_NG_8356_MSG33_MasavPaymentsToAgentTotalForBankAccount myTSH_NG_8356_MSG33_MasavPaymentsToAgentTotalForBankAccount = customResponse.TotalForBankAccount.Where(rec => (rec.bank.ToString() == myAgentMasavPaymentResult.BankCode && rec.branch.ToString() == myAgentMasavPaymentResult.Branch && rec.accountNumber == myAgentMasavPaymentResult.AccountNumber)).FirstOrDefault();

                myAgentMasavPaymentResult.AgentMasavPaymentResultHeader = myAgentMasavPaymentResult.Bank + ", סניף: " +
                    myAgentMasavPaymentResult.Branch + ", חשבון: " + myAgentMasavPaymentResult.AccountNumber;
                if (myTSH_NG_8356_MSG33_MasavPaymentsToAgentTotalForBankAccount != null)
                {
                    myAgentMasavPaymentResult.AgentMasavPaymentResultHeader = myAgentMasavPaymentResult.AgentMasavPaymentResultHeader + @"  -  סה""כ סכום ששולם במס""ב: " + myTSH_NG_8356_MSG33_MasavPaymentsToAgentTotalForBankAccount.TotalAmountForBankAccount.ToString("N2");

                }
                //Yuval Chalup 29.02.2016 TASK-19978 --->

                
                if (agentMasavPaymentItem.RelatedEntity != null)
                {
                    if (agentMasavPaymentItem.RelatedEntity.Count() == 1)
                    {
                        myAgentMasavPaymentResult.EntityIdExternalReferenceID = agentMasavPaymentItem.RelatedEntity.FirstOrDefault().entityIdExternalReferenceID;
                    }
                    else
                    {
                        myAgentMasavPaymentResult.EntityIdExternalReferenceID = "רשימה";
                    }
                    myAgentMasavPaymentResult.RelatedEntityList = new List<RelatedEntityResult>();
                    foreach(var relatedEntityItem in agentMasavPaymentItem.RelatedEntity)
                    {
                        RelatedEntityResult relatedEntity = new RelatedEntityResult();
                        relatedEntity.EntityIdExternalReferenceID = relatedEntityItem.entityIdExternalReferenceID;
                        if (string.IsNullOrWhiteSpace(relatedEntityItem.entityIdExternalReferenceID))
                        {
                            if (relatedEntityItem.entityType == 1055 && !string.IsNullOrWhiteSpace(relatedEntityItem.entityIdKey1))
                            {
                                string myCustomFileNo = declarationQueryService.GetCustomFileNoByDeclarationNumber(relatedEntityItem.entityIdKey1, requestParams.Tenant);
                                
                                if (string.IsNullOrEmpty(myCustomFileNo))
                                {
                                    relatedEntity.EntityIdExternalReferenceID = myCustomFileNo;
                                    if (agentMasavPaymentItem.RelatedEntity.Count() == 1)
                                    {
                                        myAgentMasavPaymentResult.EntityIdExternalReferenceID = myCustomFileNo;
                                    }
                                }
                            }
                        }
                        relatedEntity.EntityIdKey1 = relatedEntityItem.entityIdKey1;
                        relatedEntity.EntityIdKey2 = relatedEntityItem.entityIdKey2;
                        relatedEntity.EntityIdKey3 = relatedEntityItem.entityIdKey3;
                        relatedEntity.EntityType = relatedEntityItem.entityType.ToString();
                        relatedEntity.EntityTypeName = GetEntityTypeName(relatedEntityItem.entityType.ToString());
                        relatedEntity.EntityPath = relatedEntityItem.entityPath;
                        myAgentMasavPaymentResult.RelatedEntityList.Add(relatedEntity);
                    }
                }

                _MyAgentMasavPaymentResultList.Add(myAgentMasavPaymentResult);
            }

            _MyAgentMasavPaymentResultList = _MyAgentMasavPaymentResultList.OrderBy(c => c.Bank).ThenBy(n => n.Branch).ThenBy(a => a.AccountNumber).ToList();
            this.MyResponseData = new MasavPaymentsToAgentResponseData();
            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;
            this.MyResponseData.UserMessage = "ניתוח דוח קופה לתאריך " + requestParams.PaymentDate.Value.Date.ToString("dd/MM/yyyy");
            this.MyResponseData.AgentMasavPaymentResultList = _MyAgentMasavPaymentResultList;

            // moran 1.11.15 - Task 16978 -->

            //MemoryStream memorystream = new MemoryStream(requestParamsData);
            //XmlSerializer serializer = new XmlSerializer(typeof(CustomFileCreditRequestParams));
            //CustomFileCreditRequestParams requestParamsCredit = (CustomFileCreditRequestParams)serializer.Deserialize(memorystream);
            GenericResponse responseData = new GenericResponse();

            var setting = CustomsSettingQueryService.GetSettingByTenant(requestParams.Tenant);
            if (setting.IsConnectedToUniFreight)
            {
                //try
                //{
                    var myCustomsAGTService = new CustomsAGTService(requestParams, customResponse.MasavSentDate.masavSentDate, this.MyResponseData);
                    responseData = myCustomsAGTService.CustomsAGT();

                    var genericResponseObj = responseData.GenericResponseObj.FirstOrDefault();
                    if (genericResponseObj == null)
                    {
                        throw new System.Exception("GenericResponse.GenericResponseObj is null ");
                    }
                    if (!String.IsNullOrWhiteSpace(genericResponseObj.Status))
                    {
                        int sts;
                        int.TryParse(genericResponseObj.Status, out sts);
                        if (sts < 0)
                        {
                            string mess = "Failed To Update AGT Report in Unifreight";
                            if (!String.IsNullOrWhiteSpace(genericResponseObj.ErrorDescription))
                            {
                                mess = mess + Environment.NewLine + genericResponseObj.ErrorDescription;
                            }
                            if (!String.IsNullOrWhiteSpace(genericResponseObj.Message))
                            {
                                mess = mess + Environment.NewLine + genericResponseObj.Message;
                            }
                            LogMessagingUtil.Instance.AppendLine("CustomsAGTService>genericResponseObj>Message= " + mess);
                            throw new System.Exception(mess);
                        }
                    }

                    if (!String.IsNullOrWhiteSpace(genericResponseObj.Message))
                    {
                        LogMessagingUtil.Instance.AppendLine("CustomsAGTService>genericResponseObj>Message= " + genericResponseObj.Message);
                    }
                //}
                /*catch (System.Exception e)
                {
                    this.MyResponseData.Succeeded = false;
                    this.MyResponseData.HasException = true;
                    this.MyResponseData.UserMessage = this.MyResponseData.UserMessage + Environment.NewLine + e.ToString();
                    throw e;
                }*/
            }
            // moran 1.11.15 - Task 16978 <--

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = "דוח קופה לסוכן לתאריך " + requestParams.PaymentDate.Value.Date.ToString("dd/MM/yyyy");
        }

        private string GetExternalName(string externalID, int tenant)
        {
            if (string.IsNullOrWhiteSpace(externalID))
            {
                return externalID;
            }

            var clientQueryService = new ClientQueryService(_MyDbContext);
            ClientPM clientPM = clientQueryService.GetClientByCode(externalID, tenant);
            if (clientPM != null && !string.IsNullOrWhiteSpace(clientPM.FullName))
            {
                return clientPM.FullName;
            }
            return externalID;
        }

        /*private string GetBranchName(string bank, string branch)
        {
         * CustomsBranchQueryService myCustomsBranchQueryService = new CustomsBranchQueryService(this._MyDbContext);
            CustomsBranchPM customsBranchPM = myCustomsBranchQueryService.GetSingle(id, false, false);
        }*/

        private string GetPaymentProcessName(string paymentProcess)
        {
            PaymentProcessQueryService myPaymentProcessQueryService = new PaymentProcessQueryService(this._MyDbContext);
            PaymentProcessPM paymentProcessPM = myPaymentProcessQueryService.GetSingle(paymentProcess, false, true);
            return paymentProcessPM.LocalName;
        }

        private string GetPaymentTypeName(string paymentType)
        {
            PaymentOrderTypeQueryService myPaymentTypeQueryService = new PaymentOrderTypeQueryService(this._MyDbContext);
            PaymentOrderTypePM paymentTypePM = myPaymentTypeQueryService.GetSingle(paymentType, false, true);
            return paymentTypePM.LocalName;
        }

        private string GetEntityTypeName(string entityType)
        {
            EntityTypeLookupQueryService myEntityTypeLookupQueryService = new EntityTypeLookupQueryService(this._MyDbContext);
            EntityTypeLookupPM entityTypeLookup = myEntityTypeLookupQueryService.GetSingle(entityType, false, true);
            return entityTypeLookup.LocalName;
        }

        private string GetBankName(string bank)
        {
            BankQueryService myBankQueryService = new BankQueryService(this._MyDbContext);
            BankPM bankPM = myBankQueryService.GetSingle(bank, false, true);
            return bankPM.LocalName;
        }

        /*private AgentMasavPaymentTotal FindAgentMasavPaymentTotal(string bank, string branch, string accountnumber)
        {
            if (_MyAgentMasavPaymentResultList.Count == 0)
            {
                return null;
            }

            List<AgentMasavPaymentTotal> agentMasavPaymentTotalList = (from a in _MyAgentMasavPaymentResultList
                                        where (a.Bank == bank && a.Branch == branch
                                        && a.AccountNumber == accountnumber)
                                        select a).ToList();

            if (agentMasavPaymentTotalList.Count > 0)
            {
                return agentMasavPaymentTotalList.FirstOrDefault();
            }
            else
            {
                return null;
            }
        }*/


        public override MasavPaymentsToAgentResponseData GetResponse(TSH_NG_8356_MSG33_MasavPaymentsToAgent customResponse, MasavPaymentsToAgentRequestParams requestParams)
        {
            return this.MyResponseData;
        }
    }
}
