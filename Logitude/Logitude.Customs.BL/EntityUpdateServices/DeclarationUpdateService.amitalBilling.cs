using Logitude.AmitalMessaging.Customs.CustomFile;
using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Server.Tools.Helpers;
using Logitude.Customs.Data;
using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.AmitalMessaging.Infrastructure;
using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.BL.TraceEvents;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class DeclarationUpdateService
    {
        public const string UpdateUnifreightBillingConst = "Logitude.Customs.BL.EntityUpdateServices.DeclarationUpdateService.UpdateUnifreightBilling()";
        public const string UpdateIIGExcptionConst = "Logitude.Customs.BL.EntityUpdateServices.DeclarationUpdateService.UpdateIIGExcptionConst!!!";
        
        private void UpdateUnifreightBilling(DeclarationPM dirtyDeclarationPM, DeclarationPM dbOccDeclarationPM, string loggingUserId)
        {
            //return;
            LogMessagingUtil.Instance.AppendLine("!UpdateUnifreightBilling>FileState=" + dirtyDeclarationPM.FileState ?? "NULL");

            if (dirtyDeclarationPM.FileState != "P") return;

            var myFile = new LOGICUSTDRAFT();
            var myLogitudecustomsdraft = new Logitudecustomsdraft();
            //myFile.Logitudecustomsdraft = new Logitudecustomsdraft[] { myLogitudecustomsdraft };
            myLogitudecustomsdraft.CustomFileNo = dirtyDeclarationPM.CustomFileNo;
            myLogitudecustomsdraft.DeclarationNumber = dirtyDeclarationPM.DeclarationNumber;
            myLogitudecustomsdraft.CIFValue = dirtyDeclarationPM.CIFValue.Value;
            myLogitudecustomsdraft.TotalTax = dirtyDeclarationPM.TotalTax.Value;
            myLogitudecustomsdraft.DeclarationStatusCode = dirtyDeclarationPM.DeclarationStatusTypeCode;
            if (dirtyDeclarationPM.PaymentDate != null)
            {
                myLogitudecustomsdraft.PaymentDate = dirtyDeclarationPM.PaymentDate.Value;
            }
            myLogitudecustomsdraft.Taxes = GetDeclarationTaxes(dirtyDeclarationPM);
            myFile.Logitudecustomsdraft = new Logitudecustomsdraft[] { myLogitudecustomsdraft };

            var amitalCustomFileCommunicationModel = new Logitude.Customs.BL.Messaging.Amital.AmitalCommunicationModelBase(
               Logitude.Server.Tools.Models.AmitalStandardCommunicationModel.OperationMethod.DataAccess, "CWSFLOGIBILLING", "UpsertBilling")
           {
               Tenant = dirtyDeclarationPM.Tenant,
               objectTableName = "Customs.Declaration",

               CommunicationLoggingEntityReference = dirtyDeclarationPM.DeclarationNumber,
               EntityId = dirtyDeclarationPM.Id,
               UserId = loggingUserId,
               CommunicationSubject = "Logitude Declaration File UpsertBilling",
               //LogitudeFile = myFile,
           };

            var myUServerCommunicationService = new Logitude.Customs.BL.Messaging.Amital.UServerCommunicationService
                <Logitude.Customs.BL.Messaging.Amital.AmitalCommunicationModelBase, LOGICUSTDRAFT>(
                amitalCustomFileCommunicationModel, myFile);
            var info = myUServerCommunicationService.Send(true);
            if (String.IsNullOrWhiteSpace(info.ImmediatelyResponse))
            {
                throw new Exception("ImmediatelyResponse is null ");
            }
            var GenericResponse = XmlGenericUtil<GenericResponse>.DeSerializeObject(info.ImmediatelyResponse);
            var genericResponseObj = GenericResponse.GenericResponseObj.FirstOrDefault();
            if (genericResponseObj == null)
            {
                throw new Exception("GenericResponse.GenericResponseObj is null ");
            }
            if (!String.IsNullOrWhiteSpace(genericResponseObj.Status))
            {
                int sts;
                int.TryParse(genericResponseObj.Status, out sts);
                if (sts < 0)
                {
                    string mess = "Failed To Update Billing in Unifreight";
                    if (!String.IsNullOrWhiteSpace(genericResponseObj.ErrorDescription))
                    {
                        mess = mess + Environment.NewLine + genericResponseObj.ErrorDescription;
                    }
                    if (!String.IsNullOrWhiteSpace(genericResponseObj.Message))
                    {
                        mess = mess + Environment.NewLine + genericResponseObj.Message;
                    }
                    LogMessagingUtil.Instance.AppendLine("UpdateUnifreightBilling>genericResponseObj>Message= " + mess);
                    throw new Exception(mess);
                }
            }

            if (!String.IsNullOrWhiteSpace(genericResponseObj.Message))
            {
                LogMessagingUtil.Instance.AppendLine("UpdateUnifreightBilling>genericResponseObj>Message= " + genericResponseObj.Message);
            }

            if (dirtyDeclarationPM.DeclarationStatusTypeCode == "13")
            {
                RaiseDraftOKEvent(dirtyDeclarationPM, loggingUserId, "DOK");
            }
            RaiseINREvent(dirtyDeclarationPM, loggingUserId);
        }

        public const string CreateUnifreightPaymentConst = "Logitude.Customs.BL.EntityUpdateServices.DeclarationUpdateService.CreateUnifreightPayment()";
        private void CreateUnifreightPayment(DeclarationPM dirtyDeclarationPM, DeclarationPM dbOccDeclarationPM, string loggingUserId)
        {
            //return;
            
            LogMessagingUtil.Instance.AppendLine("!CreateUnifreightPayment>FileState=" + dirtyDeclarationPM.FileState ?? "NULL");
            LogMessagingUtil.Instance.AppendLine("!CreateUnifreightPayment>DeclarationNumber= " + dirtyDeclarationPM.DeclarationNumber ?? "NULL");
            float version;
            float.TryParse(dirtyDeclarationPM.VersionId, out version);
            if (dirtyDeclarationPM.FileState != "P" || dirtyDeclarationPM.DeclarationStatusTypeCode != "6" || version < 1) return;

            var context = CustomContext.GetContext(dbOccDeclarationPM.Tenant);
            var DeclarationPaymentQueryService = new DeclarationPaymentQueryService(context);
            var declarationPaymentPM = DeclarationPaymentQueryService.GetSingle(dbOccDeclarationPM.Id, true, false);

            var myFile = new LOGICUSTDRAFT();
            var myLogitudecustomsdraft = new Logitudecustomsdraft();
            //myFile.Logitudecustomsdraft = new Logitudecustomsdraft[] { myLogitudecustomsdraft };
            myLogitudecustomsdraft.CustomFileNo = dirtyDeclarationPM.CustomFileNo;
            myLogitudecustomsdraft.DeclarationNumber = dirtyDeclarationPM.DeclarationNumber;
            myLogitudecustomsdraft.CIFValue = dirtyDeclarationPM.CIFValue.Value;
            myLogitudecustomsdraft.TotalTax = dirtyDeclarationPM.TotalTax.Value;
            myLogitudecustomsdraft.DeclarationStatusCode = dirtyDeclarationPM.DeclarationStatusTypeCode;
            if (dirtyDeclarationPM.PaymentDate != null)
            {
                myLogitudecustomsdraft.PaymentDate = dirtyDeclarationPM.PaymentDate.Value;
            }
            myLogitudecustomsdraft.Taxes = GetDeclarationTaxes(dirtyDeclarationPM);
            myLogitudecustomsdraft.Payments = GetDeclarationPayments(declarationPaymentPM);
            myFile.Logitudecustomsdraft = new Logitudecustomsdraft[] { myLogitudecustomsdraft };

            var amitalCustomFileCommunicationModel = new Logitude.Customs.BL.Messaging.Amital.AmitalCommunicationModelBase(
               Logitude.Server.Tools.Models.AmitalStandardCommunicationModel.OperationMethod.DataAccess, "CWSFLOGIBILLING", "CreatePayment")

            {
                Tenant = dirtyDeclarationPM.Tenant,
                objectTableName = "Customs.Declaration",

                CommunicationLoggingEntityReference = dirtyDeclarationPM.DeclarationNumber,
                EntityId = dirtyDeclarationPM.Id,
                UserId = loggingUserId,
                CommunicationSubject = "Logitude Declaration File CreatePayment",

                //LogitudeFile = myFile,
            };

            var myUServerCommunicationService = new Logitude.Customs.BL.Messaging.Amital.UServerCommunicationService
                <Logitude.Customs.BL.Messaging.Amital.AmitalCommunicationModelBase, LOGICUSTDRAFT>(
                amitalCustomFileCommunicationModel, myFile);
            var info = myUServerCommunicationService.Send(true);
            if (String.IsNullOrWhiteSpace(info.ImmediatelyResponse))
            {
                throw new Exception("ImmediatelyResponse is null ");
            }

            var GenericResponse = XmlGenericUtil<GenericResponse>.DeSerializeObject(info.ImmediatelyResponse);
            var genericResponseObj = GenericResponse.GenericResponseObj.FirstOrDefault();

            if (genericResponseObj == null)
            {
                throw new Exception("GenericResponse.GenericResponseObj is null ");
            }
            if (!String.IsNullOrWhiteSpace(genericResponseObj.Status))
            {
                int sts;
                int.TryParse(genericResponseObj.Status,out sts);
                if(sts < 0)
                {
                    string mess = "Failed To Create Payment in Unifreight";
                    if (!String.IsNullOrWhiteSpace(genericResponseObj.ErrorDescription))
                    {
                        mess = mess + Environment.NewLine + genericResponseObj.ErrorDescription;
                    }
                    if (!String.IsNullOrWhiteSpace(genericResponseObj.Message))
                    {
                        mess = mess + Environment.NewLine + genericResponseObj.Message;
                    }
                    LogMessagingUtil.Instance.AppendLine("CreateUnifreightPayment>genericResponseObj>Message= " + mess);
                    throw new Exception(mess);
                }
            }
            
            if (!String.IsNullOrWhiteSpace(genericResponseObj.Message))
            {
                LogMessagingUtil.Instance.AppendLine("CreateUnifreightPayment>genericResponseObj>Message= " + genericResponseObj.Message);
            }

        }

        private static void RaiseDraftOKEvent(DeclarationPM dirtyDeclarationPM, string loggingUserId, string eventCode)
        {
            try
            {
                var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                {
                    Tenant = dirtyDeclarationPM.Tenant,
                    objectTableName = "Customs.Declaration",
                    EventCode = eventCode,
                    notes = "",
                    CommunicationLoggingEntityReference = dirtyDeclarationPM.DeclarationNumber,
                    EntityId = dirtyDeclarationPM.Id,
                    UserId = loggingUserId,

                    CommunicationSubject = "FU Status " + eventCode + " from logitude (Draft OK) ",
                    MyFUStatus = new AmitalEventTracerModel.FUStatus()
                    {
                        entname = "CFIFILEM",
                        primary_number = dirtyDeclarationPM.CustomFileNo,
                        status = "new",
                        xml_status = "new",
                        status_id = eventCode,
                        status_DateTime = DateTime.Now,
                        //status_place = "FRA",
                        //status_save = "no_fail",
                        comments = "",
                    }
                };

                LogMessagingUtil.Instance.AppendLine("AmitalEventTracer.CreateTraceEvent  eventCode =" + eventCode + "  CustomFileNo= " + dirtyDeclarationPM.CustomFileNo + "   ");
                AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel);

            }
            catch (Exception)
            {
                // TODO: BL Stop Execute or Cuntinue - Ask IHAB
                throw;
            }
        }

        public static void RaiseINREvent(DeclarationPM dirtyDeclarationPM, string loggingUserId)
        {
            try // moran 27.8.15 - Task 4154 - add handle for delete status
            {
                var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                {
                    Tenant = dirtyDeclarationPM.Tenant,
                    objectTableName = "Customs.Declaration",
                    EventCode = "INR",
                    notes = "DO_NOT_RAISE_EVENT",
                    CommunicationLoggingEntityReference = dirtyDeclarationPM.DeclarationNumber,
                    EntityId = dirtyDeclarationPM.Id,
                    UserId = loggingUserId,
                    
                    CommunicationSubject = "Delete FU Status INR from logitude (Declaration Sent To Customs)",
                    MyFUStatus = new AmitalEventTracerModel.FUStatus()
                    {
                        entname = "CFIFILEM",
                        primary_number = dirtyDeclarationPM.CustomFileNo,
                        status = "new",
                        xml_status = "del",
                        status_id = "INR",
                        status_DateTime = DateTime.Now,
                        //status_place = "FRA",
                        //status_save = "no_fail",
                        comments = "",
                    }
                };
                if (!dirtyDeclarationPM.IsConnectedToUnifreight) myAmitalEventTracerModel.NotConnectedToUniface = true;

                LogMessagingUtil.Instance.AppendLine("AmitalEventTracer.CreateTraceEvent  Delete Status INR  CustomFileNo= " + dirtyDeclarationPM.CustomFileNo + "   ");
                AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel);

            }
            catch (System.Exception)
            {
                // TODO: BL Stop Execute or Cuntinue - Ask IHAB
                throw;
            }

            try
            {
                var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                {
                    Tenant = dirtyDeclarationPM.Tenant,
                    objectTableName = "Customs.Declaration",
                    EventCode = "INR",
                    notes = "",
                    CommunicationLoggingEntityReference = dirtyDeclarationPM.DeclarationNumber,
                    EntityId = dirtyDeclarationPM.Id,
                    UserId = loggingUserId,

                    CommunicationSubject = "FU Status INR from logitude (Declaration Sent To Customs)",
                    MyFUStatus = new AmitalEventTracerModel.FUStatus()
                    {
                        entname = "CFIFILEM",
                        primary_number = dirtyDeclarationPM.CustomFileNo,
                        status = "new",
                        xml_status = "new",
                        status_id = "INR",
                        status_DateTime = DateTime.Now,
                        //status_place = "FRA",
                        //status_save = "no_fail",
                        comments = "",
                    }
                };
                if (!dirtyDeclarationPM.IsConnectedToUnifreight) myAmitalEventTracerModel.NotConnectedToUniface = true;

                LogMessagingUtil.Instance.AppendLine("AmitalEventTracer.CreateTraceEvent  eventCode = INR  CustomFileNo= " + dirtyDeclarationPM.CustomFileNo + "   ");
                AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel);

            }
            catch (System.Exception)
            {
                // TODO: BL Stop Execute or Cuntinue - Ask IHAB
                throw;
            }
        }

        private Payments[] GetDeclarationPayments(DeclarationPaymentPM declarationPaymentsPM)
        {
            var declarationPaymentList = new List<Payments>();

            foreach (var Payment in declarationPaymentsPM.DeclarationPaymentMethods)
            {
                var declarationPayment = new Payments();
                declarationPayment.PayeeType = Payment.PayerActivityTypeCode;
                declarationPayment.Amount = Payment.Amount.Value.ToString();

                declarationPaymentList.Add(declarationPayment);
            }

            return declarationPaymentList.ToArray();
        }

        private Taxes[] GetDeclarationTaxes(DeclarationPM dirtyDeclarationPM)
        {
            var declarationTaxList = new List<Taxes>();

            foreach (var Tax in dirtyDeclarationPM.DeclarationTaxes)
            {
                var declarationTax = new Taxes();
                declarationTax.TaxTypeCode = Tax.TaxTypeCode;
                declarationTax.TotalAmount = Tax.TotalAmount.Value;

                declarationTaxList.Add(declarationTax);
            }

            return declarationTaxList.ToArray();
        }
    }
}
