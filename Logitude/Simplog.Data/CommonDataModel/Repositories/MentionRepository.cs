using System;
using System.Collections.Generic;
using System.Linq;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class MentionRepository : IRepository<Mention>
    {

        ICommonDataContext commonDataContext;

        public MentionRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public MentionRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public MentionRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }

        public void Add(Mention entity)
        {
            this.context.Mentions.Add(entity);
        }

        public List<Mention> All()
        {
            return this.context.Mentions.ToList();
        }

        public List<Mention> GetMulti(EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public Mention GetSingle(EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(Mention entity)
        {
            this.context.Mentions.Attach(entity);
            this.context.Mentions.Remove(entity);
        }

        public void Update(Mention entity)
        {
            context.Mentions.Attach(entity);
            context.SetAsModified(entity);
        }

        public Mention GetSingleMention(string id, int tenant)
        {
            return context.Mentions.FirstOrDefault(x => x.Id == id && x.Tenant == tenant);
        }

        public IQueryable<Mention> GetMentions(int tenant)
        {
            return (from record in context.Mentions where record.Tenant == tenant select record);
        }
    }
}
