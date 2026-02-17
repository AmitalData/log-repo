using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityLists;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class EventTypeCategoryQuery
    {
        EventTypeCategoryRepository repository;

        public EventTypeCategoryQuery()
        {
            repository = new EventTypeCategoryRepository(); 
        }

        public EventTypeCategoryQuery(int tenant)
        {
            repository = new EventTypeCategoryRepository(tenant);
        }

        public EventTypeCategoryQuery(EventTypeCategoryRepository repository)
        {
            this.repository = repository;
        }


        public EventTypeCategoryPM GetSinglePM(string code)
        {
            return (from a in repository.context.EventTypeCategories
                    where a.Code == code
                    select new EventTypeCategoryPM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                    }).FirstOrDefault();

        }

        public EventTypeCategoryPM GetSingleEventTypeCategoryPM(string code)
        {
            return (from a in repository.context.EventTypeCategories
                    where a.Code == code
                    select new EventTypeCategoryPM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                    }).FirstOrDefault();

        }

        public EventTypeCategoryPM GetSinglePM(string code, int tenant)
        {
            return (from a in repository.context.EventTypeCategories
                    where a.Code == code
                    select new EventTypeCategoryPM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                    }).FirstOrDefault();

        }

        public IQueryable<EventTypeCategoryPM> GetEventTypeCategoryPMs()
        {
            return from a in repository.context.EventTypeCategories
                   select new EventTypeCategoryPM()
                   {
                       Code = a.Code,
                       Name = a.Name,
                       SearchFields = a.SearchFields,
                   };
        }

        public IQueryable<EventTypeCategoryList> GetIQueryableEntityList(IQueryable<EventTypeCategory> iQueryable)
        {
            IQueryable<EventTypeCategoryList> result = from entity in iQueryable
                                                select new EventTypeCategoryList()
                                                {
                                                    Name = entity.Name,
                                                    Code = entity.Code,
                                                    SearchFields = entity.SearchFields,
                                                };
            return result;
        }
    }
}