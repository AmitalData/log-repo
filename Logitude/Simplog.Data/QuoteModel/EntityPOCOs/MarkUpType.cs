using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Simplog.Data.QuoteModel.EntityPOCOs
{
    public class MarkUpType
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }

        //public List<QuoteCharge> ContainerType1QuoteCharges { get; set; }
        //public List<QuoteCharge> ContainerType2QuoteCharges { get; set; }
        //public List<QuoteCharge> ContainerType3QuoteCharges { get; set; }
        //public List<QuoteCharge> ContainerType4QuoteCharges { get; set; }
        //public List<QuoteCharge> ContainerType5QuoteCharges { get; set; }

        //public List<QuoteCharge> QuoteCharges { get; set; }
    }
}