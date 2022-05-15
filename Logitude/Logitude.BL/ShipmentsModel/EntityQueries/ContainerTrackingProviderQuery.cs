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

        public ContainerTrackingProviderPM GetSinglePM(string code)
        {
            if (string.IsNullOrEmpty(code))
                return null;

            ContainerTrackingProviderPM entity = (from a in repository.context.ContainerTrackingProviders where a.Code == code
                                                  select new ContainerTrackingProviderPM()
                                                  {
                                                      Code = a.Code,
                                                      Name = a.Name,
                                                      SearchFields = a.SearchFields,
                                                      APIKey = a.APIKey,
                                                      CallbackURL = a.CallbackURL,
                                                      ProviderURL = a.ProviderURL
                                                  }).FirstOrDefault();

            return entity;


        }

        public IQueryable<ContainerTrackingProviderList> GetIQueryableEntityList(IQueryable<ContainerTrackingProvider> iQueryable)
        {
            IQueryable<ContainerTrackingProviderList> result = (from a in iQueryable
                                                                select new ContainerTrackingProviderList()
                                                                {
                                                                    Code = a.Code,
                                                                    Name = a.Name,
                                                                    SearchFields = a.SearchFields,
                                                                    APIKey = a.APIKey,
                                                                    CallbackURL = a.CallbackURL,
                                                                    ProviderURL = a.ProviderURL
                                                                });
            return result;
        }


    }
}