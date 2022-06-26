using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class ExternalFieldMappingQueryService
    {
        public bool CheckIfFieldIsUsed(string field, int tenant)
        {
            var poco = this.repository.GetSingleByField(field, tenant);
            if(poco == null)
            {
                return false;
            }
            return true;
        }

    }
}
