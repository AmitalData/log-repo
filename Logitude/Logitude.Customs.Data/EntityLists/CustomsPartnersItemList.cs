using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Unifreight.Data.AmitalModel.EntityPOCOs;

namespace Logitude.Customs.Data.EntityLists
{
    public partial class CustomsPartnersItemList
    {
        [DataMember]
        public List<GITITEMCR> GITITEMCRs { get; set; }

        [DataMember]
        public string TariffID;
    }
}
