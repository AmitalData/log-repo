using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFreight.Web.DataContracts
{
    public class ProductFilter
    {
        //public int? PageIndex {get; set;}
        //public int? PageSize {get; set;}
        //public string[] Sizes {get; set;}
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public ProductFilter()
        {
             
        }
    }
}
