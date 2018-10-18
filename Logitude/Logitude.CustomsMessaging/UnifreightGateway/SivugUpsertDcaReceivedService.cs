using Logitude.AmitalMessaging.Customs.CustomFile.Sivug;
using Logitude.Customs.Def.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.CustomsMessaging.Common;
using System.Diagnostics;
using Logitude.Server.Tools.Models;
using Simplog.Server.Infrastructure.Helpers;
using System.Transactions;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.AmitalMessaging.Utils;
using Logitude.CustomsMessaging.Testers.Messages;
using Logitude.AmitalMessaging.Infrastructure;
using Logitude.Server.Tools.Contracts;

namespace Logitude.CustomsMessaging.UnifreightGateway
{
    public class SivugUpsertDcaReceivedService : UnifreightGenericService
    {
        private LOGISIVUG _LOGISIVUG;
        private SIVUG _SIVUG;



        public const string UpsertActionConst = "Logitude.Customs.BL.Messaging.U2L.Sivug.SivugUpsertDcaReceivedService.Upsert()";


        private Stopwatch _Stopwatch;


        protected override int ResolvedTenant()
        {

            return base.ResolvedTenant();


        }

        public SivugUpsertDcaReceivedService()
            : base(
            "1.000.000001",
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
            true
            )
        {

        }


        public override void ProccessGenericRequest(
             string xmlLOGISIVUG,
             ref string MoreParams,
             out string MessageOut
           )
        {
            MessageOut = "";
            _Stopwatch = Stopwatch.StartNew();
            System.Threading.Thread.Sleep(5000);
            MyCommunicationsParams.Subject = "SivugUpsertService ";
            if (this._SIVUG != null)
            {

            }
            else
            {
                DeserilazeObject(xmlLOGISIVUG);
            }

            AppendLogLine("DeserilazeObject:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();

            CheckIntegrity();




            string uniComm = null;
            string fileName = null;
            var transmitionDateTime = DateTime.Now;
            string xmlESBResponseXmlClass = null;



            LOGISIVUG responseLOGISIVUG = _LOGISIVUG;




            var myLOGISIVUGWithResponseContentHeader = new LOGISIVUGWithResponseContentHeader()
            {
                MyLOGISIVUG = responseLOGISIVUG,
                MyMoreParams = MoreParams,
                ResponseContentHeader = new DefaultResponseContentHeader()
                {
                    ///ApplicationID = MyGenericResponseObj.CorrelationId.ToString() ,

                    //ApplicationID="16021007405812",
                    TransmitionDateTime = transmitionDateTime
                },
            };

            var body = XmlGenericUtil<LOGISIVUGWithResponseContentHeader>.SerializeObject(myLOGISIVUGWithResponseContentHeader);
            //
            body = body.Substring(body.IndexOf(Environment.NewLine));
            var myESBResponseXmlClass = new ESBResponseXmlClass();
            var extrenalId = "62833ff7-1cd3-4faa-85a6-a4312ae4797a";
            extrenalId = uniComm ?? Guid.NewGuid().ToString();
            xmlESBResponseXmlClass = myESBResponseXmlClass.Get(Guid.NewGuid().ToString(), extrenalId, body);
            var transTime = "2016-04-19_13-35-13-481";

            transTime = transmitionDateTime.ToString("s").Replace("T", "_").Replace(":", "-");
            transTime += "-";
            transTime += transmitionDateTime.Millisecond.ToString();

            fileName = "DcaPrefixName.IL941079089." + transTime + "." + extrenalId + ".PLT.xml";






            var s = new Unifreight_L2US01_US2L01_SivugMessagingService();

            var ourRef = "";
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                //Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.Clear();
                var InterfaceManagementQS = new InterfaceManagementQueryService(ResolvedTenant());
                var InterfaceManagementPM = InterfaceManagementQS.GetSingleInterfaceManagementwithDefinition(
                    s.MainInterfaceCode +"I", 
                    ResolvedTenant());
                fileName = fileName.Replace("DcaPrefixName.", InterfaceManagementPM.DcaPrefixName);
                ourRef = s.DcaReceivedCustomResponseCorrelation(InterfaceManagementPM, ResolvedTenant(), new Customs.BL.Utils.DCAFileModel()
                {
                    SelectedFileDownload = fileName,
                    TimStamp = transmitionDateTime

                }, xmlESBResponseXmlClass);
                scope.Complete();
            }

            MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.Success;
            MyGenericResponseObj.ApplicationId = ourRef;

        }


