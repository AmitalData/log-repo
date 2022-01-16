using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
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
    public class APILogsQuery
    {
        APILogsRepository repository;

        public APILogsQuery()
        {
            repository = new APILogsRepository();
        }

        public APILogsQuery(int tenant)
        {
            repository = new APILogsRepository(tenant);
        }

        public APILogsQuery(APILogsRepository APILogsRepository)
        {
            repository = APILogsRepository;
        }

        public APILogsPM GetSinglePM(string id, int tenant)
        {
            APILogsPM entity;
            APILogsDataPM entityData;
            entity = (from a in repository.webFreightContext.APILogs
                      where a.Tenant == tenant && a.Id == id
                      select new APILogsPM()
                      {
                          Id = a.Id,
                          Tenant = a.Tenant,
                          CorrelationId = a.CorrelationId,
                          CreateDate = a.CreateDate,
                          CreateDateUTC = a.CreateDateUTC,
                          Direction = a.Direction,
                          EntityId = a.EntityId,
                          ExpirationDate = a.ExpirationDate,
                          LastExceptionMessage = a.LastExceptionMessage,
                          LastUpdateDate = a.LastUpdateDate,
                          LastUpdateDateUTC = a.LastUpdateDateUTC,
                          NumberOfRetries = a.NumberOfRetries,
                          ObjectTableId = a.ObjectTableId,
                          PartnerName = a.PartnerName,
                          Refrence = a.Refrence,
                          SearchFields = a.SearchFields,
                          Status = a.Status,
                          Subject = a.Subject,
                          StatusName = a.Status == "D" ? "Done" : a.Status == "I" ? "In Progress" : "Faild",
                          CustomerId = a.CustomerId,
                          BatchNumber = a.BatchNumber,
                          QueueMessage = a.QueueMessage,
                          QueueType = a.QueueType

                      }).FirstOrDefault();
            ObjectTableQuery OTQ = new ObjectTableQuery(tenant);
            var OT = OTQ.GetObjectTablePMById(entity.ObjectTableId, entity.Tenant);
            if (OT != null)
            {
                entity.ObjectTableName = OT.Name;
            }
            entityData = (from a in repository.webFreightContext.APILogsData
                          where a.Tenant == tenant && a.Id == id
                          select new APILogsDataPM()
                          {
                              Id = a.Id,
                              Tenant = a.Tenant,
                              DiagnosticLog = a.DiagnosticLog,
                              ExceptionsMessage = a.ExceptionsMessage,
                              RequestData = a.RequestData,
                              ResponseData = a.ResponseData

                          }).FirstOrDefault();

            entity.DiagnosticLog = entityData.DiagnosticLog;
            entity.ExceptionsMessage = entityData.ExceptionsMessage;
            entity.RequestData = entityData.RequestData;
            entity.ResponseData = entityData.ResponseData;

            return entity;
        }

        public IQueryable<APILogsList> GetIQueryableEntityList(IQueryable<APILogs> iQueryable)
        {
            IQueryable<APILogsList> result = from a in iQueryable
                                             select new APILogsList()
                                                  {
                                                      Id = a.Id,
                                                      Tenant = a.Tenant,
                                                      CorrelationId = a.CorrelationId,
                                                      CreateDate = a.CreateDate,
                                                      CreateDateUTC = a.CreateDateUTC,
                                                      Direction = a.Direction,
                                                      EntityId = a.EntityId,
                                                      ExpirationDate = a.ExpirationDate,
                                                      LastExceptionMessage = a.LastExceptionMessage,
                                                      LastUpdateDate = a.LastUpdateDate,
                                                      LastUpdateDateUTC = a.LastUpdateDateUTC,
                                                      NumberOfRetries = a.NumberOfRetries,
                                                      ObjectTableId = a.ObjectTableId,
                                                      PartnerName = a.PartnerName,
                                                      Refrence = a.Refrence,
                                                      SearchFields = a.SearchFields,
                                                      Status = a.Status,
                                                      Subject = a.Subject,
                                                      StatusName = a.Status == "D" ? "Done" : a.Status == "I" ? "In Progress" : "Faild",
                                                      CustomerId = a.CustomerId,
                                                      BatchNumber = a.BatchNumber, 
                                                  };
            //foreach (var item in result)
            //{
            //    ObjectTableQuery OTQ = new ObjectTableQuery(item.Tenant);
            //    var OT = OTQ.GetObjectTablePMById(item.ObjectTableId, item.Tenant);
            //    if (OT != null)
            //    {
            //        item.ObjectTableName = OT.Name;
            //    }
            //}
            return result;
        }


        public IQueryable<APILogsPM> GetAPILogsPMsByTenant(int tenant)
        {
            IQueryable<APILogsPM> Temp = from a in repository.webFreightContext.APILogs
                                         where a.Tenant == tenant
                                         select new APILogsPM()
                                              {
                                                  Id = a.Id,
                                                  Tenant = a.Tenant,
                                                  CorrelationId = a.CorrelationId,
                                                  CreateDate = a.CreateDate,
                                                  CreateDateUTC = a.CreateDateUTC,
                                                  Direction = a.Direction,
                                                  EntityId = a.EntityId,
                                                  ExpirationDate = a.ExpirationDate,
                                                  LastExceptionMessage = a.LastExceptionMessage,
                                                  LastUpdateDate = a.LastUpdateDate,
                                                  LastUpdateDateUTC = a.LastUpdateDateUTC,
                                                  NumberOfRetries = a.NumberOfRetries,
                                                  ObjectTableId = a.ObjectTableId,
                                                  PartnerName = a.PartnerName,
                                                  Refrence = a.Refrence,
                                                  SearchFields = a.SearchFields,
                                                  Status = a.Status,
                                                  Subject = a.Subject,
                                                  //StatusName = a.Status == "D" ? "Done" : a.Status == "I" ? "In Progress" : "Faild"
                                                  CustomerId = a.CustomerId,
                                                  BatchNumber = a.BatchNumber,
                                                  QueueMessage = a.QueueMessage,
                                                  QueueType = a.QueueType

                                              };
            foreach (var item in Temp)
            {
                ObjectTableQuery OTQ = new ObjectTableQuery(item.Tenant);
                var OT = OTQ.GetObjectTablePMById(item.ObjectTableId, item.Tenant);
                if (OT != null)
                {
                    item.ObjectTableName = OT.Name;
                }
            }
            return Temp;
        }

        public APILogsPM GetSingleByCorrelationIdAndTenant(string correlationId, int tenant)
        {
            return (from a in repository.webFreightContext.APILogs
                    where a.Tenant == tenant && a.CorrelationId == correlationId
                    select new APILogsPM() {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        CorrelationId = a.CorrelationId,
                        CreateDate = a.CreateDate,
                        CreateDateUTC = a.CreateDateUTC,
                        Direction = a.Direction,
                        EntityId = a.EntityId,
                        ExpirationDate = a.ExpirationDate,
                        LastExceptionMessage = a.LastExceptionMessage,
                        LastUpdateDate = a.LastUpdateDate,
                        LastUpdateDateUTC = a.LastUpdateDateUTC,
                        NumberOfRetries = a.NumberOfRetries,
                        ObjectTableId = a.ObjectTableId,
                        PartnerName = a.PartnerName,
                        Refrence = a.Refrence,
                        SearchFields = a.SearchFields,
                        Status = a.Status,
                        Subject = a.Subject,
                        StatusName = a.Status == "D" ? "Done" : a.Status == "I" ? "In Progress" : "Faild",
                        CustomerId = a.CustomerId,
                        BatchNumber = a.BatchNumber,
                        QueueMessage = a.QueueMessage,
                        QueueType = a.QueueType,
                        QueueMessageMoreDetailsId = a.QueueMessageMoreDetailsId
                    }).FirstOrDefault();
        }
    }
}
