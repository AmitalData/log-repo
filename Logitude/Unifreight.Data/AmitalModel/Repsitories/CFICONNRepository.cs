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
    public class CFICONNRepository : IRepository<CFICONN>
    {
        private AmitalContext currentContext;
        public CFICONNRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CFICONNRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CFICONN GetSingle(string FILENO, long CUSTOMFILE)
        {
            return (from a in context.CFICONNs
                    where a.FILENO == FILENO && a.CUSTOMFILE == CUSTOMFILE
                    select a).FirstOrDefault();
        }

        public IQueryable<CFICONN> GetAll()
        {
            return from a in context.CFICONNs
                   select a;
        }

        public List<CFICONN> GetImportFilesByCustomFile(long CUSTOMFILE)
        {
            return (from a in context.CFICONNs
                    where a.CUSTOMFILE == CUSTOMFILE
                    select a).ToList();
        }

        public void Add(CFICONN entity)
        {
            context.CFICONNs.Add(entity);
        }

        public void Remove(CFICONN entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CFICONNs.Attach(entity);
            }
            context.CFICONNs.Remove(entity);
        }

        public void Update(CFICONN entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CFICONNs.Attach(entity); context.SetAsModified(entity);
            }

        }

        public List<CFICONN> All()
        {
            return context.CFICONNs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CFICONN> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CFICONN GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CFICONNKeys;
            return this.GetSingle(keys.FILENO, keys.CUSTOMFILE);
        }
    }
}

