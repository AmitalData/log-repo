using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class CustomsBookMainView
    {
        [Key]
        public int CustomItemId { get; set; }
        public int Parent_CustomsItemId { get; set; }
        public string CustomsBookTypeID { get; set; }
        public string FullClassification { get; set; }
        public string CustomsItemHierarchicLocationID { get; set; }
        public string GoodsDescription { get; set; }
        public int? Rules { get; set; }
        public string Remarks { get; set; }
        public int Agreements { get; set; }
        public string CustomsRate { get; set; }
        public string PurchaseTax { get; set; }
        public decimal? OptionalTaxAddition { get; set; }
        public string MeasurementUnitName { get; set; }
        public string SearchFields { get; set; }
    }
}
