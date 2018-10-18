using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.Tools.EntityService;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        private ComputingPartnerTranslationQuery computingPartnerTranslationQuery;
        private ComputingPartnerTranslationRepository computingPartnerTranslationRepository;

        public ComputingPartnerTranslationPM GetSingleComputingPartnerTranslationPM(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("ComputingPartnerTranslation", "READ", tenant);

            computingPartnerTranslationQuery = new ComputingPartnerTranslationQuery(tenant);
            ComputingPartnerTranslationPM entityPM = computingPartnerTranslationQuery.GetSinglePM(id, tenant);

            return entityPM;
        }

        public List<ComputingPartnerTranslationPM> GetComputingPartnerTranslationsByPartnerAndTableId(string computingPartnerId, string objectTableId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("ComputingPartnerTable", "READ", tenant);

            computingPartnerTranslationQuery = new ComputingPartnerTranslationQuery(tenant);
            List<ComputingPartnerTranslationPM> myResult = computingPartnerTranslationQuery.GetTranslationsByPartnerAndTableId(computingPartnerId, objectTableId, tenant).ToList();


            return myResult;
        }

        public void InsertComputingPartnerTranslation(ComputingPartnerTranslationPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            //SecurityUtility.CheckContactFeature("ComputingPartner", "ComputingPartner.A.AllowTranslation", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            ComputingPartnerTranslationService service = new ComputingPartnerTranslationService(objectContext, entityPM.Tenant);
            service.Create(entityPM);

            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "ComputingPartnerTranslation");
        }

        public void UpdateComputingPartnerTranslation(ComputingPartnerTranslationPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            //SecurityUtility.CheckContactFeature("ComputingPartner", "ComputingPartner.A.AllowTranslation", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            ComputingPartnerTranslationService service = new ComputingPartnerTranslationService(objectContext, entityPM.Tenant);
            service.Update(entityPM);

            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "ComputingPartnerTranslation");
        }

        public void DeleteComputingPartnerTranslation(ComputingPartnerTranslationPM entityPM)
        {
            computingPartnerTranslationRepository = new ComputingPartnerTranslationRepository(entityPM.Tenant);

            ComputingPartnerTranslation entityPOCO = computingPartnerTranslationRepository.GetSingleComputingPartnerTranslation(entityPM.Id);

            computingPartnerTranslationRepository.Remove(entityPOCO);
            //computingPartnerTranslationRepository.SubmitChanges();
        }
    }
}