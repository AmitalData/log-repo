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
    public class CommunicationLogTypeQuery
    {
        CommunicationLogTypeRepository repository;



        public CommunicationLogTypeQuery(int tenant)
        {
            repository = new CommunicationLogTypeRepository(tenant);
        }

        public CommunicationLogTypeQuery(CommunicationLogTypeRepository communicationLogTypeRepository)
        {
            repository = communicationLogTypeRepository;
        }
        public CommunicationLogTypePM GetSinglePM(string code)
        {
            return (from a in repository.context.CommunicationLogTypes
                    where a.Code == code
                    select new CommunicationLogTypePM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, }).FirstOrDefault();
        }
        public CommunicationLogTypePM GetSingleCommunicationLogTypePM(string code)
        {
            return (from a in repository.context.CommunicationLogTypes
                    where a.Code == code
                    select new CommunicationLogTypePM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, }).FirstOrDefault();
        }

        public IQueryable<CommunicationLogTypeList> GetIQueryableEntityList(IQueryable<CommunicationLogType> iQueryable)
        {
            IQueryable<CommunicationLogTypeList> result = from entity in iQueryable
                                                          select new CommunicationLogTypeList()
                                                     {
                                                         Name = entity.Name,
                                                         Code = entity.Code,
                                                         SearchFields = entity.SearchFields,
                                                     };
            return result;
        }
    }
}