        private void DeserilazeObject(string xmlLOGISIVUG)
        {

            MyGenericResponseObj.Stage = "Initalize ProccessRequest";
            AppendLogLine("SivugUpsertService.ProccessRequest");

            AppendLogLine("Deserialize(DataIn1) ..");


            if (string.IsNullOrWhiteSpace(xmlLOGISIVUG))
            {
                throw new BusinessErrorException("DataIn1 is missing");
            }
            if (xmlLOGISIVUG.Length > 1000)
            {
                AppendLogLine("XmlIn=" + xmlLOGISIVUG.Substring(0, 1000));
                AppendLogLine(".Substring(0, 1000)");
            }
            else
            {
                AppendLogLine("XmlIn=" + xmlLOGISIVUG);
            }


            AppendLogLine("Tring DeserilazeObject");
            MyGenericResponseObj.Stage = "Trying DeserilazeObject";
            this._LOGISIVUG = XmlGenericUtil<LOGISIVUG>.DeSerializeObject(xmlLOGISIVUG);

            if (_LOGISIVUG.SIVUG == null || _LOGISIVUG.SIVUG.Length != 1)
            {
                throw new BusinessErrorException("_LOGISIVUG.SIVUG.Length != 1");
            }
            this._SIVUG = _LOGISIVUG.SIVUG[0];
        }

        private void CheckIntegrity()
        {
            MyGenericResponseObj.Stage = "Check integrity ";

            if (String.IsNullOrWhiteSpace(this._SIVUG.LOGITUDEFILE))
            {
                throw new BusinessErrorException("LOGITUDEFILE is missing");
            }
            AppendLogLine("LOGITUDEFILE = " + this._SIVUG.LOGITUDEFILE);
        }




        public override string GetAssemblyQualifiedName()
        {
            throw new NotImplementedException();
        }

        public override string GetExampleDataIn1()
        {
            var xml = "";
            var amitalObjExample = new LOGISIVUG();
            var myAmitalSivug = new SIVUG();
            var myAmitalSivugInvoice = new List<INVOICE>();
            var myAmitalSivugInvoiceItem = new List<INVOICEITEMS>();

            myAmitalSivug.LOGITUDEFILE = "1-1";
            myAmitalSivug.TENANT = "1";
            myAmitalSivugInvoice.Add(new INVOICE());
            myAmitalSivugInvoiceItem.Add(new INVOICEITEMS());
            myAmitalSivugInvoice.Add(new INVOICE());
            myAmitalSivugInvoiceItem.Add(new INVOICEITEMS()); 
            myAmitalSivugInvoice[1].INVOICELINENO = "1";
            myAmitalSivugInvoice[1].INVOICELINENO = "1";
            myAmitalSivugInvoice[1].ACCOUNTTYPE = "380";
            myAmitalSivugInvoice[1].INVOICENUMBER = "999";
            myAmitalSivugInvoice[1].VENDORNUMBER = "2000475";
            myAmitalSivugInvoice[1].CURRENCYCODE = "18";
            myAmitalSivugInvoice[1].INVOICEAMOUNT = "2";
            myAmitalSivug.INVOICE = myAmitalSivugInvoice.ToArray();

            myAmitalSivugInvoiceItem[1].CLASSIFICATIONCODE = "260300009";
            myAmitalSivugInvoiceItem[1].TRADEAGREEMENTCODE = "BGR";
            myAmitalSivugInvoiceItem[1].QUANTITY = "1";
            myAmitalSivugInvoiceItem[1].ITEMPRICE = "1";
            myAmitalSivugInvoiceItem[1].ITEMORIGINCOUNTRY = "AD";
            myAmitalSivugInvoiceItem[1].ITEMCODE = "DFDF";
            myAmitalSivug.INVOICE[1].INVOICEITEMS = myAmitalSivugInvoiceItem.ToArray();

            amitalObjExample.SIVUG = new SIVUG[] { myAmitalSivug };

            xml = XmlGenericUtil<LOGISIVUG>.SerializeObject(amitalObjExample);

            return xml;
        }

        public override string GetExampleDataIn2()
        {
            throw new NotImplementedException();
        }

        public override string GetExampleDataout1()
        {
            throw new NotImplementedException();
        }

        public override string GetExampleDataout2()
        {
            throw new NotImplementedException();
        }

        public override void ProccessRequest(string DataIn1, string DataIn2, out string DataOut1, out string DataOut2, out string SUCCESS, ref string MoreParams, out string MessageOut)
        {
            throw new NotImplementedException();
        }

        public override void ProccessBASE64Request(string BASE64DataIn1, string BASE64DataIn2, string BASE64DataIn3, out string BASE64DataOut1, out string BASE64DataOut2, out string BASE64DataOut3, out string SUCCESS, ref string MoreParams, out string MessageOut)
        {
            throw new NotImplementedException();
        }
    }
}