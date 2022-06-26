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
    public class CTBCARMODRepository : IRepository<CTBCARMOD>
    {
        private AmitalContext currentContext;
        public CTBCARMODRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CTBCARMODRepository(AmitalContext context)
        {
            currentContext = context;
        }

        /*public CTBCARMOD GetSingle(string CUSTOMERID)
        {
            return (from a in context.CTBCARMODs
                    where a.CUSTOMERID == CUSTOMERID
                    select a).FirstOrDefault();
        }*/

        public IQueryable<CTBCARMOD> GetAll()
        {
            return from a in context.CTBCARMODs
                   select a;
        }

        public void Add(CTBCARMOD entity)
        {
            context.CTBCARMODs.Add(entity);
        }

        public void Remove(CTBCARMOD entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBCARMODs.Attach(entity);
            }
            context.CTBCARMODs.Remove(entity);
        }

        public void Update(CTBCARMOD entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBCARMODs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<CTBCARMOD> All()
        {
            return context.CTBCARMODs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CTBCARMOD> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CTBCARMOD GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CTBCARMODKeys;
            return this.GetSingle(keys.CUSTOMERID, keys.CARMODEL);
        }

        public CTBCARMOD GetSingle(string CUSTOMERID, string CARMODEL)
        {
            return (from a in context.CTBCARMODs
                    where a.CUSTOMERID == CUSTOMERID && a.CARMODEL == CARMODEL
                    select a).FirstOrDefault();
        }
    }
}
