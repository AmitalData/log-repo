
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data;
using Logitude.Accounting.BL.EntityQueryServices;
using Simplog.Server.Infrastructure;
using System.Web;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.Interfaces;
using Microsoft.Practices.Unity;
using Logitude.BL.Helpers;
using Logitude.BL.Resolvers;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class BankDepositLineDataMapping: IMapping<BankDepositLinePM, BankDepositLine>
   {

        public void CustomPMToPOCO(BankDepositLinePM entityPM, BankDepositLine entityPOCO)
        {

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {

                CustomMappedPOCOProperties.Add(POCOPropertyNames.Notes);
                CustomMappedPOCOProperties.Add(POCOPropertyNames.Line);
                CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
                CustomMappedPOCOProperties.Add(POCOPropertyNames.DepositId);
                CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);

                entityPOCO.Line = entityPM.Line;
                entityPOCO.Notes = entityPM.Notes;
                entityPOCO.DepositId = entityPM.DepositId;              
                entityPOCO.Tenant = entityPM.Tenant;

            }

            if (entityPM.ARPaymentChequeId != null)
            {
                ARPaymentChequeQueryService aRPaymentChequeQueryService = new ARPaymentChequeQueryService(entityPM.Tenant);
                ARPaymentChequePM aRPaymentCheque = aRPaymentChequeQueryService.GetSingle(entityPM.ARPaymentChequeId, true, false);
                if (aRPaymentCheque != null)
                {
                    entityPOCO.SearchFields = aRPaymentCheque.ChequeNumber;
                }

            }


        }

        public void CustomPOCOToPM(BankDepositLinePM entityPM, BankDepositLine entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.ARPaymentChequeId);

            // GET logged contact, RTL
            ContactPM contact = GetLoggedContact(entityPOCO.Tenant);
            bool showLocals = !contact.DontShowLocal;



            if (entityPOCO.ARPaymentChequeId != null)
            {
                ARPaymentChequeQueryService aRPaymentChequeQueryService = new ARPaymentChequeQueryService(entityPOCO.Tenant);
                ARPaymentChequePM aRPaymentCheque = aRPaymentChequeQueryService.GetSingle(entityPOCO.ARPaymentChequeId, true, false);
                if (aRPaymentCheque != null)
                {
                    entityPM.Bank = aRPaymentCheque.BankId;
                    entityPM.ChequeNumber = aRPaymentCheque.ChequeNumber;
                    entityPM.ARPaymentNumber = aRPaymentCheque.PaymentNumber;
                    entityPM.DueDate = aRPaymentCheque.ValueDate;
                    entityPM.LocalAmount = aRPaymentCheque.LocalAmount;
                    entityPM.Currency = aRPaymentCheque.CurrencyCode;
                    entityPM.ForeignAmount = aRPaymentCheque.ForeignAmount;
                    entityPM.AccountNumber = aRPaymentCheque.BankAccount;
                    entityPM.Branch = aRPaymentCheque.BankBranch;
                    entityPM.ARPaymentId = aRPaymentCheque.PaymentId;
                    entityPM.ARPaymentChequeId = aRPaymentCheque.Id;

                    if (aRPaymentCheque.StatusCode != null)
                    {
                        ARPaymentChequeStatusQueryService qs = new ARPaymentChequeStatusQueryService(entityPOCO.Tenant);
                        ARPaymentChequeStatusPM status = qs.GetSingle(aRPaymentCheque.StatusCode, true, false);
                        entityPM.ChequeStatusName = showLocals ? status.LocalName : status.EnglishName;
                        entityPM.ChequeStatusCode = aRPaymentCheque.StatusCode;
                    }
                }

            }

            entityPM.SearchFields = entityPM.ChequeNumber;




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
   