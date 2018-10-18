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
    public class TarrifStepQuery
    {
        TarrifStepRepository repository;

        public TarrifStepQuery()
        {
            repository = new TarrifStepRepository(); 
        }

        public TarrifStepQuery(int tenant)
        {
            repository = new TarrifStepRepository(tenant);
        }

        public TarrifStepQuery(TarrifStepRepository repository)
        {
            this.repository = repository;
        }

        public TarrifStepPM GetSinglePM(string id,int tenant)
        {
            return (from a in repository.context.TarrifSteps
                    where a.Id == id
                    select new TarrifStepPM()
                    {
                        Id = a.Id,
                        MaxPrice = a.MaxPrice,
                        MinPrice = a.MinPrice,
                        Step = a.Step,
                        TarrifHeaderId = a.TarrifHeaderId,
                        Tenant = a.Tenant,
                        UnitPrice = a.UnitPrice,
                    }).FirstOrDefault();
        }

        public TarrifStepPM GetSingleTarrifStepPM(string id)
        {
            return (from a in repository.context.TarrifSteps
                    where a.Id == id
                    select new TarrifStepPM()
                    {
                        Id = a.Id,
                        MaxPrice = a.MaxPrice,
                        MinPrice = a.MinPrice,
                        Step = a.Step,
                        TarrifHeaderId = a.TarrifHeaderId,
                        Tenant = a.Tenant,
                        UnitPrice = a.UnitPrice,
                    }).FirstOrDefault();
        }

        public IQueryable<TarrifStepPM> GetTarrifStepPMsByTenant(int tenant)
        {
            return from a in repository.context.TarrifSteps
                   where a.Tenant == tenant
                   select new TarrifStepPM()
                   {
                       Id = a.Id,
                       MaxPrice = a.MaxPrice,
                       MinPrice = a.MinPrice,
                       Step = a.Step,
                       TarrifHeaderId = a.TarrifHeaderId,
                       Tenant = a.Tenant,
                       UnitPrice = a.UnitPrice,
                   };
        }

        public IQueryable<TarrifStepList> GetIQueryableEntityList(IQueryable<TarrifStep> iQueryable)
        {
            IQueryable<TarrifStepList> result = from a in iQueryable
                                                select new TarrifStepList()
                                                {
                                                    Id = a.Id,
                                                    MaxPrice = a.MaxPrice,
                                                    MinPrice = a.MinPrice,
                                                    Step = a.Step,
                                                    TarrifHeaderId = a.TarrifHeaderId,
                                                    Tenant = a.Tenant,
                                                    UnitPrice = a.UnitPrice,
                                                };
            return result;
        }
    }
}