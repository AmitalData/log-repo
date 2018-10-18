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
   
    public class ClientDrivingLicenseType
    {
	 string dbms;

        [Key]
        [ForeignKey("ClientDrivingLicense")]
        [Column("ClientId" ,Order = 1)]
	    public string ClientId { get; set; }
	      
        public virtual ClientDrivingLicense ClientDrivingLicense { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
     [Key]
        [ForeignKey("ClientDrivingLicense")]
        [Column("ClientDrivingLicenseLine" ,Order = 2)]
	    public int ClientDrivingLicenseLine { get; set; }
     [Key]
        [Column("DriversLicenseTypeCode" ,Order = 3)]
	    public string DriversLicenseTypeCode { get; set; }
    }
}
	 