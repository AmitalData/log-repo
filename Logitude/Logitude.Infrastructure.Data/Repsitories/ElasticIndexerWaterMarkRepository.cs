
using Logitude.Infrastructure.Data.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.Infrastructure.Data.Repsitories
{
    public partial class IndexerWaterMarkRepository : IRepository<IndexerWaterMark>
   {
        private IInfrastructureContext currentContext;
        public IndexerWaterMarkRepository(int tenant)
        {
            currentContext = InfrastructureContext.GetContext(tenant);
        }

        public IndexerWaterMarkRepository(IInfrastructureContext context)
        {
            currentContext = context;
        }



        public IndexerWaterMark GetSingle(string tableName)
        {
            return (from a in context.IndexerWaterMarks
                    where a.TableName == tableName 
                    select a).FirstOrDefault();
        }

        public IQueryable<IndexerWaterMark> GetAll()
        {
            return from a in context.IndexerWaterMarks
                   select a;
        }

        public IndexerWaterMark GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(IndexerWaterMark entity)
        {
            onAdd();
            context.IndexerWaterMarks.Add(entity);
        }

        public void Remove(IndexerWaterMark entity)
        {
            context.IndexerWaterMarks.Attach(entity);
            context.IndexerWaterMarks.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(IndexerWaterMark entity)
        {
            onUpdate();
            context.IndexerWaterMarks.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<IndexerWaterMark> All()
        {
            return context.IndexerWaterMarks.ToList();
        }

        private IInfrastructureContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<IndexerWaterMark> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }

}
   