using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class TenantAdditionalDataMap : EntityTypeConfiguration<TenantAdditionalData>
    {
        public TenantAdditionalDataMap()
        {
            this.HasKey(t => t.Id);
            //this.Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.DropBoxAccessToken).HasMaxLength(350).IsUnicode(false);
            this.Property(t => t.DropBoxState).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.DropBoxUID).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.DropBoxUEmail).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.PaymentGatewayPartnerCode).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.PaymentGatewayConnectionString).HasMaxLength(400).IsUnicode(false);

            this.ToTable("TenantAdditionalDatas");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.DropBoxAccessToken).HasColumnName("DropBoxAccessToken");
            this.Property(t => t.DropBoxState).HasColumnName("DropBoxState");
            this.Property(t => t.DropBoxUID).HasColumnName("DropBoxUID");
            this.Property(t => t.DropBoxUEmail).HasColumnName("DropBoxUEmail");
            this.Property(t => t.PaymentGatewayPartnerCode).HasColumnName("PaymentGatewayPartnerCode");
            this.Property(t => t.PaymentGatewayConnectionString).HasColumnName("PaymentGatewayConnectionString");

            this.HasOptional(t => t.PaymentGatewayPartner).WithMany().HasForeignKey(d => d.PaymentGatewayPartnerCode);

        }
    }
}
