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
        public static void Validate(ProductItemPM productItemPM, ICommonDataContext commonContext, bool isNew)
        {
            bool isExist = false;

            if (isNew)
            {
                isExist = (from a in commonContext.ProductItems
                           where a.SKU == productItemPM.SKU && a.Tenant == productItemPM.Tenant && a.CustomerId == productItemPM.CustomerId
                           select a).Any();
            }

            else
            {
                isExist = (from a in commonContext.ProductItems
                           where a.SKU == productItemPM.SKU && a.Tenant == productItemPM.Tenant && a.CustomerId == productItemPM.CustomerId && a.Id != productItemPM.Id
                           select a).Any();
            }

            if (isExist)
            {
                throw new ApplicationException("A Product Item with same SKU already exists");
            }
        }
    }
}
