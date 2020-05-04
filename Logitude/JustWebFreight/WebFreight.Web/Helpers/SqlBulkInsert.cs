using Intuit.Ipp.Data;
using Simplog.Data.Helpers;
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
            
          
            var props = TypeDescriptor.GetProperties(typeof(T))
                                       //Dirty hack to make sure we only have system data types 
                                       //i.e. filter out the relationships/collections
                                       .Cast<PropertyDescriptor>()
                                       .Where(propertyInfo => propertyInfo.PropertyType.Namespace.Equals("System"))
                                       .ToArray();
            //insert into shipments(id,ShipmentNumber) values ('1','dsd'),('12','sdsd')
            string insertCommand = "insert into " + tableName + "(";
            foreach (var propertyInfo in props)
            {
                insertCommand += propertyInfo.Name + ",";
                   
            }


            insertCommand = insertCommand.TrimEnd(',') + ") values";

            StringBuilder sqlStringBuilder = new StringBuilder();
            sqlStringBuilder.AppendLine(insertCommand);
            //var values = new object[props.Length];
            int rowsCount = 0;
            int allRowsCount = list.Count();
            int insertedRowsCount = 0;
            string strConnString = TenantServerConfigration.GetDbConnection(0);
            using (SqlConnection cn = new SqlConnection(strConnString))
            {
                cn.Open();

                foreach (var item in list)
                {
                    string valuesString = "(";
                    for (var i = 0; i < props.Length; i++)
                    {
                        object propValue = props[i].GetValue(item);
                        if (props[i].PropertyType != typeof(int) && props[i].PropertyType != typeof(decimal)
                                               && props[i].PropertyType != typeof(float) && props[i].PropertyType != typeof(double))
                        {
                            if (propValue != null)
                                propValue = "'" + propValue.ToString().Replace("'", "") + "'";
                            else
                                propValue = "NULL";
                        }

                        valuesString += propValue + ",";
                        //values[i] = props[i].GetValue(item);
                    }
                    valuesString = valuesString.TrimEnd(',') + "),";
                    sqlStringBuilder.AppendLine(valuesString);

                    rowsCount++;
                    insertedRowsCount++;

                    if (rowsCount == 100 || insertedRowsCount == allRowsCount)
                    {
                        string sqlCommandString = sqlStringBuilder.ToString();
                        sqlCommandString = sqlCommandString.Substring(0, sqlCommandString.LastIndexOf(","));
                        //sqlCommandString = sqlCommandString.TrimEnd(',',' ');

                        //using (SqlConnection cn = new SqlConnection(strConnString))
                        //{
                            SqlCommand cmd = new SqlCommand(sqlCommandString, cn);
                            cmd.CommandTimeout = 1200;

                            //cn.Open();
                            var output = cmd.ExecuteNonQuery();
                        //    cn.Close();
                        //}


                        sqlStringBuilder = new StringBuilder();
                        sqlStringBuilder.AppendLine(insertCommand);
                        rowsCount = 0;
                    }
                }

                cn.Close();
                //table.Rows.Add(values);
            }

            
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