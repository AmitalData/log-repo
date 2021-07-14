using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.Validating
{
    public class ProductItemValidating
    {
        public static void Validate(ProductItemPM productItemPM, CustomerPM customerPM)
        {
            int sameSKURecordscount = customerPM.CustomerProductItems.Where(a => a.SKU == productItemPM.SKU && a.Tenant == productItemPM.Tenant).Count();
             
            if (sameSKURecordscount > 1)
            {
                throw new ApplicationException("A Product Item with same SKU already exists");
            }
        }
    }
}
