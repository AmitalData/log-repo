
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
   
    public class Journal
    {

	    
	[XmlAttribute]
    public string Id { get; set; }
    
    public int Tenant { get; set; }
    
    public DateTime CreateDate { get; set; }
    
    public DateTime AccountingDate { get; set; }
    
    public string ExternalNo { get; set; }
    
    public DateTime? UpdateDate { get; set; }
    
    public DateTime? ApproveDate { get; set; }
    
    public string AccountingEntityReference { get; set; }
    
    public User UpdatedByUser { get; set; }
    
    public string ExternalSystem { get; set; }
    
    public string OriginalJournalNumber { get; set; }
    
    public User ApprovedByUser { get; set; }
    
    public User CreatedByUser { get; set; }
    
    public string JournalType { get; set; }
    
    public string JournalStatusType { get; set; }
    
    public AccountingEntity AccountingEntity { get; set; }
    
    public string AccountingEntityId { get; set; }
    
    public List<JournalLine> JournalLines { get; set; }
    
    public string JournalNumber { get; set; }
        public bool IsLedgerCreated { get; set; }
    }
} 