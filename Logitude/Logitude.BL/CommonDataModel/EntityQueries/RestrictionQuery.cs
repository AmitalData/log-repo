using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class RestrictionQuery
    {
        RestrictionRepository repository;



        public RestrictionQuery(int tenant)
        {
            repository = new RestrictionRepository(tenant);
        }

        public RestrictionQuery(RestrictionRepository repository)
        {
            this.repository = repository;
        }

        public IQueryable<RestrictionPM> GetRestrictionsByTenant(int tenant)
        {
            return (from a in repository.context.Restrictions.Include("ObjectField")
                    where a.Tenant == tenant
                    select new RestrictionPM()
                    {
                        ContactTenantId = a.ContactTenantId,
                        Id = a.Id,
                        ObjectFieldId = a.ObjectFieldId,
                        ObjectTableId = a.ObjectTableId,
                        Tenant = a.Tenant,
                        Value = a.Value,
                        ObjectFieldName = a.ObjectField.FieldName,
                        ObjectFieldCode = a.ObjectFieldCode,
                    });
        }

        public IQueryable<RestrictionPM> GetResitrictionsByObjectTableAndContact(string contactTenantId, string objectTableId, int tenant)
        {
            return (from a in repository.context.Restrictions.Include("ObjectField")
                    where a.Tenant == tenant && a.ContactTenantId == contactTenantId && a.ObjectTableId == objectTableId
                    select new RestrictionPM()
                    {
                        ContactTenantId = a.ContactTenantId,
                        Id = a.Id,
                        ObjectFieldId = a.ObjectFieldId,
                        ObjectTableId = a.ObjectTableId,
                        Tenant = a.Tenant,
                        Value = a.Value,
                        ObjectFieldName = a.ObjectField.FieldName,
                        ObjectFieldCode = a.ObjectFieldCode,
                    });
        }

        public List<RestrictionPM> GetResitrictionsByContact(string contactTenantId, int tenant)
        {
            return (from a in repository.context.Restrictions
                    where a.Tenant == tenant && a.ContactTenantId == contactTenantId
                    select new RestrictionPM()
                    {
                        ContactTenantId = a.ContactTenantId,
                        Id = a.Id,
                        ObjectFieldId = a.ObjectFieldId,
                        ObjectTableId = a.ObjectTableId,
                        Tenant = a.Tenant,
                        Value = a.Value,
                        ObjectFieldCode = a.ObjectFieldCode,
                    }).ToList();
        }
    }
}