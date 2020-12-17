using Devart.Data.Oracle;
using Intuit.Ipp.Data;
using Microsoft.Practices.Unity;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class SqlBulkInsert
    {
        public static void BulkInsert<T>(string tableName, IList<T> list)
        {
            if (list == null || list.Count() == 0)
                return;
            if (LogitudeSettings.DatabaseManagementSystem == "oracle")
            {
                RunOracleSqlInsert(tableName, list);
            }
            else
                RunSqlInsert(tableName, list);
        }

        private static void RunSqlInsert<T>(string tableName, IList<T> list)
        {
           
            PropertyDescriptor[] entityProperties = GetEntitySystemProperties<T>();
            string strConnString = TenantServerConfigration.GetDbConnection(0);
            StringBuilder sqlStringBuilder = new StringBuilder();
            string insertCommand = "insert into " + BuildInsertCommandColumnsString(tableName, entityProperties) +" values";
            sqlStringBuilder.AppendLine(insertCommand);
            //sqlStringBuilder.AppendLine("values");
            int allRowsCount = list.Count();
            int bulkInsertedRowsCount = 0;
            int insertedRowsCount = 0;
            using (SqlConnection connection = new SqlConnection(strConnString))
            {
                connection.Open();
                foreach (var item in list)
                {
                    string valuesString = BuildValuesSqlString(entityProperties, item) + "),";
                    sqlStringBuilder.AppendLine(valuesString);
                    bulkInsertedRowsCount++;
                    insertedRowsCount++;
                    if (bulkInsertedRowsCount == 100 || insertedRowsCount == allRowsCount)
                    {
                        string sqlCommandString = sqlStringBuilder.ToString();
                        sqlCommandString = sqlCommandString.Substring(0, sqlCommandString.LastIndexOf(","));
                        SqlCommand cmd = new SqlCommand(sqlCommandString, connection);
                        cmd.CommandTimeout = 1200;
                        var output = cmd.ExecuteNonQuery();

                        sqlStringBuilder = new StringBuilder();
                        sqlStringBuilder.AppendLine(insertCommand);
                        bulkInsertedRowsCount = 0;
                    }
                }
                connection.Close();
            }
        }

        private static void RunOracleSqlInsert<T>(string tableName, IList<T> list)
        {
            PropertyDescriptor[] entityProperties = GetEntitySystemProperties<T>();
            string strConnString = TenantServerConfigration.GetDbConnection(0);
            string insertCommand = BuildInsertCommandColumnsString(tableName, entityProperties);
            int allRowsCount = list.Count();
            int bulkInsertedRowsCount = 0;
            int insertedRowsCount = 0;
            using (OracleConnection connection = new OracleConnection(strConnString))
            {
                connection.Open();
                StringBuilder sqlStringBuilder = new StringBuilder();
                sqlStringBuilder.AppendLine("insert all");

                foreach (var item in list)
                {
                    string valuesString = "values" + BuildValuesSqlString(entityProperties, item) + ")";
                    string commandString = "into " + insertCommand + " " + valuesString;//+ ";";
                    sqlStringBuilder.AppendLine(commandString);
                    bulkInsertedRowsCount++;
                    insertedRowsCount++;
                    if (bulkInsertedRowsCount == 100 || insertedRowsCount == allRowsCount)
                    {
                        sqlStringBuilder.AppendLine("SELECT 1 FROM dual;");
                        
                        string sqlCommandString = sqlStringBuilder.ToString();
                        OracleScript oracleScript = new OracleScript(sqlCommandString, connection);
                        oracleScript.CommandTimeout = 1200;
                        oracleScript.Execute();
                        //OracleCommand cmd = new OracleCommand(sqlCommandString, connection);
                        //cmd.CommandTimeout = 1200;
                         
                        //var output = cmd.ExecuteNonQuery();

                        sqlStringBuilder = new StringBuilder();
                        sqlStringBuilder.AppendLine("insert all");
                        bulkInsertedRowsCount = 0;
                    }
                }
                connection.Close();
            }
        }
        private static PropertyDescriptor[] GetEntitySystemProperties<T>()
        {
            var props = TypeDescriptor.GetProperties(typeof(T))
                                       //Dirty hack to make sure we only have system data types 
                                       //i.e. filter out the relationships/collections
                                       .Cast<PropertyDescriptor>()
                                       .Where(propertyInfo => propertyInfo.PropertyType.Namespace.Equals("System"))
                                       .ToArray();
            return props;
        }

        private static string BuildInsertCommandColumnsString(string tableName, PropertyDescriptor[] entityProperties)
        {
            string insertCommand = tableName + "(";
            foreach (var propertyInfo in entityProperties)
            {
                insertCommand = AppendFieldToInsertCommand(tableName,insertCommand, propertyInfo);
            }
            insertCommand = insertCommand.TrimEnd(',') + ")";
            return insertCommand;
        }

        private static string AppendFieldToInsertCommand(string tableName, string insertCommand, PropertyDescriptor propertyInfo)
        {
            if (LogitudeSettings.DatabaseManagementSystem == "oracle")
            {
                string fieldName = propertyInfo.Name;
                if (fieldName.Length > 30) 
                {
                    fieldName = GetFieldShortName(tableName, fieldName);
                }
                insertCommand += fieldName + ",";
            }
            else
            {
                insertCommand += propertyInfo.Name + ",";
            }
            return insertCommand;
        }

        private static string BuildValuesSqlString<T>(PropertyDescriptor[] entityProperties, T item)
        {
            string valuesString = "(";
            for (var i = 0; i < entityProperties.Length; i++)
            {
                object propValue = entityProperties[i].GetValue(item);
                if (entityProperties[i].PropertyType != typeof(int) && entityProperties[i].PropertyType != typeof(decimal)
                                       && entityProperties[i].PropertyType != typeof(float) && entityProperties[i].PropertyType != typeof(double))
                {
                    if (propValue != null)
                    {
                        if (entityProperties[i].PropertyType == typeof(bool))
                            propValue = propValue.ToString().ToLower() == "true" ? 1 : 0;
                        else
                            propValue = "N'" + propValue.ToString().Replace("'", "''") + "'";
                    }
                    else
                        propValue = "NULL";
                }

                valuesString += propValue + ",";
            }
            valuesString = valuesString.TrimEnd(',');
            return valuesString;
        }

        private static string GetFieldShortName(string tableName,string fieldName)
        {
            string shortfieldName = "";
            if (LogitudeSettings.DatabaseManagementSystem == "oracle")
            {
                string fieldShortNameGetterName = tableName + "ShortNamesGetter";
                FieldShortNameGetter fieldShortNameGetter = InjectionContainer.Container.Resolve(typeof(FieldShortNameGetter), fieldShortNameGetterName, new ParameterOverride("", 1)) as FieldShortNameGetter;
                if (fieldShortNameGetter != null)
                {
                    shortfieldName = fieldShortNameGetter.GetFieldShortName(fieldName);
                }
                else
                {
                    string exceptionMessage = "Table " + tableName + " has no short names getter ,"
                        + Environment.NewLine +
                        "please add a class with the name (your tableName)+ShortNamesGetter implements IFieldShortNameGetter"
                        + Environment.NewLine + 
                        "please look at ObjectFieldsShortNamesGetter as an example and register it in InfraRegistrationHelper";
                    throw new Exception(exceptionMessage);
                }
            }
            return shortfieldName;
        }

        //public static void BulkInsert<T>(string connection, string tableName, IList<T> list)
        //{
        //    using (var bulkCopy = new SqlBulkCopy(connection))
        //    {
        //        bulkCopy.BatchSize = list.Count;
        //        bulkCopy.DestinationTableName = tableName;

        //        var table = new DataTable();
        //        var props = TypeDescriptor.GetProperties(typeof(T))
        //                                   //Dirty hack to make sure we only have system data types 
        //                                   //i.e. filter out the relationships/collections
        //                                   .Cast<PropertyDescriptor>()
        //                                   .Where(propertyInfo => propertyInfo.PropertyType.Namespace.Equals("System"))
        //                                   .ToArray();

        //        foreach (var propertyInfo in props)
        //        {
        //            bulkCopy.ColumnMappings.Add(propertyInfo.Name, propertyInfo.Name);
        //            table.Columns.Add(propertyInfo.Name, Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType);
        //        }

        //        var values = new object[props.Length];
        //        foreach (var item in list)
        //        {
        //            for (var i = 0; i < values.Length; i++)
        //            {
        //                values[i] = props[i].GetValue(item);
        //            }

        //            table.Rows.Add(values);
        //        }

        //        bulkCopy.WriteToServer(table);
        //    }
        //}
    }
}