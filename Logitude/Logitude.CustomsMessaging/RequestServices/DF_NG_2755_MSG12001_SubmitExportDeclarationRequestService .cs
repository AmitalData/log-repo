using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.BL.Models;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityLists;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Models;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;
using Unifreight.Data.AmitalModel.Repsitories;
using Logitude.Customs.BL.BL;
using UnifreightIIG.Common.SubmitExportDeclarationRequestServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class DF_NG_2755_MSG12001_SubmitExportDeclarationRequestService
        : RequestServiceBase<DF_NG_2755_MSG12001_SubmitDeclaration, GenericRequestParams>
    {
        private ICustomContext dbContext;
        public override void OnRequestFail(GenericRequestParams requestParams)
        {
            if (!String.IsNullOrWhiteSpace(requestParams.AppicationId))
            {
                CalculateDeclarationCourierStatus.UpdateCourierDeclarationStatusCode(requestParams.Tenant, requestParams.AppicationId);
            }

            base.OnRequestFail(requestParams);
        }
        public override void ManipulateRequestParams(GenericRequestParams requestParams)
        {

            this.dbContext = CustomContext.GetContext(requestParams.Tenant);
            var DeclarationPaymentQueryService = new DeclarationPaymentQueryService(this.dbContext);
            var declarationPaymentsPM = DeclarationPaymentQueryService.GetSingle(requestParams.AppicationId, true, false);
            if (requestParams.RequestVIA == SendRequestVIA.Default)
            {
                requestParams.RequestVIA = DefaultMessageController.Via(requestParams.Tenant, requestParams.MainInterfaceCode, requestParams.RequestVIA);
            }

            var srverTime = (new DualQueryService(AmitalContext.GetContext(requestParams.Tenant))).GetServerDateTime();
            if (
                declarationPaymentsPM.FuturePaymentDateTime > srverTime &&
                declarationPaymentsPM.FuturePaymentDateTime.GetValueOrDefault().Subtract(srverTime.GetValueOrDefault()) > TimeSpan.FromMinutes(1)
                )
            {

                switch (requestParams.RequestVIA)
                {
                    case SendRequestVIA.WebServiceInteractive:
                        requestParams.RequestVIA = SendRequestVIA.WebServiceBatch;
                        break;
                    case SendRequestVIA.WebServiceBatch:
                        break;
                    case SendRequestVIA.DCABatch:
                        break;
                    case SendRequestVIA.Default:
                    default:
                        throw new System.Exception("should not be SendRequestVIA.Default !!!!");
                        break;
                }
                requestParams.RequestVIAChangeDue = string.Concat("נרשמה בקשה מתוזמנת לתאריך ", declarationPaymentsPM.FuturePaymentDateTime.GetValueOrDefault().ToShortDateString(), " שעה ", declarationPaymentsPM.FuturePaymentDateTime.GetValueOrDefault().ToShortTimeString());// "הבקשה תשלח בעתיד";
                requestParams.FutureSendDateTime = declarationPaymentsPM.FuturePaymentDateTime;
            }
            if (!requestParams.FutureSendDateTime.HasValue)
            {
                switch (requestParams.RequestVIA)
                {
                    case SendRequestVIA.WebServiceInteractive:
                        var myDF_MSG10000_ImportDeclarationRequestService = new DF_MSG10000_ImportDeclarationRequestService();
                        myDF_MSG10000_ImportDeclarationRequestService.ManipulateRequestParams(requestParams);
                        break;
                    case SendRequestVIA.WebServiceBatch:
                    case SendRequestVIA.DCABatch:
                        break;
                    case SendRequestVIA.Default:
                    default:
                        throw new System.Exception("should not be SendRequestVIA.Default !!!!");
                        break;
                }
            }

            base.ManipulateRequestParams(requestParams);
        }


        private void UCB2755Batch(GenericRequestParams requestParams)
        {
            var objectTableIdCourierMaster = ObjectTableRepository.GetObjectTableByName("Customs.CourierMaster");
            if (requestParams.LoggingEntityId2 != objectTableIdCourierMaster)
            {
                return;
            }
            if (String.IsNullOrWhiteSpace(requestParams.UnifreightListOnServerOnly))
            {
                return;
            }
            var dic = UnifreightListsUtil.Deserialize(requestParams.UnifreightListOnServerOnly);
            var bankId=UnifreightListsUtil.GetValue(ref dic , "InternalBankId");
            var DeclarationQueryService = new DeclarationQueryService(this.dbContext);
            var declarationPM = DeclarationQueryService.GetSingle(requestParams.AppicationId, true, false);
            


            CheckLock(requestParams, declarationPM);
            PaymentDateISNotNull(requestParams, declarationPM);
            TotalTaxRequestedIsValid(requestParams, declarationPM);
            DelSertPayment(
          requestParams,
          declarationPM,
          bankId);
        }

        public  void DelSertPayment(
          GenericRequestParams requestParams,
          DeclarationPM myDeclarationPM,
          string bankId)
        {
            string user = requestParams.LoggingUserId;
            if (String.IsNullOrWhiteSpace(user)) user = AuthenticationUtil.ResolveUserId(requestParams.Tenant);
            
            var myQueryService = new DeclarationQueryService(this.dbContext);

            ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);

            var mydeclarationPaymentQueryService = new DeclarationPaymentQueryService(this.dbContext);
            var declarationPaymentPM = mydeclarationPaymentQueryService.GetSingle(myDeclarationPM.Id, true, false);
            if (declarationPaymentPM != null)  //@itzik M אם כבר קיימות שורות אבל אין תאריך תשלום Declaration PaymentDate המשמעות היא שלא שולם בפועל, ולכן למחוק ולכתוב מחדש לפי נתונים נוכחיים.

            {
                //Delete 

                //declarationPaymentPM.ChangeSetOp = ChangeSetOperation.Delete;
                if (declarationPaymentPM.DeclarationPaymentMethods.Any())
                {
                    foreach (var item in declarationPaymentPM.DeclarationPaymentMethods)
                    {
                        item.ChangeSetOp = ChangeSetOperation.Delete;
                    }

                    var myDeclarationPaymentMethodUpdateService = new DeclarationPaymentMethodUpdateService(this.dbContext, new Dictionary<string, IContext>() , requestParams.Tenant);
                    myDeclarationPaymentMethodUpdateService.UpdateMulti(declarationPaymentPM.DeclarationPaymentMethods, new List<DeclarationPaymentMethodPM>(), declarationPaymentPM
                        , true);
                }

                declarationPaymentPM = mydeclarationPaymentQueryService.GetSingle(myDeclarationPM.Id, true, false);
                declarationPaymentPM.ChangeSetOp = ChangeSetOperation.Update;


            }
            
            declarationPaymentPM = declarationPaymentPM ?? new DeclarationPaymentPM()
            {
                DeclarationId = myDeclarationPM.Id,
                Tenant = myDeclarationPM.Tenant,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
            };

            

            declarationPaymentPM.PaymentDate = DateTime.Now;
            declarationPaymentPM.CreatedByUserId = user;
            
            CustomsSettingQueryService customsSettingQuery = new CustomsSettingQueryService(this.dbContext);
            CustomsSettingPM CustomsSetting = customsSettingQuery.GetSingleByTenant(requestParams.Tenant);
            declarationPaymentPM.SignatoryIdentification =
                //myDeclarationPM.SignerPersonalId;
                CustomsSetting.CustomsAgentId;//לשים ח.פ של חברה 

            
            //if (declarationPaymentPM.DeclarationPaymentMethods == null || declarationPaymentPM.DeclarationPaymentMethods.Count() < 1)
            {
                

                

                CustomBankQueryService customBankQueryService = new CustomBankQueryService(this.dbContext);
                

                var customBank = customBankQueryService.GetSingle(bankId, false,false);
                if (customBank==null)
                {
                    throw new System.Exception($"customBankQueryService.GetSingle(bankId={bankId}  return null !! - maybe clear cache on client !!!!!!!@!!!!!@@@@ ");
                }

                //customBankList = GetBank(myDeclarationPM);




                DeclarationPaymentMethodPM declarationPaymentMethod = new DeclarationPaymentMethodPM()
                {
                    Tenant = myDeclarationPM.Tenant,
                    DeclarationId = myDeclarationPM.Id,
                    Line = 1,
                    SequenceNumeric = 1,
                    MethodTypeCode = "1",
                    Amount = myDeclarationPM.TotalTax,
                    ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,

                    BankCode = customBank.BankCode,
                    BranchCode = customBank.BranchCode,
                    PayerActivityTypeCode = customBank.PayerTypeCode,
                    AccountNumber = customBank.AccountNumber,
                    CustomsBranchId = customBank.CustomsBranchId
                };
                
                declarationPaymentPM.DeclarationPaymentMethods.Add(declarationPaymentMethod);
            }
            //else
            //{
            //    declarationPaymentPM.DeclarationPaymentMethods.FirstOrDefault().Amount = myDeclarationPM.TotalTax;
            //    declarationPaymentPM.DeclarationPaymentMethods.FirstOrDefault().ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            //}

            DeclarationPaymentUpdateService declarationPaymentUpdateService = new DeclarationPaymentUpdateService(this.dbContext, new Dictionary<string, IContext>(), myDeclarationPM.Tenant);
            declarationPaymentUpdateService.Update(declarationPaymentPM, true);

            




            
        }

        private CustomBankList GetBank(DeclarationPM myDeclarationPM)
        {
            CustomBankQueryService customBankQueryService = new CustomBankQueryService(this.dbContext);
            List<CustomBankList> customBanksList = new List<CustomBankList>();
            CustomBankList customBankList = new CustomBankList();
            customBanksList = customBankQueryService.GetCustomBanksByCard(myDeclarationPM.CustomerId, myDeclarationPM.Tenant);
            if (customBanksList != null && customBanksList.Count() > 0) customBankList = customBanksList.Where(d => !d.InActive).FirstOrDefault();

            return customBankList;
        }
        private void AppendLogLine(object p)
        {
            //throw new NotImplementedException();
        }

        private void TotalTaxRequestedIsValid(GenericRequestParams requestParams, DeclarationPM declarationPM)
        {
            if (declarationPM.TotalTax != declarationPM.DeclarationTaxes.Sum(el => el.TotalAmount))
            {
                throw new System.Exception("TotalTaxRequestedIsValid");
            }
           // throw new NotImplementedException();
        }

        private void PaymentDateISNotNull(GenericRequestParams requestParams, DeclarationPM declarationPM)
        {
            //ג. בדיקה האם Declaration PaymentDate <> Null - אם נכשל יש להפיל את הבקשה ללא נסיון חוזר.
            if (declarationPM.PaymentDate.HasValue)
            {
                throw new System.Exception("//ג. בדיקה האם Declaration PaymentDate <> Null - אם נכשל יש להפיל את הבקשה ללא נסיון חוזר.");
            }
         //   throw new NotImplementedException();
        }

        private static void CheckLock(GenericRequestParams requestParams, DeclarationPM declarationPM)
        {
            LogMessagingUtil.Instance.AppendLine("CourierMaster Send Batch===> CheckLock");

            long lCUSTOMFILENO;
            if (!long.TryParse(declarationPM.CustomFileNo, out lCUSTOMFILENO))
            {
                throw new BusinessErrorException("_DirtyDeclarationPaymentPM.DeclarationId could not convert to long ");
            }
            var myCCUFILEMRepository = new CCUFILEMRepository(declarationPM.Tenant);
            var ccufilem = myCCUFILEMRepository.GetFILENOByCUSTOMFILENO(lCUSTOMFILENO);


            var myCCUQUELOCKRepository = new CCUQUELOCKRepository(requestParams.Tenant);
            try
            {
                var cculock = myCCUQUELOCKRepository.GetSingleGeneralLockNOWAIT("CCUFILEM", ccufilem.ToString());

            }
            catch (System.Exception)
            {

                LogMessagingUtil.Instance.AppendLine($"GetSingleGeneralLockNOWAIT(CCUFILEM, {ccufilem.ToString()}) ==> Already Lock => try later (*5) ");
                throw;
            }
        }


        public override DF_NG_2755_MSG12001_SubmitDeclaration GetRequest(GenericRequestParams requestParams)
        {
            //Build request 2755- Change ImportDeclaration Status from Draft to Submitted
            var myDF_NG_2755_MSG12001_SubmitDeclaration = new DF_NG_2755_MSG12001_SubmitDeclaration();
            myDF_NG_2755_MSG12001_SubmitDeclaration.RequestContentHeader = new RequestContentHeader() { Convertor = "1", RecieverID = new int[] { 1 } };

            this.dbContext = CustomContext.GetContext(requestParams.Tenant);
            this.UCB2755Batch(requestParams);
            var DeclarationPaymentQueryService = new DeclarationPaymentQueryService(this.dbContext);
            var declarationPaymentsPM = DeclarationPaymentQueryService.GetSingle(requestParams.AppicationId, true, false);

            myDF_NG_2755_MSG12001_SubmitDeclaration.GeneralData = GetSubmitDeclarationGeneralData(declarationPaymentsPM);
            myDF_NG_2755_MSG12001_SubmitDeclaration.AnswerForCollateralRequest = GetSubmitDeclarationCollateralAnswer(declarationPaymentsPM);

            //Raise event PHF- Declaration Payment Sent
            SendPHF(declarationPaymentsPM, requestParams.LoggingUserId);

            return myDF_NG_2755_MSG12001_SubmitDeclaration;
        }

        private DF_NG_2755_MSG12001_SubmitDeclarationGeneralData GetSubmitDeclarationGeneralData(Customs.Def.EntityPMs.DeclarationPaymentPM myDeclarationPaymentsPM)
        {
            var myGeneralData = new DF_NG_2755_MSG12001_SubmitDeclarationGeneralData();
            int signatoryIdentification = 0;

            var declarationQueryService = new DeclarationQueryService(this.dbContext);
            var declarationPM = declarationQueryService.GetSingle(myDeclarationPaymentsPM.DeclarationId, false, false);

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.CustomFileNo = declarationPM.CustomFileNo;
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            this.MyRequestSheetParam.EntityId1 = declarationPM.Id;
            this.MyRequestSheetParam.RequestDescription = "הגשה" + declarationPM.DeclarationNumber + " " + declarationPM.VersionId;

            myGeneralData.declarationID = declarationPM.DeclarationNumber;
            myGeneralData.declarationVersion = declarationPM.VersionId;
            //myGeneralData.AgentFileReferenceID = declarationPM.ExternalDeclarationNumber;
            myGeneralData.AgentFileReferenceID = declarationPM.CustomFileNo;

            if (myDeclarationPaymentsPM.PaymentDate.HasValue)
            {
                myGeneralData.submitDate = myDeclarationPaymentsPM.PaymentDate.Value;
            }
            /*else // temp
            {
                myGeneralData.submitDate = DateTime.Now;
            }*/
            int.TryParse(myDeclarationPaymentsPM.SignatoryIdentification, out signatoryIdentification);
            myGeneralData.mainSignatoryID = signatoryIdentification;
            myGeneralData.mainSignatoryIDSpecified = myGeneralData.mainSignatoryID > 0 ? true : false;
            myGeneralData.DocumentaryInspectionRequest = myDeclarationPaymentsPM.IsProcessA;
            myGeneralData.DocumentaryInspectionRequestSpecified = myGeneralData.DocumentaryInspectionRequest != null ? true : false;
            myGeneralData.DocumentaryInspectionReason = myDeclarationPaymentsPM.ProcessADescription;
         //   myGeneralData.SubmitWithProtest = GetSubmitWithProtest(myDeclarationPaymentsPM.DeclarationPaymentProtests);
         //   myGeneralData.PaymentMethod = GetPaymentMethod(myDeclarationPaymentsPM.DeclarationPaymentMethods);

            return myGeneralData;
        }

        private DF_NG_2755_MSG12001_SubmitDeclarationGeneralDataSubmitWithProtest[] GetSubmitWithProtest(List<Customs.Def.EntityPMs.DeclarationPaymentProtestPM> declarationPaymentProtestsList)
        {
            var mySubmitWithProtestList = new List<DF_NG_2755_MSG12001_SubmitDeclarationGeneralDataSubmitWithProtest>();

            foreach (var myPaymentProtestsItem in declarationPaymentProtestsList)
            {
                var mySubmitWithProtest = new DF_NG_2755_MSG12001_SubmitDeclarationGeneralDataSubmitWithProtest();
                mySubmitWithProtest.paymentProtestType = myPaymentProtestsItem.ProtestTypeCode;
                mySubmitWithProtest.customsAgentExplanation = myPaymentProtestsItem.CustomsAgentExplanation;
                mySubmitWithProtest.InvoiceNumber = myPaymentProtestsItem.InvoiceNumber;
                //mySubmitWithProtest.goodsItemLineNumber = (int)myPaymentProtestsItem.GoodsItemLineNumber;
                //mySubmitWithProtest.goodsItemLineNumberSpecified = myPaymentProtestsItem.GoodsItemLineNumber > 0 ? true : false;
                mySubmitWithProtest.goodsItemClassification = myPaymentProtestsItem.GoodsItemClassification;
                if (mySubmitWithProtest.goodsItemClassification != null && mySubmitWithProtest.goodsItemClassification.Length == 11)
                {
                    mySubmitWithProtest.goodsItemClassification = mySubmitWithProtest.goodsItemClassification.Insert(10, "/");
                }
                //mySubmitWithProtest.amountInDispute = myPaymentProtestsItem.AmountInDispute;
                //mySubmitWithProtest.amountInDisputeSpecified = myPaymentProtestsItem.AmountInDispute > 0 ? true : false;
                if (myPaymentProtestsItem.GoodsItemLineNumber.HasValue)
                {
                    mySubmitWithProtest.goodsItemLineNumber = (int)myPaymentProtestsItem.GoodsItemLineNumber;
                    mySubmitWithProtest.goodsItemLineNumberSpecified = myPaymentProtestsItem.GoodsItemLineNumber > 0 ? true : false;
                }
                if (myPaymentProtestsItem.AmountInDispute.HasValue)
                {
                    mySubmitWithProtest.amountInDispute = myPaymentProtestsItem.AmountInDispute;
                    mySubmitWithProtest.amountInDisputeSpecified = myPaymentProtestsItem.AmountInDispute > 0 ? true : false;
                }
                mySubmitWithProtestList.Add(mySubmitWithProtest);
            }

            return mySubmitWithProtestList.ToArray();
        }

        private DF_NG_2755_MSG12001_SubmitDeclarationGeneralDataPaymentMethod[] GetPaymentMethod(List<Customs.Def.EntityPMs.DeclarationPaymentMethodPM> declarationPaymentMethodPMList)
        {
            var myPaymentMethodList = new List<DF_NG_2755_MSG12001_SubmitDeclarationGeneralDataPaymentMethod>();
            int bankCode = 0;
            int branchCode = 0;

            foreach (var myDeclarationPaymentMethodItem in declarationPaymentMethodPMList)
            {
                var myPaymentMethod = new DF_NG_2755_MSG12001_SubmitDeclarationGeneralDataPaymentMethod();
                myPaymentMethod.sequence = myDeclarationPaymentMethodItem.SequenceNumeric;
                myPaymentMethod.payerActivityType = myDeclarationPaymentMethodItem.PayerActivityTypeCode;
                myPaymentMethod.paymentMethodType = myDeclarationPaymentMethodItem.MethodTypeCode;


                if (myDeclarationPaymentMethodItem.Amount.HasValue)
                {
                    myPaymentMethod.paymentMethodAmount = (decimal)myDeclarationPaymentMethodItem.Amount.Value;
                }
                myPaymentMethod.bankIDSpecified = false;
                if (int.TryParse(myDeclarationPaymentMethodItem.BankCode, out bankCode))
                {
                    myPaymentMethod.bankID = bankCode;
                    myPaymentMethod.bankIDSpecified = true;
                }
                myPaymentMethod.BranchSpecified = false;
                if (int.TryParse(myDeclarationPaymentMethodItem.BranchCode, out branchCode))
                {
                    myPaymentMethod.Branch = branchCode;
                    myPaymentMethod.BranchSpecified = true;// ? true : false;
                }




                myPaymentMethod.accountNumber = myDeclarationPaymentMethodItem.AccountNumber;

                myPaymentMethodList.Add(myPaymentMethod);
            }

            return myPaymentMethodList.ToArray();
        }

        private AnswerForCollateral[] GetSubmitDeclarationCollateralAnswer(Customs.Def.EntityPMs.DeclarationPaymentPM declarationPaymentsPM)
        {
            var myAnswerForCollateralList = new List<AnswerForCollateral>();


            return myAnswerForCollateralList.ToArray();
        }



        private void SendPHF(DeclarationPaymentPM declarationPaymentPM, string loggingUserId)
        {
            try
            {
                var declarationQueryService = new DeclarationQueryService(this.dbContext);
                DeclarationPM connectedDeclarationPM = declarationQueryService.GetSingle(declarationPaymentPM.DeclarationId, false, false);
                if (connectedDeclarationPM.IsCourierDeclaration) return;
                var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                {
                    Tenant = declarationPaymentPM.Tenant,
                    objectTableName = "Customs.Declaration",
                    EventCode = "PHF",
                    notes = "Declaration Payment Sent",
                    CommunicationLoggingEntityReference = connectedDeclarationPM.DeclarationNumber,
                    EntityId = declarationPaymentPM.DeclarationId,
                    UserId = loggingUserId,

                    CommunicationSubject = "FU Status PHF from logitude ",
                    MyFUStatus = new AmitalEventTracerModel.FUStatus()
                    {
                        entname = "CFIFILEM",
                        primary_number = connectedDeclarationPM.CustomFileNo,
                        status = "new",
                        xml_status = "new",
                        status_id = "PHF",
                        status_DateTime = DateTime.Now,
                        //status_place = "FRA",
                        //status_save = "no_fail",
                        comments = "Declaration Payment:" + connectedDeclarationPM.DeclarationNumber + ", Payment Date:" + declarationPaymentPM.PaymentDate,
                    }
                };
                AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel);
            }
            catch (System.Exception)
            {
                // TODO: BL Stop Execute or Cuntinue - Ask IHAB
                throw;
            }
        }

        public override void PostGetRequest(DF_NG_2755_MSG12001_SubmitDeclaration customRequest, GenericRequestParams requestParams)
        {
            this.dbContext = CustomContext.GetContext(requestParams.Tenant);
            var DeclarationPaymentQueryService = new DeclarationPaymentQueryService(this.dbContext);
            var declarationPaymentsPM = DeclarationPaymentQueryService.GetSingle(requestParams.AppicationId, true, false);
            if (declarationPaymentsPM != null && !string.IsNullOrWhiteSpace(declarationPaymentsPM.DeclarationId))
            {
                var declarationQueryService = new DeclarationQueryService(this.dbContext);
                var declarationPM = declarationQueryService.GetSingle(declarationPaymentsPM.DeclarationId, false, false);
                if (declarationPM != null && declarationPM.IsCourierDeclaration)
                {
                    DeclarationCourierStatusUpdateService declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(this.dbContext, new Dictionary<string, IContext>(), requestParams.Tenant);
                    DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(this.dbContext);
                    DeclarationCourierStatusPM currentDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(declarationPM.Id, true, false);
                    if (currentDeclarationCourierStatusPM == null)
                    {
                        currentDeclarationCourierStatusPM = new DeclarationCourierStatusPM()
                        {
                            DeclarationId = declarationPM.Id,
                            Tenant = declarationPM.Tenant,
                            IsClosedForFollowUp = false,
                            IsCourierMissingClassification = false,
                        };
                        currentDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Insert;
                    }
                    else
                    {
                        currentDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                    }
                    currentDeclarationCourierStatusPM.CourierPaymentStatusCode = "I";
                    declarationCourierStatusUpdateService.Update(currentDeclarationCourierStatusPM, true);
                }
            }
        }
    }
}
