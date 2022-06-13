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
    public partial class AvailableStatusFieldQueryService
    {
        public string GetAvailableFieldByStatusFieldType(int tenant, string statusFieldType)
        {
            var availableField = repository.GetAvailableFieldByStatusFieldType(tenant, statusFieldType);
            return availableField;
        }

    }
}
