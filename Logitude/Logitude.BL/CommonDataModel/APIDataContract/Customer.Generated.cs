
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
   
    public class Customer
    {

	    
	[XmlAttribute]
    public string Id { get; set; }
    
    public string EnglishName { get; set; }
    
    public string LocalName { get; set; }
    
    public string VatNumber { get; set; }
    
    public PaymentTerm PaymentTerm { get; set; }
    
    public Address MainAddress { get; set; }
    
	[XmlAttribute]
    public string PartnerCode { get; set; }
    
    public List<Contact> Contacts { get; set; }
    
    //public GLAccount GLAccount { get; set; }
    }
} 