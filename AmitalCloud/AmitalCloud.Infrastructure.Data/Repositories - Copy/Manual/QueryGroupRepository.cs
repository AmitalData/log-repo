using System.Collections.Generic;
using System.Linq;
using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.EntityPOCOs;
using AmitalCloud.Infrastructure.Data.Interfaces;
namespace AmitalCloud.Infrastructure.Data.Repositories
{
    public class QueryGroupRepository:IRepository<QueryGroup, string>
    {
        IAmitalCloudContext amitalCloudContext;
        public QueryGroupRepository()
        {
            amitalCloudContext = new AmitalCloudContext();
        }
        public QueryGroupRepository(IAmitalCloudContext context)
        {
            amitalCloudContext = context;
        }

        public QueryGroupRepository(int tenant)
        {
            amitalCloudContext = AmitalCloudContext.GetContext(tenant);
        }
        public QueryGroup GetSingleQueryGroup(string code)
        {
            return (from a in context.QueryGroups
                    where a.Code == code
                    select a).FirstOrDefault();
        }
  
        public IQueryable<QueryGroup> GetQueryGroups()
        {
            return context.QueryGroups;
        }

   
        public void Add(QueryGroup entity)
        {
            context.QueryGroups.Add(entity);
        }

        public void Remove(QueryGroup entity)
        {
            context.QueryGroups.Attach(entity);
            context.QueryGroups.Remove(entity);
        }

        public void Update(QueryGroup entity)
        {
            context.QueryGroups.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<QueryGroup> All()
        {
            return context.QueryGroups.ToList();
        }

        public IAmitalCloudContext context
        {
            get { return amitalCloudContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<QueryGroup> GetMulti(IEntityKeyFields<QueryGroup,string> entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public QueryGroup GetSingle(IEntityKeyFields<QueryGroup,string> entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}