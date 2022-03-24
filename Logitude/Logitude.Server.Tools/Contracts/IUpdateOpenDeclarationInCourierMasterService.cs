using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.Contracts
{
    public interface IUpdateOpenDeclarationInCourierMasterService
    {
        void UpdateOpenDeclarationInCourierMaster(/*DCAInUCUW2LRequestParams requestParams*/int Tenant, string courierMasterID, string LoggingUserId);
    }
}
