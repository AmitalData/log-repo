using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class PropertiesDetailsHistoryDomainService
    {
        public PropertiesDetailsHistoryPM GetPropertiesDetailsHistoryByCustomsItemId(string customsItemId, int tenant)
        {
            if (MyContext == null)
            {
                MyContext = CustomContext.GetContext(tenant);
            }

            PropertiesDetailsHistoryQueryService propertiesDetailsHistoryQuery = new PropertiesDetailsHistoryQueryService(MyContext);
            PropertiesDetailsHistoryPM propertiesDetailsHistory = propertiesDetailsHistoryQuery.GetPropertiesDetailsHistoryByCustomsItemId(customsItemId);
            return propertiesDetailsHistory;

        }
    }
}