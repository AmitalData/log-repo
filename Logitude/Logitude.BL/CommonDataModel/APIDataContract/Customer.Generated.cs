
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
    
	[XmlAttribute]
    public string Code { get; set; }
    
    public string PartnerCode { get; set; }
    
    public User AccountManagerUser { get; set; }
    
    public User SalesmanUser { get; set; }
    
    public User Collector { get; set; }
    
    public Team Team { get; set; }
    
    public Industry Industry { get; set; }
    
    public Currency InvoiceCurrency { get; set; }
    
    public VatType VatTypeId { get; set; }
    
    public string ReceivablesExternalId { get; set; }
    
    public List<Address> Addresses { get; set; }
    
    public string LeadDescription { get; set; }
    
    public DateTime? StartWorkingDate { get; set; }
    
    public LeadSource LeadSource { get; set; }

    public  string  ComputingPartnerCode { get; set; }

    }
} 