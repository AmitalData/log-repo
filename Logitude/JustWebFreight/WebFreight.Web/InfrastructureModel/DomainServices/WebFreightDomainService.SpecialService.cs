using System;
using System.Linq;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.InfrastructureModel.DomainServices
{
    public partial class WebFreightDomainService
    {
        public IQueryable<SpecialService> GetSpecialServices(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            specialServicesRepository = new SpecialServicesRepository(tenant);
            return specialServicesRepository.GetSpecialServicesByTenant(0);
        }

        public IQueryable<SpecialService> GetSpecialServicesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            specialServicesRepository = new SpecialServicesRepository(tenant);
            return specialServicesRepository.GetSpecialServicesByTenant(tenant);
        }

        public IQueryable<SpecialService> GetSpecialServicesSearch(string code, string name, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            specialServicesRepository = new SpecialServicesRepository(tenant);
            IQueryable<SpecialService> q = specialServicesRepository.GetSpecialServicesByCodeOrName(code, name, tenant).Where(d => d.Tenant == tenant);
            return q;
        }

        public void InsertSpecialService(SpecialService entity)
        {
            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(entity.Tenant);
            }
            specialServicesRepository = new SpecialServicesRepository(objectContext);
            bool exist = (from a in specialServicesRepository.GetSpecialServices()
                          where a.Code == entity.Code && a.Tenant == entity.Tenant
                          select a).Any();

            if (!exist)
            {
                specialServicesRepository.Add(entity);
            }
            else
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyExists", entity.Tenant);
                msg = msg.Replace("%Entity", "Special service");
                throw new Exception(msg);
            }
        }

        public void UpdateSpecialService(SpecialService currentEntity)
        {
            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(currentEntity.Tenant);
            }
            specialServicesRepository = new SpecialServicesRepository(objectContext);
            bool exist = (from a in specialServicesRepository.GetSpecialServices()
                          where a.Code == currentEntity.Code && a.Tenant == currentEntity.Tenant
                          select a).Any();

            if (!exist)
            {
                specialServicesRepository.Update(currentEntity);
            }
            else
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyExists", currentEntity.Tenant);
                msg = msg.Replace("%Entity", "Special service");
                throw new Exception(msg);
            }
        }

        public void DeleteSpecialService(SpecialService entity)
        {
            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(entity.Tenant);
            }
            specialServicesRepository = new SpecialServicesRepository(objectContext);
            specialServicesRepository.Remove(entity);
        }
    }
}