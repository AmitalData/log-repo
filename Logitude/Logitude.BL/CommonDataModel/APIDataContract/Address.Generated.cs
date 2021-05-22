
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
   
    public partial class Address
    {

	    
	[XmlAttribute]
    public string Id { get; set; }
    
    public string Name { get; set; }
    
    public string Address1 { get; set; }
    
    public string Address2 { get; set; }
    
    public Country Country { get; set; }
    
    public string City { get; set; }
    
    public string ZipCode { get; set; }
    
    public string PhoneNumber { get; set; }
    
    public string FaxNumber { get; set; }
    
    public State State { get; set; }
    
	[XmlAttribute]
    public string ExternalId { get; set; }
    
    public AddressType AddressTypeId { get; set; }

    public  string  ComputingPartnerCode { get; set; }

    }
} 