using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.QuoteModel.DataContracts
{
    public class QuoteTemplateSettingData
    {
        [DataMember]
        public List<PricesFieldSettings> PricesPackagesTableSettings { get; set; }

        [DataMember]
        public List<PricesFieldSettings> PricesContainersTableSettings { get; set; }

        [DataMember]
        public string PricingPackagesSplitChargeType { get; set; }

        [DataMember]
        public string PricingContinersSplitChargeType { get; set; }

        [DataMember]
        public bool ShowUnitsContainers { get; set; }
    }

    public class PricesFieldSettings
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public int Index { get; set; }
        [DataMember]
        public bool InUse { get; set; }
    }
}
