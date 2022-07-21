using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.BL
{
    public class ExternalFieldMappingBL
    {
        public string GetAvailableFieldByStatusFieldType(int tenant, string statusFieldType)
        {
            DeclarationStatus myObject = new DeclarationStatus();
            string FieldName = "";
            switch (statusFieldType)
            {
                case "1":
                    FieldName = "FieldC";
                    break;
                case "2":
                    FieldName = "FieldD";
                    break;
                case "3":
                    FieldName = "FieldR";
                    break;
                default:
                    break;
            }
            foreach (PropertyInfo pi in myObject.GetType().GetProperties())
            {
                if (pi.Name.Contains(FieldName))
                {
                    var externalFieldMappingQueryService = new ExternalFieldMappingQueryService(tenant);
                    var IsUsed = externalFieldMappingQueryService.CheckIfFieldIsUsed(pi.Name, tenant);
                    if (!IsUsed)
                    {
                        return pi.Name;
                    }
                }
            }
            return null;

        }
    }
}
