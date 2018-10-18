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
    public class GAQFILEDATARepository : IRepository<GAQFILEDATA>
    {
        private AmitalContext currentContext;
        public GAQFILEDATARepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public GAQFILEDATARepository(AmitalContext context)
        {
            currentContext = context;
        }

        public GAQFILEDATA GetSingle(string ENTNAME, string PRIMARYNUM, string APPQID ,string PATH, string FIELDID)
        {
            return (from a in context.GAQFILEDATAs
                    where a.ENTNAME == ENTNAME && a.PRIMARYNUM == PRIMARYNUM && a.APPQID==APPQID &&
                    a.PATH == PATH && a.FIELDID == FIELDID
                    select a).FirstOrDefault();
        }

        public IQueryable<GAQFILEDATA> GetAll()
        {
            return from a in context.GAQFILEDATAs
                   select a;
        }

        public void Add(GAQFILEDATA entity)
        {
            context.GAQFILEDATAs.Add(entity);
        }

        public void Remove(GAQFILEDATA entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.GAQFILEDATAs.Attach(entity);
            }
            //context.AddToCCUPAYLINEFs 
            context.GAQFILEDATAs.Remove(entity);
        }

        public void Update(GAQFILEDATA entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.GAQFILEDATAs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<GAQFILEDATA> All()
        {
            return context.GAQFILEDATAs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<GAQFILEDATA> GetMulti(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as GAQFILEDATAKeys;

            return (from a in context.GAQFILEDATAs
                    where a.ENTNAME == keys.ENTNAME && a.PRIMARYNUM == keys.PRIMARYNUM && a.PATH == keys.PATH
                    select a).ToList();
        }

        public GAQFILEDATA GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as GAQFILEDATAKeys;
            return this.GetSingle(keys.ENTNAME, keys.PRIMARYNUM, keys.APPQID, keys.PATH, keys.FIELDID);
        }

        public List<GAQFILEDATA> GetEntity(string ENTNAME, string PRIMARYNUM, string APPQID)
        {
            return (from a in context.GAQFILEDATAs
                    where a.ENTNAME == ENTNAME && a.PRIMARYNUM == PRIMARYNUM && a.APPQID == APPQID
                    select a).ToList();
        }

        public int FastDeleteMultiEntityField(string ENTNAME, string PRIMARYNUM, string APPQID, string FIELDID)
        {

            return context.DeleteWhere<GAQFILEDATA>(rec => rec.ENTNAME == ENTNAME && rec.PRIMARYNUM == PRIMARYNUM && rec.APPQID == APPQID && rec.FIELDID == FIELDID);
        }
        
    }
}
	 