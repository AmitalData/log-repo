using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseData.Helper;

namespace WarehouseData.Service
{
    public class EnvironmentDWTableUnUseSqlProvider
    {
        private GeneralDataWarehouseService generalDataWarehouseService = null;
        private StringBuilder sqlStringBuilder = null;
        private List<TableClass> dataWarehouseTables = null;
        private List<TableClass> factTables = null;
        private List<TableClass> dimTables = null;
        private List<TableClass> dwTables = null;


        public EnvironmentDWTableUnUseSqlProvider(string connectionString)
        {
            generalDataWarehouseService = new GeneralDataWarehouseService();
            dataWarehouseTables = GetDWTableListsNotUsedInEnvironment(connectionString);
            factTables = dataWarehouseTables.Where(d => d.HasFactTable).ToList();
            dimTables = dataWarehouseTables.Where(d => d.HasDimensionTable).ToList();
            dwTables = dataWarehouseTables.Where(d => !d.HasDimensionTable && !d.HasFactTable).ToList();
        }


        public string GetUnUseDataWarehouseSQL()
        {
            sqlStringBuilder = new StringBuilder();
            AppendDataWarehouseUnUseSql(factTables);
            AppendDataWarehouseUnUseSql(dimTables);
            AppendDataWarehouseUnUseSql(dwTables);
            return sqlStringBuilder.ToString();
        }


        private void AppendDataWarehouseUnUseSql(List<TableClass> dataWarehouseTables)
        {
            foreach (TableClass tableClass in dataWarehouseTables)
            {
                sqlStringBuilder.Append("IF OBJECT_ID('" + tableClass.Dw_TableName + "', 'U')  IS NOT NULL begin drop table " + tableClass.Dw_TableName + " end \r\n");
                if (!string.IsNullOrEmpty(tableClass.DWObjectTableCode))
                {
                    sqlStringBuilder.Append("IF OBJECT_ID('" + tableClass.DWObjectTableCode + "', 'U')  IS NOT NULL begin drop table " + tableClass.DWObjectTableCode + " end \r\n");
                }
            }

        }

        private List<TableClass> GetDWTableListsNotUsedInEnvironment(string connectionString)
        {
            List<TableClass> dataWarehouseTables = generalDataWarehouseService.FillDataWarehouseTable();
            List<string> environmentFactTableCodes = generalDataWarehouseService.GetEnvironmentFactTables(connectionString);
            dataWarehouseTables = dataWarehouseTables.Where(dwTable => !environmentFactTableCodes.Any(environmentFactCode => dwTable.RelatedFactTables.Any(factCode => factCode == environmentFactCode))).ToList();
            return dataWarehouseTables;
        }


    }
}
