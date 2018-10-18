using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.Data.AmitalModel.EntityKeys;
using Unifreight.Data.AmitalModel.EntityPOCOs;

namespace Unifreight.Data.AmitalModel.Repsitories
{
    public class CTBSTORAGERepository : IRepository<CTBSTORAGE>
    {
        private AmitalContext currentContext;
        public CTBSTORAGERepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CTBSTORAGERepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CTBSTORAGE GetSingle(string STORAGESITE)
        {
            return (from a in context.CTBSTORAGEs
                    where a.STORAGESITE == STORAGESITE
                    select a).FirstOrDefault();
        }

        public IQueryable<CTBSTORAGE> GetAll()
        {
            return from a in context.CTBSTORAGEs
                   select a;
        }

        public void Add(CTBSTORAGE entity)
        {
            context.CTBSTORAGEs.Add(entity);
        }

        public void Remove(CTBSTORAGE entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBSTORAGEs.Attach(entity);
            }
            context.CTBSTORAGEs.Remove(entity);
        }

        public void Update(CTBSTORAGE entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBSTORAGEs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<CTBSTORAGE> All()
        {
            return context.CTBSTORAGEs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CTBSTORAGE> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CTBSTORAGE GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CTBSTORAGEKeys;
            return this.GetSingle(keys.STORAGESITE);
        }
    }
}
