using AmitalCloud.Infrastructure.APITools.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmitalCloud.Infrastructure.Domain.DataContracts;

namespace AmitalCloud.Infrastructure.APITools.Interfaces
{
    public interface IUnifreightGenericService
    {
        void ProccessGenericRequest(
                    string DataIn,

                    ref string MoreParams,
                    out string MessageOut);
        GenericResponseObj MyGenericResponseObj { get; }
        CommunicationsParams MyCommunicationsParams { get; }
    }
}
