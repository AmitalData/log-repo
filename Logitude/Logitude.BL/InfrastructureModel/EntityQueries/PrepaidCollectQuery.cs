using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class PrepaidCollectQuery
    {
        PrepaidCollectRepository repository;
        public PrepaidCollectQuery()
        {
            repository = new PrepaidCollectRepository(); 
        }

        public PrepaidCollectQuery(int tenant)
        {
            repository = new PrepaidCollectRepository(tenant);
        }

        public PrepaidCollectQuery(PrepaidCollectRepository prepaidCollectRepository)
        {
            repository = prepaidCollectRepository;
        }

        public IQueryable<PrepaidCollectPM> GetPrepaidCollectPMs()
        {
            return from a in repository.context.PrepaidCollects
                   select new PrepaidCollectPM()
                   {
                       Id = a.Id,
                       Name = a.Name,
                       DisplayInLOV = a.DisplayInLOV,
                       SearchFields = a.SearchFields,
                   };
        }

        public PrepaidCollectPM GetSinglePM(string id, int tenant)
        {
            return (from a in repository.context.PrepaidCollects
                    where a.Id == id
                    select new PrepaidCollectPM() { Id = a.Id, Name = a.Name, SearchFields = a.SearchFields, }).FirstOrDefault();
        }
        public PrepaidCollectPM GetSinglePM(string id)
        {
            return (from a in repository.context.PrepaidCollects
                    where a.Id == id
                    select new PrepaidCollectPM() { Id = a.Id, Name = a.Name, SearchFields = a.SearchFields, }).FirstOrDefault();
        }
        public PrepaidCollectPM GetCustomSinglePM(string id)
        {
            return (from a in repository.context.PrepaidCollects
                    where a.Id == id
                    select new PrepaidCollectPM() { Id = a.Id, Name = a.Name, SearchFields = a.SearchFields, }).FirstOrDefault();
        }
        

        public PrepaidCollectPM GetSinglePrepaidCollectPM(string id)
        {
            return (from a in repository.context.PrepaidCollects
                    where a.Id == id
                    select new PrepaidCollectPM() { Id = a.Id, Name = a.Name, SearchFields = a.SearchFields, }).FirstOrDefault();
        }
        public IQueryable<PrepaidCollectList> GetIQueryableEntityList(IQueryable<PrepaidCollect> iQueryable)
        {
            IQueryable<PrepaidCollectList> result = from entity in iQueryable
                                                    select new PrepaidCollectList()
                                                    {
                                                        Id = entity.Id,
                                                        Name = entity.Name,
                                                        DisplayInLOV = entity.DisplayInLOV,
                                                        SearchFields = entity.SearchFields,
                                                    };
            return result;
        }
    }
}