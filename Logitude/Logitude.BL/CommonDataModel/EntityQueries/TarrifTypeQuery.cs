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
    public class TarrifTypeQuery
    {
        TarrifTypeRepository repository;

        public TarrifTypeQuery()
        {
            repository = new TarrifTypeRepository(); 
        }

        public TarrifTypeQuery(int tenant)
        {
            repository = new TarrifTypeRepository(tenant);
        }

        public TarrifTypeQuery(TarrifTypeRepository repository)
        {
            this.repository = repository;
        }

        public TarrifTypePM GetSingleTarrifTypePM(string code)
        {
            return (from a in repository.context.TarrifTypes
                    where a.Code == code
                    select new TarrifTypePM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                    }).FirstOrDefault();
        }

        public TarrifTypePM GetSinglePM(string code)
        {
            return (from a in repository.context.TarrifTypes
                    where a.Code == code
                    select new TarrifTypePM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                    }).FirstOrDefault();
        }

        public IQueryable<TarrifTypePM> GetTarrifTypePMs()
        {
            return from a in repository.context.TarrifTypes

                   select new TarrifTypePM()
                   {
                       Code=a.Code,
                       Name=a.Name,
                   };
        }

        public IQueryable<TarrifTypeList> GetIQueryableEntityList(IQueryable<TarrifType> iQueryable)
        {
            IQueryable<TarrifTypeList> result = from entity in iQueryable
                                                select new TarrifTypeList()
                                                {
                                                    Name = entity.Name,
                                                    Code = entity.Code,
                                                };
            return result;
        }
    }
}