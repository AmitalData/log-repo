
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
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Resolvers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.Counters;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class InterestReportDataMapping: IMapping<InterestReportPM, InterestReport>
   {

        public void CustomPMToPOCO(InterestReportPM entityPM, InterestReport entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);
            AddPOCOPropertyName(POCOPropertyNames.CreateDateTime);
            AddPOCOPropertyName(POCOPropertyNames.UpdateDateTime);
            AddPOCOPropertyName(POCOPropertyNames.CreatedByUserId);
            AddPOCOPropertyName(POCOPropertyNames.UpdatedByUserId);
            
            AddPOCOPropertyName(POCOPropertyNames.ReportNumber);

            string loggedContactId = GetLoggedContactId(entityPM);
            entityPM.UpdateDateTime = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            entityPM.UpdatedByUserId = loggedContactId;

            entityPOCO.UpdateDateTime = entityPM.UpdateDateTime;
            entityPOCO.UpdatedByUserId = entityPM.UpdatedByUserId;
            
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPM.CreateDateTime = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                entityPM.InterestReportStatusCode = "5";
                entityPM.ReportNumber = CodeCounter.GetNumber("InterestReport", entityPM.Tenant).ToString();
                entityPM.CreatedByUserId = loggedContactId;

                entityPOCO.CreateDateTime = entityPM.CreateDateTime;
                entityPOCO.ReportNumber = entityPM.ReportNumber;
                entityPOCO.InterestReportStatusCode = entityPM.InterestReportStatusCode;
                entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;
                SetInterestReportGLAccountFields(entityPM);  
                             
             }

            if (entityPOCO.ARinvoiceId != null)
            {
                ARInvoiceRepository aRInvoiceRepository = new ARInvoiceRepository(entityPM.Tenant);
                entityPM.ARInvoiceNumber = aRInvoiceRepository.GetInvoiceNumber(entityPOCO.ARinvoiceId, entityPOCO.Tenant);
            }

            CustomerPM customerPM = new CustomerPM();
            if (entityPM.CustomerId != null)
            {
                CustomerQuery customerQuery = new CustomerQuery(entityPM.Tenant);
                customerPM = customerQuery.GetBasicSinglePM(entityPM.CustomerId, entityPM.Tenant, true);
                entityPM.CustomerName = customerPM.EnglishName;
                entityPM.CustomerLocalName = customerPM.LocalName;

            }
           
            FillSearchFields(entityPM, customerPM);

        }
        private void SetInterestReportGLAccountFields(InterestReportPM interestReport)
        {
            if (interestReport.GLAccountId != null)
            {
                GLAccountRepository gLAccountRepository = new GLAccountRepository(interestReport.Tenant);
                GLAccount gLAccount = gLAccountRepository.GetSingle(interestReport.GLAccountId, interestReport.Tenant);
                interestReport.GLAccountMinimumInterest = gLAccount.MinimumInterestInvoiceBilling;
                interestReport.CreditAllotmentPercentage = gLAccount.CreditAllotmentPercentage;
                interestReport.GLAccountInterestCreditLimit = gLAccount.InterestCreditLimit;
                interestReport.GLAccountDisplayNumber = gLAccount.DisplayNumber;
            }
        }

        public void CustomPOCOToPM(InterestReportPM entityPM, InterestReport entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.CreatedByLocalName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.UpdatedByLocalName);

            // GET logged contact, RTL
            ContactQuery contactQuery = new ContactQuery(entityPOCO.Tenant);
            ContactPM contact = GetLoggedContact(entityPOCO.Tenant) ?? new ContactPM();
            bool showLocals = !contact.DontShowLocal;

            if (entityPOCO.CreatedByUserId != null)
            {
                ContactPM createdByContact = contactQuery.GetSinglePMFromCache(entityPOCO.CreatedByUserId, entityPOCO.Tenant);
                if (createdByContact == null)
                    createdByContact = contactQuery.GetSinglePMFromCache(entityPOCO.CreatedByUserId, 0); // user is customer care, get it from tenant 0
                if (createdByContact != null)
                    entityPM.CreatedByLocalName = showLocals ? createdByContact.LocalName : createdByContact.EnglishName;
            }

            if (entityPOCO.UpdatedByUserId != null)
            {
                ContactPM updatedByContact = contactQuery.GetSinglePMFromCacheWithSystemUser(entityPOCO.UpdatedByUserId, entityPOCO.Tenant);
                if (updatedByContact == null)
                    updatedByContact = contactQuery.GetSinglePMFromCacheWithSystemUser(entityPOCO.UpdatedByUserId, 0); // user is customer care, get it from tenant 0
                if (updatedByContact != null)
                    entityPM.UpdatedByLocalName = showLocals ? updatedByContact.LocalName : updatedByContact.EnglishName;
            }

            if (entityPOCO.ARinvoiceId != null)
            {
                ARInvoiceRepository aRInvoiceRepository = new ARInvoiceRepository(entityPM.Tenant);
                entityPM.ARInvoiceNumber = aRInvoiceRepository.GetInvoiceNumber(entityPOCO.ARinvoiceId, entityPOCO.Tenant);
            }

            if (entityPOCO.InterestReportStatusCode != null)
            {
                InterestReportStatuseRepository interestReportStatuseRepository = new InterestReportStatuseRepository(entityPM.Tenant);
                InterestReportStatuse interestReportStatuse = interestReportStatuseRepository.GetSingle(entityPOCO.InterestReportStatusCode);
                entityPM.InterestReportStatusName = showLocals ? interestReportStatuse.LocalName:interestReportStatuse.EnglishName;
                entityPM.InterestReportStatusLocalName = showLocals ? interestReportStatuse.LocalName : interestReportStatuse.EnglishName;
            }

            if (entityPOCO.CustomerId != null)
            {
                CustomerQuery customerQuery = new CustomerQuery(entityPOCO.Tenant);
                CustomerPM customerPM = customerQuery.GetBasicSinglePM(entityPOCO.CustomerId, entityPOCO.Tenant,true);
                entityPM.CustomerName = showLocals ? customerPM.LocalName : customerPM.EnglishName;
                entityPM.CustomerLocalName = customerPM.LocalName;

            }
            if (entityPOCO.ReportCurrencyId != null)
            {
                CurrencyQuery currencyQuery = new CurrencyQuery(entityPOCO.Tenant);
                CurrencyPM currencyPM = currencyQuery.GetSinglePM(entityPOCO.ReportCurrencyId, entityPOCO.Tenant);

                entityPM.ReportCurrencyCode = currencyPM?.Code;
            }



            if (entityPM.InterestReportStatusCode=="1")
            {
                entityPM.IsFirstReport = IsCustomerHasReportNotCancelled(entityPM);

            }
            else
            {
                entityPM.IsFirstReport = false;
            }

            entityPM.CanRecalculate = false;
            InterestReportRepository interestReportRepository = new InterestReportRepository(entityPOCO.Tenant);
            InterestReport interestReport = interestReportRepository.GetSingleByGraterInterestCalculationDate(entityPOCO.CustomerId, entityPOCO.Id, entityPOCO.InterestCalculationDate, entityPOCO.Tenant);
            if (interestReport==null && (entityPM.InterestReportStatusCode =="1" || entityPM.InterestReportStatusCode == "6"))
            {
                entityPM.CanRecalculate = true;
            }
            if (entityPOCO.GLAccountId != null )
            {
                GLAccountRepository gLAccountRepository = new GLAccountRepository(entityPOCO.Tenant);
                GLAccount gLAccount = gLAccountRepository.GetSingle(entityPOCO.GLAccountId, entityPOCO.Tenant);
                entityPM.GLAccountMinimumInterest = gLAccount.MinimumInterestInvoiceBilling;
                entityPM.GLAccountLocalName = gLAccount.LocalName;
            }



        }

        public static Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }
        public bool SuppressFetchOpenReconcilation { get; internal set; }
        private bool IsCustomerHasReportNotCancelled(InterestReportPM entityPM)
        {
            InterestReportRepository interestReportRepository = new InterestReportRepository(entityPM.Tenant);
            InterestReport interestReport = interestReportRepository.GetSingleByCusstomerAndStatudNotCancelledOrFailed(entityPM.ReportNumber, entityPM.CustomerId, entityPM.Tenant);
            if (interestReport != null)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        private void FillSearchFields(InterestReportPM entityPM, CustomerPM customerPM)
        {
            entityPM.SearchFields = entityPM.ReportNumber + "," + entityPM.CustomerName + "," + entityPM.CustomerLocalName + "," + entityPM.GLAccountDisplayNumber + "," + entityPM.GLAccountLocalName + "," + entityPM.ARInvoiceNumber;

            if (customerPM != null && customerPM.Code != null) {
                entityPM.SearchFields = entityPM.SearchFields + "," + customerPM.Code;
            }
        }

        private static string GetLoggedContactId(InterestReportPM entityPM)
        {
            ContactPM loggedContact = null;
            string loggedContactId = null;
 
                loggedContact = LoggedContactResolver.GetLoggedContact(entityPM.Tenant);
                loggedContactId = loggedContact.Id;

            return loggedContactId;
        }


        private static ContactPM GetLoggedContact(int tenant)
        {
            if (OverrideGetLoggedContactFunc != null)
            {
                return OverrideGetLoggedContactFunc(tenant);
            }
            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }
    }


}
   