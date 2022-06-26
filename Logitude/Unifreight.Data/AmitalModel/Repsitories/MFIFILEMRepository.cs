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
    public class MFIFILEMRepository : IRepository<MFIFILEM>
    {
        private AmitalContext currentContext;
        public MFIFILEMRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public MFIFILEMRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public MFIFILEM GetSingle(int FILENO)
        {
            return (from a in context.MFIFILEMs
                    where a.FILENO == FILENO
                    select a).FirstOrDefault();
        }

        public IQueryable<MFIFILEM> GetAll()
        {
            return from a in context.MFIFILEMs
                   select a;
        }

        public void Add(MFIFILEM entity)
        {
            context.MFIFILEMs.Add(entity);
        }

        public void Remove(MFIFILEM entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.MFIFILEMs.Attach(entity);
            }
            context.MFIFILEMs.Remove(entity);
        }

        public void Update(MFIFILEM entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.MFIFILEMs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<MFIFILEM> All()
        {
            return context.MFIFILEMs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<MFIFILEM> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public MFIFILEM GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as MFIFILEMKeys;
            return this.GetSingle(keys.FILENO);
        }
    }
}
