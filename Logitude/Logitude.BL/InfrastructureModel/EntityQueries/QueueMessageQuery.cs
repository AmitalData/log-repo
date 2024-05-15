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
    public class QueueMessageQuery
    {
        QueueMessageRepository repository;

        public QueueMessageQuery()
        {
            repository = new QueueMessageRepository();
        }

        public QueueMessageQuery(int tenant)
        {
            repository = new QueueMessageRepository(tenant);
        }

        public QueueMessageQuery(QueueMessageRepository QueueMessageRepository)
        {
            repository = QueueMessageRepository;
        }

        public QueueMessagePM GetSinglePM(string id)
        {
            long? LongId = null;
            if (id != null)
            {
                LongId = long.Parse(id);
            }

            QueueMessagePM result =
          (from a in repository.context.QueueMessages
           where a.Id == LongId
           select new QueueMessagePM()
           {
               Id = a.Id,
               QueueDefinitionCode = a.QueueDefinitionCode,
               CreateDateTime = a.CreateDateTime,
               Status = a.Status,
               MessageBody = a.MessageBody,
               NextRunDateTime = a.NextRunDateTime,
               ProcessingDateTime = a.ProcessingDateTime,
               CompleteDateTime = a.CompleteDateTime,
               RetryNumber = a.RetryNumber,
               Tenant = a.Tenant,
               HashCode = a.HashCode

           }).FirstOrDefault();


            return result;

        }

        public QueueMessagePM GetSingleQueueMessagePM(long id)
        {
            QueueMessagePM result =
            (from a in repository.context.QueueMessages
             where a.Id == id
             select new QueueMessagePM()
             {

                 Id = a.Id,
                 QueueDefinitionCode = a.QueueDefinitionCode,
                 CreateDateTime = a.CreateDateTime,
                 Status = a.Status,
                 MessageBody = a.MessageBody,
                 NextRunDateTime = a.NextRunDateTime,
                 ProcessingDateTime = a.ProcessingDateTime,
                 CompleteDateTime = a.CompleteDateTime,
                 RetryNumber = a.RetryNumber,
                 Tenant = a.Tenant,
                 HashCode = a.HashCode

             }).FirstOrDefault();


            return result;

        }

        public QueueMessageList GetSingleQueueMessageList(long id)
        {
            QueueMessageList result =
            (from a in repository.context.QueueMessages
             where a.Id == id
             select new QueueMessageList()
             {

                 Id = a.Id,
                 QueueDefinitionCode = a.QueueDefinitionCode,
                 CreateDateTime = a.CreateDateTime,
                 Status = a.Status,
                 MessageBody = a.MessageBody,
                 NextRunDateTime = a.NextRunDateTime,
                 ProcessingDateTime = a.ProcessingDateTime,
                 CompleteDateTime = a.CompleteDateTime,
                 RetryNumber = a.RetryNumber,
                 Tenant = a.Tenant,
                 HashCode = a.HashCode

             }).FirstOrDefault();


            return result;

        }

        public List<QueueMessagePM> GetQueueMessagePMByHashCode(string hashCode)
        {
            List<QueueMessagePM> result =
                (from a in repository.context.QueueMessages
                 where a.HashCode == hashCode
                 select new QueueMessagePM()
                 {
                     Id = a.Id,
                     QueueDefinitionCode = a.QueueDefinitionCode,
                     CreateDateTime = a.CreateDateTime,
                     Status = a.Status,
                     MessageBody = a.MessageBody,
                     NextRunDateTime = a.NextRunDateTime,
                     ProcessingDateTime = a.ProcessingDateTime,
                     CompleteDateTime = a.CompleteDateTime,
                     RetryNumber = a.RetryNumber,
                     Tenant = a.Tenant,
                     HashCode = a.HashCode
                 }).ToList();

            return result;
        }


        public List<QueueMessagePM> GetQueueMessagePMByTenant(int tenant)
        {
            List<QueueMessagePM> result =
                (from a in repository.context.QueueMessages
                 where a.Tenant == tenant
                 select new QueueMessagePM()
                 {
                     Id = a.Id,
                     QueueDefinitionCode = a.QueueDefinitionCode,
                     CreateDateTime = a.CreateDateTime,
                     Status = a.Status,
                     MessageBody = a.MessageBody,
                     NextRunDateTime = a.NextRunDateTime,
                     ProcessingDateTime = a.ProcessingDateTime,
                     CompleteDateTime = a.CompleteDateTime,
                     RetryNumber = a.RetryNumber,
                     Tenant = a.Tenant,
                     HashCode = a.HashCode
                 }).ToList();

            return result;
        }


       public List<QueueMessagePM> GetQueueMessagePMByStatus(int status)
{
    List<QueueMessagePM> result =
        (from a in repository.context.QueueMessages
         where a.Status == status
         select new QueueMessagePM()
         {
             Id = a.Id,
             QueueDefinitionCode = a.QueueDefinitionCode,
             CreateDateTime = a.CreateDateTime,
             Status = a.Status,
             MessageBody = a.MessageBody,
             NextRunDateTime = a.NextRunDateTime,
             ProcessingDateTime = a.ProcessingDateTime,
             CompleteDateTime = a.CompleteDateTime,
             RetryNumber = a.RetryNumber,
             Tenant = a.Tenant,
             HashCode = a.HashCode
         }).ToList();

    return result;
}


        public IQueryable<QueueMessageList> GetIQueryableEntityList(IQueryable<QueueMessage> iQueryable)
        {
            IQueryable<QueueMessageList> result = from a in iQueryable
                                                  select new QueueMessageList()
                                                  {
                                                      Id = a.Id,
                                                      QueueDefinitionCode = a.QueueDefinitionCode,
                                                      CreateDateTime = a.CreateDateTime,
                                                      Status = a.Status,
                                                      MessageBody = a.MessageBody,
                                                      NextRunDateTime = a.NextRunDateTime,
                                                      ProcessingDateTime = a.ProcessingDateTime,
                                                      CompleteDateTime = a.CompleteDateTime,
                                                      RetryNumber = a.RetryNumber,
                                                      Tenant = a.Tenant,
                                                      HashCode = a.HashCode
                                                  };
            return result;
        }

       
       
        public IQueryable<QueueMessageList> GetIQueryableQueueMessagePMByHashCode(string hashCode)
        {
            IQueryable<QueueMessageList> result =
                from a in repository.context.QueueMessages
                where a.HashCode == hashCode
                select new QueueMessageList()
                {
                    Id = a.Id,
                    QueueDefinitionCode = a.QueueDefinitionCode,
                    CreateDateTime = a.CreateDateTime,
                    Status = a.Status,
                    MessageBody = a.MessageBody,
                    NextRunDateTime = a.NextRunDateTime,
                    ProcessingDateTime = a.ProcessingDateTime,
                    CompleteDateTime = a.CompleteDateTime,
                    RetryNumber = a.RetryNumber,
                    Tenant = a.Tenant,
                    HashCode = a.HashCode
                };

            return result;
        }


        public IQueryable<QueueMessageList> GetIQueryableQueueMessagePMByTenant(int tenant)
        {
            IQueryable<QueueMessageList> result =
                from a in repository.context.QueueMessages
                where a.Tenant == tenant
                select new QueueMessageList()
                {
                    Id = a.Id,
                    QueueDefinitionCode = a.QueueDefinitionCode,
                    CreateDateTime = a.CreateDateTime,
                    Status = a.Status,
                    MessageBody = a.MessageBody,
                    NextRunDateTime = a.NextRunDateTime,
                    ProcessingDateTime = a.ProcessingDateTime,
                    CompleteDateTime = a.CompleteDateTime,
                    RetryNumber = a.RetryNumber,
                    Tenant = a.Tenant,
                    HashCode = a.HashCode
                };

            return result;
        }


        public IQueryable<QueueMessageList> GetIQueryableQueueMessagePMByStatus(int status)
        {
            IQueryable<QueueMessageList> result =
                from a in repository.context.QueueMessages
                where a.Status == status
                select new QueueMessageList()
                {
                    Id = a.Id,
                    QueueDefinitionCode = a.QueueDefinitionCode,
                    CreateDateTime = a.CreateDateTime,
                    Status = a.Status,
                    MessageBody = a.MessageBody,
                    NextRunDateTime = a.NextRunDateTime,
                    ProcessingDateTime = a.ProcessingDateTime,
                    CompleteDateTime = a.CompleteDateTime,
                    RetryNumber = a.RetryNumber,
                    Tenant = a.Tenant,
                    HashCode = a.HashCode
                };

            return result;
        }


    }
}