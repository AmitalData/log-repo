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

        public IQueryable<DigitalTextCode> GetDigitalTextCodes(int tenant, string objectTableId, string profileId)
        {
            return context.DigitalTextCodes
                          .Where(a => a.Tenant == tenant
                                      && a.ObjectTableId.Equals(objectTableId)
                                      && a.ProfileId.Equals(profileId));
        }
    }
}