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
    public class CommunicationStatusTypeQuery
    {
        CommunicationStatusTypeRepository repository;

        public CommunicationStatusTypeQuery()
        {
            repository = new CommunicationStatusTypeRepository(); 
        }

        public CommunicationStatusTypeQuery(int tenant)
        {
            repository = new CommunicationStatusTypeRepository(tenant);
        }

        public CommunicationStatusTypeQuery(CommunicationStatusTypeRepository communicationStatusTypeRepository)
        {
            repository = communicationStatusTypeRepository;
        }


        public CommunicationStatusTypePM GetSinglePM(string code)
        {
            return (from a in repository.context.CommunicationStatusTypes
                    where a.Code == code
                    select new CommunicationStatusTypePM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, }).FirstOrDefault();
        }


        public CommunicationStatusTypePM GetSingleCommunicationLogTypePM(string code)
        {
            return (from a in repository.context.CommunicationStatusTypes
                    where a.Code == code
                    select new CommunicationStatusTypePM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, }).FirstOrDefault();
        }


        public IQueryable<CommunicationStatusTypeList> GetIQueryableEntityList(IQueryable<CommunicationStatusType> iQueryable)
        {
            IQueryable<CommunicationStatusTypeList> result = from entity in iQueryable
                                                             select new CommunicationStatusTypeList()
                                                             {
                                                                 Name = entity.Name,
                                                                 Code = entity.Code,
                                                                 SearchFields = entity.SearchFields,
                                                             };
            return result;
        }
    }
}