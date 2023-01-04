
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

namespace Logitude.BL.CommonDataModel.APIDataContract.ApiV1
{
   
    public partial class LeadSource
    {

	    
	[XmlAttribute]
    public string Id { get; set; }
    
	[XmlAttribute]
    public string Code { get; set; }
    
    public string Name { get; set; }
    
	[XmlAttribute]
    public string PartnerCode { get; set; }

    public  string  ComputingPartnerCode { get; set; }

    }
} 