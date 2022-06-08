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
    public class SATCancelPaymentProfact40Service
    {
        private ARPaymentPM arPaymentPM;
        private ARPayment arPayment;
        private SATCommunicationLogBuilder sATCommunicationLogBuilder;

        public SATCancelPaymentProfact40Service(ARPaymentPM arPaymentPM, ARPayment arPayment)
        {
            this.arPaymentPM = arPaymentPM;
            this.arPayment = arPayment;
            this.sATCommunicationLogBuilder = new SATCommunicationLogBuilder(arPaymentPM.Tenant);
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
            if (!string.IsNullOrEmpty(arPayment.SATXML))
            {
                return true;
            }

            return false;
        }

        private void SetSATTransferStatus(string sATStatusCode)
        {
            arPayment.SATTransferStatusCode = arPaymentPM.SATTransferStatusCode = sATStatusCode;
        }

        public void BuildProfactCommunicationLog()
        {
            try {
                Comprobante comprobante = GetProfactComprobante();
                sATCommunicationLogBuilder.Build(new SATCommunicationLogArgs { Comprobante = comprobante, ARPaymentPM = arPaymentPM, IsCancellation = true, IsPayment = true });
            }
            catch(Exception ex)
            {
                Profact.TimbraCFDI33.Comprobante comprobanteV3 = GetProfactComprobanteV3();
                sATCommunicationLogBuilder.Build(new SATCommunicationLogArgs { ComprobanteV3 = comprobanteV3, IsVersion3 = true, ARPaymentPM = arPaymentPM, IsCancellation = true, IsPayment = true });
            }
        }

        private Comprobante GetProfactComprobante()
        {
            byte[] profactoXMLData = Encoding.UTF8.GetBytes(arPayment.SATXML);
            return LogitudeXmlSerializer.DeserializeObject<Comprobante>(profactoXMLData);
        }

        private Profact.TimbraCFDI33.Comprobante GetProfactComprobanteV3()
        {
            byte[] profactoXMLData = Encoding.UTF8.GetBytes(arPayment.SATXML);
            return LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI33.Comprobante>(profactoXMLData);
        }
    }
}
