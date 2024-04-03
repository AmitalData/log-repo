using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDataViews.Service
{
  public  class FactAdditionalConditionService: GeneralDataWarehouseViewsService
    {
        private  string connectionString = string.Empty;
        private CreateDataWarehouseFactViewArgs createDataWarehouseFactViewArgs = null;
        public FactAdditionalConditionService(CreateDataWarehouseFactViewArgs createDataWarehouseFactViewArgs , string connectionString)
        {
            this.createDataWarehouseFactViewArgs = createDataWarehouseFactViewArgs;
            this.connectionString = connectionString;

        }


        public string Get()
        {
            if (string.IsNullOrEmpty(createDataWarehouseFactViewArgs.AdditionalConditions) && string.IsNullOrEmpty(createDataWarehouseFactViewArgs.ParentFactCode)) return null;
            if (!string.IsNullOrEmpty(createDataWarehouseFactViewArgs.AdditionalConditions)) return createDataWarehouseFactViewArgs.AdditionalConditions;
            return GetAdditionalConditionsByFactCode(createDataWarehouseFactViewArgs.ParentFactCode);
        }


        public string GetAdditionalConditionsByFactCode(string factCode)
        {
            string result = string.Empty;
            DataTable dataTable = GetDataTableFromSql(connectionString, "select AdditionalConditions from DWObjectTables where Code ='" + factCode+"'");
            if (dataTable == null || dataTable.Rows == null) return null;
            return dataTable.Rows[0]["AdditionalConditions"].ToString();
        }




    }
}
