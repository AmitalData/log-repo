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
    public class TarrifFromToTypeQuery
    {
        TarrifFromToTypeRepository repository;

        public TarrifFromToTypeQuery()
        {
            repository = new TarrifFromToTypeRepository(); 
        }

        public TarrifFromToTypeQuery(int tenant)
        {
            repository = new TarrifFromToTypeRepository(tenant);
        }

        public TarrifFromToTypeQuery(TarrifFromToTypeRepository repository)
        {
            this.repository = repository;
        }

        public TarrifFromToTypePM GetSinglePM(string code)
        {
            return (from a in repository.context.TarrifFromToTypes
                    where a.Code == code
                    select new TarrifFromToTypePM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                    }).FirstOrDefault();
        }

        public TarrifFromToTypePM GetSingleTarrifFromToTypePM(string code)
        {
            return (from a in repository.context.TarrifFromToTypes
                    where a.Code == code
                    select new TarrifFromToTypePM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                    }).FirstOrDefault();
        }

        public IQueryable<TarrifFromToTypePM> GetTarrifFromToTypePMs()
        {
            return from a in repository.context.TarrifFromToTypes

                   select new TarrifFromToTypePM()
                   {
                       Code = a.Code,
                       Name = a.Name,
                   };
        }

        public IQueryable<TarrifFromToTypeList> GetIQueryableEntityList(IQueryable<TarrifFromToType> iQueryable)
        {
            IQueryable<TarrifFromToTypeList> result = from entity in iQueryable
                                                      select new TarrifFromToTypeList()
                                                      {
                                                          Name = entity.Name,
                                                          Code = entity.Code,
                                                      };
            return result;
        }
    }
}