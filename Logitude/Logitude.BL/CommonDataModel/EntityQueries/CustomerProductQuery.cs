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
    public class CustomerProductQuery
    {
        CustomerProductRepository repository;

        public CustomerProductQuery()
        {
               repository = new CustomerProductRepository(); 
        }

        public CustomerProductQuery(int tenant)
        {
            repository = new CustomerProductRepository(tenant);
        }

        public CustomerProductQuery(CustomerProductRepository CustomerProductRepository)
        {
            repository = CustomerProductRepository;
        }

        public CustomerProductPM GetSinglePM(string customerId, string productTypeCode, int tenant)
        {
            CustomerProductPM entity =

                (from a in repository.context.CustomerProducts.Include("Customer").Include("ProductType")
                 where a.Tenant == tenant && a.CustomerId == customerId && a.ProductTypeCode == productTypeCode
                 select new CustomerProductPM()
                 {
                     CustomerId = a.CustomerId,
                     ProductTypeCode = a.ProductTypeCode,
                     Tenant = a.Tenant,
                     Notes = a.Notes,
                     PotentialChargeableWeight = a.PotentialChargeableWeight,
                     CommitmentChargeableWeight = a.CommitmentChargeableWeight,
                     PotentialNumberOfShipments = a.PotentialNumberOfShipments,
                     CommitmentNumberOfShipments = a.CommitmentNumberOfShipments,
                     PotentialTEU = a.PotentialTEU,
                     CommitmentTEU = a.CommitmentTEU,
                     CustomerName = a.Customer != null ? a.Customer.Card.EnglishName : null,
                     ProductTypeName = a.ProductType != null ? a.ProductType.Name : null,
                     PotentialRevenue = a.PotentialRevenue,
                     CommitmentRevenue = a.CommitmentRevenue,
                     LastShipmentDate = a.LastShipmentDate,
                     PrepaidCollectId = a.PrepaidCollectId,
                     NotesRightToLeft = a.NotesRightToLeft,

                 }).FirstOrDefault();

            return entity;
        }

        public List<CustomerProductPM> GetCustomerProductPMsByCustomerId(string customerId, int tenant)
        {
            List<CustomerProductPM> result =

                (from a in repository.context.CustomerProducts.Include("ProductType").Include("Customer").Include("PrepaidCollect")
                 where a.Tenant == tenant && a.CustomerId == customerId
                 select new CustomerProductPM()
                 {
                     CustomerId = a.CustomerId,
                     ProductTypeCode = a.ProductTypeCode,
                     Tenant = a.Tenant,
                     Notes = a.Notes,
                     PotentialChargeableWeight = a.PotentialChargeableWeight,
                     CommitmentChargeableWeight = a.CommitmentChargeableWeight,
                     PotentialNumberOfShipments = a.PotentialNumberOfShipments,
                     CommitmentNumberOfShipments = a.CommitmentNumberOfShipments,
                     PotentialTEU = a.PotentialTEU,
                     CommitmentTEU = a.CommitmentTEU,
                     CustomerName = a.Customer != null ? a.Customer.Card.EnglishName : null,
                     ProductTypeName = a.ProductType != null ? a.ProductType.Name : null,
                     PotentialRevenue = a.PotentialRevenue,
                     CommitmentRevenue = a.CommitmentRevenue,
                     LastShipmentDate = a.LastShipmentDate,
                     PrepaidCollectId = a.PrepaidCollectId,
                     PrepaidCollectName = a.PrepaidCollect != null ? a.PrepaidCollect.Name : null,
                     NotesRightToLeft = a.NotesRightToLeft,
                 }).ToList();

            CustomerProductLocationQuery locationQuery = new CustomerProductLocationQuery(tenant);

            foreach (CustomerProductPM item in result)
            {
                item.ProductLocations = locationQuery.GetLocationPMsByProduct(item.CustomerId, item.ProductTypeCode, tenant).ToList();
            }

            return result;
        }

        public IQueryable<CustomerProductList> GetIQueryableEntityList(IQueryable<CustomerProduct> iQueryable)
        {
            IQueryable<CustomerProductList> result =

                from a in iQueryable.Include("ProductType").Include("Customer")
                select new CustomerProductList()
                {
                    CustomerId = a.CustomerId,
                    ProductTypeCode = a.ProductTypeCode,
                    Tenant = a.Tenant,
                    Notes = a.Notes,
                    PotentialChargeableWeight = a.PotentialChargeableWeight,
                    CommitmentChargeableWeight = a.CommitmentChargeableWeight,
                    PotentialNumberOfShipments = a.PotentialNumberOfShipments,
                    CommitmentNumberOfShipments = a.CommitmentNumberOfShipments,
                    PotentialTEU = a.PotentialTEU,
                    CommitmentTEU = a.CommitmentTEU,
                    CustomerName = a.Customer != null ? a.Customer.Card.EnglishName : null,
                    ProductTypeName = a.ProductType != null ? a.ProductType.Name : null,
                    PotentialRevenue = a.PotentialRevenue,
                    CommitmentRevenue = a.CommitmentRevenue,
                    LastShipmentDate = a.LastShipmentDate,
                    NotesRightToLeft = a.NotesRightToLeft,
                };

            return result;
        }
    }
}