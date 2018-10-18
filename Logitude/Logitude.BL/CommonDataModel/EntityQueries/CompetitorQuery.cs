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

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class CompetitorQuery
    {
        CompetitorRepository repository;

        public CompetitorQuery()
        {
               repository = new CompetitorRepository(); 
        }

        public CompetitorQuery(int tenant)
        {
            repository = new CompetitorRepository(tenant);
        }

        public CompetitorQuery(CompetitorRepository CompetitorRepository)
        {
            repository = CompetitorRepository;
        }

        public CompetitorPM GetSinglePM(string Id, int tenant)
        {
            CompetitorPM entityPM =

                (from a in repository.context.Competitors
                 where a.Tenant == tenant && a.Id == Id 
                 select new CompetitorPM()
                 {
                     Id = a.Id,
                     Tenant = a.Tenant,
                     Name = a.Name,
                     Website = a.Website,
                     Strengths = a.Strengths,
                     Weaknesses = a.Weaknesses,
                     Opportunity = a.Opportunity,
                     Threat = a.Threat,
                     AddressId = a.AddressId,
                     SearchFields = a.SearchFields, 
                     InActive = a.InActive,
                 }).FirstOrDefault();

            AddressRepository addressRepository = new AddressRepository(tenant);
            Address address = addressRepository.GetSingleAddress(entityPM.AddressId, tenant);
            if (address != null)
            {
                entityPM.Address1 = address.Address1;
                entityPM.Address2 = address.Address2;
                entityPM.City = address.City;
                entityPM.CountryId = address.CountryId;
                entityPM.ZipCode = address.ZipCode;
                entityPM.StateId = address.StateId;
                entityPM.PhoneNumber = address.PhoneNumber;
                entityPM.FaxNumber = address.FaxNumber;

                if (address.State != null)
                {
                    entityPM.StateName = address.State.EnglishName;
                }

                if (address.Country != null)
                {
                    entityPM.CountryCode = address.Country.Code;
                    entityPM.CountryName = address.Country.EnglishName;
                }
            }

            return entityPM;
        }

        public IQueryable<CompetitorList> GetIQueryableEntityList(IQueryable<Competitor> iQueryable)
        {
            IQueryable<CompetitorList> result =

                from a in iQueryable
                select new CompetitorList()
                {
                    Id = a.Id,
                    Tenant = a.Tenant,
                    Name = a.Name,
                    Website = a.Website,
                    Strengths = a.Strengths,
                    Weaknesses = a.Weaknesses,
                    Opportunity = a.Opportunity,
                    Threat = a.Threat,
                    AddressId = a.AddressId,
                    SearchFields = a.SearchFields,
                    InActive = a.InActive,
                };

            return result;
        }
    }
}