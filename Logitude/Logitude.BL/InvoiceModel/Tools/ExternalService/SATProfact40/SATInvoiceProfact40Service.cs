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
    public class SATInvoiceProfact40Service
    {
        private ARInvoicePM arInvoicePM;
        private ARInvoice arInvoice;
        private SATInterfaceSetting satSetting;
        private ICommonDataContext commonContext;
        private TenantRepository tenantRepository;
        private ChargesTypeRepository chargesTypeRepository;
        private SATCommunicationLogBuilder sATCommunicationLogBuilder;
        
        public SATInvoiceProfact40Service(ARInvoicePM arInvoicePM, ARInvoice arInvoice, SATInterfaceSetting satSetting)
        {
            this.arInvoicePM = arInvoicePM;
            this.arInvoice = arInvoice;
            this.satSetting = satSetting;
            InitalizeContexts(arInvoicePM.Tenant);
            InitalizeRepositories();
            InitalizeServices();
        }

        private void InitalizeContexts(int tenant)
        {
            commonContext = CommonDataContext.GetContext(tenant);
        }

        private void InitalizeRepositories()
        {
            tenantRepository = new TenantRepository(commonContext);
            chargesTypeRepository = new ChargesTypeRepository(commonContext);
        }

        public void InitalizeServices()
        {
            sATCommunicationLogBuilder = new SATCommunicationLogBuilder(arInvoicePM.Tenant);
        }

        public void SendProfactoXML()
        {
            List<ChargesType> allChargesTypes = chargesTypeRepository.GetChargesTypes(arInvoicePM.Tenant).ToList();
            bool IsHasExpenseChargeInvoiceLine = HasExpenseChargeInvoiceLine(allChargesTypes);
            if (IsHasExpenseChargeInvoiceLine)
            {
                SetSATTransferStatus(SATData.NoTransferNeedSATTransferStatusCode);
                return;
            }

            Tenant currentTenant = tenantRepository.GetSingleTenant(arInvoicePM.Tenant);

            SATInvoiceComprobanteValidator sATInvoiceComprobanteValidator = new SATInvoiceComprobanteValidator(arInvoicePM, currentTenant);
            sATInvoiceComprobanteValidator.Validate(allChargesTypes);

            SATInvoiceComprobante sATInvoiceComprobante = new SATInvoiceComprobante(arInvoicePM, currentTenant, satSetting);
            Comprobante comprobante = sATInvoiceComprobante.BuildNewInvoiceComprobante();

            sATCommunicationLogBuilder.Build(new SATCommunicationLogArgs { Comprobante = comprobante, EntityId = arInvoicePM.Id, EntityReference = arInvoicePM.InvoiceNumber.ToString() });
            SetSATTransferStatus(SATData.InTransferingSATTransferStatusCode);
        }

        private bool HasExpenseChargeInvoiceLine(List<ChargesType> allChargesTypes)
        {
            return arInvoicePM.InvoiceLines.All(l => allChargesTypes.First(c => c.Id == l.ChargesTypeId).IsExpense == true);
        }

        private void SetSATTransferStatus(string satTransferStatusCode)
        {
            arInvoice.SATTransferStatusCode = arInvoicePM.SATTransferStatusCode = satTransferStatusCode;
        }
    }
}
