using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.WindowsAzure;

using Simplog.Global.Data.GlobalModel.Repositories;

using WebFreight.Web.GlobalModelDB;
using WebFreight.Web.Testing;
using Microsoft.WindowsAzure.Storage;
using System.Transactions;
using WebFreight.Web.Azure;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using WebFreight.Web.GlobalModel;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure.Azure;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Auth;
using Logitude.SystemLogs;

namespace WebFreight.Web.Monitoring
{
    public partial class DbBackup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/xml";
            //Response.Write("<?xml version='1.0' encoding='UTF-8'?>");
            Response.Write("<pingdom_http_custom_check>");

            if (IsDataBaseBackupSucceeded())
            {
                Response.Write("<status>OK</status>");
            }
            else
            {
                Response.Write("<status>Fail</status>");
            }
            int ResponseTime_Millisecond = 0;//HttpContext.Current.Timestamp.Millisecond;
            String ResponseTime = "<response_time>" + ResponseTime_Millisecond + "</response_time>";
            Response.Write(ResponseTime);
            Response.Write("</pingdom_http_custom_check>");
            Response.End();
        }

        private bool IsDataBaseBackupSucceeded()
        {
            bool issuccedded = true;

            if (DateTime.Now >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 6, 0, 0))
            {
                try
                {

                    CloudStorageAccount productionStorageAccont =
                        new CloudStorageAccount(
                            new StorageCredentials("productionbackup",
                                "TYyS+o05NvECOw9YnDYOVWpMivlLTAGiMalLHtjPKNJPFoGKF54TqtX3Bp+8+WG90E3GEfeq5fKUtPKsMskwZg=="),
                            new Uri(@"http://productionbackup.blob.core.windows.net/"),
                            new Uri(@"http://productionbackup.queue.core.windows.net/"),
                            new Uri(@"http://productionbackup.table.core.windows.net/"),null);

                    List<GlobalDB> GlobalDatabases;
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())//TransactionFactory.GetNewTransaction())
                    {
                        GlobalDBRepository globaldbRep = new GlobalDBRepository();

                        try
                        {
                            GlobalDatabases = globaldbRep.All();
                            scope.Complete();
                        }
                        catch (Exception errorInfo)
                        {
                            ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "DatabaseBackup",
                                "Bug in IsDataBaseBackupSucceeded Method : globaldbRep.All()",null);
                            issuccedded = false;
                            scope.Complete();
                            return issuccedded;
                        }

                    }



                   // foreach (GlobalDB db in GlobalDatabases)
                  //  {
                    
                    // check global db backup. Main backup is automatically done.
                        CloudBlobClient productionBlobClient = productionStorageAccont.CreateCloudBlobClient();
                        CloudBlobContainer container = productionBlobClient.GetContainerReference("dacpacs");

                        //SimplogGlobal_temp_31_03_2013.bacpac
                        string blob = "SimplogGlobal" + "_" + "temp" + "_" + String.Format("{0:dd_MM_yyyy}" + ".bacpac", DateTime.Now);//db.DBConnection.Split(',')[0].ToString() + "_" + "temp" + "_" + String.Format("{0:dd_MM_yyyy}" + ".bacpac", DateTime.Now);
                        CloudBlockBlob lastBackup = container.GetBlockBlobReference(blob);


                        if (lastBackup.Exists())
                        {
                            if (lastBackup.StreamWriteSizeInBytes > 0)
                            {
                                issuccedded = true;
                            }
                            else
                                issuccedded = false;
                        }
                        else
                            issuccedded = false;

                   // }



                }
                catch (Exception e)
                {
                    ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "DatabaseBackup", "Bug in IsDataBaseBackupSucceeded Method",null);
                    issuccedded = false;
                    return issuccedded;
                }
            }
            return issuccedded;
        }
    }
}