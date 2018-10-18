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
  public class CustomerTenantAccessStatusTypeMapping
    {
      public static void MapEntity(CustomerTenantAccessStatusTypePM entityPM, CustomerTenantAccessStatusType poco, bool isNewEntity)
      {
          poco.Code = entityPM.Code;
          poco.EnglishName = entityPM.EnglishName;
          poco.LocalName = entityPM.LocalName;

          BuildSearchFields(entityPM, poco);
      }

      private static void BuildSearchFields(CustomerTenantAccessStatusTypePM entityPM, CustomerTenantAccessStatusType poco)
      {
          string mySearchFields = "";

          MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Code);
          MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.EnglishName);
          MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.LocalName);
        

          if (mySearchFields.Length > 52)
          {
              mySearchFields = mySearchFields.Substring(0, 52);
          }

          entityPM.SearchFields = mySearchFields;
          poco.SearchFields = mySearchFields;
      }
    
    }
}
