 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Accounting.Data.Repositories
{
   public partial class AutomaticReconcileMethodRepository:IRepository<AutomaticReconcileMethod>
   {
        
		public List<AutomaticReconcileMethod> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public bool CheckWhetherCodeExists(string code, string id, int tenant)
        {
            bool exists;
            if (String.IsNullOrEmpty(id))
            {
                exists = (from a in context.AutomaticReconcileMethods
                          where a.Code == code && a.Tenant == tenant
                          select a).Any();
            }
            else
            {
                exists = (from a in context.AutomaticReconcileMethods
                          where a.Code == code && a.Tenant == tenant && a.Id != id
                          select a).Any();
            }
            return exists;
        }

        public bool CheckUniqueMethods(string method1, string method2, string method3, int tenant)
        {
            bool exists;
            exists = (from a in context.AutomaticReconcileMethods
                      where a.AutomaticReconcile1 == method1 && a.AutomaticReconcile2 == method2 && a.AutomaticReconcile3 == method3 
                            && a.Tenant == tenant
                        select a).Any();
            return exists;
        }


   }

}
   