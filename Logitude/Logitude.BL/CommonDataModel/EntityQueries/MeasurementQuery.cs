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
    public class MeasurementQuery
    {
        MeasurementRepository repository;



        public MeasurementQuery(int tenant)
        {
            repository = new MeasurementRepository(tenant);
        }

        public MeasurementQuery(MeasurementRepository repository)
        {
            this.repository = repository;
        }

        public MeasurementPM GetSingleMeasurementPM(string id)
        {
            return (from a in repository.context.Measurements
                    where a.Id == id
                    select new MeasurementPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        InActive = a.InActive,
                        IsContainer = a.IsContainer,
                        IsContainerMeasurement = a.IsContainerMeasurement,
                        Code = a.Code,
                        Name = a.Name,
                        ShortName = a.ShortName,
                        SearchFields = a.SearchFields,
                        LocalName = a.LocalName,                        
                    }).FirstOrDefault();
        }

        public MeasurementPM GetSinglePM(string id, int tenant)
        {
            MeasurementPM entity = null;
            string entityName = "MeasurementPM" + id + tenant;
            if (HttpContext.Current != null)
            {
                if (CacheManager.CacheWrapper.Get(entityName) == null)
                {

                    var systems = (from a in repository.context.Measurements
                                   where a.Id == id
                                   select new MeasurementPM()
                                   {
                                       Id = a.Id,
                                       Tenant = a.Tenant,
                                       InActive = a.InActive,
                                       IsContainer = a.IsContainer,
                                       IsContainerMeasurement = a.IsContainerMeasurement,
                                       Code = a.Code,
                                       Name = a.Name,
                                       ShortName = a.ShortName,
                                       SearchFields = a.SearchFields,
                                       LocalName = a.LocalName,
                                   });

                    foreach (var c in systems)
                    {
                        string cname = "MeasurementPM" + c.Id + c.Tenant;

                        if (CacheManager.CacheWrapper.Get(cname) == null)
                        {
                            CacheManager.CacheWrapper.Insert(cname, c, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                        }
                    }

                    entity = (MeasurementPM)CacheManager.CacheWrapper.Get(entityName);

                }
                else
                {
                    entity = (MeasurementPM)CacheManager.CacheWrapper.Get(entityName);
                }
            }
            else
            {
                entity = (from a in repository.context.Measurements
                          where a.Id == id
                          select new MeasurementPM()
                          {
                              Id = a.Id,
                              Tenant = a.Tenant,
                              InActive = a.InActive,
                              IsContainer = a.IsContainer,
                              IsContainerMeasurement = a.IsContainerMeasurement,
                              Code = a.Code,
                              Name = a.Name,
                              ShortName = a.ShortName,
                              SearchFields = a.SearchFields,
                              LocalName = a.LocalName,
                          }).FirstOrDefault();
            }

            return entity;
        }

        public IQueryable<MeasurementPM> GetUnitOfMeasurementPMs(int tenant)
        {
            return from a in repository.context.Measurements
                   where a.Tenant == tenant
                   select new MeasurementPM()
                   {
                       Id = a.Id,
                       Tenant = a.Tenant,
                       InActive = a.InActive,
                       IsContainer = a.IsContainer,
                       IsContainerMeasurement = a.IsContainerMeasurement,
                       ShortName = a.ShortName,
                       Name = a.Name,
                       Code = a.Code,
                       SearchFields = a.SearchFields,
                       LocalName = a.LocalName,
                   };
        }

        public IQueryable<MeasurementPM> GetMeasurementPMsByTenant(int tenant)
        {
            return from a in repository.context.Measurements
                   where a.Tenant == tenant
                   select new MeasurementPM()
                   {
                       Id = a.Id,
                       Tenant = a.Tenant,
                       InActive = a.InActive,
                       IsContainer = a.IsContainer,
                       IsContainerMeasurement = a.IsContainerMeasurement,
                       ShortName = a.ShortName,
                       Name = a.Name,
                       Code = a.Code,
                       SearchFields = a.SearchFields,
                       LocalName = a.LocalName,
                   };
        }


        public IQueryable<MeasurementList> GetIQueryableEntityList(IQueryable<Measurement> iQueryable)
        {
            IQueryable<MeasurementList> result = from a in iQueryable
                                                 select new MeasurementList()
                                                 {
                                                     Id = a.Id,
                                                     InActive = a.InActive,
                                                     IsContainer = a.IsContainer,
                                                     IsContainerMeasurement = a.IsContainerMeasurement,
                                                     Tenant = a.Tenant,
                                                     Code = a.Code,
                                                     Name = a.Name,
                                                     ShortName = a.ShortName,
                                                     SearchFields = a.SearchFields,
                                                     LocalName = a.LocalName,  
                                                 };
            return result;
        }

        public MeasurementPM GetSinglePMByCode(string code, int tenant)
        {
            MeasurementPM entity = (from a in repository.context.Measurements
                          where a.Code == code && a.Tenant == tenant
                          select new MeasurementPM()
                          {
                              Id = a.Id,
                              Tenant = a.Tenant,
                              InActive = a.InActive,
                              IsContainer = a.IsContainer,
                              IsContainerMeasurement = a.IsContainerMeasurement,
                              Code = a.Code,
                              Name = a.Name,
                              ShortName = a.ShortName,
                              SearchFields = a.SearchFields,
                              LocalName = a.LocalName,
                          }).FirstOrDefault();            

            return entity;
        }

    }
}
