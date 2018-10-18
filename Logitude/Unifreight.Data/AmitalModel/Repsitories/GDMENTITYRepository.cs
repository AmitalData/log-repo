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
    public class GDMENTITYRepository : IRepository<GDMENTITY>
    {
        private AmitalContext currentContext;
        public GDMENTITYRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public GDMENTITYRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public GDMENTITY GetSingle(string COMID, string PRIMARYID, string PRIMARYNUM)
        {
            return (from a in context.GDMENTITIES
                    where a.COMID == COMID && a.PRIMARYID == PRIMARYID && a.PRIMARYNUM == PRIMARYNUM
                    select a).FirstOrDefault();
        }

        public IQueryable<GDMENTITY> GetAll()
        {
            return from a in context.GDMENTITIES
                   select a;
        }

        public void Add(GDMENTITY entity)
        {
            context.GDMENTITIES.Add(entity);
        }

        public void Remove(GDMENTITY entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.GDMENTITIES.Attach(entity);
            }
            //context.AddToGDMENTITYs 
            //context.GDMENTITYs.DeleteAllOnSubmit //ther isn't
            //http://forums.devart.com/viewtopic.php?t=13223
            //context.ExecuteStoreCommand 
            context.GDMENTITIES.Remove(entity);
        }

        public void Update(GDMENTITY entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.GDMENTITIES.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<GDMENTITY> All()
        {
            return context.GDMENTITIES.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<GDMENTITY> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public List<GDMENTITY> GetGDMENTITYListByPrimarys(string PRIMARYID, string PRIMARYNUM)
        {
            return (from a in context.GDMENTITIES
                    where a.PRIMARYID == PRIMARYID && a.PRIMARYNUM == PRIMARYNUM
                    select a).ToList();
        }

        public GDMENTITY GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as GDMENTITYKeys;
            return this.GetSingle(keys.COMID, keys.PRIMARYID, keys.PRIMARYNUM);
        }

        public int FastDeleteMulti(CCUFILEMKeys parentEntityKeys)
        {
            throw new NotImplementedException();

        }
    }
}

