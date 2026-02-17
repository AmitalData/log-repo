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
    public class CCUPAYHANDRepository : IRepository<CCUPAYHAND>
    {
        private AmitalContext currentContext;
        public CCUPAYHANDRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CCUPAYHANDRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CCUPAYHAND GetSingle(int FILENO)
        {
            return (from a in context.CCUPAYHANDs
                    where a.FILENO == FILENO
                    select a).FirstOrDefault();
        }

        public IQueryable<CCUPAYHAND> GetAll()
        {
            return from a in context.CCUPAYHANDs
                   select a;
        }

        public void Add(CCUPAYHAND entity)
        {
            context.CCUPAYHANDs.Add(entity);
        }

        public void Remove(CCUPAYHAND entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CCUPAYHANDs.Attach(entity);
            }
            //context.AddToCCUPAYHANDs 
            context.CCUPAYHANDs.Remove(entity);
        }

        public void Update(CCUPAYHAND entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CCUPAYHANDs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<CCUPAYHAND> All()
        {
            return context.CCUPAYHANDs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CCUPAYHAND> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CCUPAYHAND GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CCUPAYHANDKeys;
            return this.GetSingle(keys.FILENO);
        }

        public int FastDeleteMulti(EntityKeyFields parentEntityKeys) // moran 5.1.16 - AMI-55274
        {
            var keys = parentEntityKeys as CCUFILEMKeys;
            return context.DeleteWhere<CCUPAYHAND>(rec => rec.FILENO == keys.FILENO);
        }
    }
}
	 