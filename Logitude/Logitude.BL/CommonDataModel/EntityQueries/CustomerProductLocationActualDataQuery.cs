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
    public class CustomerProductLocationActualDataQuery
    {
        CustomerProductLocationActualDataRepository repository;



        public CustomerProductLocationActualDataQuery(int tenant)
        {
            repository = new CustomerProductLocationActualDataRepository(tenant);
        }

        public CustomerProductLocationActualDataQuery(CustomerProductLocationActualDataRepository CustomerProductLocationActualDataRepository)
        {
            repository = CustomerProductLocationActualDataRepository;
        }

        public CustomerProductLocationActualDataPM GetSinglePM(string customerId, string typeCode, string countryId, int month, int year, int tenant)
        {
            CustomerProductLocationActualDataPM instance = (from a in repository.context.CustomerProductLocationActualDatas.Include("Country")
                                     where a.Tenant == tenant && a.CustomerId == customerId && a.ProductTypeCode == typeCode
                                     && a.Month == month && a.Year == year && a.CountryId == countryId
                                     select new CustomerProductLocationActualDataPM()
                                     {
                                         CustomerId = a.CustomerId,
                                         ProductTypeCode = a.ProductTypeCode,
                                         Month = a.Month,
                                         Year = a.Year,
                                         CountryId = a.CountryId,
                                         Tenant = a.Tenant,
                                         TEU = a.TEU,
                                         ChargeableWeight = a.ChargeableWeight,
                                         NumberOfShipments = a.NumberOfShipments,
                                         Revenue = a.Revenue,
                                         CountryCode = a.Country.Code,
                                         CountryName = a.Country.EnglishName,
                                     }).FirstOrDefault();

            CustomerProductLocationActualDataPM securedPm = new CustomerProductLocationActualDataPM();
            SecuredMapping.GetMappedPM(instance, securedPm, "CustomerProductLocationActualData", tenant);

            return securedPm;
        }

        public IQueryable<CustomerProductLocationActualDataPM> GetCustomerProductLocationActualDataPMsByTenant(int tenant)
        {
            IQueryable<CustomerProductLocationActualDataPM> data =

                from a in repository.context.CustomerProductLocationActualDatas.Include("Country")
                where a.Tenant == tenant
                select new CustomerProductLocationActualDataPM()
                {
                    CustomerId = a.CustomerId,
                    ProductTypeCode = a.ProductTypeCode,
                    Month = a.Month,
                    Year = a.Year,
                    CountryId = a.CountryId,
                    Tenant = a.Tenant,
                    TEU = a.TEU,
                    ChargeableWeight = a.ChargeableWeight,
                    NumberOfShipments = a.NumberOfShipments,
                    Revenue = a.Revenue,
                    CountryCode = a.Country.Code,
                    CountryName = a.Country.EnglishName,
                };

            return data;
        }

        public IQueryable<CustomerProductLocationActualDataPM> GetLocationPMsByProduct(string customerId, string productTypeCode, int year, int month, int tenant)
        {
            IQueryable<CustomerProductLocationActualDataPM> data =

                from a in repository.context.CustomerProductLocationActualDatas.Include("Country")
                where a.Tenant == tenant
                && a.Year == year
                && a.Month == month
                && a.CustomerId == customerId
                && a.ProductTypeCode == productTypeCode
                select new CustomerProductLocationActualDataPM()
                {
                    CustomerId = a.CustomerId,
                    ProductTypeCode = a.ProductTypeCode,
                    CountryId = a.CountryId,
                    Month = a.Month,
                    Year = a.Year,
                    Tenant = a.Tenant,
                    TEU = a.TEU,
                    NumberOfShipments = a.NumberOfShipments,
                    ChargeableWeight = a.ChargeableWeight,
                    Revenue = a.Revenue,
                    CountryCode = a.Country.Code,
                    CountryName = a.Country.EnglishName,
                };

            return data;
        }


        public IQueryable<CustomerProductLocationActualDataPM> GetCustomerProductLocationPMs(string customerId, string productTypeCode, int tenant)
        {
            IQueryable<CustomerProductLocationActualDataPM> data =

                from a in repository.context.CustomerProductLocationActualDatas.Include("Country")
                where a.Tenant == tenant
                && a.CustomerId == customerId
                && a.ProductTypeCode == productTypeCode
                select new CustomerProductLocationActualDataPM()
                {
                    CustomerId = a.CustomerId,
                    ProductTypeCode = a.ProductTypeCode,
                    CountryId = a.CountryId,
                    Month = a.Month,
                    Year = a.Year,
                    Tenant = a.Tenant,
                    TEU = a.TEU,
                    NumberOfShipments = a.NumberOfShipments,
                    ChargeableWeight = a.ChargeableWeight,
                    Revenue = a.Revenue, 
                    CountryCode = a.Country.Code,
                    CountryName = a.Country.EnglishName,
                };

            return data;
        }

        public IQueryable<CustomerProductLocationActualDataList> GetIQueryableEntityList(IQueryable<CustomerProductLocationActualData> iQueryable)
        {
            IQueryable<CustomerProductLocationActualDataList> result = from a in iQueryable
                                            select new CustomerProductLocationActualDataList()
                                            {
                                                CustomerId = a.CustomerId,
                                                ProductTypeCode = a.ProductTypeCode,
                                                Month = a.Month,
                                                Year = a.Year,
                                                CountryId = a.CountryId,
                                                Tenant = a.Tenant,
                                                TEU = a.TEU,
                                                ChargeableWeight = a.ChargeableWeight,
                                                NumberOfShipments = a.NumberOfShipments,
                                                Revenue = a.Revenue,
                                            };
            return result;
        }


    }
}
