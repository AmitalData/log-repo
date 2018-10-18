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
    public class GTRTRANRepository : IRepository<GTRTRAN>
    {
        private AmitalContext currentContext;
        public GTRTRANRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public GTRTRANRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public GTRTRAN GetSingle(string PARTNERID, string TABLEID, string PARTNERCODE, string LOCALCODE)
        {
            if (String.IsNullOrWhiteSpace(LOCALCODE))
            {
                return GetTranslationP2L(PARTNERID, TABLEID, PARTNERCODE);
            }
            else if (String.IsNullOrWhiteSpace(PARTNERCODE))
            {
                return GetTranslationL2P(PARTNERID, TABLEID, LOCALCODE);
            }
            return (from a in context.GTRTRANs
                    where a.PARTNERID == PARTNERID && a.TABLEID == TABLEID && a.PARTNERCODE == PARTNERCODE && a.LOCALCODE == LOCALCODE
                    select a).FirstOrDefault();
        }

        public IQueryable<GTRTRAN> GetAll()
        {
            return from a in context.GTRTRANs
                   select a;
        }

        public void Add(GTRTRAN entity)
        {
            context.GTRTRANs.Add(entity);
        }

        public void Remove(GTRTRAN entity)
        {

            AttachIfNot(entity);context.SetAsModified(entity); //context.GTRTRANs.Attach(entity);
            //context.AddToGTRTRANs 
            context.GTRTRANs.Remove(entity);
        }

        public void Update(GTRTRAN entity)
        {
            AttachIfNot(entity);context.SetAsModified(entity);

            //context.SetAsModified(entity);
        }
        void AttachIfNot(GTRTRAN entity)
        {
            // if ((entity as EntityObject).EntityState == System.Data.EntityState.Unchanged)
            {
                context.GTRTRANs.Attach(entity);
            }
        }

        public List<GTRTRAN> All()
        {
            return context.GTRTRANs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<GTRTRAN> GetMulti(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as GTRTRANParentKeys;
            var list = (from a in context.GTRTRANs
                       where a.PARTNERID == keys.PARTNERID && a.TABLEID == keys.TABLEID 
                       select a).ToList();
            return list;
        }

        public GTRTRAN GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as GTRTRANKeys;
            return this.GetSingle(keys.PARTNERID, keys.TABLEID, keys.PARTNERCODE, keys.LOCALCODE);
        }

        public GTRTRAN GetTranslationP2L(string partnerID, string tableID, string partnerCode)
        {
           
            var rec = (from a in context.GTRTRANs
                       where a.PARTNERID == partnerID && a.TABLEID == tableID && a.PARTNERCODE == partnerCode
                       select a).FirstOrDefault();
            if (rec == null)
            {
                return null;
            }
            return rec;
        }

        

        public GTRTRAN GetTranslationL2P(string partnerID, string tableID, string localCode)
        {
            var rec = (from a in context.GTRTRANs
                       where a.PARTNERID == partnerID && a.TABLEID == tableID && a.LOCALCODE == localCode
                       select a).FirstOrDefault();
            if (rec == null)
            {
                return null;
            }
            return rec;
        }
    }
}
	 