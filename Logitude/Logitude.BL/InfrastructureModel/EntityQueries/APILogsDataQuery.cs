using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class APILogsDataQuery
    {
        APILogsDataRepository repository;

        public APILogsDataQuery()
        {
            repository = new APILogsDataRepository();
        }

        public APILogsDataQuery(int tenant)
        {
            repository = new APILogsDataRepository(tenant);
        }

        public APILogsDataQuery(APILogsDataRepository APILogsDataRepository)
        {
            repository = APILogsDataRepository;
        }

        public APILogsDataPM GetSinglePM(string id, int tenant)
        {
            APILogsDataPM entity;
            entity = (from a in repository.webFreightContext.APILogsData
                      where a.Tenant == tenant && a.Id == id
                      select new APILogsDataPM()
                      {
                          Id = a.Id,
                          Tenant = a.Tenant,
                          RequestData = a.RequestData,
                          ResponseData = a.ResponseData,
                          DiagnosticLog = a.DiagnosticLog,
                          ExceptionsMessage = a.ExceptionsMessage
                      }).FirstOrDefault();  

            return entity;
        }

        public IQueryable<APILogsDataList> GetIQueryableEntityList(IQueryable<APILogsData> iQueryable)
        {
            IQueryable<APILogsDataList> result = from a in iQueryable
                                                 select new APILogsDataList()
                                                  {
                                                      Id = a.Id,
                                                      Tenant = a.Tenant,
                                                      RequestData = a.RequestData,
                                                      ResponseData = a.ResponseData,
                                                      DiagnosticLog = a.DiagnosticLog,
                                                      ExceptionsMessage = a.ExceptionsMessage
                                                  };
            return result;
        }


        public IQueryable<APILogsDataPM> GetAPILogsDataPMsByTenant(int tenant)
        {
            IQueryable<APILogsDataPM> Temp = from a in repository.webFreightContext.APILogsData
                                                       where a.Tenant == tenant
                                             select new APILogsDataPM()
                                                       {
                                                           Id = a.Id,
                                                           Tenant = a.Tenant,
                                                           RequestData = a.RequestData,
                                                           ResponseData = a.ResponseData,
                                                           DiagnosticLog = a.DiagnosticLog,
                                                           ExceptionsMessage = a.ExceptionsMessage
                                                       };
            return Temp;
        }

        
    }
}
