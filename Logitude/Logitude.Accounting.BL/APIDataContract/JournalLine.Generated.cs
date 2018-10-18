
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

namespace Logitude.Accounting.BL.APIDataContract.ApiV1
{
   
    public class JournalLine
    {

	    
    public string JournalId { get; set; }
    
    public int Line { get; set; }
    
    public int Tenant { get; set; }
    
    public GLAccount DebitControlAccount { get; set; }
    
    public GLAccount DebitAccount { get; set; }
    
    public GLAccount CreditControlAccount { get; set; }
    
    public GLAccount CreditAccount { get; set; }
    
    public DateTime DocumentDate { get; set; }
    
    public DateTime AccountingDate { get; set; }
    
    public DateTime DueDate { get; set; }
    
    public decimal LocalAmount { get; set; }
    
    public Currency Currency { get; set; }
    
    public decimal ForeignAmount { get; set; }
    
    public decimal? ExchangeRate { get; set; }
    
    public string Reference1 { get; set; }
    
    public string Reference2 { get; set; }
    
    public string Reference3 { get; set; }
    
    public string CreditAccountNumber { get; set; }
    
    public string DebitAccountNumber { get; set; }
    
    public string Notes { get; set; }
    
    public decimal? ExternalOpenAmount { get; set; }
    
    public bool? IsCreditAccountMulti { get; set; }
    
    public bool? IsDebitAccountMulti { get; set; }
    }
} 