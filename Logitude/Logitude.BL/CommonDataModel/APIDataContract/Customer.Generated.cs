
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
   
    public partial class Customer
    {

	    
    public string Id { get; set; }
    
    public string EnglishName { get; set; }
    
    public string LocalName { get; set; }
    
    public string VatNumber { get; set; }
    
    public PaymentTerm PaymentTerm { get; set; }
    
    public Address MainAddress { get; set; }
    
    public List<Contact> Contacts { get; set; }
    
    public Address BillingAddress { get; set; }
    
    public GLAccount GLAccount { get; set; }
    
    public string Code { get; set; }
    
    public string PartnerCode { get; set; }

    public  string  ComputingPartnerCode { get; set; }

    }
} 