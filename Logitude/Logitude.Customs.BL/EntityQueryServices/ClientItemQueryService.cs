using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;


namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class ClientItemQueryService : EntityQueryService<ClientItem, ClientItemKeys, ClientItemPM, object, ClientItemKeys>
    {


        public ClientItemPM GetSingleWithTenant(string itemCode, string exporterCode, int tenant)
        {
            var poco = repository.GetSingleWithTenant(itemCode, exporterCode, tenant);
            if(poco == null)
            {
                return null;
            }
            var pm = this.GetEntityPM(poco);
            return pm;
        }

        public ClientItemPM GetSingleWithTenantDescription(string itemDescription, string exporterCode, int tenant)
        {
            var poco = repository.GetSingleWithTenantDescription(itemDescription, exporterCode, tenant);
            if(poco == null)
            {
                return null;
            }
            var pm = this.GetEntityPM(poco);
            return pm;
        }
    }
}
