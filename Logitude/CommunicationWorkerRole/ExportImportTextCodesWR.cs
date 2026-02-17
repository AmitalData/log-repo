using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using System.Data.SqlClient;

using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Azure;


using System.Data;
using System.IO;
using System.Threading;
using System.Xml;
using System.Linq;

using System.Transactions;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Blob;
using Simplog.Server.Infrastructure.Helpers;

namespace CommunicationWorkerRole
{
    class ExportImportTextCodesWR : WorkerEntryPoint
    {


        CloudQueue exporttextcodequeue;

        public override void Run()
        {
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    if (exporttextcodequeue.Exists())
                    {
                        var exporttextcodespmsg = exporttextcodequeue.GetMessage();
                        LastActivity = DateTime.UtcNow;
                        if (exporttextcodespmsg != null)
                        {
                            string[] result = exporttextcodespmsg.AsString.Split(',');

                            string servername = result[0];
                            string username = result[1];
                            string password = result[2];
                            string databaseName = result[3];
                            string blobname = result[4];
                            string messageType = result[5];


                            SqlConnectionStringBuilder sqlSB;

                            sqlSB = new SqlConnectionStringBuilder()
                            {
                                DataSource = servername,
                                InitialCatalog = databaseName,
                                IntegratedSecurity = false,
                                UserID = username,
                                Password = password,
                                Encrypt = true,
                                TrustServerCertificate = true,
                                MultipleActiveResultSets = true,

                            };

                            CloudBlobContainer container = StorageAcountDetails.BlobClient.GetContainerReference("textcodes");
                            container.CreateIfNotExists();
                            //container.SetPermissions(new BlobContainerPermissions() { PublicAccess = BlobContainerPublicAccessType.Container });
                            //var blobName = blobname;//string.Format("{0}.dacpac", DatabaseName.ToLower() + DateTime.Now.ToString("_yyyyMMddHHmmss"));
                            var blob = container.GetBlockBlobReference(blobname.ToLower());

                            #region export region

                            if (messageType == "export")
                            {
                                DataSet ds = new DataSet("TextCodes");

                                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter("Select * from TextCodes Where Tenant=0", sqlSB.ConnectionString);
                                da.Fill(ds);

                                MemoryStream memStream;
                                using (Stream blobstream = blob.OpenWrite())
                                {

                                    memStream = new MemoryStream();

                                    ds.WriteXml(blobstream, XmlWriteMode.IgnoreSchema);


                                }
                            }
                            #endregion

                            #region import Region
                            else
                            {
                                XmlDocument doc = null;
                                using (MemoryStream memstream = new MemoryStream())
                                {
                                    if (blob.Exists())
                                    {
                                        blob.DownloadToStream(memstream);
                                        memstream.Seek(0, System.IO.SeekOrigin.Begin);


                                        doc = new XmlDocument();

                                        doc.Load(memstream);
                                    }

                                }

                                if (doc != null)
                                {
                                    List<GlobalTenant> tenantList;
                                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                                    {
                                        GlobalTenantRepository globaltenantRep = new GlobalTenantRepository();
                                        tenantList = globaltenantRep.GetActiveTenants();
                                    }


                                    foreach (GlobalTenant tenant in tenantList)
                                    {
                                        TextCodeRepository textCodeRep = new TextCodeRepository(tenant.Id);
                                        List<TextCode> textCodeList = textCodeRep.GetTextCodesByTenant(tenant.Id).ToList();

                                        XmlNodeList textCodeNodes = doc.GetElementsByTagName("Table");//doc.SelectNodes("//IsSpellChecked[contains(., 'true')]");//
                                        int submitCounter = 0;

                                        foreach (XmlNode node in textCodeNodes)
                                        {
                                            XmlNode isSpellCheckedNode = node.SelectSingleNode("IsSpellChecked");
                                            bool isSpellChecked = Convert.ToBoolean(isSpellCheckedNode.InnerText.ToUpper());
                                            if (isSpellChecked)
                                            {
                                                XmlNode codeNode = node.SelectSingleNode("Code");
                                                XmlNode defaultTextNode = node.SelectSingleNode("DefaultText");
                                                XmlNode localDefaultTextNode = node.SelectSingleNode("LocalDefaultText");
                                                XmlNode spellCheckDateNode = node.SelectSingleNode("SpellCheckDate");

                                                TextCode textcode = textCodeList.Where(d => d.Code == codeNode.InnerText).FirstOrDefault();

                                                if (textcode != null)
                                                {                      
                                                    textcode.DefaultText = defaultTextNode != null ? defaultTextNode.InnerText : null;
                                                    textcode.IsSpellChecked = Convert.ToBoolean(isSpellCheckedNode.InnerText.ToUpper());
                                                    textcode.LocalDefaultText = localDefaultTextNode != null ? localDefaultTextNode.InnerText : null;

                                                    if (spellCheckDateNode != null)
                                                    {
                                                        textcode.SpellCheckDate = Convert.ToDateTime(spellCheckDateNode.InnerText);
                                                    }

                                                    textCodeRep.Update(textcode);
                                                }

                                                submitCounter++;

                                                if (submitCounter == 300)
                                                {
                                                    submitCounter = 0;
                                                    textCodeRep.SubmitChanges();
                                                }
                                            }

                                        }

                                        textCodeRep.SubmitChanges();
                                    }

                                }

                            }
                            #endregion

                            exporttextcodequeue.DeleteMessage(exporttextcodespmsg);
                            LogDoneItemInMemory();
                        }
                    }

                    Thread.Sleep(60000);
                }
                else
                {
                    Thread.Sleep(180000);
                }
            }

        }

        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "ExportImportTextCodes";
            DoneItemsInRange = new Dictionary<DateTime, int>();

            exporttextcodequeue = StorageAcountDetails.QueueClient.GetQueueReference("textcodeexport");
            exporttextcodequeue.CreateIfNotExists();
            exporttextcodequeue.Clear();
            return base.OnStart();


        }
    }
}
