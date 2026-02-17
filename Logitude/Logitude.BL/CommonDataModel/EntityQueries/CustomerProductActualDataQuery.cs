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
    public class CustomerProductActualDataQuery
    {
        CustomerProductActualDataRepository repository;

        public CustomerProductActualDataQuery()
        {
               repository = new CustomerProductActualDataRepository(); 
        }

        public CustomerProductActualDataQuery(int tenant)
        {
            repository = new CustomerProductActualDataRepository(tenant);
        }

        public CustomerProductActualDataQuery(CustomerProductActualDataRepository CustomerProductActualDataRepository)
        {
            repository = CustomerProductActualDataRepository;
        }

        public CustomerProductActualDataPM GetSinglePM(string customerId, string typeCode, int month, int year, int tenant)
        {
            CustomerProductActualDataPM instance = (from a in repository.context.CustomerProductActualDatas
                                     where a.Tenant == tenant && a.CustomerId == customerId && a.ProductTypeCode == typeCode
                                     && a.Month == month && a.Year == year
                                     select new CustomerProductActualDataPM()
                                     {
                                         CustomerId = a.CustomerId,
                                         ProductTypeCode = a.ProductTypeCode,
                                         Month = a.Month,
                                         Year = a.Year,
                                         Tenant = a.Tenant,
                                         TEU = a.TEU,
                                         ChargeableWeight = a.ChargeableWeight,
                                         NumberOfShipments = a.NumberOfShipments,
                                         Revenue = a.Revenue,
                                     }).FirstOrDefault();

            instance.MonthCode = String.Format("{0:MMM}", new DateTime(instance.Year, instance.Month, 1));

            CustomerProductActualDataPM securedPm = new CustomerProductActualDataPM();
            SecuredMapping.GetMappedPM(instance, securedPm, "CustomerProductActualData", tenant);

            return securedPm;
        }

        public IQueryable<CustomerProductActualDataPM> GetCustomerProductActualDataPMsByTenant(int tenant)
        {
            IQueryable<CustomerProductActualDataPM> data = from a in repository.context.CustomerProductActualDatas
                                            where a.Tenant == tenant
                                            select new CustomerProductActualDataPM()
                                            {
                                                CustomerId = a.CustomerId,
                                                ProductTypeCode = a.ProductTypeCode,
                                                Month = a.Month,
                                                Year = a.Year,
                                                Tenant = a.Tenant,
                                                TEU = a.TEU,
                                                ChargeableWeight = a.ChargeableWeight,
                                                NumberOfShipments = a.NumberOfShipments,
                                                Revenue = a.Revenue,
                                            };

            foreach (CustomerProductActualDataPM item in data)
            {
                item.MonthCode = String.Format("{0:MMM}", new DateTime(item.Year, item.Month, 1));
            }

            return data;
        }

        public List<CustomerProductActualDataPM> GetCustomerActualData(string customerId, int year, int month, int tenant)
        {
            List<CustomerProductActualDataPM> data =

                (from a in repository.context.CustomerProductActualDatas
                 where a.Tenant == tenant
                 && a.CustomerId == customerId
                 && a.Year == year
                 && a.Month == month
                 select new CustomerProductActualDataPM()
                 {
                     CustomerId = a.CustomerId,
                     ProductTypeCode = a.ProductTypeCode,
                     Month = a.Month,
                     Year = a.Year,
                     Tenant = a.Tenant,
                     TEU = a.TEU,
                     ChargeableWeight = a.ChargeableWeight,
                     NumberOfShipments = a.NumberOfShipments,
                     Revenue = a.Revenue,
                 }).ToList();

            CustomerProductLocationActualDataQuery locationQuery = new CustomerProductLocationActualDataQuery(tenant);

            foreach (CustomerProductActualDataPM item in data)
            {
                item.MonthCode = String.Format("{0:MMM}", new DateTime(item.Year, item.Month, 1));
                item.ProductLocations = locationQuery.GetLocationPMsByProduct(item.CustomerId, item.ProductTypeCode, year, month, tenant).ToList();
            }

            return data;
        }

        public List<CustomerProductActualDataPM> GetCustomerActualData(string customerId, string productTypeCode, int tenant)
        {
            List<CustomerProductActualDataPM> data =

                (from a in repository.context.CustomerProductActualDatas
                 where a.Tenant == tenant
                 && a.CustomerId == customerId
                 && a.ProductTypeCode == productTypeCode
                 select new CustomerProductActualDataPM()
                 {
                     CustomerId = a.CustomerId,
                     ProductTypeCode = a.ProductTypeCode,
                     Month = a.Month,
                     Year = a.Year,
                     Tenant = a.Tenant,
                     TEU = a.TEU,
                     ChargeableWeight = a.ChargeableWeight,
                     NumberOfShipments = a.NumberOfShipments,
                     Revenue = a.Revenue,
                 }).ToList();

            CustomerProductLocationActualDataQuery locationQuery = new CustomerProductLocationActualDataQuery(tenant);

            foreach (CustomerProductActualDataPM item in data)
            {
                item.MonthCode = String.Format("{0:MMM}", new DateTime(item.Year, item.Month, 1));
                item.ProductLocations = locationQuery.GetCustomerProductLocationPMs(item.CustomerId, item.ProductTypeCode, tenant).ToList();
            }

            return data;
        }

        public IQueryable<CustomerProductActualDataList> GetIQueryableEntityList(IQueryable<CustomerProductActualData> iQueryable)
        {
            IQueryable<CustomerProductActualDataList> result = from a in iQueryable
                                            select new CustomerProductActualDataList()
                                            {
                                                CustomerId = a.CustomerId,
                                                ProductTypeCode = a.ProductTypeCode,
                                                Month = a.Month,
                                                Year = a.Year,
                                                Tenant = a.Tenant,
                                                TEU = a.TEU,
                                                ChargeableWeight = a.ChargeableWeight,
                                                NumberOfShipments = a.NumberOfShipments,
                                                Revenue = a.Revenue,
                                            };

            foreach (CustomerProductActualDataList item in result)
            {
                item.MonthCode = String.Format("{0:MMM}", new DateTime(item.Year, item.Month, 1));
            }

            return result;
        }



    }
}
