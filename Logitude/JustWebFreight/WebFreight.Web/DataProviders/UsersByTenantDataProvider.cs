using Logitude.BL.CommonDataModel.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataProviders
{
    public class UsersByTenantDataProvider: BaseDataProvider
    {
        public List<UsersByTenantItem> UsersByTenantLists { get; set; }

    }

  

}