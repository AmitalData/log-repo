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
    public class CCUTSRUFOTRepository : IRepository<CCUTSRUFOT>
    {
        private AmitalContext currentContext;
        public CCUTSRUFOTRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CCUTSRUFOTRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CCUTSRUFOT GetSingle(int FILENO, int LINENO)
        {
            return (from a in context.CCUTSRUFOTs
                    where a.FILENO == FILENO && a.LINENO == LINENO
                    select a).FirstOrDefault();
        }

        public IQueryable<CCUTSRUFOT> GetAll()
        {
            return from a in context.CCUTSRUFOTs
                   select a;
        }

        public void Add(CCUTSRUFOT entity)
        {
            context.CCUTSRUFOTs.Add(entity);
        }

        public void Remove(CCUTSRUFOT entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CCUTSRUFOTs.Attach(entity);
            }
            //context.AddToCCUTSRUFOTs 
            //context.CCUTSRUFOTs.DeleteAllOnSubmit //ther isn't
            //http://forums.devart.com/viewtopic.php?t=13223
            //context.ExecuteStoreCommand 
            context.CCUTSRUFOTs.Remove(entity);
        }

        public void Update(CCUTSRUFOT entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CCUTSRUFOTs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<CCUTSRUFOT> All()
        {
            return context.CCUTSRUFOTs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CCUTSRUFOT> GetMulti(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CCUFILEMKeys;
            return (from a in context.CCUTSRUFOTs
                    where a.FILENO == keys.FILENO
                    select a).ToList();
        }

        public CCUTSRUFOT GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CCUTSRUFOTKeys;
            return this.GetSingle(keys.FILENO, keys.LINENO);
        }

        public int FastDeleteMulti(CCUFILEMKeys parentEntityKeys)
        {
            var keys = parentEntityKeys as CCUFILEMKeys;
            return context.DeleteWhere<CCUTSRUFOT>(rec => rec.FILENO == keys.FILENO);
        }
    }
}

