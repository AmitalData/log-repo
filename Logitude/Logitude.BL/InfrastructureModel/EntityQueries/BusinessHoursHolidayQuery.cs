using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class BusinessHoursHolidayQuery
    {
        BusinessHoursHolidayRepository repository;

        public BusinessHoursHolidayQuery()
        {
            repository = new BusinessHoursHolidayRepository();
        }

        public BusinessHoursHolidayQuery(int tenant)
        {
            repository = new BusinessHoursHolidayRepository(tenant);
        }

        public BusinessHoursHolidayQuery(BusinessHoursHolidayRepository BusinessHoursHolidaysRepository)
        {
            repository = BusinessHoursHolidaysRepository;
        }

        public BusinessHoursHolidayPM GetSinglePM(string id, int tenant)
        {
            BusinessHoursHolidayPM entity;
            entity = (from a in repository.webFreightContext.BusinessHoursHolidays
                      where a.Tenant == tenant && a.Id == id
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

                      }).FirstOrDefault();
            return entity;
        }

        public IQueryable<BusinessHoursHolidayList> GetIQueryableEntityList(IQueryable<BusinessHoursHoliday> iQueryable)
        {
            IQueryable<BusinessHoursHolidayList> result = from a in iQueryable
                                                          select new BusinessHoursHolidayList()
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
                                                          };
            return result;
        }

        public BusinessHoursHoliday GetFirstBusinessHoursHolidaysForTenant(int tenant)
        {
            return (from a in repository.webFreightContext.BusinessHoursHolidays
                    where a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        //  BusinessHoursHolidaysService
        public IQueryable<BusinessHoursHolidayPM> GetBusinessHoursHolidaysPMsByTenant(int tenant)
        {
            IQueryable<BusinessHoursHolidayPM> BusinessHoursHolidays = from a in repository.webFreightContext.BusinessHoursHolidays
                                                                       where a.Tenant == tenant
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
                                                                       };
            return BusinessHoursHolidays;
        }

        public List<BusinessHoursHolidayList> GetBusinessHoursHolidayListByBusinessHourId(string businessId, int tenant)
        {
            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("Ticket", 0, true);

            IQueryable<BusinessHoursHolidayList> query = (from a in repository.webFreightContext.BusinessHoursHolidays
                                                          where a.Tenant == tenant && a.BusinessHourId == businessId

                                                          select new BusinessHoursHolidayList()
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

                                                    });

            return query.ToList();
        }
    }
}
