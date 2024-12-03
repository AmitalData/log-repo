using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class InboundEmailLineRepository: IRepository<InboundEmailLine>
    {
        public IWebFreightContext webFreightContext;

        public InboundEmailLineRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public InboundEmailLineRepository() : this(0)
        {
        }

        public InboundEmailLineRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

        public InboundEmailLine GetSingleInboundEmailLine(string id, int tenant)
        {
            InboundEmailLine entity = this.webFreightContext.InboundEmailLines.Where(d => d.Id == id && d.Tenant == tenant).FirstOrDefault();
            return entity;
        }

        public IQueryable<InboundEmailLine> GetInboundEmailLinesByTenant(int tenant)
        {
            IQueryable<InboundEmailLine> InboundEmailLines = from a in webFreightContext.InboundEmailLines
                                                       where a.Tenant == tenant
                                                       select a;
            return InboundEmailLines;
        }
       
        public IQueryable<InboundEmailLine> GetInboundEmailLines(int tenant)
        {
            return (from record in webFreightContext.InboundEmailLines where record.Tenant == tenant select record);
        }

        public void Add(InboundEmailLine entity)
        {
            webFreightContext.InboundEmailLines.Add(entity);
        }

        public void Remove(InboundEmailLine entity)
        {
            webFreightContext.InboundEmailLines.Attach(entity);
            webFreightContext.InboundEmailLines.Remove(entity);
        }

        public void Update(InboundEmailLine entity)
        {
            webFreightContext.InboundEmailLines.Attach(entity);
            webFreightContext.SetAsModified(entity);
        }

        public List<InboundEmailLine> All()
        {
            return webFreightContext.InboundEmailLines.ToList();
        }

        public void SubmitChanges()
        {
            webFreightContext.SaveChanges();
        }

        public List<InboundEmailLine> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public InboundEmailLine GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public List<InboundEmailLine> GetWaitingLines()
        {
            List<InboundEmailLine> myResult = new List<InboundEmailLine>();

            var myGroup = (from a in webFreightContext.InboundEmailLines
                           where (a.CommunicationLogId == null)
                           group a by new { a.Tenant, a.InboundEmailId } into g
                           select new
                           {
                               Tenant = g.Key.Tenant,
                               InboundEmailId = g.Key.InboundEmailId,
                               Count = g.Count()
                           });

            if (myGroup.Where(d => d.Count > 1).Any())
            {
                var myItem = myGroup.Where(d => d.Count > 1).FirstOrDefault();

                InboundEmailLine myLine = webFreightContext.InboundEmailLines.Where(d => d.Tenant == myItem.Tenant && d.InboundEmailId == myItem.InboundEmailId).OrderBy(o => o.CreateDate).Skip(1).FirstOrDefault();

                if (myLine != null)
                {
                    int myTenant = myLine.Tenant;
                    string myInboundEmailId = myLine.InboundEmailId;

                    List<InboundEmailLine> list_All = webFreightContext.InboundEmailLines.Where(d => d.InboundEmailId == myInboundEmailId && d.Tenant == myTenant).ToList();

                    if (list_All.Where(d => d.CommunicationLogId == null).Count() > 1)
                    {
                        InboundEmailLine myFirstLine = list_All.Where(d => d.CommunicationLogId == null).OrderBy(o => o.CreateDate).Skip(1).FirstOrDefault();

                        myResult = (from a in list_All
                                    where a.CreateDate <= myFirstLine.CreateDate
                                    select a).ToList();
                    }
                }
            }
            
            return myResult;
        }

        public List<InboundEmailLine> GetInboundEmailLinesByInboudEmailId(string Id, int tenant)
        {
            IQueryable<InboundEmailLine> InboundEmailLines = (from a in webFreightContext.InboundEmailLines
                                                             where a.Tenant == tenant && a.InboundEmailId == Id
                                                             select a);
            return InboundEmailLines.ToList();
        }

        public InboundEmailLine GetSingleByEntityLineId(string entityLineId, int tenant)
        {
           return this.webFreightContext.InboundEmailLines.Where(d => d.EntityLineId == entityLineId && d.Tenant == tenant && d.Direction != "I").FirstOrDefault();
        }
    }
}
