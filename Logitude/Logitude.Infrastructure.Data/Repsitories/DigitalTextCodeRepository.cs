using System;
using System.Collections.Generic;
using System.Linq;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Logitude.Infrastructure.Data.Repsitories
{
    public partial class DigitalTextCodeRepository:IRepository<DigitalTextCode>
    {
		public List<DigitalTextCode> GetMulti(EntityKeyFields entityKeys)
        {
			throw new NotImplementedException();
        }

        public IQueryable<DigitalTextCode> GetDigitalTextCodes(int tenant, string objectTableId)
        {
            return context.DigitalTextCodes
                          .Where(a => a.Tenant == tenant
                                      && a.ObjectTableId.Equals(objectTableId));
        }

        public bool CheckTenantTranslation(int tenant, string objectTableId)
        {
            return context.DigitalTextCodes
                          .Any(a => a.Tenant == tenant
                                    && a.ObjectTableId.Equals(objectTableId));
        }
    }
}