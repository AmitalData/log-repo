using Logitude.AmitalMessaging.Infrastructure.FuStatus;
using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.TraceEvents
{
    public   class AmitalInsertToQueueService
    {
        public static void insertToQueue(DeclarationPM  declarationPM , string tadpisPrintDate)
        {
            var mySetting = Logitude.Customs.BL.EntityQueryServices.CustomsSettingQueryService.GetSettingByTenant(declarationPM.Tenant);

            LogistictFile logistictFile = setLogistictFile(declarationPM, tadpisPrintDate);
            AmitalEventTracerModel myAmitalEventTracer = createEvent(declarationPM);
            if (!mySetting.IsConnectedToUniFreight)
            {

                var unifreightHybridQueueTaskService = new UnifreightHybridQueueTaskService<AmitalEventTracerModel, LogistictFile>(myAmitalEventTracer, logistictFile);
                unifreightHybridQueueTaskService.Send(new UnifreightHybridQueueTaskParam()
                {
                    Action = "UpdateExportCustomsFile",
                    ParameterName = "transmission",
                    UServerDelayTime = DateTime.Now.TimeOfDay
                });
            }
        }


        public static LogistictFile setLogistictFile(DeclarationPM myDeclaration , string tadpisPrintDate=null)
        {
            LogistictFile LogistictFile = new LogistictFile();
            bool ifCurrecyEquals = myDeclaration.SupplierInvoices.TrueForAll(s => s.InvoiceCurrencyTypeCode.Equals(myDeclaration.SupplierInvoices[0].InvoiceCurrencyTypeCode));
            LogistictFile.logitudeCustomsFile = new LogitudeCustomsFiles()
            {
                customFileNo = myDeclaration.CustomFileNo,
                id = myDeclaration.Id,
                declarationNumber = myDeclaration.DeclarationNumber,
                tadpisPrintDate = tadpisPrintDate,
                TotalSum = ifCurrecyEquals ? myDeclaration.SupplierInvoices.Sum(s => s.InvoiceAmount).ToString() : null,
                currecy = ifCurrecyEquals ? myDeclaration.SupplierInvoices[0].InvoiceCurrencyTypeCode : null,
                totalNisSum = myDeclaration.SupplierInvoices.Sum(s => s.SupplierInvoiceItems.Sum(si => si.ItemFOBAmountNIS)).ToString(),
                totalFreightSum = myDeclaration.SupplierInvoices.Sum(s => s.TotalFreightInFreightCurrency).ToString(),
                totalPackages = myDeclaration.SupplierInvoices.Sum(s => s.SupplierInvoiceItems.Sum(si => si.PackageQuantity)).ToString(),
                loadingDateTime = myDeclaration?.LoadingDateTime.ToString(),
                direction = myDeclaration.Direction,

            };
            for (int i = 0; i < myDeclaration.SupplierInvoices.Count; i++)
            {
                Invoices invoice = new Invoices();
                invoice.invoiceNumber = myDeclaration.SupplierInvoices[i].InvoiceNumber;
                invoice.invoiceTotal = myDeclaration.SupplierInvoices[i].InvoiceAmount.ToString();
                invoice.invoiceCurrecy = myDeclaration.SupplierInvoices[i].InvoiceCurrencyTypeCode;
                PratList prat = new PratList();
                for (int j = 0; j < myDeclaration.SupplierInvoices[i].SupplierInvoiceItems.Count; j++)
                {
                    prat.pratMeches[j] = myDeclaration.SupplierInvoices[i].SupplierInvoiceItems[j].ClassificationCode;
                }
                invoice.pratList = prat;

                LogistictFile.logitudeCustomsFile.invoice[i] = invoice;
            }

            return LogistictFile;
        }


        public static AmitalEventTracerModel createEvent(DeclarationPM declarationPM)
        {

            var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
            {
                Tenant = declarationPM.Tenant,
                objectTableName = "Customs.Declaration",
                EventCode = null,
                notes = "",
                CommunicationLoggingEntityReference = declarationPM.DeclarationNumber,
                EntityId = declarationPM.Id,
                UserId = declarationPM.CreatedByUserId,

                CommunicationSubject = "",
                
            };
            return myAmitalEventTracerModel;


        }

    }
}
