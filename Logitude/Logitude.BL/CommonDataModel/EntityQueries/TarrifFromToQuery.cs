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
    public class TarrifFromToQuery
    {
        TarrifFromToRepository repository;


        public TarrifFromToQuery(int tenant)
        {
            repository = new TarrifFromToRepository(tenant);
        }

        public TarrifFromToQuery(TarrifFromToRepository repository)
        {
            this.repository = repository;
        }


        public TarrifFromToPM GetSinglePM(string id,int tenant)
        {
            return (from a in repository.context.TarrifFromToes.Include("Port").Include("Country")
                    where a.Id == id
                    select new TarrifFromToPM()
                    {
                        Id = a.Id,
                        CountryId = a.CountryId,
                        TarrifHeaderId = a.TarrifHeaderId,
                        Tenant = a.Tenant,
                        PortId = a.PortId,
                        TarrifFromToTypeCode = a.TarrifFromToTypeCode,
                        PortCode = a.Port == null ? null : a.Port.Code,
                        CountryCode = a.Country == null ? null : a.Country.Code,
                    }).FirstOrDefault();
        }

        public TarrifFromToPM GetSingleTarrifFromToPM(string id)
        {
            return (from a in repository.context.TarrifFromToes.Include("Port").Include("Country")
                    where a.Id == id
                    select new TarrifFromToPM()
                    {
                        Id = a.Id,
                        CountryId = a.CountryId,
                        TarrifHeaderId = a.TarrifHeaderId,
                        Tenant = a.Tenant,
                        PortId = a.PortId,
                        TarrifFromToTypeCode = a.TarrifFromToTypeCode,
                        PortCode = a.Port == null ? null : a.Port.Code,
                        CountryCode = a.Country == null ? null : a.Country.Code,
                    }).FirstOrDefault();
        }

        public IQueryable<TarrifFromToPM> GetTarrifFromToPMsByTenant(int tenant)
        {
            return from a in repository.context.TarrifFromToes.Include("Port").Include("Country")
                   where a.Tenant == tenant
                   select new TarrifFromToPM()
                   {
                       Id = a.Id,
                       CountryId = a.CountryId,
                       TarrifHeaderId = a.TarrifHeaderId,
                       Tenant = a.Tenant,
                       PortId = a.PortId,
                       TarrifFromToTypeCode = a.TarrifFromToTypeCode,
                       PortCode = a.Port == null ? null : a.Port.Code,
                       CountryCode = a.Country == null ? null : a.Country.Code,
                   };
        }

        public IQueryable<TarrifFromToPM> GetTarrifFromToByTarrifHeaderId(string headerId, int tenant)
        {
            return from a in repository.context.TarrifFromToes.Include("Port").Include("Country")
                   where a.Tenant == tenant
                   && a.TarrifHeaderId == headerId
                   select new TarrifFromToPM()
                   {
                       Id = a.Id,
                       CountryId = a.CountryId,
                       TarrifHeaderId = a.TarrifHeaderId,
                       Tenant = a.Tenant,
                       PortId = a.PortId,
                       TarrifFromToTypeCode = a.TarrifFromToTypeCode,
                       PortCode = a.Port == null ? null : a.Port.Code,
                       CountryCode = a.Country == null ? null : a.Country.Code,
                   };
        }



        public IQueryable<TarrifFromToList> GetIQueryableEntityList(IQueryable<TarrifFromTo> iQueryable)
        {
            IQueryable<TarrifFromToList> result = from a in iQueryable
                                                  select new TarrifFromToList()
                                                      {
                                                          Id = a.Id,
                                                          CountryId = a.CountryId,
                                                          TarrifHeaderId = a.TarrifHeaderId,
                                                          Tenant = a.Tenant,
                                                          PortId = a.PortId,
                                                          TarrifFromToTypeCode = a.TarrifFromToTypeCode,
                                                              
                                                      };
            return result;
        }
    }
}