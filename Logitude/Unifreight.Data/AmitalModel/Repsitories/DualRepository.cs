using Devart.Data.Oracle;
using Logitude.Customs.Data;
using Simplog.Global.Data.GlobalModel;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.Repsitories
{


    public class DualRepository
    {

        const string SELECT_SYSDATE_FROM_DUAL = "SELECT SYSDATE FROM DUAL";
        private DbContextBase _CurrentContext;
        //private static TimeSpan? _Delta_TimeSpan = null;
        public DualRepository(int tenant)
        {
            _CurrentContext = GlobalContext.GetContext() as DbContextBase;
        }

        public DualRepository(DbContext context)
        {
            _CurrentContext = context as DbContextBase;
        }
        public DateTime? GetServerDateTime(bool forceFromDB = true)
        {


            const int len = 11;
            int recCount = 0;
            DateTime? serverTime = null;
            var entityKeyString = "GetServerDateTime,Delta_TimeSpan";
            TimeSpan? _Delta_TimeSpan =CacheManager.CacheWrapper.Get(entityKeyString) as TimeSpan?;
            if (!forceFromDB && _Delta_TimeSpan != null)
            {
                var meTime = DateTime.Now;
                serverTime = meTime.Subtract(_Delta_TimeSpan.Value);
                return serverTime;
            }

            var OpenReaderSingleResult = new OpenReaderSingleResult(_CurrentContext);
            serverTime = OpenReaderSingleResult.ExecuteReaderSingleResult<DateTime>(SELECT_SYSDATE_FROM_DUAL,
                (dataReader) =>
            {
                return dataReader.GetDateTime(0);

            });

            _Delta_TimeSpan = DateTime.Now.Subtract(serverTime.Value);
            CacheManager.CacheWrapper.Insert(entityKeyString, _Delta_TimeSpan);

            return serverTime;

        }




        private OracleConnection GetOracleConnectionFrom(DbContextBase currentContext)
        {
            throw new Exception("private OracleConnection GetOracleConnectionFrom(AmitalContext currentContext) BAAD BAD !!!");
            return new OracleConnection(currentContext.Database.Connection.ConnectionString);
            //(currentContext.Database.Connection as EntityConnection).StoreConnection as OracleConnection

        }
    }
    public partial class DualServerDateTime
    {

        public DateTime? ServerDateTime { get; set; }
    }

    public class OpenReaderSingleResult
    {
        private DbContextBase _CurrentContext;

        public OpenReaderSingleResult(int tenant)
        {
            _CurrentContext = CustomContext.GetContext(tenant) as DbContextBase;
        }

        public OpenReaderSingleResult(DbContext context)
        {
            _CurrentContext = context as DbContextBase;
        }
        public Nullable<returnType> ExecuteReaderSingleResult<returnType>(string sqlReturn1Row, Func<DbDataReader, Nullable<returnType>> GetReturnTypeFromReader)
            where returnType : struct
        {
            {





                using (var command = _CurrentContext.Database.Connection.CreateCommand())
                {


                    if (_CurrentContext.Database.Connection.State != System.Data.ConnectionState.Open)
                    {
                        _CurrentContext.Database.Connection.Open();
                    }
                    command.CommandText = sqlReturn1Row;


                    using (var dataReader = command.ExecuteReader(CommandBehavior.CloseConnection | CommandBehavior.SingleResult))
                    {

                        if (dataReader.FieldCount < 1)
                        {
                            return null;
                        }

                        if (!dataReader.Read())
                        {
                            return null;
                        }
                        if (dataReader.IsDBNull(0))
                        {
                            return null;
                        }



                        var ReturnValue = GetReturnTypeFromReader(dataReader);

                        return ReturnValue;
                    }
                }


            }


        }

        public string GetSchemaUserId()
        {

            var toSchema = DbContextBaseUtil.GetSchemaAMITAL_DB();
            
            return toSchema;
        }
    }
}
