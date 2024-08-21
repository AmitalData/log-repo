using Logitude.Server.Tools.SQL;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
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
        public static void DeleteAllTableMetadata(string tableName)
        {
            System.Collections.Generic.List<StoredProcedureParam> paramList = new System.Collections.Generic.List<StoredProcedureParam>()
                        {
                            new StoredProcedureParam()   { Direction = ParameterDirection.Input, ParamDBType = SqlDbType.VarChar, ParamSize = 50, ParamName = "@pTableName",Value = tableName },

                        };

            ExecuteStoredProcedures.Execute("dbo.usp_DeleteObjectTableMetadata", 0, paramList);
        }

        public static void RunPreDeleteProcedure()
        {
            System.Collections.Generic.List<StoredProcedureParam> paramList = new System.Collections.Generic.List<StoredProcedureParam>()
                        {
                            new StoredProcedureParam()   { Direction = ParameterDirection.Input, ParamDBType = SqlDbType.VarChar, ParamSize = 50, ParamName = "@pTableName",Value = "" },

                        };

            ExecuteStoredProcedures.Execute("dbo.usp_PreDeleteMetadata", 0, paramList);


        }
        public static void RunPostDeleteProcedure()
        {
            System.Collections.Generic.List<StoredProcedureParam> paramList = new System.Collections.Generic.List<StoredProcedureParam>()
                        {
                            new StoredProcedureParam()   { Direction = ParameterDirection.Input, ParamDBType = SqlDbType.VarChar, ParamSize = 50, ParamName = "@pTableName",Value = "" },

                        };

            ExecuteStoredProcedures.Execute("dbo.usp_ReconnectObjectTableMetadata", 0, paramList);


        }
    } 
}