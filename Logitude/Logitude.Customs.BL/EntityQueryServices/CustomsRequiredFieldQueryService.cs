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
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class CustomsRequiredFieldQueryService : EntityQueryService<CustomsRequiredField, CustomsRequiredFieldKeys, CustomsRequiredFieldPM, object, CustomsRequiredFieldKeys>
    {


        public List<CustomsRequiredFieldPM> GetCustomRequiredFieldsByObjectTable(string ObjectTableId, int Tenant, string type = "A")

        {
            string entityKeyString = $"GetCustomRequiredFieldsByObjectTableFromCache({ObjectTableId},{Tenant},{type})";
            var res = CacheManager.GetOrInsertNewObject<List<CustomsRequiredFieldPM>>(entityKeyString, () =>
            {

                return this.GetCustomRequiredFieldsByObjectTableNoCache(ObjectTableId, Tenant, type);

            });
            return res;
        }
        /*
        private List<CustomsRequiredFieldPM> GetCustomRequiredFieldsByObjectTableCore(string ObjectTableId, int Tenant,string type="A")

 
        {
            string entityKeyString = $"GetCustomRequiredFieldsByObjectTableFromCache({ObjectTableId},{Tenant})";
            var res = CacheManager.GetOrInsertNewObject<List<CustomsRequiredFieldPM>>(entityKeyString, () =>
            {
                return this.GetCustomRequiredFieldsByObjectTableCore(ObjectTableId, Tenant);
            });
            return res;
        }
        */
        public List<CustomsRequiredFieldPM> GetCustomRequiredFieldsByObjectTableNoCache(string ObjectTableId, int Tenant, string type = "A")

        {
            CustomsRequiredFieldRepository rep = new CustomsRequiredFieldRepository(context);
            List<CustomsRequiredField> requiredFields = rep.GetCustomRequiredFieldsByObjectTable(ObjectTableId, Tenant, type);
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
