 
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
   public partial class CB_CustomsItemComputedDataQueryService: EntityQueryService<CB_CustomsItemComputedData,CB_CustomsItemComputedDataKeys,CB_CustomsItemComputedDataPM,object,CB_CustomsItemComputedDataKeys>
   {
        public List<CB_CustomsItemComputedDataList> GetCustomsBookMainViewSearchByText(string searchFields, string customsBookType, string customsItemHierarchic, bool isReamarks, bool isRules, int tenant, bool isDiscountCodes)
        {
            return this.repository.GetCustomsBookMainViewSearchByText(searchFields, customsBookType, customsItemHierarchic, isReamarks, isRules, tenant, isDiscountCodes);
        }

        public List<CB_CustomsItemComputedDataList> GetCustomsBookMainViewSearchByClassification(string customsBookType, string fullClassification, int tenant, bool isDiscountCodes)
        {
            return this.repository.GetCustomsBookMainViewSearchByClassification(customsBookType, fullClassification, tenant, isDiscountCodes);
        }

        public List<CB_CustomsItemComputedDataList> GetCustomsBookMainView(string customsBookType, int tenant, bool isDiscountCodes)
        {
            return this.repository.GetCustomsBookMainView(customsBookType, tenant, isDiscountCodes);
        }

        public List<CustomsItemValidationResult> GetCustomsBookMainViewByList(string customsItems, string customsBookType, bool isDiscountCodes)
        {
            return this.repository.GetCustomsBookMainViewByList(customsItems, customsBookType, isDiscountCodes);
        }

    }
   
}
	 