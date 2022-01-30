using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.DataContracts;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Data.Helpers;
using Logitude.BL.CommonDataModel.BusinessUnitFilters;
using System.Data.Entity.Core.Objects;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityDws;
using Logitude.Server.Tools;
using System.Transactions;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System.IO;
using System.Xml.Serialization;
using Logitude.BL.CommonDataModel.CustomFilters;
using System.Reflection;
using Logitude.Accounting.Def.EntityQueryServicesExt;
using Microsoft.Practices.Unity;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class CustomerTeamQuery
    {
        CustomerTeamRepository repository;
        public CustomerTeamQuery()
        {
            repository = new CustomerTeamRepository();
        }
        public CustomerTeamQuery(int tenant)
        {
            repository = new CustomerTeamRepository(tenant);
        }
        public CustomerTeamQuery(CustomerTeamRepository repository)
        {
            this.repository = repository;
        }

        public CustomerTeamPM GetSinglePM(string id, int tenant)
        {
            CustomerTeamPM customerTeam = (from a in repository.context.CustomerTeams
                                           where a.Id == id && a.Tenant == tenant
                                           select new CustomerTeamPM()
                                           {
                                               Id = a.Id,
                                               Tenant = a.Tenant,
                                               Name = a.Name,
                                               LocalName = a.LocalName,
                                               InActive = a.InActive,
                                               CreateDate = a.CreateDate,
                                               UpdateDate = a.UpdateDate,
                                               CreatedByUserId = a.CreatedByUserId,
                                               UpdatedByUserId = a.UpdatedByUserId,
                                           }).FirstOrDefault();

            return customerTeam;
        }

        public IQueryable<CustomerTeamPM> GetCustomerTeamPMsByTenant(int tenant)
        {
            IQueryable<CustomerTeamPM> customerTeams = from a in repository.context.CustomerTeams
                                                       where a.Tenant == tenant
                                                       select new CustomerTeamPM()
                                                       {
                                                           Id = a.Id,
                                                           Tenant = a.Tenant,
                                                           Name = a.Name,
                                                           LocalName = a.LocalName,
                                                           InActive = a.InActive,
                                                           CreateDate = a.CreateDate,
                                                           UpdateDate = a.UpdateDate,
                                                           CreatedByUserId = a.CreatedByUserId,
                                                           UpdatedByUserId = a.UpdatedByUserId,
                                                       };

            return customerTeams;
        }


        public IQueryable<CustomerTeamList> GetIQueryableEntityList(IQueryable<CustomerTeam> iQueryable)
        {
            IQueryable<CustomerTeamList> result = from a in iQueryable
                                             select new CustomerTeamList()
                                             {
                                                 Id = a.Id,
                                                 Tenant = a.Tenant,
                                                 Name = a.Name,
                                                 LocalName = a.LocalName,
                                                 InActive = a.InActive,
                                                 CreateDate = a.CreateDate,
                                                 UpdateDate = a.UpdateDate,
                                                 CreatedByUserId = a.CreatedByUserId,
                                                 UpdatedByUserId = a.UpdatedByUserId,
                                             };
            return result;
        }
    }
}