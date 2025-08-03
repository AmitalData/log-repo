using Logitude.AmitalMessaging.Customs.CustomFile;
using Logitude.AmitalMessaging.Infrastructure.FuStatus;
using Logitude.Customs.BL.Messaging.Amital;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logitude.Customs.BL.TraceEvents
{

    public class AmitalInsertToQueueService<TransmissionBodyType>
               where TransmissionBodyType : class

    {
         private TransmissionBodyType _TransmissionBodyModel;

        public AmitalInsertToQueueService(TransmissionBodyType eve)
        {
           this._TransmissionBodyModel = eve;
        }

        public void InsertToQueue(AmitalEventTracerModel myAmitalEventTracer, string action)
        {
            var mySetting = Logitude.Customs.BL.EntityQueryServices.CustomsSettingQueryService.GetSettingByTenant(myAmitalEventTracer.Tenant);
 
            if (!mySetting.IsConnectedToUniFreight )
            {

                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(this._TransmissionBodyModel.GetType());
                x.Serialize(Console.Out, this._TransmissionBodyModel);
                var unifreightHybridQueueTaskService = new UnifreightHybridQueueTaskService<AmitalEventTracerModel, TransmissionBodyType>(myAmitalEventTracer, _TransmissionBodyModel);
                unifreightHybridQueueTaskService.Send(new UnifreightHybridQueueTaskParam()
                {
                    Action = action,
                    ParameterName = "transmission"

                }, false);

            }
        }




        public static AmitalEventTracerModel createEvent(DeclarationPM declarationPM, string action)
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

                CommunicationSubject = action,

            };
            return myAmitalEventTracerModel;


        }

    }


    public class AmitalInsertToQueueEzer
    {

        public static Logitude.AmitalMessaging.Infrastructure.FuStatus.LOGICUSTFILE setLogistictFile(DeclarationPM myDeclaration, string tadpisPrintDate = null)
        {
            Logitude.AmitalMessaging.Infrastructure.FuStatus.LOGICUSTFILE LogistictFile = new Logitude.AmitalMessaging.Infrastructure.FuStatus.LOGICUSTFILE();

            bool ifCurrecyEquals = false;
            var invoices = myDeclaration.SupplierInvoices;
            var baseCode = invoices[0]?.InvoiceCurrencyTypeCode;
            if (!string.IsNullOrEmpty(baseCode) && invoices != null && invoices.Count > 0)
            {
                ifCurrecyEquals = invoices.All(s => s != null && string.Equals(s.InvoiceCurrencyTypeCode, baseCode, StringComparison.OrdinalIgnoreCase));
            }

            LogistictFile.logitudeCustomsFile = new LogitudeCustomsFiles()
            {
                customFileNo = myDeclaration?.CustomFileNo,
                id = myDeclaration?.Id,
                declarationNumber = myDeclaration.DeclarationNumber,
                tadpisPrintDate = tadpisPrintDate,
                TotalSum = ifCurrecyEquals ? myDeclaration?.SupplierInvoices.Sum(s => s.InvoiceAmount).ToString() : null,
                currecy = ifCurrecyEquals ? myDeclaration?.SupplierInvoices[0]?.InvoiceCurrencyTypeCode : null,
                totalNisSum = myDeclaration?.SupplierInvoices.Sum(s => s.SupplierInvoiceItems.Sum(si => si.ItemFOBAmountNIS)).ToString(),
                totalFreightSum = myDeclaration?.SupplierInvoices.Sum(s => s.TotalFreightInFreightCurrency).ToString(),
                totalPackages = myDeclaration?.Consignments.Where(s => s.ConsignmentType == "E").Sum(s => s?.ConsignmentPackages.Sum(c => c.PackageQuantity)).ToString(),
                loadingDateTime = myDeclaration?.LoadingDateTime.ToString(),
                direction = myDeclaration?.Direction,
                exportFile = myDeclaration?.ExportFile,
                TransportModeId = myDeclaration?.TransportModeId,

            };
            LogistictFile.logitudeCustomsFile.invoice = new Invoices[myDeclaration.SupplierInvoices.Count];
            for (int i = 0; i < myDeclaration.SupplierInvoices.Count; i++)
            {
                Invoices invoice = new Invoices();
                invoice.invoiceNumber = myDeclaration.SupplierInvoices[i].InvoiceNumber;
                invoice.invoiceTotal = myDeclaration.SupplierInvoices[i].InvoiceAmount.ToString();
                invoice.invoiceCurrecy = myDeclaration.SupplierInvoices[i].InvoiceCurrencyTypeCode;

                string[] pratMeches = new string[myDeclaration.SupplierInvoices[i].SupplierInvoiceItems.Count];


                for (int j = 0; j < myDeclaration.SupplierInvoices[i].SupplierInvoiceItems.Count; j++)
                {

                    pratMeches[j] = myDeclaration.SupplierInvoices[i].SupplierInvoiceItems[j].ClassificationCode;
                }
                invoice.pratList = new PratList();
                invoice.pratList.pratMeches = pratMeches;

                LogistictFile.logitudeCustomsFile.invoice[i] = invoice;
            }

            return LogistictFile;
        }



    }



}