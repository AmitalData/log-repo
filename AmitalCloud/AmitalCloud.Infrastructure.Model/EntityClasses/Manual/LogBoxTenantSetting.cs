using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class LogBoxTenantSetting
    {
        [Key]

        public int Id { get; set; }


        public bool IsDocumentsArchive { get; set; }

        public bool CustomerTenantShareImportFile { get; set; }

        public string LogBoxAdminUserId { get; set; }

        [ForeignKey("LogBoxAdminUserId")]
        public Contact LogBoxAdminUser { get; set; }


        public bool DocumentShareAsDefault { get; set; }

        public string StockTypeCode { get; set; }
        public bool AutoArchiveOnInvoice { get; set; }
        public bool ShowTaxAmountWarning { get; set; }
        public bool AutoArchiveOnPODExport { get; set; }
		[ForeignKey("Id")]
        public Tenant Tenant { get; set; }
    }
}