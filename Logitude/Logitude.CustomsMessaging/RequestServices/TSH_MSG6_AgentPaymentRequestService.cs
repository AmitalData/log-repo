using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.AgentPaymentReplyServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class TSH_MSG6_AgentPaymentRequestService : RequestServiceBase<TSH_MSG6_AgentPayment, GenericRequestParams>
    {
        public override TSH_MSG6_AgentPayment GetRequest(GenericRequestParams requestParams)
        {
            //Build request 3051- Set one or more payment methods
            ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);
            var paymentOrderQueryService = new PaymentOrderQueryService(dbContext);
            var clientQueryService = new ClientQueryService(dbContext);

            PaymentOrderPM paymentOrderPM = paymentOrderQueryService.GetSingle(requestParams.AppicationId, true, false);

            var myTSH_MSG6_AgentPaymentRequest = new TSH_MSG6_AgentPayment();
            myTSH_MSG6_AgentPaymentRequest.RequestContentHeader = new RequestContentHeader() { Convertor = "1", RecieverID = new int[] { 1 } };

            myTSH_MSG6_AgentPaymentRequest.PaymentDetails = new PaymentDetails();
            int paymentID = 0;
            int.TryParse(paymentOrderPM.PaymentNumber, out paymentID);
            myTSH_MSG6_AgentPaymentRequest.PaymentDetails.paymentID = paymentID;

            var clientPM = clientQueryService.GetSingle(paymentOrderPM.ImporterId, false, false);
            if (clientPM != null)
            {
                int externalID = 0;
                int.TryParse(clientPM.Code, out externalID);
                myTSH_MSG6_AgentPaymentRequest.PaymentDetails.externalID = externalID;
                myTSH_MSG6_AgentPaymentRequest.PaymentDetails.externalIDSpecified = true;
            }

            if (paymentOrderPM.CustomerActivityTypeCode != null)
            {
                int customerActivityTypeCode = 0;
                int.TryParse(paymentOrderPM.CustomerActivityTypeCode, out customerActivityTypeCode);
                myTSH_MSG6_AgentPaymentRequest.PaymentDetails.CustomerActivityType = customerActivityTypeCode;
                myTSH_MSG6_AgentPaymentRequest.PaymentDetails.CustomerActivityTypeSpecified = true;
            }
            else
            {
                myTSH_MSG6_AgentPaymentRequest.PaymentDetails.CustomerActivityTypeSpecified = false;
            }

            myTSH_MSG6_AgentPaymentRequest.AgentPaymentMethods = GetAgentPaymentMethods(paymentOrderPM);
            myTSH_MSG6_AgentPaymentRequest.SubmitWithProtest = GetSubmitWithProtest(paymentOrderPM);

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.PaymentOrder");
            this.MyRequestSheetParam.EntityId1 = paymentOrderPM.Id;
            this.MyRequestSheetParam.RequestDescription = "תשלום הוראה " + paymentOrderPM.PaymentNumber;
            if (paymentOrderPM.PaymentOrderConnectionTables != null)
            {
                foreach (var connectedEntityItem in paymentOrderPM.PaymentOrderConnectionTables)
                {
                    if (connectedEntityItem.ConnectedEntityCode == "D")
                    {
                        this.MyRequestSheetParam.ObjectTableId2 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                        this.MyRequestSheetParam.EntityId2 = connectedEntityItem.ConnectedEntityId;
                    }
                }
            }

            return myTSH_MSG6_AgentPaymentRequest;
        }

        private TSH_MSG6_AgentPaymentAgentPaymentMethods[] GetAgentPaymentMethods(PaymentOrderPM requestParams)
        {
            var myListOfAgentPaymentMethods = new List<TSH_MSG6_AgentPaymentAgentPaymentMethods>();
            foreach (PaymentOrderMethodPM agentPaymentMethod in requestParams.PaymentOrderMethods)
            {
                myListOfAgentPaymentMethods.Add(CreateAgentPaymentMethod(agentPaymentMethod));
            }
            return myListOfAgentPaymentMethods.ToArray();
       }

        private static TSH_MSG6_AgentPaymentAgentPaymentMethods CreateAgentPaymentMethod(PaymentOrderMethodPM agentPaymentMethodsitem)
        {
            var myAgentPaymentMethod = new TSH_MSG6_AgentPaymentAgentPaymentMethods();

            int type = 0;
            int customerActivityType = 0;
            int bankCode = 0;
            int branch = 0;

            int.TryParse(agentPaymentMethodsitem.TypeCode, out type);
            myAgentPaymentMethod.type = type;
            myAgentPaymentMethod.amount = (decimal)agentPaymentMethodsitem.Amount;
            int.TryParse(agentPaymentMethodsitem.CustomerActivityTypeCode, out customerActivityType);
            myAgentPaymentMethod.CustomerActivityType = customerActivityType;
#if dueRefersh20140917
            myAgentPaymentMethod.alternativePayingAgent = agentPaymentMethodsitem.AlternativePayingAgent;
            myAgentPaymentMethod.alternativePayingAgentSpecified = agentPaymentMethodsitem.AlternativePayingAgent > 0 ? true : false;
#endif


            myAgentPaymentMethod.AccountDetails = new AccountDetails();
            myAgentPaymentMethod.AccountDetails.accountNumber = agentPaymentMethodsitem.AccountNumber;
            int.TryParse(agentPaymentMethodsitem.BankCode, out bankCode);
            myAgentPaymentMethod.AccountDetails.bank = bankCode;
            myAgentPaymentMethod.AccountDetails.bankSpecified = bankCode > 0 ? true : false;
            int.TryParse(agentPaymentMethodsitem.BranchCode, out branch);
            myAgentPaymentMethod.AccountDetails.branch = branch;
            myAgentPaymentMethod.AccountDetails.branchSpecified = branch > 0 ? true : false;

            return myAgentPaymentMethod;
        }

        private ProtestDetails[] GetSubmitWithProtest(PaymentOrderPM requestParams)
        {
            var myListOfAgentPaymentSubmitWithProtest = new List<ProtestDetails>();
            foreach (PaymentOrderProtestReasonPM agentPaymentSubmitWithProtest in requestParams.PaymentOrderProtestReasons)
            {
                myListOfAgentPaymentSubmitWithProtest.Add(CreateAgentPaymentSubmitWithProtest(agentPaymentSubmitWithProtest));
            }
            return myListOfAgentPaymentSubmitWithProtest.ToArray();
        }

        private ProtestDetails CreateAgentPaymentSubmitWithProtest(PaymentOrderProtestReasonPM agentPaymentSubmitWithProtest)
        {
            var myAgentPaymentSubmitWithProtest = new ProtestDetails();
            int paymentProtestType = 0;
            
            int.TryParse(agentPaymentSubmitWithProtest.ProtestTypeCode, out paymentProtestType);
            myAgentPaymentSubmitWithProtest.paymentProtestType = paymentProtestType;
            myAgentPaymentSubmitWithProtest.paymentProtestTypeSpecified = paymentProtestType > 0 ? true : false;
            myAgentPaymentSubmitWithProtest.customsAgentExplanation = agentPaymentSubmitWithProtest.CustomsAgentExplanation;
            myAgentPaymentSubmitWithProtest.InvoiceNumber = agentPaymentSubmitWithProtest.InvoiceNumber;
            if (agentPaymentSubmitWithProtest.GoodsItemLineNumber != null)
            {
                myAgentPaymentSubmitWithProtest.goodsItemLineNumber = (int)agentPaymentSubmitWithProtest.GoodsItemLineNumber;
            }
            myAgentPaymentSubmitWithProtest.goodsItemLineNumberSpecified = myAgentPaymentSubmitWithProtest.goodsItemLineNumber > 0 ? true : false;
            myAgentPaymentSubmitWithProtest.goodsItemClassification = agentPaymentSubmitWithProtest.GoodsItemClassification;
            if (myAgentPaymentSubmitWithProtest.goodsItemClassification != null && myAgentPaymentSubmitWithProtest.goodsItemClassification.Length == 11)
            {
                myAgentPaymentSubmitWithProtest.goodsItemClassification = myAgentPaymentSubmitWithProtest.goodsItemClassification.Insert(10, "/");
            }
            myAgentPaymentSubmitWithProtest.amountInDispute = agentPaymentSubmitWithProtest.AmountInDispute;
            myAgentPaymentSubmitWithProtest.amountInDisputeSpecified = myAgentPaymentSubmitWithProtest.amountInDispute > 0 ? true : false;
            return myAgentPaymentSubmitWithProtest;
        }

    }
}
