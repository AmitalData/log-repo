using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Accounting.BL.EntityUpdateServices;

namespace Logitude.Accounting.BL.EntityQueryServices
{
    public partial class AutomaticReconcileMethodQueryService : EntityQueryService<AutomaticReconcileMethod, AutomaticReconcileMethodKeys, AutomaticReconcileMethodPM, object, AutomaticReconcileMethodKeys>
    {
        public bool CheckWhetherCodeExists(string code, string id, int tenant)
        {
            return this.repository.CheckWhetherCodeExists(code, id, tenant);
        }

        public bool CheckUniqueMethods(string method1, string method2, string method3, int tenant)
        {
            return this.repository.CheckUniqueMethods(method1, method2, method3,tenant);
        }

        public AutomaticReconcileMethodPM GetSinglePM(string id, int tenant)
        {

            return (from a in context.AutomaticReconcileMethods// repository.All()
                    where a.Id == id
                    select new AutomaticReconcileMethodPM()
                    {
                        Code = a.Code,
                        AutomaticReconcile1 = a.AutomaticReconcile1,
                        AutomaticReconcile2 = a.AutomaticReconcile2,
                        AutomaticReconcile3 = a.AutomaticReconcile3,
                        
                        SearchFields = a.SearchFields,
                    }).FirstOrDefault();
        }

        public void CopyFromTenant0(int tenant, int tenatToCopy)
        {
            AutomaticReconcileMethodUpdateService service = new AutomaticReconcileMethodUpdateService(context, new Dictionary<string, IContext>(), tenatToCopy);
            List<AutomaticReconcileMethod> pocos = repository.GetAll(tenant).Where(a => !a.Inactive).ToList();
            foreach (var item in pocos)
            {
                AutomaticReconcileMethodPM automaticReconcileMethod = new AutomaticReconcileMethodPM();
                automaticReconcileMethod.Tenant = tenatToCopy;
                automaticReconcileMethod.Code =item.Code;
                automaticReconcileMethod.AutomaticReconcile1 = item.AutomaticReconcile1;
                automaticReconcileMethod.AutomaticReconcile2 = item.AutomaticReconcile2;
                automaticReconcileMethod.AutomaticReconcile3 = item.AutomaticReconcile3;
                                automaticReconcileMethod.Inactive = item.Inactive;

                automaticReconcileMethod.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                service.Update(automaticReconcileMethod, true);

            }
            this.context.SaveChanges();

        }


    }
}
