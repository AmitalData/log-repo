using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Services;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using WebFreight.Web.CommonDataModel;
using WebFreight.Web.InvoiceModel;
using Logitude.BL.InvoiceModel.EntityPMs;
using Microsoft.WindowsAzure.Storage;
using Simplog.Server.Infrastructure.Azure;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using Logitude.BL.CommonDataModel.EntityPMs;
using System.Xml.Serialization;
using System.Xml;
using Logitude.BL.CommonDataModel.EntityQueries;
using System.Xml.Linq;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityOtherServices;

namespace WebFreight.Web.MessageModel
{
    /// <summary>
    /// Summary description for MessageWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class MessageWebService : System.Web.Services.WebService
    {
        [WebMethod]
        public string GetARInvoiceMessageData(string invoiceId, int tenant)
        {
            ARInvoiceMessageHelper myHelper = new ARInvoiceMessageHelper(tenant);
            string myResult = myHelper.GetMessageData(invoiceId, tenant);
            return myResult;
        }

        [WebMethod]
        public string GetAPInvoiceMessageData(string invoiceId, int tenant)
        {
            APInvoiceMessageHelper myHelper = new APInvoiceMessageHelper(tenant);
            string myResult = myHelper.GetMessageData(invoiceId, tenant);
            return myResult;
        }


        // Transfer
        public void TransferARInvoices(List<ARInvoice> entities, string filename, int tenant)
        {
            MessageEntityService MessageEntityService = new MessageEntityService();
            MessageEntityService.TransferARInvoices(entities, filename, tenant);
        }
        public void TransferAPInvoices(List<APInvoice> entities, string filename, int tenant)
        {
            MessageEntityService MessageEntityService = new MessageEntityService();
            MessageEntityService.TransferAPInvoices(entities, filename, tenant);
        }
        public void TransferARPayments(List<ARPayment> entities, string filename, int tenant)
        {
            MessageEntityService MessageEntityService = new MessageEntityService();
            MessageEntityService.TransferARPayments(entities, filename, tenant);
        }

        // Rebuild
        public void RebuildTransferFile(List<ARInvoice> entities, string filename, int tenant)
        {
            MessageEntityService MessageEntityService = new MessageEntityService();
            MessageEntityService.RebuildTransferFile(entities, filename, tenant);
        }
        public void RebuildTransferFile(List<APInvoice> entities, string filename, int tenant)
        {
            MessageEntityService MessageEntityService = new MessageEntityService();
            MessageEntityService.RebuildTransferFile(entities, filename, tenant);
        }
        public void RebuildTransferFile(List<ARPayment> entities, string filename, int tenant)
        {
            MessageEntityService MessageEntityService = new MessageEntityService();
            MessageEntityService.RebuildTransferFile(entities, filename, tenant);
        }
    }
}
