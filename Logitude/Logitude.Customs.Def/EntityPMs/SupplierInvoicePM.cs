using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Def.EntityPMs
{
    public partial class SupplierInvoicePM
    {
        //[DataMember]
        //public int SupplierInvoiceItemLastLineNumber { get; set; }
        //[DataMember]
        //public int fullItemsCount { get; set; }
        [DataMember]
        public List<Unifreight.BL.EntityPMs.GTBITEMPM> GTBITEMsToUpdate { get; set; }
    }
}
