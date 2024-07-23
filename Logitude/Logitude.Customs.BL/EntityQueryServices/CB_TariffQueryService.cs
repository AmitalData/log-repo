 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
using Logitude.Customs.Data.EntityLists;

namespace Logitude.Customs.BL.EntityQueryServices
{ 
   public partial class CB_TariffQueryService: EntityQueryService<CB_Tariff,CB_TariffKeys,CB_TariffPM,object,CB_TariffKeys>
   {

        public List<CB_TariffList> GetCustomsBookAgreementLevelData(int customsItemId, int measurementUnitMalamId)
        {
            return this.repository.GetCustomsBookAgreementLevelData(customsItemId, measurementUnitMalamId);
        }      
        public List<CB_TariffList> GetCustomsBookTaxRates(int customsItemId)
        {
            return this.repository.GetCustomsBookTaxRates(customsItemId);
        }

    }
   
}
	 