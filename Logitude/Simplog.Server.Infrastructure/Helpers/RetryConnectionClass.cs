using System;

using System.Data.SqlClient;
using System.Threading;
//using Logitude.SystemLogs;

using Simplog.Server.Infrastructure.Azure;

using System.Data.Entity.Core.EntityClient;
using System.Data.Common;

namespace Simplog.Server.Infrastructure.Helpers
{
    public static class RetryConnectionClass
    {
        public static void CheckConnection(DbConnection connection)
        {
            
            int counter = 0;
            bool endLoop = false;
            bool enableNewConnection = false;
            string connectionstring = connection.ConnectionString;
            string errorMessage = "";
            while (true)
            {

                try
                {
                   
                    connection.Open();
                    SqlConnection sqlconnection = (SqlConnection)connection;
                    SqlCommand command = new SqlCommand("declare @i int", sqlconnection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    break;
                }
                catch (Exception errorInfo)
                {

                    //SqlConnection.ClearAllPools();

                    errorMessage = errorInfo.Message;

                    if (errorInfo.InnerException != null)
                    {
                        errorMessage += Environment.NewLine + errorInfo.InnerException.Message;
                    }
                    errorMessage += Environment.NewLine + errorInfo.ToString();
                    if (!string.IsNullOrEmpty(errorInfo.StackTrace))
                    {
                        errorMessage += Environment.NewLine + errorInfo.StackTrace;
                    }

                    int delay = 0;
                    switch (counter)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            {
                                delay = 10;
                                break;
                            }
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                            {
                                delay = 100;
                                break;
                            }
                        case 10:
                        case 11:
                        case 12:
                        case 13:
                        case 14:
                            {
                                delay = 1000;
                                break;
                            }
                        case 15:
                        case 16:
                        case 17:
                        case 18:
                        case 19:
                            {
                                delay = 5000;
                                break;
                            }
                        case 20:
                            {
                                endLoop = true;
                                break;
                            }
                    }

                    if (endLoop)
                    {
                        break;
                    }
                    else
                    {
                        counter++;
                        //AzureLog.SaveLogsInStorage("Connection retry  " + counter.ToString() + Environment.NewLine + errorMessage, "E", DateTime.Now, errorInfo.Message, errorInfo.StackTrace, 0, null, null,null);
                        //try
                        //{
                        //    connection.Close();
                        //}
                        //catch (Exception errorInfo1)
                        //{
                        //    //string ErrorMessage = "";
                        //    errorMessage = errorInfo1.Message;

                        //    if (errorInfo1.InnerException != null)
                        //    {
                        //        errorMessage += Environment.NewLine + errorInfo1.InnerException.Message;
                        //    }
                        //    errorMessage += Environment.NewLine + errorInfo1.ToString();
                        //    if (!string.IsNullOrEmpty(errorInfo1.StackTrace))
                        //    {
                        //        errorMessage += Environment.NewLine + errorInfo1.StackTrace;
                        //    }

                        //    AzureLog.SaveLogsInStorage("connection.Close() faild  " + counter.ToString() + Environment.NewLine + errorMessage, "E", DateTime.Now, errorInfo1.Message, errorInfo1.StackTrace, 0, null, null,null);
                        //}
                        Thread.Sleep(delay);
                       
                    }
                   
                }
            }
        }
    }

    
}