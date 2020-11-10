
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.QuoteModel.APIDataContract.ApiV1; 
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using System.Xml.Serialization;
using Simplog.Data.QuoteModel.EntityPOCOs;

namespace Logitude.BL.ShipmentsModel.APIDataContract.ApiV1
{
   
    public partial class Payable
    {

	    
    public string Id { get; set; }
    
    public ChargesType ChargesType { get; set; }
    
    public Measurement Measurement { get; set; }
    
    public double? Quantity { get; set; }
    
    public double? UnitPrice { get; set; }
    
    public Currency Currency { get; set; }
    
    public double? Rate { get; set; }
    
    public PrepaidCollect PrepaidCollect { get; set; }
    
    public double? Amount { get; set; }
    
    public Vendor Vendor { get; set; }

    public  string  ComputingPartnerCode { get; set; }

    }
} 