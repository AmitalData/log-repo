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
   public class InboundEmailLineQuery
    {
        InboundEmailLineRepository repository;

        public InboundEmailLineQuery()
        {
            repository = new InboundEmailLineRepository();
        }

        public InboundEmailLineQuery(int tenant)
        {
            repository = new InboundEmailLineRepository(tenant);
        }

        public InboundEmailLineQuery(InboundEmailLineRepository InboundEmailLinesRepository)
        {
            repository = InboundEmailLinesRepository;
        }

        public InboundEmailLinePM GetSinglePM(string id, int tenant)
        {
            InboundEmailLinePM entity;
            entity = (from a in repository.webFreightContext.InboundEmailLines
                      where a.Tenant == tenant && a.Id == id
                      select new InboundEmailLinePM()
                      {
                          Id = a.Id,
                          Tenant = a.Tenant,
                          InboundEmailId=a.InboundEmailId,
                          Sender=a.Sender,
                          Recepient=a.Recepient,
                          Body=a.Body,
                          CCs=a.CCs,
                          Bcc = a.Bcc,
                          Subject=a.Subject,
                          Direction=a.Direction,
                          CommunicationLogId=a.CommunicationLogId,
                          CreateDate=a.CreateDate,
                          EntityLineId=a.EntityLineId,
                          FullBody = a.FullBody,
                          HTMLFullBody = a.HTMLFullBody,
                          InternalUsers = a.InternalUsers,

                      }).FirstOrDefault();

            return entity;
        }

        public IQueryable<InboundEmailLineList> GetIQueryableEntityList(IQueryable<InboundEmailLine> iQueryable)
        {
            IQueryable<InboundEmailLineList> result = from InboundEmailLines in iQueryable
                                                   select new InboundEmailLineList()
                                                   {
                                                       Id = InboundEmailLines.Id,
                                                       Tenant = InboundEmailLines.Tenant,
                                                       InboundEmailId = InboundEmailLines.InboundEmailId,
                                                       Sender = InboundEmailLines.Sender,
                                                       Recepient = InboundEmailLines.Recepient,
                                                       Body = InboundEmailLines.Body,
                                                       CCs = InboundEmailLines.CCs,
                                                       Bcc = InboundEmailLines.Bcc,
                                                       Subject = InboundEmailLines.Subject,
                                                       Direction = InboundEmailLines.Direction,
                                                       CommunicationLogId = InboundEmailLines.CommunicationLogId,
                                                       CreateDate = InboundEmailLines.CreateDate,
                                                       EntityLineId = InboundEmailLines.EntityLineId,
                                                       FullBody = InboundEmailLines.FullBody,
                                                       HTMLFullBody = InboundEmailLines.HTMLFullBody,
                                                       InternalUsers = InboundEmailLines.InternalUsers,
                                                   };
            return result;
        }

        public InboundEmailLine GetFirstInboundEmailLinesForTenant(int tenant)
        {
            return (from a in repository.webFreightContext.InboundEmailLines
                    where a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        //  InboundEmailLinesService
        public IQueryable<InboundEmailLinePM> GetInboundEmailLinesPMsByTenant(int tenant)
        {
            IQueryable<InboundEmailLinePM> InboundEmailLines = from a in repository.webFreightContext.InboundEmailLines
                                                        where a.Tenant == tenant
                                                        select new InboundEmailLinePM()
                                                        {
                                                            Id = a.Id,
                                                            Tenant = a.Tenant,
                                                            InboundEmailId = a.InboundEmailId,
                                                            Sender = a.Sender,
                                                            Recepient = a.Recepient,
                                                            Body = a.Body,
                                                            CCs = a.CCs,
                                                            Bcc = a.Bcc,
                                                            Subject = a.Subject,
                                                            Direction = a.Direction,
                                                            CommunicationLogId = a.CommunicationLogId,
                                                            CreateDate = a.CreateDate,
                                                            EntityLineId = a.EntityLineId,
                                                            FullBody = a.FullBody,
                                                            HTMLFullBody = a.HTMLFullBody,
                                                            InternalUsers = a.InternalUsers,
                                                        };
            return InboundEmailLines;
        }

    }
}
