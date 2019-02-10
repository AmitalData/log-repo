
using Logitude.Server.Tools;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Server.Infrastructure;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.BL.CommonDataModel.EntityPMs;
using System.Web;
using Logitude.BL.CommonDataModel.EntityQueries;
using System;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.Interfaces;
using Microsoft.Practices.Unity;
using Logitude.BL.Helpers;
using Logitude.BL.Resolvers;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class CashBookLineDataMapping: IMapping<CashBookLinePM, CashBookLine>
   {

        public void CustomPMToPOCO(CashBookLinePM entityPM, CashBookLine entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.CashBookId);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.CashBookId = entityPM.CashBookId;
                entityPOCO.ARPChequeId = entityPM.ARPChequeId;
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }

        public void CustomPOCOToPM(CashBookLinePM entityPM, CashBookLine entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.ChequeNumber);

            // GET logged contact, RTL
            ContactPM contact = GetLoggedContact(entityPOCO.Tenant);
            bool showLocals = true;
            if (contact != null)
                showLocals = !contact.DontShowLocal;

            if (entityPOCO.ARPChequeId != null)
            {
                ARPaymentChequeQueryService chequeQueryService = new ARPaymentChequeQueryService(entityPOCO.Tenant);
                ARPaymentChequeStatusQueryService chequeStatusQueryService = new ARPaymentChequeStatusQueryService(entityPOCO.Tenant);
                ARPaymentChequePM cheque = chequeQueryService.GetSingle(entityPOCO.ARPChequeId, false, false);
                if (cheque != null)
                {
                    entityPM.Bank = cheque.BankId;
                    entityPM.ChequeNumber = cheque.ChequeNumber;
                    entityPM.ARPaymentNumber = cheque.PaymentNumber;
                    entityPM.DueDate = cheque.ValueDate;
                    entityPM.LocalAmount = cheque.LocalAmount;
                    entityPM.Currency = cheque.CurrencyCode;
                    entityPM.ForeignAmount = cheque.ForeignAmount;
                    entityPM.AccountNumber = cheque.BankAccount;
                    entityPM.Branch = cheque.BankBranch;
                    entityPM.ARPaymentId = cheque.PaymentId;
                    entityPM.ARPChequeStatusCode = cheque.StatusCode;
                    if (cheque.StatusCode != null)
                    {
                        ARPaymentChequeStatusPM chequeStatus = chequeStatusQueryService.GetSingle(cheque.StatusCode, false, false);
                        entityPM.ARPChequeStatusName = showLocals ? chequeStatus.LocalName : chequeStatus.EnglishName;
                    }
                }


            }


           


        }


        public static Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }



        private static ContactPM GetLoggedContact(int tenant)
        {
            if (OverrideGetLoggedContactFunc != null)
            {
                return OverrideGetLoggedContactFunc(tenant);
            }
            //ILoggedContactUtil loggedContactUtil = ContainerAccessor.Container.Resolve(typeof(ILoggedContactUtil), "LoggedContactUtil", new ParameterOverride("", tenant)) as ILoggedContactUtil;
            //ContactPM loggedcontact = loggedContactUtil.GetLoggedContact(tenant);

            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }




    }


}
   