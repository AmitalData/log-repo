

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
    public class GDMFILEVERRepository : IRepository<GDMFILEVER>
    {
        private AmitalContext currentContext;
        public GDMFILEVERRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public GDMFILEVERRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public List<GDMFILEVER> GetList(string COMID)
        {
            return (from a in context.GDMFILEVERs
                    where a.COMID == COMID 
                    select a).ToList();
        }
        public GDMFILEVER GetSingle(string COMID, int VERSION)
        {
            return (from a in context.GDMFILEVERs
                    where a.COMID == COMID && a.VERSION== VERSION 
                    select a).FirstOrDefault();
        }

        public IQueryable<GDMFILEVER> GetAll()
        {
            return from a in context.GDMFILEVERs
                   select a;
        }

        public void Add(GDMFILEVER entity)
        {
            context.GDMFILEVERs.Add(entity);
        }

        public void Remove(GDMFILEVER entity)
        {

            AttachIfNot(entity);context.SetAsModified(entity); //context.GDMFILEVERs.Attach(entity);
            //context.AddToGDMFILEVERs 
            context.GDMFILEVERs.Remove(entity);
        }

        public void Update(GDMFILEVER entity)
        {
            AttachIfNot(entity);context.SetAsModified(entity);

            //context.SetAsModified(entity);
        }
        void AttachIfNot(GDMFILEVER entity)
        {
            // if ((entity as EntityObject).EntityState == System.Data.EntityState.Unchanged)
            {
                context.GDMFILEVERs.Attach(entity);
            }
        }

        public List<GDMFILEVER> All()
        {
            return context.GDMFILEVERs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<GDMFILEVER> GetMulti(EntityKeyFields entityKeys)
        {
            var myGDMFILINGKeys = entityKeys as GDMFILINGKeys;
            return (from a in context.GDMFILEVERs
                    where a.COMID == myGDMFILINGKeys.COMID 
                    select a).ToList();
        }

        public GDMFILEVER GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as GDMFILEVERKeys;
            return this.GetSingle(keys.COMID ,keys.VERSION );
        }

        
    }
}
	 