using Logitude.BL.InvoiceModel.EntityOtherServices;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.Server.Tools;
using Simplog.Data.Helpers;
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
            string calculatedFieldName = "arinvoice_"+ invoiceNumber;
            
            return calculatedFieldName;
        }

        public object GetObject(SendInterfaceDataContractObjectArgs sendInterfaceDataContractObjectArgs)
        {
            const string genericInterfaceCode = "GI";
            return GetDataContractObject(sendInterfaceDataContractObjectArgs, genericInterfaceCode);
        }

        public object GetDataContractObject(SendInterfaceDataContractObjectArgs sendInterfaceDataContractObjectArgs, string selectedInterfaceCode)
        {
            ARInvoiceMessageHelper aRInvoiceMessageHelper = new ARInvoiceMessageHelper(entityPM, sendInterfaceDataContractObjectArgs.Tenant, selectedInterfaceCode);
            object sendInterfaceDataContractObject = aRInvoiceMessageHelper.Transfer();

            return sendInterfaceDataContractObject;
        }
    }
}
