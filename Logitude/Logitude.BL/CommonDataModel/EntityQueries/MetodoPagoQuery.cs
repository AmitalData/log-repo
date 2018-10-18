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
    public class MetodoPagoQuery
    {
        MetodoPagoRepository repository;

        public MetodoPagoQuery()
        {
            repository = new MetodoPagoRepository();
        }

        public MetodoPagoQuery(int tenant)
        {
            repository = new MetodoPagoRepository(tenant);
        }

        public MetodoPagoQuery(MetodoPagoRepository repository)
        {
            this.repository = repository;
        }

        public MetodoPagoPM GetSingleMetodoPagoPM(string code)
        {
            return (from a in repository.context.MetodoPagos
                    where a.Code == code
                    select new MetodoPagoPM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, }).FirstOrDefault();
        }

        public MetodoPagoPM GetSinglePM(string code, int tenant = 0)
        {
            return (from a in repository.context.MetodoPagos
                    where a.Code == code
                    select new MetodoPagoPM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, }).FirstOrDefault();
        }

        public IQueryable<MetodoPagoPM> GetMetodoPagoPMs()
        {
            return (from a in repository.context.MetodoPagos

                    select new MetodoPagoPM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, });
        }

        public IQueryable<MetodoPagoList> GetIQueryableEntityList(IQueryable<MetodoPago> iQueryable)
        {
            IQueryable<MetodoPagoList> result = from entity in iQueryable
                                             select new MetodoPagoList()
                                             {
                                                 Name = entity.Name,
                                                 Code = entity.Code,
                                                 SearchFields = entity.SearchFields,
                                             };
            return result;
        }
    }
}