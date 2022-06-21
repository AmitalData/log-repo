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


        public ScreenSectionPM GetSinglePM(string id, int tenant)
        {
            return (from a in repository.context.ScreenSections
                    where a.Id == id && a.Tenant == tenant
                    select new ScreenSectionPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        Name = a.Name,
                        ScreenCode = a.ScreenCode,
                        CreateByUserId = a.CreateByUserId,
                    }).FirstOrDefault();
        }

        public IQueryable<ScreenSectionPM> GetScreenSectionPMs(int tenant)
        {
            return (from a in repository.context.ScreenSections
                    where a.Tenant == tenant
                    select new ScreenSectionPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        Name = a.Name,
                        ScreenCode = a.ScreenCode,
                        CreateByUserId = a.CreateByUserId,
                    });
        }

        public IQueryable<ScreenSectionList> GetIQueryableEntityList(IQueryable<ScreenSection> iQueryable)
        {
            IQueryable<ScreenSectionList> result = from a in iQueryable
                                              select new ScreenSectionList()
                                              {
                                                  Id = a.Id,
                                                  Tenant = a.Tenant,
                                                  Name = a.Name,
                                                  ScreenCode = a.ScreenCode,
                                                  CreateByUserId = a.CreateByUserId,
                                              };

            return result;
        }

       

    }
}