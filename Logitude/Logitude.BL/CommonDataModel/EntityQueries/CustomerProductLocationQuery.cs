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
    public class CustomerProductLocationQuery
    {
        CustomerProductLocationRepository repository;

        public CustomerProductLocationQuery()
        {
               repository = new CustomerProductLocationRepository(); 
        }

        public CustomerProductLocationQuery(int tenant)
        {
            repository = new CustomerProductLocationRepository(tenant);
        }

        public CustomerProductLocationQuery(CustomerProductLocationRepository CustomerProductLocationRepository)
        {
            repository = CustomerProductLocationRepository;
        }

        public CustomerProductLocationPM GetSinglePM(string customerId, string typeCode, string countryId, int tenant)
        {
            CustomerProductLocationPM instance = (from a in repository.context.CustomerProductLocations.Include("Country")
                                     where a.Tenant == tenant && a.CustomerId == customerId && a.ProductTypeCode == typeCode
                                     && a.CountryId == countryId
                                     select new CustomerProductLocationPM()
                                     {
                                         CustomerId = a.CustomerId,
                                         ProductTypeCode = a.ProductTypeCode,
                                         CountryId = a.CountryId,
                                         Tenant = a.Tenant,
                                         PotentialTEU = a.PotentialTEU,
                                         PotentialNumberOfShipments = a.PotentialNumberOfShipments,
                                         PotentialChargeableWeight = a.PotentialChargeableWeight,
                                         CommitmentTEU = a.CommitmentTEU,
                                         CommitmentNumberOfShipments = a.CommitmentNumberOfShipments,
                                         CommitmentChargeableWeight = a.CommitmentChargeableWeight,
                                         PotentialRevenue = a.PotentialRevenue,
                                         CommitmentRevenue = a.CommitmentRevenue,
                                         CountryCode = a.Country.Code,
                                         CountryName = a.Country.EnglishName,
                                     }).FirstOrDefault();

            CustomerProductLocationPM securedPm = new CustomerProductLocationPM();
            SecuredMapping.GetMappedPM(instance, securedPm, "CustomerProductLocation", tenant);

            return securedPm;
        }

        public IQueryable<CustomerProductLocationPM> GetCustomerProductLocationPMsByTenant(int tenant)
        {
            IQueryable<CustomerProductLocationPM> data =

                from a in repository.context.CustomerProductLocations.Include("Country")
                where a.Tenant == tenant
                select new CustomerProductLocationPM()
                {
                    CustomerId = a.CustomerId,
                    ProductTypeCode = a.ProductTypeCode,
                    CountryId = a.CountryId,
                    Tenant = a.Tenant,
                    PotentialTEU = a.PotentialTEU,
                    PotentialNumberOfShipments = a.PotentialNumberOfShipments,
                    PotentialChargeableWeight = a.PotentialChargeableWeight,
                    CommitmentTEU = a.CommitmentTEU,
                    CommitmentNumberOfShipments = a.CommitmentNumberOfShipments,
                    CommitmentChargeableWeight = a.CommitmentChargeableWeight,
                    PotentialRevenue = a.PotentialRevenue,
                    CommitmentRevenue = a.CommitmentRevenue,
                    CountryCode = a.Country.Code,
                    CountryName = a.Country.EnglishName,
                };

            return data;
        }

        public IQueryable<CustomerProductLocationPM> GetLocationPMsByProduct(string customerId, string productTypeCode, int tenant)
        {
            IQueryable<CustomerProductLocationPM> data =

                from a in repository.context.CustomerProductLocations.Include("Country")
                where a.Tenant == tenant
                && a.CustomerId == customerId
                && a.ProductTypeCode == productTypeCode
                select new CustomerProductLocationPM()
                {
                    CustomerId = a.CustomerId,
                    ProductTypeCode = a.ProductTypeCode,
                    CountryId = a.CountryId,
                    Tenant = a.Tenant,
                    PotentialTEU = a.PotentialTEU,
                    PotentialNumberOfShipments = a.PotentialNumberOfShipments,
                    PotentialChargeableWeight = a.PotentialChargeableWeight,
                    CommitmentTEU = a.CommitmentTEU,
                    CommitmentNumberOfShipments = a.CommitmentNumberOfShipments,
                    CommitmentChargeableWeight = a.CommitmentChargeableWeight,
                    PotentialRevenue = a.PotentialRevenue,
                    CommitmentRevenue = a.CommitmentRevenue,
                    CountryCode = a.Country.Code,
                    CountryName = a.Country.EnglishName,
                };

            return data;
        }


        public IQueryable<CustomerProductLocationList> GetIQueryableEntityList(IQueryable<CustomerProductLocation> iQueryable)
        {
            IQueryable<CustomerProductLocationList> result = from a in iQueryable
                                            select new CustomerProductLocationList()
                                            {
                                                CustomerId = a.CustomerId,
                                                ProductTypeCode = a.ProductTypeCode,
                                                CountryId = a.CountryId,
                                                Tenant = a.Tenant,
                                                PotentialTEU = a.PotentialTEU,
                                                PotentialNumberOfShipments = a.PotentialNumberOfShipments,
                                                PotentialChargeableWeight = a.PotentialChargeableWeight,
                                                CommitmentTEU = a.CommitmentTEU,
                                                CommitmentNumberOfShipments = a.CommitmentNumberOfShipments,
                                                CommitmentChargeableWeight = a.CommitmentChargeableWeight,
                                                PotentialRevenue = a.PotentialRevenue,
                                                CommitmentRevenue = a.CommitmentRevenue,
                                            };
            return result;
        }
    }
}
