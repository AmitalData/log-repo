
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

namespace Logitude.BL.CommonDataModel.APIDataContract.ApiV1
{
   
    public class PaymentTerm
    {

	    
	[XmlAttribute]
    public string Id { get; set; }
    
    public string EnglishName { get; set; }
    
    public string LocalName { get; set; }
    
    public int Days { get; set; }

    public  string  ComputingPartnerCode { get; set; }

    }
} 