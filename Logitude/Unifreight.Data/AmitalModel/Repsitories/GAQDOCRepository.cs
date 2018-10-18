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
    public class GAQDOCRepository : IRepository<GAQDOC>
    {
        private AmitalContext currentContext;
        public GAQDOCRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public GAQDOCRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public GAQDOC GetSingle(string APPQID, string FOLDERCODE, string DOCID)
        {
            return (from a in context.GAQDOCs
                    where a.APPQID == APPQID && a.FOLDERCODE == FOLDERCODE && a.DOCID == DOCID
                    select a).FirstOrDefault();
        }

        public IQueryable<GAQDOC> GetAll()
        {
            return from a in context.GAQDOCs
                   select a;
        }

        public void Add(GAQDOC entity)
        {
            context.GAQDOCs.Add(entity);
        }

        public void Remove(GAQDOC entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.GAQDOCs.Attach(entity);
            }
            //context.AddToGAQDOCs 
            //context.GAQDOCs.DeleteAllOnSubmit //ther isn't
            //http://forums.devart.com/viewtopic.php?t=13223
            //context.ExecuteStoreCommand 
            context.GAQDOCs.Remove(entity);
        }

        public void Update(GAQDOC entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.GAQDOCs.Attach(entity); context.SetAsModified(entity);
            }


        }

        public List<GAQDOC> All()
        {
            return context.GAQDOCs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<GAQDOC> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public GAQDOC GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as GAQDOCKeys;
            return this.GetSingle(keys.APPQID, keys.FOLDERCODE, keys.DOCID);
        }
    }
}

