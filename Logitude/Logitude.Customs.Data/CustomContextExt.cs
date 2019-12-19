using Devart.Data.Oracle;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Data
{
    public partial class CustomContext : DbContextBase, ICustomContext
    {
        public static void CommandExecuteNonQuery(int tenant, string cmd, int? commandTimeout=null)
        {
            var context = CustomContext.GetContext(tenant);
            var strConnString = context.GetConnection().ConnectionString;
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
                
                using (var cn = (context.GetConnection() as OracleConnection))  //new OracleConnection(strConnString))
                {
                    Debug.WriteLine($"CommandExecuteNonQuery({cmd})");


                    var command = new OracleCommand(cmd, cn);
                    if (commandTimeout.HasValue)
                    {
                        command.CommandTimeout = commandTimeout.Value;
                    }
                    cn.Open();
                    command.ExecuteNonQuery();
                    cn.Close();
                }
            }

            else
            {
                using (SqlConnection cn = new SqlConnection(strConnString))
                {
                    Debug.WriteLine($"CommandExecuteNonQuery({cmd})");

                    SqlCommand sqlCommand = new SqlCommand(cmd, cn);

                    cn.Open();
                    sqlCommand.ExecuteNonQuery();
                    cn.Close();
                }
            }


        }
    }
}
