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
    public class EFIFILEMRepository : IRepository<EFIFILEM>
    {
        private AmitalContext currentContext;
        public EFIFILEMRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public EFIFILEMRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public EFIFILEM GetSingle(int FILENO)
        {
            var q = (from a in context.EFIFILEMs
                     where a.FILENO == FILENO
                     select a);
            return  q                    
                    .FirstOrDefault();
        }

        public IQueryable<EFIFILEM> GetAll()
        {
            return from a in context.EFIFILEMs
                   select a;
        }

        public void Add(EFIFILEM entity)
        {
            context.EFIFILEMs.Add(entity);
        }

        public void Remove(EFIFILEM entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.EFIFILEMs.Attach(entity);
            }
            context.EFIFILEMs.Remove(entity);
        }

        public void Update(EFIFILEM entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.EFIFILEMs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<EFIFILEM> All()
        {
            return context.EFIFILEMs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<EFIFILEM> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public EFIFILEM GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as EFIFILEMKeys;
            return this.GetSingle(keys.FILENO);
        }
    }
}
