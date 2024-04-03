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
            const string SATCancelErrorsInRelationReasonCode = "01";
            if (arInvoice.SATTransferStatusCode == SATData.InTransferingSATTransferStatusCode && arInvoice.SATCancelReasonCode != SATCancelErrorsInRelationReasonCode)
                throw new ApplicationException("You are not allowed to void the invoice while its status is Transferring to SAT");

            if (!string.IsNullOrEmpty(arInvoice.SATXML))
            {
                return true;
            }

            return false;
        }

        private void SetSATTransferStatus(string sATStatusCode)
        {
            if (FeatureToggleHelper.HasFeatureToggle("INU", arInvoice.Tenant))
            {
                arInvoicePM.SATTransferStatusCode = sATStatusCode;
            }
            else
            {
                arInvoice.SATTransferStatusCode = arInvoicePM.SATTransferStatusCode = sATStatusCode;
            }
        }

        private void BuildProfactCommunicationLog()
        {
            try
            {
                Comprobante comprobante = GetProfactComprobante();
                sATCommunicationLogBuilder.Build(new SATCommunicationLogArgs { Comprobante = comprobante, ARInvoicePM = arInvoicePM, IsCancellation = true });
            }
            catch (Exception ex)
            {
                Profact.TimbraCFDI33.Comprobante comprobanteV3 = GetProfactComprobanteV3();
                sATCommunicationLogBuilder.Build(new SATCommunicationLogArgs { ComprobanteV3 = comprobanteV3, IsVersion3 = true, ARInvoicePM = arInvoicePM, IsCancellation = true });
            }
        }

        private Comprobante GetProfactComprobante()
        {
            byte[] profactoXMLData = Encoding.UTF8.GetBytes(arInvoice.SATXML);
            return LogitudeXmlSerializer.DeserializeObject<Comprobante>(profactoXMLData);
        }

        private Profact.TimbraCFDI33.Comprobante GetProfactComprobanteV3()
        {
            byte[] profactoXMLData = Encoding.UTF8.GetBytes(arInvoice.SATXML);
            return LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI33.Comprobante>(profactoXMLData);
        }
    }
}
