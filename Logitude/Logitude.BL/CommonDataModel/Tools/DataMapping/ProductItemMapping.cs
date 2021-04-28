using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class ProductItemMapping
    {
        public static void MapEntity(ProductItemPM entityPM, ProductItem poco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                poco.Id = entityPM.Id;
                poco.Tenant = entityPM.Tenant;
            }

            poco.CustomerId = entityPM.CustomerId;
            poco.SKU = entityPM.SKU;
            poco.Remarks = entityPM.Remarks;
            poco.InActive = entityPM.InActive;
            poco.Description = entityPM.Description;
            BuildSearchField(entityPM, poco);
        }

        private static void BuildSearchField(ProductItemPM entityPM, ProductItem entityPoco)
        {
  
        }
    }
}
