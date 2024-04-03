using System.Linq;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using System.Collections.Generic;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class ScreenSectionQuery
    {
        ScreenSectionRepository repository;

        public ScreenSectionQuery()
        {
            repository = new ScreenSectionRepository();
        }

        public ScreenSectionQuery(int tenant)
        {
            repository = new ScreenSectionRepository(tenant);
        }

        public ScreenSectionQuery(ScreenSectionRepository ScreenSectionRepository)
        {
            repository = ScreenSectionRepository;
        }


        public ScreenSectionPM GetSinglePM(string id , int tenant)
        {
            return (from a in repository.context.ScreenSections
                    where a.Id == id && a.Tenant == tenant
                    select new ScreenSectionPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        Name = a.Name,
                        ScreenCode = a.ScreenCode,
                        CreatedByUserId = a.CreatedByUserId,
                        Number = a.Number,
                        NumberOfRows = a.NumberOfRows,
                        Inactive = a.Inactive,
                        Type = a.Type,
                        RelatedScreenCode = a.RelatedScreenCode,

                    }).FirstOrDefault();
        }




        public IQueryable<ScreenSectionPM> GetByScreenCode( string screenCode,int tenant)
        {
            return (from a in repository.context.ScreenSections
                    where a.Tenant == tenant && a.ScreenCode == screenCode
                    select new ScreenSectionPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        Name = a.Name,
                        ScreenCode = a.ScreenCode,
                        CreatedByUserId = a.CreatedByUserId,
                        Number = a.Number,
                        NumberOfRows = a.NumberOfRows,
                        Inactive = a.Inactive,
                        Type = a.Type,
                        RelatedScreenCode = a.RelatedScreenCode,
                    });
        }


  
        public IQueryable<ScreenSectionList> GetIQueryableEntityList(IQueryable<ScreenSection> iQueryable)
        {
            IQueryable<ScreenSectionList> result = from a in iQueryable
                                                   where a.Inactive == false
                                              select new ScreenSectionList()
                                              {
                                                  Id = a.Id,
                                                  Tenant = a.Tenant,
                                                  Name = a.Name,
                                                  ScreenCode = a.ScreenCode,
                                                  CreatedByUserId = a.CreatedByUserId,
                                                  Number = a.Number,
                                                  NumberOfRows = a.NumberOfRows,
                                                  Inactive = a.Inactive,
                                                  Type = a.Type,
                                                  RelatedScreenCode = a.RelatedScreenCode,
                                              };

            return result;
        }

       

    }
}