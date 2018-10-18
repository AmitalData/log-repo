using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.StimulReport
{
    public class SupplierInvioceItemVehiclesM
    {
        public string RichbitFileNumber { get; set; }

        public string RichbitFileStatus { get; set; }

        public string VehicleChassisNumber { get; set; }

        public string VehicleId { get; set; }

        public string VehicleTypeCode { get; set; }

        public string VehicleTypeName { get; set; }


        List<SupplierInvioceItemVehicleModsM> acc_supplieritemsVehicleMods;

        public List<SupplierInvioceItemVehicleModsM> Acc_supplieritemsVehicleMods
        {
            get { return acc_supplieritemsVehicleMods; }
            set { acc_supplieritemsVehicleMods = value; }
        }


        List<SupplierInvioceItemVehicleAddsM> acc_supplieritemsVehicleAdds;

        public List<SupplierInvioceItemVehicleAddsM> Acc_supplieritemsVehicleAdds
        {
            get { return acc_supplieritemsVehicleAdds; }
            set { acc_supplieritemsVehicleAdds = value; }
        }

    }
}
