using AmitalCloud.Infrastructure.APITools.DataContracts;
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
