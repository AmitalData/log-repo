using Logitude.Server.Tools.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools
{
    public class APILogsUtility
    {
        public static void UpdateAPILogStatus(string id, int tenant, object status, int RetriesNo, DateTime UpdateDate, DateTime UpdateDateUTC, object log, object RequestData, object ResponseData, object exceptionMessage, string LastExceptionMessage)//,string CustomerId = null,string BatchNumber = null)
        {
             
            log = (log == null ? DBNull.Value : log);
            RequestData = (RequestData == null ? DBNull.Value : RequestData);
            ResponseData = (ResponseData == null ? DBNull.Value : ResponseData);
            exceptionMessage = (exceptionMessage == null ? DBNull.Value : exceptionMessage);


            System.Collections.Generic.List<StoredProcedureParam> paramList = new System.Collections.Generic.List<StoredProcedureParam>()
                        {
                            new StoredProcedureParam()   { Direction = ParameterDirection.Input, ParamDBType = SqlDbType.VarChar, ParamSize = 15, ParamName = "@pLogId",Value = id },
                            new StoredProcedureParam()   { Direction = ParameterDirection.Input, ParamDBType = SqlDbType.Int, ParamName = "@pTenant",Value = tenant },
                            new StoredProcedureParam()   { Direction = ParameterDirection.Input, ParamDBType = SqlDbType.VarChar, ParamSize = 1, ParamName = "@pStatus",Value= status },
                            new StoredProcedureParam()   { Direction = ParameterDirection.Input, ParamDBType = SqlDbType.Int, ParamName = "@pRetriesNo",Value = RetriesNo },
                            new StoredProcedureParam()   { Direction = ParameterDirection.Input, ParamDBType = SqlDbType.DateTime, ParamSize = 1, ParamName = "@pUpdateDate",Value= UpdateDate },
                            new StoredProcedureParam()   { Direction = ParameterDirection.Input, ParamDBType = SqlDbType.DateTime, ParamSize = 1, ParamName = "@pUpdateDateUTC",Value= UpdateDateUTC },
                            new StoredProcedureParam()   { Direction = ParameterDirection.Input, ParamDBType = SqlDbType.NVarChar, ParamSize = -1, ParamName = "@pLog",Value = log },
                            new StoredProcedureParam()   { Direction = ParameterDirection.Input, ParamDBType = SqlDbType.NVarChar, ParamSize = -1, ParamName = "@pRequestData",Value = RequestData },
                            new StoredProcedureParam()   { Direction = ParameterDirection.Input, ParamDBType = SqlDbType.NVarChar, ParamSize = -1, ParamName = "@pResponseData",Value = ResponseData },
                            new StoredProcedureParam()   { Direction = ParameterDirection.Input, ParamDBType = SqlDbType.NVarChar, ParamSize = -1, ParamName = "@pExceptionMessage",Value = exceptionMessage }, 
                            new StoredProcedureParam()   { Direction = ParameterDirection.Input, ParamDBType = SqlDbType.NVarChar, ParamSize = 250, ParamName = "@pLastExceptionMessage",Value = LastExceptionMessage }, 
                        };

            object value = ExecuteStoredProcedures.Execute("dbo.usp_UpdateAPILog", tenant, paramList);
        }
    }
}
