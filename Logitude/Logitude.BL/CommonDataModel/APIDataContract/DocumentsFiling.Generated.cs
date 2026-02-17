
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
   
    public class DocumentsFiling
    {

	    
    public string Id { get; set; }
    
	[XmlAttribute]
    public string Code { get; set; }
    
    public User CreatedByUser { get; set; }
    
    public string EntityNumber { get; set; }
    
    public ObjectTable EntityType { get; set; }
    
    public DocumentType DocumentType { get; set; }
    
    public string BlobId { get; set; }
    
    public bool IsDigitallySigned { get; set; }
    
    public string SignersList { get; set; }
    
    public string BlobName { get; set; }
    
    public string Description { get; set; }
    
    public bool IsSharedWithCustomer { get; set; }

    public  string  ComputingPartnerCode { get; set; }

    }
} 