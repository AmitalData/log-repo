//using Logitude.SystemLogs;
//using Microsoft.ServiceBus.Messaging;
//using Microsoft.WindowsAzure.ServiceRuntime;
//using Simplog.Data.InvoiceModel;

//using Simplog.Server.Infrastructure.Azure;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Web;
//using System.Xml;
//using System.Xml.Serialization;
//using System.Xml.XPath;
//using Logitude.BL.InvoiceModel.EntityPMs;
//using Logitude.BL.InvoiceModel.EntityQueries;
//using Logitude.BL.InvoiceModel.Tools.EntityService;

//namespace CommunicationWorkerRole
//{
//  public  class ExactOnlineWorkerRole : WorkerEntryPoint
//    {

//        QueueDescription queueDescription;
//        QueueClient client;

//        const int maxSizeToLoad = 500;
//        HttpData http = new HttpData();

//      public override void Run()
//      {

//          while (IsRunning)
//          {
//              if (!General.IsUpdating())
//              {
//                  try
//                  {
//                      CheckForNewExternalCodes();
//                      CheckForReadyInvoices();
//                  }
//                  catch (Exception ex)
//                  {
//                      ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "exact online worker", null, null);
//                      Thread.Sleep(10000);
//                  }

//              }
//              else
//              {
//                  Thread.Sleep(60000);
//              }
//          }
//      }

   
//      private void CheckForNewExternalCodes()
//      {
//          http.CredentialsChanged = true;
//          RetrieveAdministrations();
//        //  FillTopics();
//          ProcessSingle();

//      }

//      private void ProcessSingle()
//      {
//          HttpWebRequest request = default(HttpWebRequest);
//          HttpWebResponse response = default(HttpWebResponse);
//          Stream receiveStream = default(Stream);
//          StreamReader readStream = default(StreamReader);
//            string sMessages = "";
//        try
//        {
//        SetCredentials();


//        //if (cmbDirection.SelectedIndex == HttpData.Directions.Import) 
//        //{
//        //    request = http.CreateWebRequest((HttpData.Directions)cmbDirection.SelectedIndex, txtURL.Text, Convert.ToString(cmbDivision.SelectedValue), Convert.ToString(cmbTopic.SelectedItem), txtApplicationKey.Text, txtFile.Text);

//        //    if ((request != null))
//        //    {
//        //        response = (HttpWebResponse)request.GetResponse();
//        //        receiveStream = response.GetResponseStream();

//        //        http.GetMessages(receiveStream, sMessages);
//        //    }

//        //} 
//        //else
//        //{
//            string sPagingTimeStamp = "";
//            int iResultCount = 0;
//            int iPageSize = 0;
//            int i = 1;
//            do {
//                request = http.CreateWebRequest(HttpData.Directions.Export, http.txtURL, http.cmbDivision, "Accounts", http.appKey, "", sPagingTimeStamp);
//                request = http.CreateWebRequest(HttpData.Directions.Export, http.txtURL, http.cmbDivision, "VATs", http.appKey, "", sPagingTimeStamp);
//                request = http.CreateWebRequest(HttpData.Directions.Export, http.txtURL, http.cmbDivision, "Items", http.appKey, "", sPagingTimeStamp);
//                request = http.CreateWebRequest(HttpData.Directions.Export, http.txtURL, http.cmbDivision, "PaymentConditions", http.appKey, "", sPagingTimeStamp);

//                //if ((request != null)) {
//                //    response = (HttpWebResponse)request.GetResponse();
//                //    receiveStream = response.GetResponseStream();
//                //    readStream = new StreamReader(receiveStream, Encoding.UTF8);

//                //    //It can be read only once, so store in this buffer
//                //    string sBuffer = readStream.ReadToEnd();
//                //    byte[] bBuffer = Encoding.UTF8.GetBytes(sBuffer);

//                //    //No messages means that all went correctly
//                //    http.GetMessages(new MemoryStream(bBuffer), sMessages);

//                //    //If no timestamp and no result count found there is nothing to download (anymore)
//                //    sPagingTimeStamp = http.GetTimeStamp(new MemoryStream(bBuffer));

//                //    iResultCount = http.GetResultCount(new MemoryStream(bBuffer));
//                //    iPageSize = http.GetPageSize(new MemoryStream(bBuffer));

//                    //if (sMessages.Length == 0)
//                    //{
//                    //    if (iResultCount > 0) {
//                    //        string sFile = String.Replace(txtFile.Text, ".XML", "_" + i + ".XML", , , CompareMethod.Text);

