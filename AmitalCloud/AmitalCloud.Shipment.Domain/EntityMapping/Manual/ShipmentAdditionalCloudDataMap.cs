using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AmitalCloud.Shipment.Domain.EntityPOCOs;

namespace AmitalCloud.Shipment.Domain.EntityMapping
{
    public class ShipmentAdditionalCloudDataMap : EntityTypeConfiguration<ShipmentAdditionalCloudData>
    {
        public ShipmentAdditionalCloudDataMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);


            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.IsImporterApprovalRequried)
                .IsRequired();

            this.Property(t => t.SendUpdatesToAgentEnabled)
               .IsRequired();

            this.Property(t => t.DocsSentToAgent)
               .IsRequired();

            this.Property(t => t.ApprovedByUserName)
               .HasMaxLength(200)
               .IsUnicode(true);

            this.Property(t => t.DocumentsApprovedByUserName)
               .HasMaxLength(200)
               .IsUnicode(true);

            this.Property(t => t.VersionApproved).HasMaxLength(10)
              .IsUnicode(true);

            this.Property(t => t.DeclarationXmlData).IsMaxLength();
            this.Property(t => t.DenyReason).HasMaxLength(1024);
            this.Property(t => t.ShipmentAddtionalDataXML);

            this.Property(t => t.PaymentRequestXML).IsMaxLength().IsUnicode(true);
            this.Property(t => t.IsPaymentRequired).IsRequired();
            this.Property(t => t.DeclarationWCOXml).IsMaxLength();
            this.Property(t => t.UserIdNumberXMLData).IsMaxLength();
            this.Property(t => t.UserIdNumber).HasMaxLength(35);

            // Table & Column Mappings
            this.ToTable("ShipmentAdditionalCloudDatas");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ApproveDateTime).HasColumnName("ApproveDateTime");
            this.Property(t => t.ApprovedByUserName).HasColumnName("ApprovedByUserName");
            this.Property(t => t.DocumentsApprovedByUserName).HasColumnName("DocumentsApprovedByUserName");
            this.Property(t => t.DeclarationXmlData).HasColumnName("DeclarationXmlData");
            this.Property(t => t.DeclarationWCOXml).HasColumnName("DeclarationWCOXml");
            this.Property(t => t.IsImporterApprovalRequried).HasColumnName("IsImporterApprovalRequried");
            this.Property(t => t.VersionApproved).HasColumnName("VersionApproved");
            this.Property(t => t.DenyReason).HasColumnName("DenyReason");
            this.Property(t => t.ShipmentAddtionalDataXML).HasColumnName("ShipmentAddtionalDataXML");
            this.Property(t => t.SendUpdatesToAgentEnabled).HasColumnName("SendUpdatesToAgentEnabled");
            this.Property(t => t.DocsSentToAgent).HasColumnName("DocsSentToAgent");

            this.Property(t => t.IsPaymentRequired).HasColumnName("IsPaymentRequired");
            this.Property(t => t.PaymentRequestXML).HasColumnName("PaymentRequestXML");
            this.Property(t => t.PaymentDateTime).HasColumnName("PaymentDateTime");
            this.Property(t => t.IsUserIDNumberRequired).HasColumnName("IsUserIDNumberRequired");
            this.Property(t => t.UserIdNumberUpdateDate).HasColumnName("UserIdNumberUpdateDate");
            this.Property(t => t.UserIdNumberXMLData).HasColumnName("UserIdNumberXMLData");
            this.Property(t => t.UserIdNumber).HasColumnName("UserIdNumber");

            this.Property(t => t.DocumentInspection).HasColumnName("DocumentInspection");
            this.Property(t => t.GatepassDocumentsReady).HasColumnName("GatepassDocumentsReady");
            this.Property(t => t.GoodsClassification).HasColumnName("GoodsClassification");

            this.Property(t => t.PaymentRequestDateTime).HasColumnName("PaymentRequestDateTime");
            this.Property(t => t.InvoiceIssuedDate).HasColumnName("InvoiceIssuedDate");
            this.Property(t => t.UserAcceptSaveID).HasColumnName("UserAcceptSaveID");
            this.HasRequired(t => t.Shipment);
              
        }
    }
}
