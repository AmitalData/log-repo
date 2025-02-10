using Logitude.Server.Tools.SQL;
using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Logitude.Server.Tools.Helpers
{
    public class MetadataUpdateUtility
    {
        public static bool IsChangedMetadataTable(string tableName, Dictionary<string, ObjectTable> ObjectTables, string generatedHashString)
        {
            return (!ObjectTables.ContainsKey(tableName) || ((ObjectTables.ContainsKey(tableName) && generatedHashString != ObjectTables[tableName].HashString)) || (ObjectTables.ContainsKey(tableName) && ObjectTables[tableName].HashString == null));
        }
        public static void DeleteAllTableMetadata(string tableName,int contextTenant=0)
        {
            System.Collections.Generic.List<StoredProcedureParam> paramList = new System.Collections.Generic.List<StoredProcedureParam>()
                        {
                            new StoredProcedureParam()   { Direction = ParameterDirection.Input, ParamDBType = SqlDbType.VarChar, ParamSize = 50, ParamName = "@pTableName",Value = tableName },

                        };

            ExecuteStoredProcedures.Execute("dbo.usp_DeleteObjectTableMetadata", contextTenant, paramList);
        }

        public static void RunPreDeleteProcedure(int contextTenant = 0)
        {
            System.Collections.Generic.List<StoredProcedureParam> paramList = new System.Collections.Generic.List<StoredProcedureParam>()
                        {
                            new StoredProcedureParam()   { Direction = ParameterDirection.Input, ParamDBType = SqlDbType.VarChar, ParamSize = 50, ParamName = "@pTableName",Value = "" },

                        };

            ExecuteStoredProcedures.Execute("dbo.usp_PreDeleteMetadata", contextTenant, paramList);


        }
        public static void RunPostDeleteProcedure(int contextTenant = 0)
        {
            System.Collections.Generic.List<StoredProcedureParam> paramList = new System.Collections.Generic.List<StoredProcedureParam>()
                        {
                            new StoredProcedureParam()   { Direction = ParameterDirection.Input, ParamDBType = SqlDbType.VarChar, ParamSize = 50, ParamName = "@pTableName",Value = "" },

                        };

            ExecuteStoredProcedures.Execute("dbo.usp_ReconnectObjectTableMetadata", contextTenant, paramList);


        }
    } 
}