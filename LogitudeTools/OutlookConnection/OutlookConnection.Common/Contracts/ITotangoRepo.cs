using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlookConnection.Common.Contracts
{
    public interface ITotangoRepo
    {
        void SendUserActivity(string Email, string orgDisplayName, string module, string activity, int tenant);
    }
}
