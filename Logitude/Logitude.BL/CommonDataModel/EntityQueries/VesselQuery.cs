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
    public class VesselQuery
    {
        VesselRepository repository;

        public VesselQuery()
        {
            repository = new VesselRepository(); 
        }

        public VesselQuery(int tenant)
        {
            repository = new VesselRepository(tenant);
        }

        public VesselQuery(VesselRepository repository)
        {
            this.repository = repository;
        }

        public VesselPM GetSinglePM(string id, int tenant)
        {
            if (!string.IsNullOrEmpty(id))
            {
                string entityName = "VesselPM" + id + tenant;
                VesselPM entity;

                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        var entitystatuses = (from a in repository.context.Vessels
                                              where a.Tenant == tenant
                                              select new VesselPM()
                                              {
                                                  AddedManually = a.AddedManually,
                                                  Code = a.Code,
                                                  EnglishName = a.EnglishName,
                                                  Id = a.Id,
                                                  InActive = a.InActive,
                                                  LocalName = a.LocalName,
                                                  Notes = a.Notes,
                                                  Tenant = a.Tenant,
                                                  SearchFields = a.SearchFields,
                                                  ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                                  IMOCode = a.IMOCode,
                                                  CountryId = a.CountryId,
                                              });

                        foreach (var s in entitystatuses)
                        {
                            string name = "VesselPM" + s.Id + tenant;
                            if (CacheManager.CacheWrapper.Get(name) == null)
                            {
                                CacheManager.CacheWrapper.Insert(name, s, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            }
                        }
                        entity = (VesselPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                    else
                    {
                        entity = (VesselPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                }
                else
                {
                    entity = (from a in repository.context.Vessels
                              where a.Tenant == tenant && a.Id == id
                              select new VesselPM()
                              {
                                  AddedManually = a.AddedManually,
                                  Code = a.Code,
                                  EnglishName = a.EnglishName,
                                  Id = a.Id,
                                  InActive = a.InActive,
                                  LocalName = a.LocalName,
                                  Notes = a.Notes,
                                  Tenant = a.Tenant,
                                  SearchFields = a.SearchFields,
                                  ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                  IMOCode = a.IMOCode,
                                  CountryId = a.CountryId,
                              }).FirstOrDefault();
                }
                VesselPM securedPm = new VesselPM();
                SecuredMapping.GetMappedPM(entity, securedPm, "Vessel", tenant);

                return securedPm;
            }
            return null;
        }

        public VesselPM GetVesselPMByCode(string code, int tenant)
        {
            VesselPM vesselPM = (from a in repository.context.Vessels
                                 where a.Tenant == tenant && a.Code == code
                                 select new VesselPM()
                                 {
                                     AddedManually = a.AddedManually,
                                     Code = a.Code,
                                     EnglishName = a.EnglishName,
                                     Id = a.Id,
                                     IMOCode = a.IMOCode,
                                     CountryId = a.CountryId,
                                 }).FirstOrDefault();

            return vesselPM;
        }

        public VesselPM GetSinglePMByCode(string code, int tenant)
        {
            VesselPM vesselPM = (from a in repository.context.Vessels
                                 where a.Tenant == tenant && a.Code == code
                                 select new VesselPM()
                                 {
                                     AddedManually = a.AddedManually,
                                     Code = a.Code,
                                     EnglishName = a.EnglishName,
                                     Id = a.Id,
                                     IMOCode = a.IMOCode,
                                     CountryId = a.CountryId,
                                 }).FirstOrDefault();

            return vesselPM;
        }

        public IQueryable<VesselPM> GetVesselPMsByTenant(int tenant)
        {
            IQueryable<VesselPM> ports = (from a in repository.context.Vessels
                                         where a.Tenant == tenant 
                                         select new VesselPM()
                                        {
                                             AddedManually = a.AddedManually,
                                             Code = a.Code,
                                             EnglishName = a.EnglishName,
                                             Id = a.Id,
                                             InActive = a.InActive,
                                             LocalName = a.LocalName,
                                             Notes = a.Notes,
                                             Tenant = a.Tenant,
                                             SearchFields = a.SearchFields,
                                             ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                             IMOCode = a.IMOCode,
                                             CountryId = a.CountryId,
                                         });
            return ports;
        }

        public IQueryable<VesselList> GetIQueryableEntityList(IQueryable<Vessel> iQueryable)
        {
            IQueryable<VesselList> result = from entity in iQueryable
                                            select new VesselList()
                                            {
                                                AddedManually = entity.AddedManually,
                                                Code = entity.Code,
                                                EnglishName = entity.EnglishName,
                                                Id = entity.Id,
                                                InActive = entity.InActive,
                                                LocalName = entity.LocalName,
                                                Notes = entity.Notes,
                                                Tenant = entity.Tenant,
                                                SearchFields = entity.SearchFields,
                                                IMOCode = entity.IMOCode,
                                                CountryId = entity.CountryId,
                                                CountryName = entity.Country == null ? null : entity.Country.EnglishName,
                                                CountryCode = entity.Country == null ? null : entity.Country.Code,
                                            };
            return result;
        }
    }
}
