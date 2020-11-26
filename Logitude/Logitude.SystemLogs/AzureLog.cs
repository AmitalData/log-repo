using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Text;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using Logitude.SystemLogs.POCOs;
using Logitude.SystemLogs;
using Logitude.SystemLogs.Repositories;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;
using System.Data.Entity.Validation;

namespace Logitude.SystemLogs
{
    public static class AzureLog
    {
        public static void SaveFileToStorage(string filename,string fileContent,int tenant)
        {
            try
            {
                if (LogitudeSettings.UsingAzure)
                {
                    CloudBlobContainer blobContainer = StorageAcountDetails.GetCurrentContainer(tenant);
                    CloudBlockBlob blobfile = null;
                    blobContainer.CreateIfNotExists();
                    blobfile = blobContainer.GetBlockBlobReference(filename);
                    string newlog = "DateTime : " + DateTime.Now.ToString() + " : " + Environment.NewLine + fileContent + Environment.NewLine;
                    StringBuilder stringbuilder = new StringBuilder();
                    if (BlobExtensions.Exists(blobfile))
                    {
                        //Read data from errorlogs
                        using (Stream blbstrRead = blobfile.OpenRead())
                        {
                            if (blbstrRead != null)
                            {
                                byte[] previousErrordata = new byte[blbstrRead.Length];
                                blbstrRead.Read(previousErrordata, 0, previousErrordata.Length);

                                Encoding encoding = new UTF8Encoding();
                                stringbuilder.Append(encoding.GetString(previousErrordata));
                            }
                        }
                    }
                    using (Stream blbstr = blobfile.OpenWrite())
                    {
                        Encoding encoding = new UTF8Encoding();
                        stringbuilder.AppendLine(newlog);
                        byte[] errordata = encoding.GetBytes(stringbuilder.ToString());
                        blbstr.Write(errordata, 0, errordata.Length);
                    }

                }
            }
            catch (Exception ex) { }
        }
        public static void SaveLogsInStorage(string log, string type, DateTime clientDate, string message, string stackTrace, int tenant, string userId, string userName, string IP,Exception cachedException = null)
        {
            CloudBlobContainer blobContainer = null;
            CloudBlockBlob blobfile = null;

            if (type == "E")
            {
                AddLogRecord(log, tenant, stackTrace, clientDate, userId, userName, IP, cachedException);
            }

            if (LogitudeSettings.UsingAzure && LogitudeSettings.IsLogEnabled)
            {
                try
                {
                    StringBuilder stringbuilder = new StringBuilder();
                    DateTime now = DateTime.Now;
                    string newlog = "DateTime : " + now.ToString() + " : " + Environment.NewLine + log + Environment.NewLine;
                    blobContainer = StorageAcountDetails.GetCurrentContainer(tenant);
                    blobContainer.CreateIfNotExists();

                    switch (type)
                    {
                        case "VD":
                            {
                                blobfile = blobContainer.GetBlockBlobReference("arinvoicevoid.txt");

                                if (BlobExtensions.Exists(blobfile))
                                {
                                    //Read data from errorlogs
                                    using (Stream blbstrRead = blobfile.OpenRead())
                                    {
                                        if (blbstrRead != null)
                                        {
                                            byte[] previousErrordata = new byte[blbstrRead.Length];
                                            blbstrRead.Read(previousErrordata, 0, previousErrordata.Length);

                                            Encoding encoding = new UTF8Encoding();
                                            stringbuilder.Append(encoding.GetString(previousErrordata));
                                        }
                                    }
                                }

                                //write data to errorlogs
                                using (Stream blbstr = blobfile.OpenWrite())
                                {

                                    Encoding encoding = new UTF8Encoding();
                                    stringbuilder.AppendLine(newlog);
                                    byte[] errordata = encoding.GetBytes(stringbuilder.ToString());
                                    blbstr.Write(errordata, 0, errordata.Length);
                                }

                                break;
                            }

                        case "E": // Errors Log
                           // AddLogRecord(log, tenant, stackTrace, clientDate, userId, userName, IP);
                            //blobfile = blobContainer.GetBlockBlobReference("errorslog.txt");
                            break;

                        case "P": // Performance Log
                            blobfile = blobContainer.GetBlockBlobReference("performancelog.txt");

                            if (BlobExtensions.Exists(blobfile))
                            {
                                //Read data from errorlogs
                                using (Stream blbstrRead = blobfile.OpenRead())
                                {
                                    if (blbstrRead != null)
                                    {
                                        byte[] previousErrordata = new byte[blbstrRead.Length];
                                        blbstrRead.Read(previousErrordata, 0, previousErrordata.Length);

                                        Encoding encoding = new UTF8Encoding();
                                        stringbuilder.Append(encoding.GetString(previousErrordata));
                                    }
                                }
                            }

                            //write data to errorlogs
                            using (Stream blbstr = blobfile.OpenWrite())
                            {

                                Encoding encoding = new UTF8Encoding();
                                stringbuilder.AppendLine(newlog);
                                byte[] errordata = encoding.GetBytes(stringbuilder.ToString());
                                blbstr.Write(errordata, 0, errordata.Length);
                            }


                            break;


                        case "L": // Performance Log
                            blobfile = blobContainer.GetBlockBlobReference("log.txt");

                            if (BlobExtensions.Exists(blobfile))
                            {
                                //Read data from errorlogs
                                using (Stream blbstrRead = blobfile.OpenRead())
                                {
                                    if (blbstrRead != null)
                                    {
                                        byte[] previousErrordata = new byte[blbstrRead.Length];
                                        blbstrRead.Read(previousErrordata, 0, previousErrordata.Length);

                                        Encoding encoding = new UTF8Encoding();
                                        stringbuilder.Append(encoding.GetString(previousErrordata));
                                    }
                                }
                            }

                            //write data to errorlogs
                            using (Stream blbstr = blobfile.OpenWrite())
                            {

                                Encoding encoding = new UTF8Encoding();
                                stringbuilder.AppendLine(newlog);
                                byte[] errordata = encoding.GetBytes(stringbuilder.ToString());
                                blbstr.Write(errordata, 0, errordata.Length);
                            }


                            break;

                        default:
                            AddLogRecord(log, tenant, stackTrace, clientDate, userId, userName, IP, cachedException);
                            // blobfile = blobContainer.GetBlockBlobReference("errorslog.txt");
                            break;


                    }

                }

                catch (Exception exception)
                {

                    string errorMessage = exception.Message;

                    if (exception.InnerException != null)
                    {
                        errorMessage += Environment.NewLine + exception.InnerException.Message;
                    }

                    errorMessage += Environment.NewLine + exception.ToString();

                    if (!string.IsNullOrEmpty(exception.StackTrace))
                    {
                        errorMessage += Environment.NewLine + exception.StackTrace;
                    }


                    //Write exception to event log
                    //EventLog _EventLog = new EventLog();
                    //_EventLog.Source = "SaveLogsInStorage_Method";
                    //_EventLog.WriteEntry(ErrorMessage, EventLogEntryType.Error);
                }
            }
        }


