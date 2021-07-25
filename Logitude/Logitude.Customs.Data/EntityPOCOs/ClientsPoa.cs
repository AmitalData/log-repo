using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Logitude.Customs.Data.EntityPOCOs
{
   
    public class ClientsPoa
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
     [Key]
        [ForeignKey("Client")]
        [Column("ClientId")]
	    public string ClientId { get; set; }
	      
        public virtual Client Client { get; set; }
        [Column("PoaID")]
	    public string PoaID { get; set; }
        [Column("AuthorizedExternalId")]
	    public string AuthorizedExternalId { get; set; }
        [Column("AuthorizerExternalId")]
	    public string AuthorizerExternalId { get; set; }
        [Column("AuthorizerPassportNumber")]
	    public string AuthorizerPassportNumber { get; set; }
        [ForeignKey("CustomsCountry")]
        [Column("AuthorizerPassportCountry")]
	    public string AuthorizerPassportCountry { get; set; }
	      
        public virtual CustomsCountry CustomsCountry { get; set; }
        [ForeignKey("PassportType")]
        [Column("AuthorizerPassportType")]
	    public string AuthorizerPassportType { get; set; }
	      
        public virtual PassportType PassportType { get; set; }
        [Column("StartDate")]
	    public DateTime StartDate { get; set; }
        [Column("EndDate")]
	    public DateTime EndDate { get; set; }
        [ForeignKey("PoaStatusTypeLookUp")]
        [Column("PoaStatus")]
	    public string PoaStatus { get; set; }
	      
        public virtual PoaStatusTypeLookUp PoaStatusTypeLookUp { get; set; }
        [ForeignKey("PoaAuthorizationTypeLookup")]
        [Column("PoaAuthorizationType")]
	    public string PoaAuthorizationType { get; set; }
	      
        public virtual PoaAuthorizationTypeLookup PoaAuthorizationTypeLookup { get; set; }
    }
}
	 