//                    //        if (!File.Exists(sFile) || Interaction.MsgBox("File '" + sFile + "' already exists, overwrite?", MsgBoxStyle.YesNoCancel, Title) == MsgBoxResult.Yes) {
//                    //            File.WriteAllText(sFile, sBuffer, Encoding.UTF8);
//                    //        }

//                    //        if (chkOpenAttachments.Checked) {
//                    //            //Save attachments and pictures and open them
//                    //            http.SaveAttachments(Convert.ToString(cmbTopic.SelectedItem), new MemoryStream(bBuffer));
//                    //        }
//                    //    } else if (i == 1) {
//                    //        Interaction.MsgBox("No data found.");
//                    //    }
//                    //}
//                //}
//                i += 1;
//            } while (sPagingTimeStamp.Length > 0 & iResultCount >= iPageSize);
//        //}
//    } 
    
    
//    catch (Exception ex) 
//    {
//        //Interaction.MsgBox("Error in method ProcessSingle: " + ex.Message, , Title);
//    }

//    //this.Cursor = Cursors.Default;

//    if (sMessages.Length >= 200) 
//    {
//        //dlgMessages.txtMessages.Text = sMessages;
//        //dlgMessages.ShowDialog();
//    } else if (sMessages.Length > 0)
//    {
//        //Interaction.MsgBox(sMessages, , Title);
//    }

//    if ((request != null))
//        request.Abort();
//    if ((readStream != null))
//        readStream.Close();
//    if ((receiveStream != null))
//        receiveStream.Close();
//    if ((response != null))
//        response.Close();
//      }

	



//      private void RetrieveAdministrations()
//      {
//       //  this.Cursor = Cursors.WaitCursor;
//          SetCredentials();
//          try
//          {
//              http.GetAdministrations(http.txtURL, http.cmbDivision);
//          }


//          catch (Exception ex)
//          {
//              //Interaction.MsgBox(ex.Message, , Title);
//          }
//      // this.Cursor = Cursors.Default;
//      }
       
	



//      private void SetCredentials()
//      {
//          http.Credentials.UserName = "jalal@logitudeworld.com";
//          http.Credentials.Password = "!J123456";
//      }



//      private void CheckForReadyInvoices()
//      {

//          IInvoiceContext context = InvoiceContext.GetContext(1);
//          ARInvoiceQuery invoiceQuery = new ARInvoiceQuery(1);
//          ARInvoicePM invoice = invoiceQuery.GetReadyForTransferOrErrorInTransferInvoicePM(1);
//          string systemEmail = "system@tenant" + 1 + ".com";
//          ARInvoiceService invoiceService = new ARInvoiceService(context, 1, systemEmail);
//          string invoiceXML = null;
//          if (invoice != null)
//          {
//              if (!invoice.IsTransferStarted)
//              {
//                  invoice.IsTransferStarted = true;
//                  invoice.TransferTries++;
//                  invoiceXML = AddNewInvoice(invoice);
//              }
//              else if (invoice.IsTransferStarted && invoice.TransferTries < 5)
//              {
//                  invoice.TransferTries++;
//                  invoiceXML = AddNewInvoice(invoice);
              
//              }

//              invoiceService.Update(invoice, invoice.InvoiceLines, invoice.InvoicePayments,invoice.ConstituentInvoices);






//              HttpWebRequest request = default(HttpWebRequest);
//              HttpWebResponse response = default(HttpWebResponse);
//              Stream receiveStream = default(Stream);
//              StreamReader readStream = default(StreamReader);
//              string sMessages = "";


//              try
//              {
//                  SetCredentials();



//                  request = http.CreateWebRequest(HttpData.Directions.Import, http.txtURL, http.cmbDivision, "Invoices", http.appKey, invoiceXML);

//                  if ((request != null))
//                  {
//                      response = (HttpWebResponse)request.GetResponse();
//                      receiveStream = response.GetResponseStream();

//                      http.GetMessages(receiveStream, sMessages);
                     
//                  }

//              }

//              catch (Exception ex)
//              {
//                  //Interaction.MsgBox("Error in method ProcessSingle: " + ex.Message, , Title);
//              }

//              //this.Cursor = Cursors.Default;

//              if (sMessages.Length >= 200)
//              {
//                  //dlgMessages.txtMessages.Text = sMessages;
//                  //dlgMessages.ShowDialog();
//              }
//              else if (sMessages.Length > 0)
//              {
//                  //Interaction.MsgBox(sMessages, , Title);
//              }

//              if ((request != null))
//                  request.Abort();
//              if ((readStream != null))
//                  readStream.Close();
//              if ((receiveStream != null))
//                  receiveStream.Close();
//              if ((response != null))
//                  response.Close();

