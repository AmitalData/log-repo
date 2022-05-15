using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFreight.Web.Helpers.AutomationModel.SendInterface
{
    public interface ISendInterfaceDataContract
    {
        string GetFileName(SendInterfaceDataContractFileNameArgs sendInterfaceDataContractFileNameArgs);
        object GetObject(SendInterfaceDataContractObjectArgs SendInterfaceDataContractObjectArgs);
    }
}
