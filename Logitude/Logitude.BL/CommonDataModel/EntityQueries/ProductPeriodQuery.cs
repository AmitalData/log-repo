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
    public class ProductPeriodQuery
    {
        ProductPeriodRepository repository;

        public ProductPeriodQuery()
        {
            repository = new ProductPeriodRepository(); 
        }

        public ProductPeriodQuery(int tenant)
        {
            repository = new ProductPeriodRepository(tenant);
        }

        public ProductPeriodQuery(ProductPeriodRepository repository)
        {
            this.repository = repository;
        }


        public ProductPeriodPM GetSinglePM(string code)
        {
            return (from a in repository.context.ProductPeriods
                    where a.Code == code
                    select new ProductPeriodPM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, }).FirstOrDefault();
        }

        public ProductPeriodPM GetSingleProductPeriodPM(string code)
        {
            return (from a in repository.context.ProductPeriods
                    where a.Code == code
                    select new ProductPeriodPM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, }).FirstOrDefault();
        }

        public ProductPeriodPM GetSinglePM(string code, int tenant)
        {
            return (from a in repository.context.ProductPeriods
                    where a.Code == code
                    select new ProductPeriodPM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, }).FirstOrDefault();
        }

        public IQueryable<ProductPeriodPM> GetProductPeriodPMs()
        {
            return (from a in repository.context.ProductPeriods

                    select new ProductPeriodPM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, });
        }

        public IQueryable<ProductPeriodList> GetIQueryableEntityList(IQueryable<ProductPeriod> iQueryable)
        {
            IQueryable<ProductPeriodList> result = from entity in iQueryable
                                             select new ProductPeriodList()
                                             {
                                                 Name = entity.Name,
                                                 Code = entity.Code,
                                                 SearchFields = entity.SearchFields,
                                             };
            return result;
        }
    }
}