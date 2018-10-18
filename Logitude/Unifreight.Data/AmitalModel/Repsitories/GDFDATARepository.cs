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
    public class GDFDATARepository : IRepository<GDFDATA>
    {
        private AmitalContext currentContext;
        public GDFDATARepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public GDFDATARepository(AmitalContext context)
        {
            currentContext = context;
        }

        public GDFDATA GetSingle(string DISTRID, string DEFID, string BRANCHID, string CARDID)
        {
            return (from a in context.GDFDATAs
                    where a.DISTRID == DISTRID && a.DEFID == DEFID && a.BRANCHID == BRANCHID && a.CARDID == CARDID
                    select a).FirstOrDefault();
        }

        public IQueryable<GDFDATA> GetAll()
        {
            return from a in context.GDFDATAs
                   select a;
        }

        public void Add(GDFDATA entity)
        {
            context.GDFDATAs.Add(entity);
        }

        public void Remove(GDFDATA entity)
        {

            AttachIfNot(entity);context.SetAsModified(entity); //context.GDFDATAs.Attach(entity);
            //context.AddToGDFDATAs 
            context.GDFDATAs.Remove(entity);
        }

        public void Update(GDFDATA entity)
        {
            AttachIfNot(entity);context.SetAsModified(entity);

            //context.SetAsModified(entity);
        }
        void AttachIfNot(GDFDATA entity)
        {
            // if ((entity as EntityObject).EntityState == System.Data.EntityState.Unchanged)
            {
                context.GDFDATAs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<GDFDATA> All()
        {
            return context.GDFDATAs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<GDFDATA> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public GDFDATA GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as GDFDATAKeys;
            return this.GetSingle(keys.DISTRID, keys.DEFID, keys.BRANCHID, keys.CARDID);
        }
    }
}
	 