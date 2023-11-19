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
   
    public class CustomsEnvironmentSetting
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
     [Key]
        [Column("EnvironmentCode")]
	    public string EnvironmentCode { get; set; }
        [Column("UseRabbitMQ")]
	    public bool UseRabbitMQ { get; set; }
        [Column("RabbitHost")]
	    public string RabbitHost { get; set; }
        [Column("RabbitUserName")]
	    public string RabbitUserName { get; set; }
        [Column("RabbitPassword")]
	    public string RabbitPassword { get; set; }
        [Column("HSMSignProcess")]
	    public string HSMSignProcess { get; set; }
        [Column("HSMToken")]
	    public string HSMToken { get; set; }
        [Column("HSMActiveCertUrl")]
	    public string HSMActiveCertUrl { get; set; }
        [Column("HSMSignServiceUrl")]
	    public string HSMSignServiceUrl { get; set; }
        [Column("OcrToken")]
	    public string OcrToken { get; set; }
        [Column("UpdateDocOcrServiceUrl")]
	    public string UpdateDocOcrServiceUrl { get; set; }
    }
}
	 