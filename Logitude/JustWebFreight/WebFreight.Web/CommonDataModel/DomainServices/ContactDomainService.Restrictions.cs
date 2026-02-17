using System.Collections.Generic;
using System.Linq;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class ContactDomainService
    {
        public IQueryable<RestrictionPM> GetRestrictionsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            restrictionQuery = new RestrictionQuery(tenant);
            return restrictionQuery.GetRestrictionsByTenant(tenant);
        }

        //public void MapRestrictionRestrictionPM(RestrictionPM restrictionPm, Restriction restriction)
        //{
        //    restriction.ContactTenantId = restrictionPm.ContactTenantId;
        //    restriction.ObjectFieldId = restrictionPm.ObjectFieldId;
        //    restriction.ObjectTableId = restrictionPm.ObjectTableId;
        //    restriction.Tenant = restrictionPm.Tenant;
        //    restriction.Value = restrictionPm.Value;
        //}

        public List<RestrictionPM> GetRestrictionsForUser(string userId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
            }
            restrictionRepository = new RestrictionRepository(objectContext);
            restrictionQuery = new RestrictionQuery(restrictionRepository);

            ContactTenantRepository contactTenantsRepository = new ContactTenantRepository(objectContext);
            ContactTenantQuery contactTenantQuery = new ContactTenantQuery(contactTenantsRepository);
            ContactTenantPM contactTenant = contactTenantQuery.GetContactTenantForUser(userId, tenant);
            if (contactTenant != null)
            {
                List<RestrictionPM> restrictions = restrictionQuery.GetResitrictionsByContact(contactTenant.Id, tenant);
                return restrictions;
            }
            else
            {
                return null;
            }
        }

        public void InsertRestriction(RestrictionPM restriction)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(restriction.Tenant);
            }

            RestrictionService service = new RestrictionService(objectContext, restriction.Tenant);
            service.Create(restriction);
            TableLastUpdateClass.UpdateTableHistory(restriction.Tenant, "Restriction");

            //restrictionRepository = new RestrictionRepository(objectContext);
            //restriction.Id = IdCounter.GetNumber("Restriction", restriction.Tenant).ToString();
            //Restriction newRestriction = new Restriction();
            //newRestriction.Id = restriction.Id;
            //ContactTenantRepository contactTenantsRepository = new ContactTenantRepository(objectContext);
            //ContactTenantQuery contactTenantQuery = new ContactTenantQuery(contactTenantsRepository);
            //ContactTenantPM contactTenant = contactTenantQuery.GetContactTenantForUser(restriction.UserId, restriction.Tenant);
            //restriction.ContactTenantId = contactTenant.Id;
            //MapRestrictionRestrictionPM(restriction, newRestriction);
            //restrictionRepository.Add(newRestriction);


            //TableLastUpdateClass.UpdateTableHistory(restriction.Tenant, "Restriction");
        }

        public void UpdateRestriction(RestrictionPM currentRestriction)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currentRestriction.Tenant);
            }

            RestrictionService service = new RestrictionService(objectContext, currentRestriction.Tenant);
            service.Update(currentRestriction);


            //restrictionRepository = new RestrictionRepository(currentRestriction.Tenant);
            //Restriction entity = restrictionRepository.GetSingleRestriction(currentRestriction.Id);
            //MapRestrictionRestrictionPM(currentRestriction, entity);
            //restrictionRepository.Update(entity);

            TableLastUpdateClass.UpdateTableHistory(currentRestriction.Tenant, "Restriction");
        }

        public void DeleteRestriction(RestrictionPM restriction)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(restriction.Tenant);
            }
            restrictionRepository = new RestrictionRepository(objectContext);
            Restriction entity = restrictionRepository.GetSingleRestriction(restriction.Id);
            restrictionRepository.Remove(entity);
            TableLastUpdateClass.UpdateTableHistory(entity.Tenant, "Restriction");
        }
    }
}