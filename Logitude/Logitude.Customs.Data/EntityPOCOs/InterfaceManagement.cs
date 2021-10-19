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
   
    public class InterfaceManagement
    {
	 string dbms;

        [Key]
        [Column("Code")]
	    public string Code { get; set; }
        [Column("DcaPrefixName")]
	    public string DcaPrefixName { get; set; }
        [Column("Description")]
	    public string Description { get; set; }
        [Column("InOut")]
	    public string InOut { get; set; }
        [ForeignKey("InterfaceSendOption")]
        [Column("DefaultSendOptionsCode")]
	    public string DefaultSendOptionsCode { get; set; }
	      
        public virtual InterfaceSendOption InterfaceSendOption { get; set; }
        [Column("DefaultPriority")]
	    public int? DefaultPriority { get; set; }
        [Column("AllowRestore")]
	    public bool AllowRestore { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [Column("Active")]
	    public bool Active { get; set; }
        [Column("SendAsDual")]
	    public bool SendAsDual { get; set; }
        [Column("ResponseInterfaceCode")]
	    public string ResponseInterfaceCode { get; set; }
        [ForeignKey("SignatureType")]
        [Column("SignatureTypeCode")]
	    public string SignatureTypeCode { get; set; }
	      
        public virtual SignatureType SignatureType { get; set; }
        [Column("DcaPrefixName2")]
	    public string DcaPrefixName2 { get; set; }
        [Column("DcaPrefixName3")]
	    public string DcaPrefixName3 { get; set; }
        [Column("DcaPrefixName4")]
	    public string DcaPrefixName4 { get; set; }
        [Column("InterfaceType")]
	    public string InterfaceType { get; set; }
        [Column("UseRabbitMQ")]
	    public bool UseRabbitMQ { get; set; }
    }
}
	 