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
    public class GDMREFRepository : IRepository<GDMREF>
    {
        private AmitalContext currentContext;
        public GDMREFRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public GDMREFRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public GDMREF GetSingle(string COMID, string REFID, string REFERENCE)
        {
            return (from a in context.GDMREFs
                    where a.COMID == COMID && a.REFID == REFID && a.REFERENCE == REFERENCE
                    select a).FirstOrDefault();
        }

        public IQueryable<GDMREF> GetAll()
        {
            return from a in context.GDMREFs
                   select a;
        }

        public void Add(GDMREF entity)
        {
            context.GDMREFs.Add(entity);
        }

        public void Remove(GDMREF entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.GDMREFs.Attach(entity);
            }
            //context.AddToGDMREFs 
            //context.GDMREFs.DeleteAllOnSubmit //ther isn't
            //http://forums.devart.com/viewtopic.php?t=13223
            //context.ExecuteStoreCommand 
            context.GDMREFs.Remove(entity);
        }

        public void Update(GDMREF entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.GDMREFs.Attach(entity); context.SetAsModified(entity);
            }


        }

        public List<GDMREF> All()
        {
            return context.GDMREFs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<GDMREF> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public GDMREF GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as GDMREFKeys;
            return this.GetSingle(keys.COMID, keys.REFID, keys.REFERENCE);
        }
    }
}