        static void AddLogRecord(string exception,int tenant ,string stackTrace, DateTime clientDate,  string userId, string userName,string IP, Exception cachedException)
        {

            if (!string.IsNullOrEmpty(exception) && exception.Contains("Sorry! you have no permission to do this operation"))
                return;
            string myStackTrace = GetStackTrace(exception);
            if (!string.IsNullOrEmpty(myStackTrace))
            {
                stackTrace = myStackTrace;
            }
            if (stackTrace.Length > 7000)
            {
                stackTrace = stackTrace.Substring(0, 6999);
            }
            var better1st7000ThenNothing = true;//itzik 
            if (better1st7000ThenNothing)
            {
                exception = exception ?? "";
                if (cachedException != null && cachedException.Source == "EntityFramework" && exception.Length > 7000)//Islam: take the start and the end if it is a db exception.
                {
                    exception = exception.Substring(0, 3500) + exception.Substring(exception.Length - 3500, 3500);
                    //exception = HandleExceptionLength(exception);
                }
                else
                {
                    exception = exception.Substring(0, Math.Min(7000, exception.Length));
                }
            }
            
            

            if (String.IsNullOrWhiteSpace(userName))
            {
                userName=userId;
            }
            if (String.IsNullOrWhiteSpace(userName) && LogitudeSettings.GetUserNameInject != null)
            {
                userName = LogitudeSettings.GetUserNameInject(tenant);
            }
            if (String.IsNullOrWhiteSpace(userName))
            {
                userName = "UnKnown";//Ismust
            }
            if (!string.IsNullOrEmpty(userName))
            {
                userName = TruncateLongString(userName, 99);
            }


            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                ErrorLogRepository errorLogRrp = new ErrorLogRepository();
                ErrorLog errorLog = new ErrorLog()
                                   {
                                       Id = Guid.NewGuid().ToString(),
                                       StackTrace = stackTrace,
                                       Exception = exception,
                                       Tenant = tenant,
                                       UserName = userName,
                                       LogDate = DateTime.Now,
                                       Tier = "Server",
                                       ClientDate = clientDate,
                                       SearchFields = "Server" + "," + userName + "," + exception + "," + tenant,
                                       IP = IP,
                                   };



                errorLogRrp.Add(errorLog);
                errorLogRrp.SubmitChanges();

                scope.Complete();

                try// mohammad : i didn't understand this code but it keeps throwing an exception
                {
                    var curr = LogtitudeDomainScope.GetCurrent<ExceptionInErrorLog>();
                    if (curr != null)
                    {
                        curr.ErrorlogId = errorLog.Id;
                        curr.Message = exception;
                    }
                }
                catch
                {
                }


            }
            //StorageAcountDetails.TableClient.GetTableReference("ErrorLogs");
            //var serviceContext = StorageAcountDetails.TableClient.GetTableServiceContext();
            //serviceContext.AddObject("ErrorLogs", errorLog);
            //serviceContext.SaveChangesWithRetries();

