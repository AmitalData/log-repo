namespace AmitalCloud.Infrastructure.Domain.EntityPMs
{
    public partial class DocumentsFilingPM
    {

        public string InvoiceBillTo { get; set; }
        public bool FromCTool { get; set; }
        public bool IsApprovalRequired { get; set; }
        public bool IsFromDigital { get; set; }
        public bool IsUoloadedField { get; set; }

    }
}
