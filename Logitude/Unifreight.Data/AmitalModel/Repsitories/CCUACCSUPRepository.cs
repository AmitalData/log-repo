using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.Data.AmitalModel.EntityKeys;
using Unifreight.Data.AmitalModel.EntityPOCOs;
//using EntityFramework.Extensions;

namespace Unifreight.Data.AmitalModel.Repsitories
{
    public class CCUACCSUPRepository : IRepository<CCUACCSUP>
    {
        private AmitalContext currentContext;
        public CCUACCSUPRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CCUACCSUPRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CCUACCSUP GetSingle(int FILENO, int LINENO)
        {
            return (from a in context.CCUACCSUPs
                    where a.FILENO == FILENO && a.LINENO == LINENO
                    select a).FirstOrDefault();
        }

        public IQueryable<CCUACCSUP> GetAll()
        {
            return from a in context.CCUACCSUPs
                   select a;
        }

        public void Add(CCUACCSUP entity)
        {
            context.CCUACCSUPs.Add(entity);
        }

        public void Remove(CCUACCSUP entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CCUACCSUPs.Attach(entity);
            }
            //context.AddToCCUACCSUPs 
            //context.CCUACCSUPs.DeleteAllOnSubmit //ther isn't
        //http://forums.devart.com/viewtopic.php?t=13223
            //context.ExecuteStoreCommand 
            context.CCUACCSUPs.Remove(entity);
        }

        public void Update(CCUACCSUP entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CCUACCSUPs.Attach(entity); context.SetAsModified(entity);
            }
            

        }

        public List<CCUACCSUP> All()
        {
            return context.CCUACCSUPs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CCUACCSUP> GetMulti(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CCUFILEMKeys;
            return (from a in context.CCUACCSUPs
                    where a.FILENO == keys.FILENO
                    select a).ToList();
        }

        public CCUACCSUP GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CCUACCSUPKeys;
            return this.GetSingle(keys.FILENO, keys.LINENO);
        }

        public int FastDeleteMulti(CCUFILEMKeys parentEntityKeys)
        {
            var keys = parentEntityKeys as CCUFILEMKeys;
            return context.DeleteWhere<CCUACCSUP>(rec => rec.FILENO == keys.FILENO);
        }
    }
}
	 
