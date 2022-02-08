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
   
    public class DeclarationCasualDetails
    {
	 string dbms;

        [Key]
        [ForeignKey("Declaration")]
        [Column("DeclarationId")]
	    public string DeclarationId { get; set; }
	      
        public virtual Declaration Declaration { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("CasualSupplierName")]
	    public string CasualSupplierName { get; set; }
        [Column("CasualSupplierAddress")]
	    public string CasualSupplierAddress { get; set; }
        [Column("CasualImporterAddress1")]
	    public string CasualImporterAddress1 { get; set; }
        [Column("CasualImporterAddress2")]
	    public string CasualImporterAddress2 { get; set; }
        [Column("CasualImporterCity")]
	    public string CasualImporterCity { get; set; }
        [Column("CasualImporterZipCode")]
	    public string CasualImporterZipCode { get; set; }
        [Column("CasualImporterFax")]
	    public string CasualImporterFax { get; set; }
        [Column("CasualImporterEmail")]
	    public string CasualImporterEmail { get; set; }
        [Column("CasualImporterTel")]
	    public string CasualImporterTel { get; set; }
        [Column("CasualImporterContact")]
	    public string CasualImporterContact { get; set; }
    }
}
	 