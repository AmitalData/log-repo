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
    class AdvancedARInvoiceAPISendInterfaceDataContract : ARInvoiceAPISendInterfaceDataContract, ISendInterfaceDataContract
    {
        public AdvancedARInvoiceAPISendInterfaceDataContract(byte[] objectData) : base(objectData)
        {
        }
        public string GetFileName(SendInterfaceDataContractFileNameArgs sendInterfaceDataContractFileNameArgs)
        {
            return base.GetFileName(sendInterfaceDataContractFileNameArgs);
        }

        public object GetObject(SendInterfaceDataContractObjectArgs sendInterfaceDataContractObjectArgs)
        {
            const string advancedGenericInterfaceCode = "AI";
            return base.GetDataContractObject(sendInterfaceDataContractObjectArgs, advancedGenericInterfaceCode);
        }
    }
}
