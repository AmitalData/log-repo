using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.XSD.Analyzers.INTTRAAnalyzer
{
    public partial class INTTRAAnalyzer
    {
        private void Analyze_CONTRL()
        {
            if (this.iMessage != null)
            {
                this.shipmentPM.IsUpdatedByINTTRAAnalyzer = true;
                string systemEmail = "system@tenant" + this.Tenant + ".com";

                if (this.isBookingControl)
                {
                    if (this.IsAccepted)
                    {
                        shipmentPM.INTTRABookingStatusCode = "CD";
                    }

                    else
                    {
                        shipmentPM.INTTRABookingStatusCode = "RU";
                    }
                }

                else
                {
                    if (this.IsAccepted)
                    {
                        shipmentPM.INTTRASIStatusCode = "ACIN";
                    }

                    else
                    {
                        shipmentPM.INTTRASIStatusCode = "RJIN";
                    }
                }

                shipmentPM.INTTRASIStatusDate = TenantServerConfigration.GetCurrentDateTime(this.Tenant);
                ShipmentService service = new ShipmentService(myShipmentContext, shipmentPM, systemEmail);
                service.Update();
            }
        }
    }
}
