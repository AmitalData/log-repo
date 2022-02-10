using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.Helpers;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.Security;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
//using Profact.TimbraCFDI;
//using Profact.TimbraCFDI;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Logitude.BL.Resolvers;
using Profact.TimbraCFDI40;

namespace Logitude.BL.InvoiceModel.Tools.ExternalService.SATProfact40
{
    public class SATCancelInvoiceProfact40Service
    {
        private ARInvoicePM arInvoicePM;
        private ARInvoice arInvoice;
        private SATCommunicationLogBuilder sATCommunicationLogBuilder;

        public SATCancelInvoiceProfact40Service(ARInvoicePM arInvoicePM, ARInvoice arInvoice)
        {
            this.arInvoicePM = arInvoicePM;
            this.arInvoice = arInvoice;
            this.sATCommunicationLogBuilder = new SATCommunicationLogBuilder(arInvoicePM.Tenant);
        }

        public void SendRequest()
        {
            bool isValidToTransferToSAT = ValidateTransferToSAT();
            if (!isValidToTransferToSAT)
            {
                SetSATTransferStatus(SATData.NoTransferNeedSATTransferStatusCode);
                return; 
            }

            BuildProfactCommunicationLog();
            SetSATTransferStatus(SATData.InTransferingSATTransferStatusCode);
        }

        private bool ValidateTransferToSAT()
        {
            if (arInvoice.SATTransferStatusCode == SATData.InTransferingSATTransferStatusCode)
                throw new ApplicationException("You are not allowed to void the invoice while its status is Transferring to SAT");

            if (!string.IsNullOrEmpty(arInvoice.SATXML))
            {
                return true;
            }

            return false;
        }

        private void SetSATTransferStatus(string sATStatusCode)
        {
            arInvoice.SATTransferStatusCode = arInvoicePM.SATTransferStatusCode = sATStatusCode;
        }

        private void BuildProfactCommunicationLog()
        {
            Comprobante comprobante = GetProfactComprobante();
            sATCommunicationLogBuilder.Build(new SATCommunicationLogArgs{ Comprobante = comprobante, EntityId = arInvoicePM.Id, EntityReference = arInvoicePM.InvoiceNumber.ToString(), IsCancellation = true });
        }

        private Comprobante GetProfactComprobante()
        {
            Encoding uTF8Encoding = Encoding.UTF8;
            byte[] profactoXMLData = uTF8Encoding.GetBytes(arInvoice.SATXML);
            return LogitudeXmlSerializer.DeserializeObject<Comprobante>(profactoXMLData);
        }
    }
}
