using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
 
 
using Logitude.BL.InfrastructureModel.EntityLists;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class QueueMessageMoreDetailsQuery
    {
        QueueMessageMoreDetailsRepository repository;

        public QueueMessageMoreDetailsQuery()
        {
            repository = new QueueMessageMoreDetailsRepository(); 
        }

        public QueueMessageMoreDetailsQuery(int tenant)
        {
            repository = new QueueMessageMoreDetailsRepository(tenant);
        }

        public QueueMessageMoreDetailsQuery(QueueMessageMoreDetailsRepository QueueMessageMoreDetailsRepository)
        {
            repository = QueueMessageMoreDetailsRepository;
        }

        public QueueMessageMoreDetailsPM GetSinglePM(string id)
        {
            long? LongId = null;
            if (id != null)
            {
                LongId = long.Parse(id);
            }
        
            QueueMessageMoreDetailsPM result =
          (from a in repository.context.QueueMessageMoreDetails
           where a.Id == LongId
           select new QueueMessageMoreDetailsPM()
           {
               CompleteDateTime = a.CompleteDateTime,
               CreateDateTime = a.CreateDateTime,
               Field1 = a.Field1,
               Field2 = a.Field2,
               Field3 = a.Field3,
               Id = a.Id,
               MessageBody = a.MessageBody,
               NextRunDateTime = a.NextRunDateTime,
               ProcessingDateTime = a.ProcessingDateTime,
               QueueDefinitionCode = a.QueueDefinitionCode,
               RetryNumber = a.RetryNumber,
               Status = a.Status

           }).FirstOrDefault();


            return result;

        }

        public QueueMessageMoreDetailsPM GetSingleQueueMessageMoreDetailsPM(long id)
        {
            QueueMessageMoreDetailsPM result =
            (from a in repository.context.QueueMessageMoreDetails
             where a.Id == id
             select new QueueMessageMoreDetailsPM()
             {
                  CompleteDateTime = a.CompleteDateTime,
                  CreateDateTime = a.CreateDateTime,
                  Field1 = a.Field1,
                  Field2 = a.Field2,
                  Field3 = a.Field3,
                  Id = a.Id,
                  MessageBody = a.MessageBody,
                  NextRunDateTime = a.NextRunDateTime,
                  ProcessingDateTime = a.ProcessingDateTime,
                  QueueDefinitionCode = a.QueueDefinitionCode,
                  RetryNumber = a.RetryNumber,
                  Status = a.Status
                  
             }).FirstOrDefault();

             
            return result;

        }

        public QueueMessageMoreDetailsList GetSingleQueueMessageMoreDetailsList(long id)
        {
            QueueMessageMoreDetailsList result =
            (from a in repository.context.QueueMessageMoreDetails
             where a.Id == id
             select new QueueMessageMoreDetailsList()
             {
                 CompleteDateTime = a.CompleteDateTime,
                 CreateDateTime = a.CreateDateTime,
                 Field1 = a.Field1,
                 Field2 = a.Field2,
                 Field3 = a.Field3,
                 Id = a.Id,
                 MessageBody = a.MessageBody,
                 NextRunDateTime = a.NextRunDateTime,
                 ProcessingDateTime = a.ProcessingDateTime,
                 QueueDefinitionCode = a.QueueDefinitionCode,
                 RetryNumber = a.RetryNumber,
                 Status = a.Status

             }).FirstOrDefault();


            return result;

        }

        public List<QueueMessageMoreDetailsPM> GetQueueMessageMoreDetailsPMByField1(string Field1)
        {
            List<QueueMessageMoreDetailsPM> result =
            (from a in repository.context.QueueMessageMoreDetails
             where a.Field1 == Field1
             select new QueueMessageMoreDetailsPM()
             {
                 CompleteDateTime = a.CompleteDateTime,
                 CreateDateTime = a.CreateDateTime,
                 Field1 = a.Field1,
                 Field2 = a.Field2,
                 Field3 = a.Field3,
                 Id = a.Id,
                 MessageBody = a.MessageBody,
                 NextRunDateTime = a.NextRunDateTime,
                 ProcessingDateTime = a.ProcessingDateTime,
                 QueueDefinitionCode = a.QueueDefinitionCode,
                 RetryNumber = a.RetryNumber,
                 Status = a.Status

             }).ToList();


            return result;

        }

        public List<QueueMessageMoreDetailsPM> GetQueueMessageMoreDetailsPMByField2(string Field2)
        {
            List<QueueMessageMoreDetailsPM> result =
            (from a in repository.context.QueueMessageMoreDetails
             where a.Field2 == Field2
             select new QueueMessageMoreDetailsPM()
             {
                 CompleteDateTime = a.CompleteDateTime,
                 CreateDateTime = a.CreateDateTime,
                 Field1 = a.Field1,
                 Field2 = a.Field2,
                 Field3 = a.Field3,
                 Id = a.Id,
                 MessageBody = a.MessageBody,
                 NextRunDateTime = a.NextRunDateTime,
                 ProcessingDateTime = a.ProcessingDateTime,
                 QueueDefinitionCode = a.QueueDefinitionCode,
                 RetryNumber = a.RetryNumber,
                 Status = a.Status

             }).ToList();


            return result;

        }

        public IQueryable<QueueMessageMoreDetailsList> GetIQueryableEntityList(IQueryable<QueueMessageMoreDetails> iQueryable)
        {
            IQueryable<QueueMessageMoreDetailsList> result = from a in iQueryable
                                                             select new QueueMessageMoreDetailsList()
                                               {
                                                   CompleteDateTime = a.CompleteDateTime,
                                                   CreateDateTime = a.CreateDateTime,
                                                   Field1 = a.Field1,
                                                   Field2 = a.Field2,
                                                   Field3 = a.Field3,
                                                   Id = a.Id,
                                                   MessageBody = a.MessageBody,
                                                   NextRunDateTime = a.NextRunDateTime,
                                                   ProcessingDateTime = a.ProcessingDateTime,
                                                   QueueDefinitionCode = a.QueueDefinitionCode,
                                                   RetryNumber = a.RetryNumber,
                                                   Status = a.Status,
                                                   StatusName = a.Status == 0 ? "In Progress" : (a.Status == 1 ? "Done" : "Failed")
                                               };
            return result;
        }
        
        public IQueryable<QueueMessageMoreDetailsPM> GetIQueryableQueueMessageMoreDetailsPMByField1(string Field1)
        {
            IQueryable<QueueMessageMoreDetailsPM> result =
            (from a in repository.context.QueueMessageMoreDetails
             where a.Field1 == Field1
             select new QueueMessageMoreDetailsPM()
             {
                 CompleteDateTime = a.CompleteDateTime,
                 CreateDateTime = a.CreateDateTime,
                 Field1 = a.Field1,
                 Field2 = a.Field2,
                 Field3 = a.Field3,
                 Id = a.Id,
                 MessageBody = a.MessageBody,
                 NextRunDateTime = a.NextRunDateTime,
                 ProcessingDateTime = a.ProcessingDateTime,
                 QueueDefinitionCode = a.QueueDefinitionCode,
                 RetryNumber = a.RetryNumber,
                 Status = a.Status

             });


            return result;

        }

        public IQueryable<QueueMessageMoreDetailsPM> GetIQueryableQueueMessageMoreDetailsPMByField1Field2(string Field1,string Field2)
        {
            IQueryable<QueueMessageMoreDetailsPM> result =
            (from a in repository.context.QueueMessageMoreDetails
             where a.Field1 == Field1 && a.Field2 == Field2
             select new QueueMessageMoreDetailsPM()
             {
                 CompleteDateTime = a.CompleteDateTime,
                 CreateDateTime = a.CreateDateTime,
                 Field1 = a.Field1,
                 Field2 = a.Field2,
                 Field3 = a.Field3,
                 Id = a.Id,
                 MessageBody = a.MessageBody,
                 NextRunDateTime = a.NextRunDateTime,
                 ProcessingDateTime = a.ProcessingDateTime,
                 QueueDefinitionCode = a.QueueDefinitionCode,
                 RetryNumber = a.RetryNumber,
                 Status = a.Status

             });


            return result;

        }

        public IQueryable<QueueMessageMoreDetailsList> GetIQueryableQueueMessageMoreDetailsPMByField1List(string Field1)
        {
            IQueryable<QueueMessageMoreDetailsList> result =
            (from a in repository.context.QueueMessageMoreDetails
             where a.Field1 == Field1
             select new QueueMessageMoreDetailsList()
             {
                 CompleteDateTime = a.CompleteDateTime,
                 CreateDateTime = a.CreateDateTime,
                 Field1 = a.Field1,
                 Field2 = a.Field2,
                 Field3 = a.Field3,
                 Id = a.Id,
                 MessageBody = a.MessageBody,
                 NextRunDateTime = a.NextRunDateTime,
                 ProcessingDateTime = a.ProcessingDateTime,
                 QueueDefinitionCode = a.QueueDefinitionCode,
                 RetryNumber = a.RetryNumber,
                 Status = a.Status,
                 StatusName = a.Status == 0 ? "In Progress" : (a.Status == 1 ? "Done" : "Failed")

             });


            return result;

        }

        public IQueryable<QueueMessageMoreDetailsList> GetIQueryableQueueMessageMoreDetailsPMByField2List(string Field2)
        {
            IQueryable<QueueMessageMoreDetailsList> result =
            (from a in repository.context.QueueMessageMoreDetails
             where a.Field2 == Field2
             select new QueueMessageMoreDetailsList()
             {
                 CompleteDateTime = a.CompleteDateTime,
                 CreateDateTime = a.CreateDateTime,
                 Field1 = a.Field1,
                 Field2 = a.Field2,
                 Field3 = a.Field3,
                 Id = a.Id,
                 MessageBody = a.MessageBody,
                 NextRunDateTime = a.NextRunDateTime,
                 ProcessingDateTime = a.ProcessingDateTime,
                 QueueDefinitionCode = a.QueueDefinitionCode,
                 RetryNumber = a.RetryNumber,
                 Status = a.Status,
                 StatusName = a.Status == 0 ? "In Progress" : (a.Status == 1 ? "Done" : "Failed")

             });


            return result;

        }
         
         
    }
}