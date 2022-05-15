using Logitude.BL.InvoiceModel.EntityOtherServices;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.Server.Tools;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WebFreight.Web.Helpers.AutomationModel.SendInterface
{
    class ARInvoiceAPISendInterfaceDataContract : SendInterfaceDataContract,ISendInterfaceDataContract
    {
        private ARInvoicePM entityPM;
        public ARInvoiceAPISendInterfaceDataContract(byte[] objectData)
        {
            entityPM = LogitudeXmlSerializer.DeserializeObject<ARInvoicePM>(objectData);
        }
        public string GetFileName(SendInterfaceDataContractFileNameArgs sendInterfaceDataContractFileNameArgs)
        {
            string invoiceNumber = GetPropertyValueFromObject("InvoiceNumber", entityPM);
            string calculatedFieldName = invoiceNumber + "." + sendInterfaceDataContractFileNameArgs.Format;
            
            return calculatedFieldName;
        }

        public object GetObject(SendInterfaceDataContractObjectArgs sendInterfaceDataContractObjectArgs)
        {
            ARInvoiceMessageHelper aRInvoiceMessageHelper = new ARInvoiceMessageHelper(entityPM, sendInterfaceDataContractObjectArgs.Tenant, true);
            object sendInterfaceDataContractObject = aRInvoiceMessageHelper.Transfer();

            return sendInterfaceDataContractObject;
        }
    }
}
