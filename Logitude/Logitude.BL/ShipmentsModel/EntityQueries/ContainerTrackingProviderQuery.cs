using System;
using System.Linq;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityLists;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class ContainerTrackingProviderQuery
    {
        ContainerTrackingProviderRepository repository;

        public ContainerTrackingProviderQuery(int tenant)
        {
            repository = new ContainerTrackingProviderRepository(tenant);
        }

        public ContainerTrackingProviderQuery(ContainerTrackingProviderRepository repository)
        {
            this.repository = repository;
        }
        
        public ContainerTrackingProviderPM GetSinglePM(string id, int tenant)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            ContainerTrackingProviderPM entity = (from a in repository.context.ContainerTrackingProviders
                                                  where a.Id == id && a.Tenant == tenant
                                                  select new ContainerTrackingProviderPM()
                                                  {
                                                      SourceCode = a.SourceCode,
                                                      Name = a.Name,
                                                      Id = a.Id,
                                                      Tenant = a.Tenant,
                                                      SearchFields = a.SearchFields,
                                                      APIKey = a.APIKey,
                                                      CallbackURL = a.CallbackURL,
                                                      ProviderURL = a.ProviderURL,
                                                      LogitudeToken = a.LogitudeToken
                                                  }).FirstOrDefault();

            return entity;


        }

        public IQueryable<ContainerTrackingProviderList> GetIQueryableEntityList(IQueryable<ContainerTrackingProvider> iQueryable)
        {
            IQueryable<ContainerTrackingProviderList> result = (from a in iQueryable
                                                                select new ContainerTrackingProviderList()
                                                                {
                                                                    SourceCode = a.SourceCode,
                                                                    Name = a.Name,
                                                                    Id = a.Id,
                                                                    Tenant = a.Tenant,
                                                                    SearchFields = a.SearchFields,
                                                                    APIKey = a.APIKey,
                                                                    CallbackURL = a.CallbackURL,
                                                                    ProviderURL = a.ProviderURL,
                                                                    LogitudeToken = a.LogitudeToken
                                                                });
            return result;
        }


    }
}