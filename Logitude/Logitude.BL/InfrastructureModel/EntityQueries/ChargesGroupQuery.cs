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
    public class ChargesGroupQuery
    {
        ChargesGroupRepository repository;

        public ChargesGroupQuery()
        {
            repository = new ChargesGroupRepository(); 
        }

        public ChargesGroupQuery(int tenant)
        {
            repository = new ChargesGroupRepository(tenant);
       
        }

        public ChargesGroupQuery(ChargesGroupRepository chargesGroupRepository)
        {
            repository = chargesGroupRepository;
        }

        public ChargesGroupPM GetSingleChargesGroupPM(string id, int tenant)
        {
            return (from a in repository.context.ChargesGroups
                    where a.Id == id && a.Tenant == tenant
                    select new ChargesGroupPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        LocalName = a.LocalName,
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                        ViewOrder = a.ViewOrder,
                        QuoteGroupSectionID =a.QuoteGroupSectionID

                    }).FirstOrDefault();
        }

        public ChargesGroupPM GetSingleChargesGroupPMByCode(string code, int tenant)
        {
            return (from a in repository.context.ChargesGroups
                    where a.Code == code && a.Tenant == tenant
                    select new ChargesGroupPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        LocalName = a.LocalName,
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                        ViewOrder = a.ViewOrder,
                        QuoteGroupSectionID = a.QuoteGroupSectionID

                    }).FirstOrDefault();
        }



        public ChargesGroupPM GetSinglePM(string id , int tenant)
        {
            return (from a in repository.context.ChargesGroups
                    where a.Id == id && a.Tenant == tenant
                    select new ChargesGroupPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        LocalName = a.LocalName,
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                        ViewOrder = a.ViewOrder,
                        QuoteGroupSectionID = a.QuoteGroupSectionID

                    }).FirstOrDefault();
        }

        public IQueryable<ChargesGroupPM> GetChargesGroupPMs()
        {
            return from a in repository.context.ChargesGroups
                   select new ChargesGroupPM() { Id = a.Id , Tenant  = a.Tenant,Code = a.Code, Name = a.Name, SearchFields = a.SearchFields };
        }
        public IQueryable<ChargesGroupList> GetIQueryableEntityList(IQueryable<ChargesGroup> iQueryable)
        {
            IQueryable<ChargesGroupList> result = from entity in iQueryable
                                                  select new ChargesGroupList()
                                                  {
                                                      Id = entity.Id,
                                                      Tenant = entity.Tenant,
                                                      LocalName = entity.LocalName,
                                                      Code = entity.Code,
                                                      Name = entity.Name,
                                                      SearchFields = entity.SearchFields,
                                                      ViewOrder = entity.ViewOrder,
                                                      QuoteGroupSectionID = entity.QuoteGroupSectionID

                                                  };
            return result;
        }

        public IQueryable<ChargesGroupPM> GetChargesGroupPMsByTenant(int tenant)
        {

            IQueryable<ChargesGroupPM> charges = from a in repository.context.ChargesGroups
                                                where a.Tenant == tenant
                                                 select new ChargesGroupPM()
                                                {
                                                    Id = a.Id,
                                                    Tenant = a.Tenant,
                                                    LocalName = a.LocalName,
                                                    Code = a.Code,
                                                    Name = a.Name,
                                                    SearchFields = a.SearchFields,
                                                     ViewOrder = a.ViewOrder,
                                                     QuoteGroupSectionID = a.QuoteGroupSectionID

                                                 };
            return charges;
        }



        public IQueryable<ChargesGroupList> GetChargesGroupListsByTenant(int tenant)
        {

            IQueryable<ChargesGroupList> charges = from a in repository.context.ChargesGroups
                                                 where a.Tenant == tenant
                                                 select new ChargesGroupList()
                                                 {
                                                     Id = a.Id,
                                                     Tenant = a.Tenant,
                                                     LocalName = a.LocalName,
                                                     Code = a.Code,
                                                     Name = a.Name,
                                                     SearchFields = a.SearchFields,
                                                     ViewOrder = a.ViewOrder,
                                                     QuoteGroupSectionID = a.QuoteGroupSectionID

                                                 };
            return charges;
        }

    }
}