//          }
//      }

//      private string AddNewInvoice(ARInvoicePM invoice)
//      {

      

//          eExact exact = new eExact();
//          exact.Invoices = new Invoice[1];
//          exact.Invoices[0] = new Invoice()
//          {
//              ordernumber = "1",
//              invoicenumber = invoice.InvoiceNumber,
//              typeSpecified = true,
//              type = ExactOnlineIntegration.exact.InvoiceType.Item8020,
//              status = InvoiceStatus.Item50,
//              InvoiceDate = invoice.InvoiceDate.Value,
              
//              InvoiceDateSpecified = true,
//              statusSpecified = true,
            
//              DueDate = invoice.DueDate.Value,//DateTime.Now.AddDays(30),
//              //Description = new typeDescription() { Value = "Invoice1" },
//              //YourRef = "123",
//             // Notes = "Nothing",
//              OrderDateSpecified = true,
//              OrderedBy =  new typeAccount() {  ID=invoice.DebitAccount, },
//              DeliverTo = new typeAccount() { ID=invoice.DebitAccount, },
//              //DeliveryAddress = new DeliveryAddress() { Country = new ExactOnlineIntegration.exact.Country() { code = "PS" } },
//              InvoiceTo =  new typeAccount() { ID = invoice.DebitAccount, },
//              PaymentCondition = new PaymentCondition() { code = invoice.PaymentTermExternalId, },
        
//              Journal = new Journal() { code = "JJ1", Description = new typeDescription() { Value = "Journal  1" } },


//          };

//          int i = 0;
//          int count = invoice.InvoiceLines.Count();
//          exact.Invoices[0].InvoiceLine = new InvoiceInvoiceLine[count];
//          foreach (ARInvoiceLinePM line in invoice.InvoiceLines)
//          {
//              exact.Invoices[0].InvoiceLine[i] = new InvoiceInvoiceLine()
//              {
//                  line = (i + 1).ToString(),
//                  Quantity = (float)line.Quantity,
                     
//                  UnitPrice = new typePrice() { Value = (float)line.UnitPrice, Currency = new typeCurrency() { code = line.InvoiceCurrencyCode } },
//                  // Description = new typeDescription() { Value = "line1" },
//                 // ForeignAmount = new ForeignAmount() {   } line.InvoiceCurrencyAmount
//                  Item = new typeItem() { code= line.CreditAccount ,  Description = new typeDescription() { Value= line.Description} },
//               NetPrice = new typePrice() { VAT = new VAT(){ code = line.ExternalVATCard}}
//              };
//              i++;
//          }

//          //exact.Invoices[0].InvoiceLine = new InvoiceInvoiceLine[2];

//          //exact.Invoices[0].InvoiceLine[0] = new InvoiceInvoiceLine()
//          //{
//          //    line = "1",
//          //    Quantity = 2.2f,
//          //    UnitPrice = new typePrice() { Value = 20, Currency = new typeCurrency() { code = "USD" } },
//          //    Description = new typeDescription() { Value = "line1" },
//          //    Item = new typeItem() { code = "AFT", Description = new typeDescription() { Value = "Air Freight" } },
//          //};


//          //exact.Invoices[0].InvoiceLine[1] = new InvoiceInvoiceLine()
//          //{
//          //    Description = new typeDescription() { Value = "line2" },
//          //    Item = new typeItem() { code = "AFT", Description = new typeDescription() { Value = "Air Freight" } },
//          //    line = "2",
//          //    Quantity = 1.3f,
//          //    UnitPrice = new typePrice() { Value = 30, Currency = new typeCurrency() { code = "USD" } },
//          //    Unit = new Unit() { code = "cm", Description = new typeDescription() { Value = "Distance" } },

//          //};


//          XmlSerializer xsSubmit = new XmlSerializer(typeof(eExact));

//          StringWriter sww = new StringWriter();
//          XmlWriter writer = XmlWriter.Create(sww);
//          xsSubmit.Serialize(writer, exact);
//          var xml = sww.ToString();
//          return xml;
//      }

     

//      public override bool OnStart()
//      {
//          // Set the maximum number of concurrent connections 
//          ServicePointManager.DefaultConnectionLimit = 12;


//          //DiagnosticMonitor.Start("DiagnosticsConnectionString");

//          // For information on handling configuration changes
//          // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
//          RoleEnvironment.Changing += RoleEnvironmentChanging;

//          return base.OnStart();
//      }

//      private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
//      {

//          // If a configuration setting is changing
//          if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
//          {

//              // Set e.Cancel to true to restart this role instance
//              e.Cancel = true;
//          }
//      }

//    }
//}
