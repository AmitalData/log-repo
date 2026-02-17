using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class ProductTypeMapping
    {
        public static void MapEntity(ProductTypePM entityPM, ProductType poco, bool isNewEntity, ProductTypeModification modifiation)
        {
            modifiation.InActive = entityPM.InActive;
            modifiation.QuotationDefaultTemplateId = entityPM.QuotationDefaultTemplateId;
            
        }
    }
}
