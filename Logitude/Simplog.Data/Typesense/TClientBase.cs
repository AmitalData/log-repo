using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Simplog.Data.Typesense
{
    public class TClientBase<T>
    {
        public readonly TClient client;
        public readonly string TableName;
        public readonly string default_sorting_field;
        internal Field[] fields;

        public TClientBase(string apiKey, string url, string tableName, string default_sorting_field, Field[] fields)
        {
            client = new TClient(apiKey, url);
            TableName = tableName;
            this.default_sorting_field = default_sorting_field;
            this.fields = fields;
        }

        public async Task CreateTable()
        {
            CreateTableRequest schema = new CreateTableRequest
            {
                name = TableName,
                fields = fields,
                default_sorting_field = default_sorting_field
            };

            await client.CreateTableAsync(schema);
        }

        public async Task Insert(DataTable dt)
        {
            List<T> objList = new List<T>();

            foreach (DataRow row in dt.Rows)
            {
                T obj = DataRowToObject(row);
                objList.Add(obj);
            }

            await client.InsertAsync(TableName, objList.ToArray());
        }

        virtual public T DataRowToObject(DataRow row)
        {
            throw new NotImplementedException();
        }

        public async Task InsertAsync(T record) => await client.UpsertAsync(TableName, record);

        public async Task DropTableAsync() => await client.DropTableAsync(TableName);

        public async Task ReCreateTableAsync(DataTable dt = null)
        {
            try
            {
                await DropTableAsync();
            }
            catch { }

            await CreateTable();

            if (dt != null)
                await Insert(dt);
        }

        public async Task<List<T>> SearchAsync(string value, string[] columns, string filterBy = null)
        {            
            List<T> responseList = await client.SearchAsync<T>(TableName, columns, value, filterBy);
            return responseList;
        }

        public async Task<List<T>> SearchByFilterOnlyAsync(string filter) =>
            await client.SearchAsync<T>(TableName, "*", "*", filter);

        protected int GetIntDataRow(DataRow row, string columnName) => row[columnName] == DBNull.Value ? 0 : Convert.ToInt32(row[columnName]);
        protected bool GetBooleanDataRow(DataRow row, string columnName) => row[columnName] == DBNull.Value ? false : Convert.ToBoolean(row[columnName]);
    }
}