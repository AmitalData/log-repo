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
    public class CCUTAXRepository : IRepository<CCUTAX>
    {
        private AmitalContext currentContext;
        public CCUTAXRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CCUTAXRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CCUTAX GetSingle(int FILENO, int LINENO)
        {
            return (from a in context.CCUTAXES 
                    where a.FILENO == FILENO && a.LINENO == LINENO
                    select a).FirstOrDefault();
        }

        public IQueryable<CCUTAX> GetAll()
        {
            return from a in context.CCUTAXES
                   select a;
        }

        public void Add(CCUTAX entity)
        {
            context.CCUTAXES.Add(entity);
        }

        public void Remove(CCUTAX entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CCUTAXES.Attach(entity);
            }
            //context.AddToCCUTAXES 
            context.CCUTAXES.Remove(entity);
        }

        public void Update(CCUTAX entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CCUTAXES.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<CCUTAX> All()
        {
            return context.CCUTAXES.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CCUTAX> GetMulti(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CCUFILEMKeys;

            return (from a in context.CCUTAXES
                    where a.FILENO == keys.FILENO
                    select a).ToList();
        }

        public CCUTAX GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CCUTAXKeys;
            return this.GetSingle(keys.FILENO, keys.LINENO);
        }

        public int FastDeleteMulti(CCUFILEMKeys parentEntityKeys, string taxType = null)
        {
            var keys = parentEntityKeys as CCUFILEMKeys;
            switch (taxType)
            {
                case null:
                    return context.DeleteWhere<CCUTAX>(rec => rec.FILENO == keys.FILENO);
                    break;
                case "Declaration":
                    return context.DeleteWhere<CCUTAX>(rec => rec.FILENO == keys.FILENO && (rec.PRATMEHES == null || rec.PRATMEHES == ""));
                    break;
                case "SupplierInvoiceItem":
                    return context.DeleteWhere<CCUTAX>(rec => rec.FILENO == keys.FILENO && rec.PRATMEHES != null && rec.PRATMEHES != "");
                    break;
                default:
                    return context.DeleteWhere<CCUTAX>(rec => rec.FILENO == keys.FILENO);
                    break;
            }
        }
    }
}
	 
