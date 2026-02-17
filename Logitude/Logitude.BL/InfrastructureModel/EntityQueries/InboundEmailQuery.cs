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
    public class InboundEmailQuery
    {
        InboundEmailRepository repository;

        public InboundEmailQuery()
        {
            repository = new InboundEmailRepository();
        }

        public InboundEmailQuery(int tenant)
        {
            repository = new InboundEmailRepository(tenant);
        }

        public InboundEmailQuery(InboundEmailRepository InboundEmailsRepository)
        {
            repository = InboundEmailsRepository;
        }

        public InboundEmailPM GetSinglePM(string id, int tenant)
        {
            InboundEmailPM entity;

            if (tenant == 0)
            {   
                entity = (from a in repository.webFreightContext.InboundEmails
                          where a.Id == id
                          select new InboundEmailPM()
                          {
                              Id = a.Id,
                              Tenant = a.Tenant,
                              ObjectTableId = a.ObjectTableId,
                              EntityId = a.EntityId,
                              CreateDate = a.CreateDate,
                              UpdateDate = a.UpdateDate,
                              Uniquekey = a.Uniquekey,
                              CreatedByContactId = a.CreatedByContactId,
                              ObjectTableName = a.ObjectTable == null ? "" : a.ObjectTable.Name,
                              IsRejected = a.IsRejected,
                              AnalyzeQueueId = a.AnalyzeQueueId,

                          }).FirstOrDefault();
            }

            else
            {
                entity = (from a in repository.webFreightContext.InboundEmails
                          where a.Tenant == tenant && a.Id == id
                          select new InboundEmailPM()
                          {
                              Id = a.Id,
                              Tenant = a.Tenant,
                              ObjectTableId = a.ObjectTableId,
                              EntityId = a.EntityId,
                              CreateDate = a.CreateDate,
                              UpdateDate = a.UpdateDate,
                              Uniquekey = a.Uniquekey,
                              CreatedByContactId = a.CreatedByContactId,
                              ObjectTableName = a.ObjectTable == null ? "" : a.ObjectTable.Name,
                              IsRejected = a.IsRejected,
                              AnalyzeQueueId = a.AnalyzeQueueId,

                          }).FirstOrDefault();

            }

            //List Of Composition 
            entity.InboundEmailLines =(from d in repository.webFreightContext.InboundEmailLines
                                                   where d.InboundEmailId == id
                                                   select new InboundEmailLinePM()
                                                   {
                                                       Id=d.Id, 
                                                       Tenant=d.Tenant,
                                                       InboundEmailId=d.InboundEmailId,
                                                       Sender=d.Sender,
                                                       Recepient=d.Recepient,
                                                       Body=d.Body,
                                                       FullBody = d.FullBody,
                                                       Subject=d.Subject,
                                                       CreateDate=d.CreateDate,
                                                       CCs=d.CCs,
                                                       Bcc = d.Bcc,
                                                       InternalUsers=d.InternalUsers,
                                                       Direction=d.Direction,
                                                       CommunicationLogId=d.CommunicationLogId,

                                                   }).ToList();

            return entity;
        }

        public IQueryable<InboundEmailList> GetIQueryableEntityList(IQueryable<InboundEmail> iQueryable)
        {
            IQueryable<InboundEmailList> result = from InboundEmails in iQueryable
                                                   select new InboundEmailList()
                                                   {
                                                       Id = InboundEmails.Id,
                                                       Tenant = InboundEmails.Tenant,
                                                       ObjectTableId = InboundEmails.ObjectTableId,
                                                       EntityId = InboundEmails.EntityId,
                                                       CreateDate = InboundEmails.CreateDate,
                                                       UpdateDate = InboundEmails.UpdateDate,
                                                       Uniquekey = InboundEmails.Uniquekey,
                                                       CreatedByContactId = InboundEmails.CreatedByContactId,
                                                       ObjectTableName = InboundEmails.ObjectTable == null ? "" : InboundEmails.ObjectTable.Name,
                                                       IsRejected = InboundEmails.IsRejected,
                                                       AnalyzeQueueId = InboundEmails.AnalyzeQueueId,
                                                   };
            return result;
        }

        public InboundEmail GetFirstInboundEmailsForTenant(int tenant)
        {
            return (from a in repository.webFreightContext.InboundEmails
                    where a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        //  InboundEmailsService
        public IQueryable<InboundEmailPM> GetInboundEmailsPMsByTenant(int tenant)
        {
            IQueryable<InboundEmailPM> InboundEmails = from a in repository.webFreightContext.InboundEmails
                                                        where a.Tenant == tenant
                                                        select new InboundEmailPM()
                                                        {
                                                            Id = a.Id,
                                                            Tenant = a.Tenant,
                                                            ObjectTableId = a.ObjectTableId,
                                                            EntityId = a.EntityId,
                                                            CreateDate = a.CreateDate,
                                                            UpdateDate = a.UpdateDate,
                                                            Uniquekey = a.Uniquekey,
                                                            CreatedByContactId = a.CreatedByContactId,
                                                            ObjectTableName = a.ObjectTable == null ? "" : a.ObjectTable.Name,
                                                            IsRejected = a.IsRejected,
                                                            AnalyzeQueueId = a.AnalyzeQueueId,
                                                        };
            return InboundEmails;
        }
    }
}
