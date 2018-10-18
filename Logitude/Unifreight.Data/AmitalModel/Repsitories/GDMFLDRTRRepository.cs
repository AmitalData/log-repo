
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.Data.AmitalModel.EntityKeys;
using Unifreight.Data.AmitalModel.EntityPOCOs;

namespace Unifreight.Data.AmitalModel.Repsitories
{
    public class GDMFLDRTRRepository : IRepository<GDMFLDRTR>
    {
        private AmitalContext currentContext;
        public GDMFLDRTRRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public GDMFLDRTRRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public GDMFLDRTR GetSingle(string FOLDERCODE)
        {
            return (from a in context.GDMFLDRTRs
                    where a.FOLDERCODE == FOLDERCODE
                    select a).FirstOrDefault();
        }

        public IQueryable<GDMFLDRTR> GetAll()
        {
            return from a in context.GDMFLDRTRs
                   select a;
        }

        public void Add(GDMFLDRTR entity)
        {
            context.GDMFLDRTRs.Add(entity);
        }

        public void Remove(GDMFLDRTR entity)
        {

            AttachIfNot(entity);context.SetAsModified(entity); //context.GDMFLDRTRs.Attach(entity);
            //context.AddToGDMFLDRTRs 
            context.GDMFLDRTRs.Remove(entity);
        }

        public void Update(GDMFLDRTR entity)
        {
            AttachIfNot(entity);context.SetAsModified(entity);

            //context.SetAsModified(entity);
        }
        void AttachIfNot(GDMFLDRTR entity)
        {
            // if ((entity as EntityObject).EntityState == System.Data.EntityState.Unchanged)
            {
                context.GDMFLDRTRs.Attach(entity);
            }
        }

        public List<GDMFLDRTR> All()
        {
            return context.GDMFLDRTRs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<GDMFLDRTR> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public GDMFLDRTR GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as GDMFLDRTRKeys;
            return this.GetSingle(keys.FOLDERCODE);
        }
    }
}
	 