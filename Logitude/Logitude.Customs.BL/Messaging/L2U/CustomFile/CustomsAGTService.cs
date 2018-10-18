
using Logitude.AmitalMessaging.Customs.CustomFile;
using Logitude.AmitalMessaging.Infrastructure;
using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Logitude.Customs.BL.Messaging.L2U.CustomFile
{
    public partial class CustomsAGTService
    {
        private MasavPaymentsToAgentRequestParams _MasavRequestParams;
        private MasavPaymentsToAgentResponseData _MasavPaymentsResponseData;
        private DateTime? _MasavSentDate;

        public CustomsAGTService(MasavPaymentsToAgentRequestParams MasavRequestParams,  DateTime? MasavSentDate, MasavPaymentsToAgentResponseData MasavPaymentsResponseData)
        {
            _MasavRequestParams = MasavRequestParams;
            _MasavPaymentsResponseData = MasavPaymentsResponseData;
            _MasavSentDate = MasavSentDate;
        }

        public GenericResponse CustomsAGT()
        {
            if (_MasavPaymentsResponseData.AgentMasavPaymentResultList == null)
            {
                throw new Exception("AGT Details are missing");
            }
            string DeclarationNumber = null;
            string DeclarationId = null;
            var context = CustomContext.GetContext(_MasavRequestParams.Tenant);
            /*
            if(!String.IsNullOrWhiteSpace(_MasavPaymentsResponseData.AgentMasavPaymentResultList.FirstOrDefault().EntityIdExternalReferenceID))
            {
                var myDeclarationQueryService = new DeclarationQueryService(context);
                string id = myDeclarationQueryService.GetIdByCustomFileNo(_MasavPaymentsResponseData.AgentMasavPaymentResultList.FirstOrDefault().EntityIdExternalReferenceID, _MasavRequestParams.Tenant);
                if (!String.IsNullOrWhiteSpace(id))
                {
                    var declarationPM = myDeclarationQueryService.GetSingle(id, false, false);
                    if (declarationPM != null)
                    {
                        DeclarationNumber = declarationPM.DeclarationNumber;
                        DeclarationId = declarationPM.Id;
                    }
                }
            }*/
            var amitalCustomFileCommunicationModel = new Logitude.Customs.BL.Messaging.Amital.AmitalCommunicationModelBase(
               Logitude.Server.Tools.Models.AmitalStandardCommunicationModel.OperationMethod.DataAccess,
               "CWSFLOGICUSTAGT", "UpdateCustomsAGT")
            {
                Tenant = _MasavRequestParams.Tenant,
                objectTableName = "Customs.Declaration",
                CommunicationLoggingEntityReference = DeclarationNumber,
                EntityId = DeclarationId,
                UserId = _MasavRequestParams.LoggingUserId,
                CommunicationSubject = "Logitude Customs AGT Report",
            };

            var myCustomsAGT = new CustomsAGT();
            myCustomsAGT.CustomsAGTRep = new CustomsAGTRep[] { new CustomsAGTRep() };
            //myCustomsAGT.CustomsAGTRep[0].CustomsFile = _MasavPaymentsResponseData.AgentMasavPaymentResultList.FirstOrDefault().EntityIdExternalReferenceID;
            //myCustomsAGT.CustomsAGTRep[0].DeclarationNum = DeclarationNumber;
            if (_MasavSentDate.HasValue) myCustomsAGT.CustomsAGTRep[0].MasavSentDate = _MasavSentDate.Value.Date.ToString("dd.MM.yy");
            myCustomsAGT.CustomsAGTRep[0].AgentMasavPayment = GetMasavPayments(_MasavPaymentsResponseData.AgentMasavPaymentResultList);
            
            
            var myUServerCommunicationService = new Logitude.Customs.BL.Messaging.Amital.UServerCommunicationService
                <Logitude.Customs.BL.Messaging.Amital.AmitalCommunicationModelBase, CustomsAGT>(
                amitalCustomFileCommunicationModel, myCustomsAGT);
            bool myImmediately = true;

            var info = myUServerCommunicationService.Send(myImmediately);
            if (String.IsNullOrWhiteSpace(info.ImmediatelyResponse))
            {
                throw new Exception("ImmediatelyResponse is null");
            }
            var GenericResponse = XmlGenericUtil<GenericResponse>.DeSerializeObject(info.ImmediatelyResponse);
            var genericResponseObj = GenericResponse.GenericResponseObj.FirstOrDefault();
            if (genericResponseObj == null)
            {
                throw new Exception("GenericResponse.GenericResponseObj is null");
            }

            if (genericResponseObj.ResponseXml == null && genericResponseObj.ResponseXml == "")
            {
                throw new Exception("genericResponseObj.ResponseXml is null");
            }

            return GenericResponse;

        }

        private AgentMasavPayment[] GetMasavPayments(List<AgentMasavPaymentResult> MasavPaymentResultlist)
        {
            var MasavPaymentList = new List<AgentMasavPayment>();
            foreach (var MasavPayment in MasavPaymentResultlist)
            {
                var myLogitudeMasavPayment = new AgentMasavPayment();
                if (MasavPayment.EntityIdExternalReferenceID != null && MasavPayment.EntityIdExternalReferenceID != "רשימה")
                {
                    myLogitudeMasavPayment.AccountingCustomsFile = MasavPayment.EntityIdExternalReferenceID;
                }
                else
                {
                    myLogitudeMasavPayment.AccountingCustomsFile = MasavPayment.ExternalID;
                }
                myLogitudeMasavPayment.Bank = MasavPayment.BankCode;
                myLogitudeMasavPayment.Branch = MasavPayment.Branch;
                myLogitudeMasavPayment.AccountNumber = MasavPayment.AccountNumber;
                myLogitudeMasavPayment.PaymentMethodAmount = MasavPayment.PaymentMethodAmount;
                myLogitudeMasavPayment.PaymentID = MasavPayment.PaymentID;
                myLogitudeMasavPayment.PaymentProcess = MasavPayment.PaymentProcess;
                myLogitudeMasavPayment.PaymentType = MasavPayment.PaymentType;
                if (MasavPayment.AgentAccountPosessionV == "Visible")
                {
                    myLogitudeMasavPayment.PayeeType = 1;
                }
                var EntityList = new List<AGTEntity>();
                foreach (var Entity in MasavPayment.RelatedEntityList)
                {
                    var myEntity = new AGTEntity();
                    myEntity.Primary = Entity.EntityIdKey1;
                    myEntity.Entname = Entity.EntityType;
                    EntityList.Add(myEntity);
                }
                myLogitudeMasavPayment.AGTEntity = EntityList.ToArray();

                MasavPaymentList.Add(myLogitudeMasavPayment);
            }
            return MasavPaymentList.ToArray();
        }

    }
}
