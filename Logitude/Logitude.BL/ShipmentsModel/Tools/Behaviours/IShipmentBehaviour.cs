using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.Behaviours
{
    interface IShipmentBehaviour
    {
        bool ReceivablePricingUpdated { get; set; }
        void Handle();

        void Save();

    }
}
