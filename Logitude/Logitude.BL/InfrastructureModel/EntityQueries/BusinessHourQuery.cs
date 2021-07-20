using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class BusinessHourQuery
    {
        BusinessHourRepository repository;

        public BusinessHourQuery()
        {
            repository = new BusinessHourRepository();
        }

        public BusinessHourQuery(int tenant)
        {
            repository = new BusinessHourRepository(tenant);
        }

        public BusinessHourQuery(BusinessHourRepository BusinessHoursRepository)
        {
            repository = BusinessHoursRepository;
        }

        public BusinessHourPM GetSinglePM(string id, int tenant)
        {
            BusinessHourPM entity;
            entity = (from a in repository.webFreightContext.BusinessHours
                      where a.Tenant == tenant && a.Id == id
                      select new BusinessHourPM()
                      {
                          Id = a.Id,
                          Tenant = a.Tenant,
                          Code = a.Code,
                          Name = a.Name,
                          Description = a.Description,

                          Is247 = a.Is247,

                          CreatedByUserId = a.CreatedByUserId,

                          UpdatedByUserId = a.UpdatedByUserId,

                          CreateDate = a.CreateDate,
                          UpdateDate = a.UpdateDate,

                          IsMondayEnabeled = a.IsMondayEnabeled,
                          IsTuesdayEnabeled = a.IsTuesdayEnabeled,
                          IsWednesdayEnabeled = a.IsWednesdayEnabeled,
                          IsThursdayEnabeled = a.IsThursdayEnabeled,
                          IsFridayEnabeled = a.IsFridayEnabeled,
                          IsSaturdayEnabeled = a.IsSaturdayEnabeled,
                          IsSundayEnabeled = a.IsSundayEnabeled,

                          MondayFromHour = a.MondayFromHour,
                          TuesdayFromHour = a.TuesdayFromHour,
                          WednesdayFromHour = a.WednesdayFromHour,
                          ThursdayFromHour = a.ThursdayFromHour,
                          FridayFromHour = a.FridayFromHour,
                          SaturdayFromHour = a.SaturdayFromHour,
                          SundayFromHour = a.SundayFromHour,

                          MondayToHour = a.MondayToHour,
                          TuesdayToHour = a.TuesdayToHour,
                          WednesdayToHour = a.WednesdayToHour,
                          ThursdayToHour = a.ThursdayToHour,
                          FridayToHour = a.FridayToHour,
                          SaturdayToHour = a.SaturdayToHour,
                          SundayToHour = a.SundayToHour,
                          SearchFields = a.SearchFields,

                      }).FirstOrDefault();

            //List Of Composition 
            entity.BusinessHoursHolidays = (from a in repository.webFreightContext.BusinessHoursHolidays
                                        where a.BusinessHourId == id
                                        select new BusinessHoursHolidayPM()
                                        {
                                            Id = a.Id,
                                            Tenant = a.Tenant,

                                            Day = a.Day,
                                            Month = a.Month,
                                            Year = a.Year,

                                            HolidayName = a.HolidayName,

                                            IsRecurring = a.IsRecurring,

                                            Inactive = a.Inactive,

                                            CreateDate = a.CreateDate,
                                            UpdateDate = a.UpdateDate,

                                            CreatedByUserId = a.CreatedByUserId,

                                            UpdatedByUserId = a.UpdatedByUserId,

                                            BusinessHourId = a.BusinessHourId,

                                        }).ToList();


            return entity;
        }

        public IQueryable<BusinessHourList> GetIQueryableEntityList(IQueryable<BusinessHour> iQueryable)
        {
            IQueryable<BusinessHourList> result = from a in iQueryable
                                                  select new BusinessHourList()
                                                  {
                                                      Id = a.Id,
                                                      Tenant = a.Tenant,
                                                      Code = a.Code,
                                                      Name = a.Name,
                                                      Description = a.Description,

                                                      Is247 = a.Is247,

                                                      CreatedByUserId = a.CreatedByUserId,

                                                      UpdatedByUserId = a.UpdatedByUserId,

                                                      CreateDate = a.CreateDate,
                                                      UpdateDate = a.UpdateDate,

                                                      IsMondayEnabeled = a.IsMondayEnabeled,
                                                      IsTuesdayEnabeled = a.IsTuesdayEnabeled,
                                                      IsWednesdayEnabeled = a.IsWednesdayEnabeled,
                                                      IsThursdayEnabeled = a.IsThursdayEnabeled,
                                                      IsFridayEnabeled = a.IsFridayEnabeled,
                                                      IsSaturdayEnabeled = a.IsSaturdayEnabeled,
                                                      IsSundayEnabeled = a.IsSundayEnabeled,

                                                      MondayFromHour = a.MondayFromHour,
                                                      TuesdayFromHour = a.TuesdayFromHour,
                                                      WednesdayFromHour = a.WednesdayFromHour,
                                                      ThursdayFromHour = a.ThursdayFromHour,
                                                      FridayFromHour = a.FridayFromHour,
                                                      SaturdayFromHour = a.SaturdayFromHour,
                                                      SundayFromHour = a.SundayFromHour,

                                                      MondayToHour = a.MondayToHour,
                                                      TuesdayToHour = a.TuesdayToHour,
                                                      WednesdayToHour = a.WednesdayToHour,
                                                      ThursdayToHour = a.ThursdayToHour,
                                                      FridayToHour = a.FridayToHour,
                                                      SaturdayToHour = a.SaturdayToHour,
                                                      SundayToHour = a.SundayToHour,
                                                      SearchFields = a.SearchFields,
                                                  };
            return result;
        }

        public BusinessHour GetFirstBusinessHoursForTenant(int tenant)
        {
            return (from a in repository.webFreightContext.BusinessHours
                    where a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        //  BusinessHoursService
        public IQueryable<BusinessHourPM> GetBusinessHoursPMsByTenant(int tenant)
        {
            IQueryable<BusinessHourPM> BusinessHours = from a in repository.webFreightContext.BusinessHours
                                                       where a.Tenant == tenant
                                                       select new BusinessHourPM()
                                                       {
                                                           Id = a.Id,
                                                           Tenant = a.Tenant,
                                                           Code = a.Code,
                                                           Name = a.Name,
                                                           Description = a.Description,

                                                           Is247 = a.Is247,

                                                           CreatedByUserId = a.CreatedByUserId,

                                                           UpdatedByUserId = a.UpdatedByUserId,

                                                           CreateDate = a.CreateDate,
                                                           UpdateDate = a.UpdateDate,

                                                           IsMondayEnabeled = a.IsMondayEnabeled,
                                                           IsTuesdayEnabeled = a.IsTuesdayEnabeled,
                                                           IsWednesdayEnabeled = a.IsWednesdayEnabeled,
                                                           IsThursdayEnabeled = a.IsThursdayEnabeled,
                                                           IsFridayEnabeled = a.IsFridayEnabeled,
                                                           IsSaturdayEnabeled = a.IsSaturdayEnabeled,
                                                           IsSundayEnabeled = a.IsSundayEnabeled,

                                                           MondayFromHour = a.MondayFromHour,
                                                           TuesdayFromHour = a.TuesdayFromHour,
                                                           WednesdayFromHour = a.WednesdayFromHour,
                                                           ThursdayFromHour = a.ThursdayFromHour,
                                                           FridayFromHour = a.FridayFromHour,
                                                           SaturdayFromHour = a.SaturdayFromHour,
                                                           SundayFromHour = a.SundayFromHour,

                                                           MondayToHour = a.MondayToHour,
                                                           TuesdayToHour = a.TuesdayToHour,
                                                           WednesdayToHour = a.WednesdayToHour,
                                                           ThursdayToHour = a.ThursdayToHour,
                                                           FridayToHour = a.FridayToHour,
                                                           SaturdayToHour = a.SaturdayToHour,
                                                           SundayToHour = a.SundayToHour,
                                                           SearchFields = a.SearchFields,

                                                       };
            return BusinessHours;
        }

        public BusinessHourPM GetSinglePMByTenant(int tenant)
        {
            BusinessHourPM entity;
            entity = (from a in repository.webFreightContext.BusinessHours
                      where a.Tenant == tenant && a.Code == "BUS"
                      select new BusinessHourPM()
                      {
                          Id = a.Id,
                          Tenant = a.Tenant,
                          Code = a.Code,
                          Name = a.Name,
                          Description = a.Description,

                          Is247 = a.Is247,

                          CreatedByUserId = a.CreatedByUserId,
                          UpdatedByUserId = a.UpdatedByUserId,

                          CreateDate = a.CreateDate,
                          UpdateDate = a.UpdateDate,

                          IsMondayEnabeled = a.IsMondayEnabeled,
                          IsTuesdayEnabeled = a.IsTuesdayEnabeled,
                          IsWednesdayEnabeled = a.IsWednesdayEnabeled,
                          IsThursdayEnabeled = a.IsThursdayEnabeled,
                          IsFridayEnabeled = a.IsFridayEnabeled,
                          IsSaturdayEnabeled = a.IsSaturdayEnabeled,
                          IsSundayEnabeled = a.IsSundayEnabeled,

                          MondayFromHour = a.MondayFromHour,
                          TuesdayFromHour = a.TuesdayFromHour,
                          WednesdayFromHour = a.WednesdayFromHour,
                          ThursdayFromHour = a.ThursdayFromHour,
                          FridayFromHour = a.FridayFromHour,
                          SaturdayFromHour = a.SaturdayFromHour,
                          SundayFromHour = a.SundayFromHour,

                          MondayToHour = a.MondayToHour,
                          TuesdayToHour = a.TuesdayToHour,
                          WednesdayToHour = a.WednesdayToHour,
                          ThursdayToHour = a.ThursdayToHour,
                          FridayToHour = a.FridayToHour,
                          SaturdayToHour = a.SaturdayToHour,
                          SundayToHour = a.SundayToHour,
                          SearchFields = a.SearchFields,

                      }).FirstOrDefault();

            this.MapBusinessHourDatesFields(entity);
            return entity;
        }

        private void MapBusinessHourDatesFields(BusinessHourPM entity)
        {
            if (entity.MondayFromHour != null)
                entity.MondayFromHourDate = TenantServerConfigration.GetCurrentDateTime(entity.Tenant).Date.Add((TimeSpan)entity.MondayFromHour);
            if (entity.TuesdayFromHour != null)
                entity.TuesdayFromHourDate = TenantServerConfigration.GetCurrentDateTime(entity.Tenant).Date.Add((TimeSpan)entity.TuesdayFromHour);
            if (entity.WednesdayFromHour != null)
                entity.WednesdayFromHourDate = TenantServerConfigration.GetCurrentDateTime(entity.Tenant).Date.Add((TimeSpan)entity.WednesdayFromHour);
            if (entity.ThursdayFromHour != null)
                entity.ThursdayFromHourDate = TenantServerConfigration.GetCurrentDateTime(entity.Tenant).Date.Add((TimeSpan)entity.ThursdayFromHour);
            if (entity.FridayFromHour != null)
                entity.FridayFromHourDate = TenantServerConfigration.GetCurrentDateTime(entity.Tenant).Date.Add((TimeSpan)entity.FridayFromHour);
            if (entity.SaturdayFromHour != null)
                entity.SaturdayFromHourDate = TenantServerConfigration.GetCurrentDateTime(entity.Tenant).Date.Add((TimeSpan)entity.SaturdayFromHour);
            if (entity.SundayFromHour != null)
                entity.SundayFromHourDate = TenantServerConfigration.GetCurrentDateTime(entity.Tenant).Date.Add((TimeSpan)entity.SundayFromHour);
            if (entity.MondayToHour != null)
                entity.MondayToHourDate = TenantServerConfigration.GetCurrentDateTime(entity.Tenant).Date.Add((TimeSpan)entity.MondayToHour);
            if (entity.TuesdayToHour != null)
                entity.TuesdayToHourDate = TenantServerConfigration.GetCurrentDateTime(entity.Tenant).Date.Add((TimeSpan)entity.TuesdayToHour);
            if (entity.WednesdayToHour != null)
                entity.WednesdayToHourDate = TenantServerConfigration.GetCurrentDateTime(entity.Tenant).Date.Add((TimeSpan)entity.WednesdayToHour);
            if (entity.ThursdayToHour != null)
                entity.ThursdayToHourDate = TenantServerConfigration.GetCurrentDateTime(entity.Tenant).Date.Add((TimeSpan)entity.ThursdayToHour);
            if (entity.FridayToHour != null)
                entity.FridayToHourDate = TenantServerConfigration.GetCurrentDateTime(entity.Tenant).Date.Add((TimeSpan)entity.FridayToHour);
            if (entity.SaturdayToHour != null)
                entity.SaturdayToHourDate = TenantServerConfigration.GetCurrentDateTime(entity.Tenant).Date.Add((TimeSpan)entity.SaturdayToHour);
            if (entity.SundayToHour != null)
                entity.SundayToHourDate = TenantServerConfigration.GetCurrentDateTime(entity.Tenant).Date.Add((TimeSpan)entity.SundayToHour);
        }
    }
}
