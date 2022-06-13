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
    public class EFIMMNRepository : IRepository<EFIMMN>
    {
        private AmitalContext currentContext;
        public EFIMMNRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public EFIMMNRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public EFIMMN GetSingle(long FILENO,int STORGENO)
        {
            return (from a in context.EFIMMNs
                    where a.FILENO == FILENO && a.STORGENO == STORGENO
                    select a).FirstOrDefault();
        }

        public List<EFIMMN> GetByFileNo(long FILENO)
        {
            return (from a in context.EFIMMNs
                    where a.FILENO == FILENO
                    select a).ToList();
        }

        public IQueryable<EFIMMN> GetAll()
        {
            return from a in context.EFIMMNs
                   select a;
        }

        public void Add(EFIMMN entity)
        {
            context.EFIMMNs.Add(entity);
        }

        public void Remove(EFIMMN entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.EFIMMNs.Attach(entity);
            }
            context.EFIMMNs.Remove(entity);
        }

        public void Update(EFIMMN entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.EFIMMNs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<EFIMMN> All()
        {
            return context.EFIMMNs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<EFIMMN> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public EFIMMN GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as EFIMMNKeys;
            return this.GetSingle(keys.FILENO,keys.STORGENO);
        }
    }
}
