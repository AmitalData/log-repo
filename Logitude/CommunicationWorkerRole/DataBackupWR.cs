using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Azure;


using System.Threading;
using System.Diagnostics;
using System.Data;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;


using System.Transactions;
using Microsoft.WindowsAzure.Storage.Queue;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.SystemLogs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using ICSharpCode.SharpZipLib.Core;
using System.Text.RegularExpressions;
using System.Linq;

namespace CommunicationWorkerRole
{
    class DataBackupWR : WorkerEntryPoint
    {
        CloudQueue clientdatapackupqueue;
        SqlConnection connect;
        int tablesNumber;
        int counter = 0;
        StringBuilder schemaStringbuilder;
        int encodingCodePage = Encoding.UTF8.CodePage;
        //string SourceFilePath2 = @"C:\temp\CSVFolder2\";
        //DirectoryInfo LocalDirectory;
        int tenant;
        public override void Run()
        {
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    if (clientdatapackupqueue.Exists())
                    {
                        var clientdatapackupmsg = clientdatapackupqueue.GetMessage();
                        LastActivity = DateTime.UtcNow;
                        if (clientdatapackupmsg != null)
                        {
                            try
                            {
                                string[] result = clientdatapackupmsg.AsString.Split(',');
                                clientdatapackupqueue.DeleteMessage(clientdatapackupmsg);
                                string messageType = result[0];
                                string tenantString = result[1];
                                int.TryParse(tenantString, out tenant);
                                switch (messageType)
                                {
                                    case "ClientDataBackup":
                                        {
                                            PrepareBackup(encodingCodePage);
                                            //CommonDataDomainService commonDomain = new CommonDataDomainService();
                                            //commonDomain.SetDatabaseDataBackupReady(tenant);

                                            //TenantRepository tenantRepository = new TenantRepository(tenant);
                                            //Tenant tenantObject = tenantRepository.GetSingleTenant(tenant);
                                            //tenantObject.IsDataBackupBuilt = true;
                                            //tenantRepository.Update(tenantObject);
                                            //tenantRepository.SubmitChanges();

                                            TenantRepository tenantRepository = new TenantRepository(tenant);
                                            Tenant tenantObject = tenantRepository.GetSingleTenant(tenant);
                                            tenantObject.IsDataBackupBuilt = true;
                                            tenantRepository.Update(tenantObject);
                                            tenantRepository.SubmitChanges();
                                            LogDoneItemInMemory();
                                            break;
                                        }
                                }


                            }
                            catch (Exception e)
                            {
                                ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "DataBackupWR : Run() Method", null);
                                Thread.Sleep(10000);
                            }

                        }
                        else
                        {
                            Thread.Sleep(3000);
                        }
                    }
                }
                else
                {
                    Thread.Sleep(300000);
                }
            }

        }

        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "DataBackup";
            DoneItemsInRange = new Dictionary<DateTime, int>();

            clientdatapackupqueue = StorageAcountDetails.QueueClient.GetQueueReference("clientdatabackup");
            clientdatapackupqueue.CreateIfNotExists();
            clientdatapackupqueue.Clear();
            return base.OnStart();


        }

        //public void StartBackUp(int Enco


        public string PrepareBackup(int encodingcodepage)
        {
            if (encodingcodepage != null && encodingcodepage != 0)
            {
                encodingCodePage = encodingcodepage;
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();


            string con = GetConnection(tenant);//ConfigurationManager.ConnectionStrings["str"].ConnectionString;
            connect = new SqlConnection(con);
			

			connect.Open();

            DataTable tables = connect.GetSchema("Tables");
            schemaStringbuilder = new StringBuilder();
            // TablesNumber = tables.Rows.Count;

            List<string> dataBackupTables = new List<string>() 
            {
                "Shipments",
                "ShipmentReceivables",
                "ShipmentPayables",
                "Cards",
                "ShipmentPackages",
                "InsideShipmentPackages",
                "ShipmentPickUpDeliveries",
                "Agents",
                "Customers",
                "ChargesTypes",
                "PaymentTerms",
                "Countries",
                "Ports",
                "ARInvoices",
                "ARInvoiceLines",
                "APInvoices",
                "APInvoiceLines",
                "ARPayments",
                "APPayments",
                "Contacts",
                "Addresses",
                "ShipmentMasterDatas",
                "Vendors",
                "Airlines",
                "ShippingLines",
                "Truckers",
                "Users",
                "ShipmentDataView",
                "States",

            };
            tablesNumber = dataBackupTables.Count;//tables.Rows.Count;
            foreach (string tablename in dataBackupTables)
            {
                WriteTableToCSV(tablename);
            }
            //foreach (DataRow row in tables.Rows)
            //{
            //    object d = row[2];
            //    string tablename = d.ToString();
            //    WriteTableToCSV(tablename);
            //}


            //string containername = "tenant" + tenant.ToString() + "databackup";
            //CloudBlobContainer blobContainer = StorageAcountDetails.GetCurrentContainer(tenant);
            //var packupBlob = blobContainer.GetBlockBlobReference("datapackup.zip");

            BlobFileInfo zipfileInfo = new BlobFileInfo()
            {
                FileName = "datapackup",
                //FolderName = "others",
                Extension = "zip",
                Tenant = tenant,
                HasExternalContainer = true,
                ExternalContainerName = "tenant" + tenant

            };
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;


           // Crc32 crc32 = new Crc32();
            MemoryStream zipMemoryStream = new MemoryStream();
                ZipOutputStream zOutput = new ZipOutputStream(zipMemoryStream);
                zOutput.SetLevel(3);
                //if (blobContainer != null)
                //{
                foreach (string tablename in dataBackupTables)//(DataRow row in tables.Rows)
                {

                    //  object d = row[2];
                    //string tablename = d.ToString();

                    BlobFileInfo fileInfo = new BlobFileInfo()
                    {
                        FileName = tablename,
                        FolderName = "tenantbackup",
                        Extension = "csv",
                        Tenant = tenant,

                    };

                    //var blob = blobContainer.GetBlockBlobReference(tablename + ".csv");
                ZipEntry entry = new ZipEntry(tablename + ".csv");//Path.GetFileName(blob.Uri.AbsolutePath));

                    entry.DateTime = DateTime.Now;

                    byte[] buffer = storageservice.Read(fileInfo);
                    if (buffer != null)
                    {
                        //using (Stream bs = blob.OpenRead())
                        //{
                        try
                        {

                            //long length = 1;
                            //if (buffer.Length > 0)
                            //{
                            //    length = buffer.Length;
                            //}
                            //else
                            //{ length = 1; }
                            // new byte[length];
                            // bs.Read(buffer, 0, buffer.Length);
                            // entry.Size = length;
                            //bs.Close();


                            zOutput.PutNextEntry(entry);
                        MemoryStream inStream = new MemoryStream(buffer);
                        long inStreamLength = inStream.Length;
                        if (inStreamLength < 200)
                            {
                            inStreamLength = 200;
                                }

                        StreamUtils.Copy(inStream, zOutput, new byte[inStreamLength]);
                        inStream.Close();
                        zOutput.CloseEntry();


                        //crc32.Reset();
                        //crc32.Update(buffer);
                        //entry.Crc = crc32.Value;
                        //zOutput.PutNextEntry(entry);
                        //int size = 20480;
                        //int remaining = buffer.Length; 
                        //for (int i = 0; i < buffer.Length; )
                        //{
                        //    if (remaining < size)
                        //    {
                        //        size = remaining;
                        //    }
                        //    zOutput.Write(buffer, i, size);
                        //    remaining = remaining - size;
                        //    i = i + size;
                        //    zOutput.Flush();
                        //}

                            // zOutput.CloseEntry();
                        }

                        catch (Exception ex)
                        {
                            ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "WorkerRole", "DataBackupWR for tenant " + tenant.ToString() + " for table " + tablename, "");
                        }
                        // }

                    }
                }



            zOutput.IsStreamOwner = false;
            zOutput.Close();
                zipMemoryStream.Position = 0;


            //zipMemoryStream.Position = 0;

                zipfileInfo.FileSize = zipMemoryStream.ToArray().Length;
                storageservice.Write(zipMemoryStream.ToArray(), zipfileInfo);

            //zOutput.Finish();
           // zOutput.Close();

                //}


            TimeSpan ts = stopwatch.Elapsed;
            stopwatch.Stop();
            return ts.Seconds.ToString();

        }



        private void CreateTableSchema2(string tablename, SqlDataReader reader)
        {

            string filename = tablename + ".csv";
            string filepath = tablename + "schema.txt";

            //Create string builder
            StringBuilder schemaStringbuilder2 = new StringBuilder();
            schemaStringbuilder2.AppendLine("[" + filename + "]");
            schemaStringbuilder2.AppendLine("ColNameHeader=True");
            schemaStringbuilder2.AppendLine("Format=CSVDelimited");

            DataTable schema = reader.GetSchemaTable();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                string colName = reader.GetName(i);
                Type type = reader.GetFieldType(i);


                if (type.Name == "String")
                {
                    string colsize = schema.Rows[i]["ColumnSize"].ToString();
                    schemaStringbuilder2.AppendLine(colName + "=" + colName + " " + type + " Width " + colsize);

                }
                else
                    schemaStringbuilder2.AppendLine(colName + "=" + colName + " " + type);
            }

            Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
            {
                FileName = tablename + "schema",
                FolderName = "tenantbackup",
                Extension = "txt",
                Tenant = tenant,

            };
            Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
            MemoryStream blobstream = new MemoryStream();
            using (StreamWriter writer = new StreamWriter(blobstream))
            {
                writer.Write(schemaStringbuilder2);
                writer.Close();
                writer.Dispose();
            }
            storageservice.Write(blobstream.ToArray(), fileInfo);


            //var blobContainer = StorageAcountDetails.GetCurrentContainer(tenant);
            //var blobfile = blobContainer.GetBlockBlobReference(StorageAcountDetails.GetBlobNameByLocation(filepath, "tenantbackup"));



            //using (Stream blobstream = blobfile.OpenWrite())
            //{


            //    using (StreamWriter writer = new StreamWriter(blobstream))
            //    {
            //        writer.Write(schemaStringbuilder2);
            //        writer.Close();
            //        writer.Dispose();
            //    }



            //}



        }

        private void WriteTableToCSV(string tablename)
        {
			try
			{
				//if Exists(select * from sys.columns where Name = N'columnName'  
				//and Object_ID = Object_ID(N'tableName'))

				/*if Exists(select * from sys.columns where Name = N'Tenant' and Object_ID = Object_ID(N'"+tablename+"')) BEGIN Select * from " + tablename + " Where Tenant=" + tenant + " END"*/

				/*SELECT * FROM INFORMATION_SCHEMA.COLUMNS
	WHERE  TABLE_NAME='tablename' AND COLUMN_NAME='columname' )*/


				string command = "Select * from " + tablename + " Where Tenant=" + tenant;

				//SqlCommand SelectTableCommand = new SqlCommand("Select * from " + tablename+" Where Tenant="+tenant, connect);
				SqlCommand selectTableCommand = new SqlCommand(command, connect);
				selectTableCommand.CommandTimeout = 1200;

				bool noHeaderYet = true;
				using (SqlDataReader reader = selectTableCommand.ExecuteReader())
				{
					// if (Reader.HasRows)
					//  {
					StringBuilder sb = new StringBuilder();

					while (reader.Read())
					{

						if (noHeaderYet)
						{
							for (int a = 0; a < reader.FieldCount; a++)
							{
								string colName = reader.GetName(a);
								sb.Append(colName);
								sb.Append(",");
							}

							sb.AppendLine();
							noHeaderYet = false;
						}
                        
						for (int a = 0; a < reader.FieldCount; a++)
						{
							object colValue = reader.GetValue(a);
                           
							Type type = reader.GetFieldType(a);

                            if (reader.IsDBNull(a))
                            {
                                colValue = "NULL";
                            }

                            else
                            {

                                if (type.Name == "Byte[]" && colValue.ToString() != "")
                                {
                                    colValue = Convert.ToBase64String((byte[])colValue);
                                }

                                if (colValue.ToString().Contains("\""))
                                {
                                    string str = colValue.ToString().Replace("\"", "");
                                    colValue = str;
                                }

                                if (colValue.ToString().Contains(","))
                                {
                                    colValue = string.Concat("\"", colValue.ToString(), "\"");
                                }

                                else if (colValue.ToString().Contains(Environment.NewLine))
                                {
                                    colValue = string.Concat("\"", colValue.ToString(), "\"");
                                }

                                else if (colValue.ToString().Contains("\r"))
                                {
                                    colValue = string.Concat("\"", colValue.ToString(), "\"");
                                }

                                if (colValue.ToString() == string.Empty)
                                {
                                    colValue = string.Empty;
                                }
                            }

                          

                            string colStringValue = Regex.Replace(colValue.ToString(), @"\t|\n|\r", "");
                            sb.Append(colStringValue);
							sb.Append(",");
						}

                       

                        sb.Remove(sb.Length - 1, 1);
                        sb.AppendLine();

					}

					SaveTableToCSV(sb, tablename);
					// CreateTableSchema(tablename, Reader);
					// CreateTableSchema2(tablename, Reader);
					// }
				}

			}
			catch (Exception ex)
			{
				ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "WorkerRole", "DataBackupWR for tenant " + tenant.ToString() + " for table " + tablename + Environment.NewLine, "");
			}
		}


        private void SaveTableToCSV(StringBuilder sb, string tablename)
        {

            Encoding currentEncoding = Encoding.GetEncoding(encodingCodePage);
            byte[] sbByte = currentEncoding.GetBytes(sb.ToString());
            string encodedString = currentEncoding.GetString(sbByte);

            //string resultString = Regex.Replace(encodedString, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);

            string filename = tablename + ".csv";


            Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
            {
                FileName = tablename,
                FolderName = "tenantbackup",
                Extension = "csv",
                Tenant = tenant,

            };
            Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
            MemoryStream blobstream = new MemoryStream();
            using (StreamWriter writer = new StreamWriter(blobstream))
            {
                writer.Write(sb);
                writer.Close();
                writer.Dispose();
            }
            storageservice.Write(blobstream.ToArray(), fileInfo);

            //string containername = "tenant" + tenant.ToString();
            //CloudBlobContainer blobContainer = StorageAcountDetails.BlobClient.GetContainerReference(containername);
            //blobContainer.CreateIfNotExists();

            //var blobfile = blobContainer.GetBlockBlobReference(filename);



            //using (Stream blobstream = blobfile.OpenWrite())
            //{


            //    using (StreamWriter writer = new StreamWriter(blobstream))
            //    {
            //        writer.Write(sb);
            //        writer.Close();
            //        writer.Dispose();
            //    }



            //}




        }


        public static string GetConnection(int tenant)
        {
            GlobalDBRepository globalDbRep;
            GlobalDB currentDb;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                //GlobalDBRep = new GlobalDBRepository();
                currentDb = GlobalDBRepository.GetGlobalDBByTenant(tenant);
            }
            string dbConnectionInfo = !string.IsNullOrEmpty(currentDb.SecondaryAzureDBConnection) ? currentDb.SecondaryAzureDBConnection: currentDb.DBConnection;

            // Specify the provider name, server and database.
            string providerName = "System.Data.SqlClient";
            string serverName = ".";
            string databaseName = "";


            string[] information = dbConnectionInfo.Split(',');
            string dbName = information[0];
            string userName = information[1];
            string pass = information[2];
            string servername = information[3];
            SqlConnectionStringBuilder sqlBuilder =
                new SqlConnectionStringBuilder();

            // Set the properties for the data source.
            sqlBuilder.DataSource = servername;
            sqlBuilder.InitialCatalog = dbName;
            sqlBuilder.IntegratedSecurity = false;
            //sqlBuilder.PersistSecurityInfo = true;
            sqlBuilder.Password = pass;
            sqlBuilder.UserID = userName;
            sqlBuilder.MultipleActiveResultSets = true;
            // Build the SqlConnection connection string.
            string providerString = sqlBuilder.ToString();

            // Initialize the EntityConnectionStringBuilder.
            //EntityConnectionStringBuilder entityBuilder =
            //    new EntityConnectionStringBuilder();

            //Set the provider name.
            //entityBuilder.Provider = providerName;

            // Set the provider-specific connection string.
            //entityBuilder.ConnectionString = providerString;
            //entityBuilder.Metadata = string.Format(@"res://*/{0}.csdl|res://*/{0}.ssdl|res://*/{0}.msl",
            //    "CommonDataModel.CommonDataModel");

            // EntityConnection connection = new EntityConnection(entityBuilder.ConnectionString);//(entityBuilder.ConnectionString);


            return providerString;// entityBuilder.ConnectionString;
        }

    }
}
