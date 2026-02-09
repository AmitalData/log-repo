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

        public CCUTAX GetSingle(int FILENO, int LINENO , int? tenant)
        {
            return (from a in context.CCUTAXES 
                    where a.FILENO == FILENO && a.LINENO == LINENO && a.TENANT == tenant
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
            SyncRecordCache.ClearCacheLastSync(entity.FILENO.ToString(), entity.TENANT.Value);
        }

        public void Remove(CCUTAX entity)
        {
             {
                context.CCUTAXES.Attach(entity);
            }
             context.CCUTAXES.Remove(entity);
            SyncRecordCache.ClearCacheLastSync(entity.FILENO.ToString(), entity.TENANT.Value);
        }

        public void Update(CCUTAX entity)
        {
             {
                context.CCUTAXES.Attach(entity); context.SetAsModified(entity);
            }
            SyncRecordCache.ClearCacheLastSync(entity.FILENO.ToString(), entity.TENANT.Value);
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
                    where a.FILENO == keys.FILENO && a.TENANT == keys.TENANT
                    select a).ToList();
        }

        public CCUTAX GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CCUTAXKeys;
            return this.GetSingle(keys.FILENO, keys.LINENO, keys.Tenant);
        }

        public int FastDeleteMulti(CCUFILEMKeys parentEntityKeys, string taxType = null)
        {
            var keys = parentEntityKeys as CCUFILEMKeys;
            switch (taxType)
            {
                case null:
                    return context.DeleteWhere<CCUTAX>(rec => rec.FILENO == keys.FILENO && rec.TENANT == keys.TENANT);
                    break;
                case "Declaration":
                    return context.DeleteWhere<CCUTAX>(rec => rec.FILENO == keys.FILENO  && (rec.PRATMEHES == null || rec.PRATMEHES == "") && rec.TENANT == keys.TENANT);
                    break;
                case "SupplierInvoiceItem":
                    return context.DeleteWhere<CCUTAX>(rec => rec.FILENO == keys.FILENO && rec.PRATMEHES != null && rec.PRATMEHES != "" && rec.TENANT == keys.TENANT);
                    break;
                default:
                    return context.DeleteWhere<CCUTAX>(rec => rec.FILENO == keys.FILENO && rec.TENANT == keys.TENANT);
                    break;
            }
        }
    }
}
	 
