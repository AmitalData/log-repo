using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;
using System.Data.Common;
using System.Data.SqlClient;
using System.Transactions;
using Logitude.Customs.Data.EntityLists;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class CB_CustomsItemQueryService : EntityQueryService<CB_CustomsItem, CB_CustomsItemKeys, CB_CustomsItemPM, object, CB_CustomsItemKeys>
    {
        public List<CB_CustomsItemList> GetCustomsBookMainViewSearchByText(string searchFields, string customsBookType, string customsItemHierarchic, bool isReamarks, bool isRules, int skippedRows, int pageSize)
        {
            return this.repository.GetCustomsBookMainViewSearchByText(searchFields, customsBookType, customsItemHierarchic, isReamarks, isRules, skippedRows, pageSize);
        }

        public List<CB_CustomsItemList> GetCustomsBookMainViewSearchByClassification(string searchFields, string customsBookType, string customsItemHierarchic, int skippedRows, int pageSize)
        {
            return this.repository.GetCustomsBookMainViewSearchByClassification(searchFields, customsBookType, customsItemHierarchic, skippedRows, pageSize);
        }

        public List<CB_CustomsItemList> GetCustomsBookMainView(string customsBookType, int skippedRows, int pageSize)
        {
            return this.repository.GetCustomsBookMainView(customsBookType, skippedRows, pageSize);
        }

    }
}
