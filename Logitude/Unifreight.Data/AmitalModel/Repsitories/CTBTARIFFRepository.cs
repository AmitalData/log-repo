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
    public class CTBTARIFFRepository : IRepository<CTBTARIFF>
    {
        private AmitalContext currentContext;
        public CTBTARIFFRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CTBTARIFFRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CTBTARIFF GetSingle(string TARIFFID)
        {
            return (from a in context.CTBTARIFFs
                    where a.TARIFFID == TARIFFID
                    select a).FirstOrDefault();
        }

        public IQueryable<CTBTARIFF> GetAll()
        {
            return from a in context.CTBTARIFFs
                   select a;
        }

        public void Add(CTBTARIFF entity)
        {
            context.CTBTARIFFs.Add(entity);
        }

        public void Remove(CTBTARIFF entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBTARIFFs.Attach(entity);
            }
            context.CTBTARIFFs.Remove(entity);
        }

        public void Update(CTBTARIFF entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBTARIFFs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<CTBTARIFF> All()
        {
            return context.CTBTARIFFs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CTBTARIFF> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CTBTARIFF GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CTBTARIFFKeys;
            return this.GetSingle(keys.TARIFFID);
        }
    }
}
