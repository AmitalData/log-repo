
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
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class InterestReportDataMapping: IMapping<InterestReportPM, InterestReport>
   {

        public void CustomPMToPOCO(InterestReportPM entityPM, InterestReport entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
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
                ContactPM updatedByContact = contactQuery.GetSinglePMFromCache(entityPOCO.UpdatedByUserId, entityPOCO.Tenant);
                if (updatedByContact == null)
                    updatedByContact = contactQuery.GetSinglePMFromCache(entityPOCO.UpdatedByUserId, 0); // user is customer care, get it from tenant 0
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
                CustomerPM customerPM = customerQuery.GetSinglePM(entityPOCO.CustomerId, entityPOCO.Tenant);
                entityPM.CustomerName = showLocals ? customerPM.LocalName : customerPM.EnglishName;
             }
        }

        public static Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }
        public bool SuppressFetchOpenReconcilation { get; internal set; }

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
   