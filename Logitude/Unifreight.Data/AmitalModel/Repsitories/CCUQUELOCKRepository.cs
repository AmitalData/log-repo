using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.Data.AmitalModel.EntityKeys;
using Unifreight.Data.AmitalModel.EntityPOCOs;

namespace Unifreight.Data.AmitalModel.Repsitories
{
    public class CCUQUELOCKRepository : IRepository<CCUQUELOCK>
    {
        private AmitalContext currentContext;
        public CCUQUELOCKRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CCUQUELOCKRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CCUQUELOCK GetSingle(string ENTNAME, string FILENO)
        {
            return (from a in context.CCUQUELOCKs
                    where a.ENTNAME == ENTNAME && a.FILE_NO == FILENO
                    select a).FirstOrDefault();
        }

        public IQueryable<CCUQUELOCK> GetAll()
        {
            return from a in context.CCUQUELOCKs
                   select a;
        }

        public void Add(CCUQUELOCK entity)
        {
            context.CCUQUELOCKs.Add(entity);
        }

        public void Remove(CCUQUELOCK entity)
        {

            AttachIfNot(entity);context.SetAsModified(entity); //context.CCUQUELOCKs.Attach(entity);
            //context.AddToCCUQUELOCKs 
            context.CCUQUELOCKs.Remove(entity);
        }

        public void Update(CCUQUELOCK entity)
        {
            AttachIfNot(entity);context.SetAsModified(entity);

            context.SetAsModified(entity);
        }
        void AttachIfNot(CCUQUELOCK entity)
        {
            // if ((entity as EntityObject).EntityState == System.Data.EntityState.Unchanged)
            {
                context.CCUQUELOCKs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<CCUQUELOCK> All()
        {
            return context.CCUQUELOCKs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CCUQUELOCK> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CCUQUELOCK GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CCUQUELOCKKeys;
            return this.GetSingle(keys.ENTNAME, keys.FILENO);
        }


        public CCUQUELOCK GetSingleGeneralLockNOWAIT(string ENTNAME, string FILENO)
        {
            ////**** ITZIK :YUVAL PLS SET THE PROPERTY FROM FILENO TO FILE_NO !!!!
            ////**** ITZIK :YUVAL PLS SET THE PROPERTY FROM FILENO TO FILE_NO !!!!
            return (context as DbContextBase)
                .GetListNOWAITWhere<CCUQUELOCK>(rec => rec.ENTNAME == ENTNAME &&
                ////**** ITZIK :YUVAL PLS SET THE PROPERTY FROM FILENO TO FILE_NO !!!!
                rec.FILE_NO ////**** ITZIK :YUVAL PLS SET THE PROPERTY FROM FILENO TO FILE_NO !!!!
                ////**** ITZIK :YUVAL PLS SET THE PROPERTY FROM FILENO TO FILE_NO !!!!
                == FILENO
                ).FirstOrDefault(); ;
        }
    }


 
}
	 