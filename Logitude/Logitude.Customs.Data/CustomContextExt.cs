using Devart.Data.Oracle;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Data
{
    public static class CustomContextExt
    {
        public static void CommandExecuteNonQuery(this CustomContext customContext,int tenant, string cmd)
        {
            var context = CustomContext.GetContext(tenant);

            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
                var strConnString =customContext.GetConnection().ConnectionString;
                using (var cn = (context.GetConnection() as OracleConnection))  //new OracleConnection(strConnString))
                {
                    Debug.WriteLine($"CommandExecuteNonQuery({cmd})");


                    var command = new OracleCommand(cmd, cn);

                    cn.Open();
                    command.ExecuteNonQuery();
                    cn.Close();
                }
            }

            else
            {
                throw new System.Exception("CustomContext is 4 oracle ");
            }


        }
    }
}
