using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Unifreight.BL.EntityPMs.UGenerated;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;
using Unifreight.Data.AmitalModel.EntityKeys;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class DefaultValueQueryService : EntityQueryService<DefaultValue, DefaultValueKeys, DefaultValuePM, object, DefaultValueKeys>
    {


        public string GetDefault(string Distr, string DefaultTypeCode, string BranchCode, string CardCode , int Tenant)
        {

            if (!CustomsSettingQueryService.GetSettingByTenant(Tenant).IsConnectedToUniFreight)
            {
                if (Distr == null || DefaultTypeCode == null || BranchCode == null || CardCode == null)
                {
                    return ("");
                }

                string myDefaultValue = repository.GetDefaultValue_Cache(Distr, DefaultTypeCode, BranchCode, CardCode, Tenant);
            
                return myDefaultValue == null ? "" : myDefaultValue;
            }
            else
            {

                AmitalContext amitalContext = AmitalContext.GetContext(Tenant);
                var myGDFDATAQueryService = new GDFDATAQueryService(amitalContext);

                if (Distr == null || DefaultTypeCode == null || BranchCode == null || CardCode == null)
                {
                    return ("");
                }

                GDFDATAPM myGDFDATAPM = myGDFDATAQueryService.GetSingle(Distr, DefaultTypeCode, BranchCode, CardCode, false, true);
                if (myGDFDATAPM == null)
                {
                    return ("");
                }
                return (myGDFDATAPM.DEFDATA);
            }
           
        }
        public string GetDefaultByCardId(string Distr, string DefaultTypeCode, string BranchCode, string CardId, int Tenant)
        {

            if (!CustomsSettingQueryService.GetSettingByTenant(Tenant).IsConnectedToUniFreight)
            {
                if (Distr == null || DefaultTypeCode == null || BranchCode == null || CardId == null)
                {
                    return ("");
                }

                string myDefaultValue = repository.GetDefaultByCardId_Cache(Distr, DefaultTypeCode, BranchCode, CardId, Tenant);

                return myDefaultValue == null ? "" : myDefaultValue;
            }
            else
            {

                AmitalContext amitalContext = AmitalContext.GetContext(Tenant);
                var myGDFDATAQueryService = new GDFDATAQueryService(amitalContext);

                if (Distr == null || DefaultTypeCode == null || BranchCode == null || CardId == null)
                {
                    return ("");
                }

                GDFDATAPM myGDFDATAPM = myGDFDATAQueryService.GetSingle(Distr, DefaultTypeCode, BranchCode, CardId, false, true);
                if (myGDFDATAPM == null)
                {
                    return ("");
                }
                return (myGDFDATAPM.DEFDATA);
            }

        }

        public string GetDefaultAccountNumber(string Distr, string DefaultTypeCode, string BranchCode, string ShortValue, int Tenant)
        {

            if (Distr == null || DefaultTypeCode == null || BranchCode == null || ShortValue == null)
            {
                return ("");
            }

            string accountNumber = repository.GetDefaultAccountNumberByDefaultValue(Distr, DefaultTypeCode, BranchCode, ShortValue, Tenant);

            return accountNumber;
        }
        public List<string> GetCardIdsByDefaultValue(string Distr, string DefaultTypeCode, List<string> groups, int Tenant)
        {
            List<string> cardIds = repository.GetCardIdsByDefaultValue(Distr, DefaultTypeCode, groups, Tenant);
            return cardIds;
        }


    }
}
