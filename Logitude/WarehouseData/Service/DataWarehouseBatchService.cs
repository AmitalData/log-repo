using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using WarehouseData.Helper;

namespace WarehouseData.Service
{
    public class DataWarehouseBatchService : GeneralDataWarehouseService
    {

        private string connectionString = string.Empty;
        private TableClass table = null;
        private string rowNumberColumnName = string.Empty;
        private DataWarehouseBatchServiceArgs dataWarehouseBatchServiceArgs = null;
        private long timeOut = 10000000000000000;
        private int dataCounts = 0;
        private int batchSize;
        private List<int> batchesNumbers = new List<int>();


        public DataWarehouseBatchService() {}


        public DataWarehouseBatchService(DataWarehouseBatchServiceArgs dataWarehouseBatchServiceArgs)
        {
            this.dataWarehouseBatchServiceArgs = dataWarehouseBatchServiceArgs;
            table = dataWarehouseBatchServiceArgs.table;
            connectionString = dataWarehouseBatchServiceArgs.ConnectionString;
            rowNumberColumnName = "RowNumber_" + table.DWObjectTableCode;
            dataCounts = GetDataCount();
            batchSize = dataCounts / dataWarehouseBatchServiceArgs.table.BatchesCount;
            batchesNumbers = Enumerable.Range(0, (table.BatchesCount + 1)).ToList();
        }


        public void ExecuteScriptAsBatches()
        {
            UpdateRowNumberField();
            Parallel.ForEach(batchesNumbers, (batchNumber) =>{ ExecuteScriptAsBatch(batchNumber); });
        }


        private void ExecuteScriptAsBatch(int batchNumber)
        {
            string queryString = Regex.Replace(dataWarehouseBatchServiceArgs.QueryString, "[$]LastCounterWhere[$]", (table.Dw_TableName + "." + rowNumberColumnName + " = " + batchNumber), RegexOptions.IgnoreCase);
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand(queryString, sqlConnection);
            sqlCommand.CommandTimeout = (int)timeOut;
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }



        private void UpdateRowNumberField()
        {
            batchesNumbers.ForEach((batchNumber) => {

                ExecuteSql("UPDATE TOP(" + batchSize.ToString() + ")" + "[" + table.Dw_TableName + "] SET " + rowNumberColumnName + " = " + batchNumber.ToString() + " WHERE " + rowNumberColumnName + " is null", connectionString);
            });
        }


        public string AddRowNumberField(TableClass table,string factTableName , string connectionString)
        {
            string columnName = "RowNumber_" + factTableName;
            string sql = "ALTER TABLE " + table.Dw_TableName + " Add " + columnName + " int  NULL \n";
            sql += "CREATE NONCLUSTERED INDEX [IX_" + table.Dw_TableName + "_" + columnName + "] ON[dbo].[" + table.Dw_TableName + "]([" + columnName + "])";

            ExecuteSql(sql, connectionString);
            return columnName;
        }


        private int GetDataCount()
        {
            int result = 0;
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            SqlCommand countRowDetail = new SqlCommand("SELECT COUNT(*) FROM dbo." + table.Dw_TableName, sqlConnection);
            result=  System.Convert.ToInt32(countRowDetail.ExecuteScalar());
            sqlConnection.Close();
            return result;
        }
    }


    public class DataWarehouseBatchServiceArgs
    {
        public string QueryString { get; set; }
        public TableClass table { get; set; }
        public string ConnectionString { get; set; }


    }


}