          //  errorsContext.ErrorLogsEntity.Add(errorLog);
             
           // errorsContext.SaveChanges();
        }

        private static string GetStackTrace(string exception)
        {
            string StackTrace = "";
            string[] myException = exception.Split('~');
           
            if (myException.Length == 3)
            {
                //string Header = myException[0];
                //string Body = myException[1]; 
                StackTrace = myException[2];
            }
            return StackTrace;
        }

        public static void SaveWarmingLogsInStorage(string log, string type, int tenant)
        {
            CloudBlobContainer blobContainer = null;
            CloudBlockBlob blobfile = null;

            if (LogitudeSettings.UsingAzure && LogitudeSettings.IsLogEnabled)
            {
                StringBuilder stringbuilder = new StringBuilder();
                DateTime now = DateTime.Now;
                string newlog = "DateTime : " + now.ToString() + " : " + Environment.NewLine + log + Environment.NewLine;
                blobContainer = StorageAcountDetails.GetCurrentContainer("warminglogs");
                blobContainer.CreateIfNotExists();
                string machineInfo = (!string.IsNullOrEmpty(Environment.MachineName) ? Environment.MachineName : "").ToLower();
                machineInfo = machineInfo +"_"+ DateTime.Now.Date.ToShortDateString().ToLower();
                switch (type)
                {

                    case "E": // Errors Log

                        blobfile = blobContainer.GetBlockBlobReference("warmingerrors_" + machineInfo+ ".txt");
                        break;

                    case "M": // Performance Log
                        blobfile = blobContainer.GetBlockBlobReference("warminglogs_" + machineInfo + ".txt");
                        break;

                    default:
                        blobfile = blobContainer.GetBlockBlobReference("warminglogs_" + machineInfo + ".txt");
                        // blobfile = blobContainer.GetBlockBlobReference("errorslog.txt");
                        break;


                }

                try
                {
                    if (BlobExtensions.Exists(blobfile))
                    {
                        //Read data from errorlogs
                        using (Stream blbstrRead = blobfile.OpenRead())
                        {
                            if (blbstrRead != null)
                            {
                                byte[] previousErrordata = new byte[blbstrRead.Length];
                                blbstrRead.Read(previousErrordata, 0, previousErrordata.Length);

                                Encoding encoding = new UTF8Encoding();
                                stringbuilder.Append(encoding.GetString(previousErrordata));
                            }
                        }
                    }
                    //write data to errorlogs
                    using (Stream blbstr = blobfile.OpenWrite())
                    {
                        Encoding encoding = new UTF8Encoding();
                        stringbuilder.AppendLine(newlog);
                        byte[] errordata = encoding.GetBytes(stringbuilder.ToString());
                        blbstr.Write(errordata, 0, errordata.Length);
                    }
                }

                catch (Exception e)
                {
                    //string ErrorMessage = e.Message;

                    //if (e.InnerException != null)
                    //{
                    //    ErrorMessage += Environment.NewLine + e.InnerException.Message;
                    //}
                    //ErrorMessage += Environment.NewLine + e.ToString();

                    //AzureLog.SaveLogsInStorage(ErrorMessage, "E");
                }
            }
        }

        public static string TruncateLongString(string str, int maxLength)
        {
            if (!string.IsNullOrEmpty(str))

                return str.Substring(0, Math.Min(str.Length, maxLength));

            else
                return str;
        }
    }
}