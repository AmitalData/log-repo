using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Server.Tools;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class CustomsRequiredFieldQueryService : EntityQueryService<CustomsRequiredField, CustomsRequiredFieldKeys, CustomsRequiredFieldPM, object, CustomsRequiredFieldKeys>
    {
        public List<CustomsRequiredFieldPM> GetCustomRequiredFieldsByObjectTable(string ObjectTableId, int Tenant)
        {
            CustomsRequiredFieldRepository rep = new CustomsRequiredFieldRepository(context);
            List<CustomsRequiredField> requiredFields = rep.GetCustomRequiredFieldsByObjectTable(ObjectTableId,Tenant);
            List<CustomsRequiredFieldPM> requiredFieldsPms = new List<CustomsRequiredFieldPM>();
            foreach (CustomsRequiredField field in requiredFields)
            {
                CustomsRequiredFieldPM requiredFieldpm = new CustomsRequiredFieldPM();
                mapping.CustomPOCOToPM(requiredFieldpm, field);
                mapping.POCOToPM(requiredFieldpm, field);
                requiredFieldsPms.Add(requiredFieldpm);
            }

            return requiredFieldsPms;
        }

        public CustomsRequiredFieldPM GetCustomRequiredFieldsByObjectFieldCode(string ObjectFieldCode, int Tenant)
        {
            CustomsRequiredFieldRepository rep = new CustomsRequiredFieldRepository(context);
            CustomsRequiredField requiredFields = rep.GetCustomRequiredFieldsByObjectFieldCode(ObjectFieldCode, Tenant);
            CustomsRequiredFieldPM requiredFieldsPms = new CustomsRequiredFieldPM();
           
            CustomsRequiredFieldPM requiredFieldpm = new CustomsRequiredFieldPM();
            mapping.CustomPOCOToPM(requiredFieldpm, requiredFields);
            mapping.POCOToPM(requiredFieldpm, requiredFields);
            requiredFieldsPms = requiredFieldpm;
            

            return requiredFieldsPms;
        }

    }
}
