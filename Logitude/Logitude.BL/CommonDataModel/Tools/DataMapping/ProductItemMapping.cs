using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
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
            poco.InActive = entityPM.InActive;
            poco.Description = entityPM.Description;
            poco.Name = entityPM.Name;
            poco.Brand = entityPM.Brand;
            poco.ASIN = entityPM.ASIN;
            poco.UPC = entityPM.UPC;
            poco.OriginCountryId = entityPM.OriginCountryId;

            BuildSearchField(entityPM, poco);
        }

        private static void BuildSearchField(ProductItemPM entityPM, ProductItem entityPoco)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.SKU);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Name);

            entityPM.SearchFields = mySearchFields;
            entityPoco.SearchFields = mySearchFields;
        }
    }
}
