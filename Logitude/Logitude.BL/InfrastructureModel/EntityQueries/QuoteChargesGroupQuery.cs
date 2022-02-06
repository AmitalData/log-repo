using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class QuoteChargesGroupQuery
    {
        QuoteChargesGroupRepository repository;

        public QuoteChargesGroupQuery()
        {
            repository = new QuoteChargesGroupRepository(); 
        }

        public QuoteChargesGroupQuery(int tenant)
        {
            repository = new QuoteChargesGroupRepository(tenant);
       
        }

        public QuoteChargesGroupQuery(QuoteChargesGroupRepository chargesGroupRepository)
        {
            repository = chargesGroupRepository;
        }

        public QuoteChargesGroupPM GetSingleQuoteChargesGroupPM(string id, int tenant)
        {
            return (from a in repository.context.QuoteChargesGroups
                    where a.Id == id && a.Tenant == tenant
                    select new QuoteChargesGroupPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        LocalName = a.LocalName,
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                        ViewOrder = a.ViewOrder,

                    }).FirstOrDefault();
        }

        public QuoteChargesGroupPM GetSingleQuoteChargesGroupPMByCode(string code, int tenant)
        {
            return (from a in repository.context.QuoteChargesGroups
                    where a.Code == code && a.Tenant == tenant
                    select new QuoteChargesGroupPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        LocalName = a.LocalName,
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                        ViewOrder = a.ViewOrder,
                    }).FirstOrDefault();
        }



        public QuoteChargesGroupPM GetSinglePM(string id , int tenant)
        {
            return (from a in repository.context.QuoteChargesGroups
                    where a.Id == id && a.Tenant == tenant
                    select new QuoteChargesGroupPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        LocalName = a.LocalName,
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                        ViewOrder = a.ViewOrder,
                    }).FirstOrDefault();
        }

        public IQueryable<QuoteChargesGroupPM> GetQuoteChargesGroupPMs()
        {
            return from a in repository.context.QuoteChargesGroups
                   select new QuoteChargesGroupPM() { Id = a.Id , Tenant  = a.Tenant,Code = a.Code, Name = a.Name, SearchFields = a.SearchFields };
        }
        public IQueryable<QuoteChargesGroupList> GetIQueryableEntityList(IQueryable<QuoteChargesGroup> iQueryable)
        {
            IQueryable<QuoteChargesGroupList> result = from entity in iQueryable
                                                  select new QuoteChargesGroupList()
                                                  {
                                                      Id = entity.Id,
                                                      Tenant = entity.Tenant,
                                                      LocalName = entity.LocalName,
                                                      Code = entity.Code,
                                                      Name = entity.Name,
                                                      SearchFields = entity.SearchFields,
                                                      ViewOrder = entity.ViewOrder,
                                                  };
            return result;
        }

        public IQueryable<QuoteChargesGroupPM> GetQuoteChargesGroupPMsByTenant(int tenant)
        {

            IQueryable<QuoteChargesGroupPM> charges = from a in repository.context.QuoteChargesGroups
                                                where a.Tenant == tenant
                                                 select new QuoteChargesGroupPM()
                                                {
                                                    Id = a.Id,
                                                    Tenant = a.Tenant,
                                                    LocalName = a.LocalName,
                                                    Code = a.Code,
                                                    Name = a.Name,
                                                    SearchFields = a.SearchFields,
                                                     ViewOrder = a.ViewOrder,
                                                 };
            return charges;
        }



        public IQueryable<QuoteChargesGroupList> GetQuoteChargesGroupListsByTenant(int tenant)
        {

            IQueryable<QuoteChargesGroupList> charges = from a in repository.context.QuoteChargesGroups
                                                 where a.Tenant == tenant
                                                 select new QuoteChargesGroupList()
                                                 {
                                                     Id = a.Id,
                                                     Tenant = a.Tenant,
                                                     LocalName = a.LocalName,
                                                     Code = a.Code,
                                                     Name = a.Name,
                                                     SearchFields = a.SearchFields,
                                                     ViewOrder = a.ViewOrder,
                                                 };
            return charges;
        }

    }
}