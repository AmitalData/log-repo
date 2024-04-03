using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace Simplog.Global.Data.GlobalModel.Mapping
{
    public class TenantManagmentPrivateLabelsMap : EntityTypeConfiguration<TenantManagmentPrivateLabels>
    {
        public TenantManagmentPrivateLabelsMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.PrivateLabelName).IsRequired().HasMaxLength(60).IsUnicode(true);
            this.Property(t => t.PrivateLabelShortName).IsRequired().HasMaxLength(60).IsUnicode(false);
            this.Property(t => t.PrivateLabelUrl).IsRequired().HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.PrivateLabelDomain).HasMaxLength(100).IsUnicode(false);
            //this.Property(t => t.MainLogo).IsRequired();
            this.Property(t => t.ContactUsEmail).IsRequired().HasMaxLength(70).IsUnicode(true);
            this.Property(t => t.ReceiveAllStatuses).IsRequired();
            this.Property(t => t.HybridPartnerId).IsRequired().HasMaxLength(15).IsUnicode(true);
            this.Property(t => t.InActive).IsRequired();
            this.Property(t => t.SearchFields).HasMaxLength(500);
            this.Property(t => t.MainColor).HasMaxLength(100);
            this.Property(t => t.BackgroundImageId).HasMaxLength(15);
            this.Property(t => t.LoginImageId).HasMaxLength(15);
            this.Property(t => t.LoginProgressImageId).HasMaxLength(15);
            this.Property(t => t.ForgetPasswordImageId).HasMaxLength(15);
            this.Property(t => t.SecondaryColor).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.DocumentTypeHighlightColor).HasMaxLength(100);
            this.Property(t => t.MainTabHighlightColor).HasMaxLength(100);
            this.Property(t => t.QueryFiltersHighlightColor).HasMaxLength(100);
            this.Property(t => t.FilingInboxDomain).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.DistributorCode).HasMaxLength(15).IsUnicode(true);

            this.ToTable("TenantManagmentPrivateLabels");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PrivateLabelName).HasColumnName("PrivateLabelName");
            this.Property(t => t.PrivateLabelShortName).HasColumnName("PrivateLabelShortName");
            this.Property(t => t.PrivateLabelUrl).HasColumnName("PrivateLabelUrl");
            this.Property(t => t.PrivateLabelDomain).HasColumnName("PrivateLabelDomain");
            this.Property(t => t.MainLogo).HasColumnName("MainLogo");
            this.Property(t => t.ContactUsEmail).HasColumnName("ContactUsEmail");
            this.Property(t => t.ReceiveAllStatuses).HasColumnName("ReceiveAllStatuses");
            this.Property(t => t.HybridPartnerId).HasColumnName("HybridPartnerId");
            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.SmallLogo).HasColumnName("SmallLogo");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.BackgroundImageId).HasColumnName("BackgroundImageId");
            this.Property(t => t.LoginImageId).HasColumnName("LoginImageId");
            this.Property(t => t.MainColor).HasColumnName("MainColor");
            this.Property(t => t.LoginProgressImageId).HasColumnName("LoginProgressImageId");
            this.Property(t => t.ForgetPasswordImageId).HasColumnName("ForgetPasswordImageId");
            this.Property(t => t.SecondaryColor).HasColumnName("SecondaryColor");
            this.Property(t => t.HasLogboxAccess).HasColumnName("HasLogboxAccess");
            this.Property(t => t.MainTabHighlightColor).HasColumnName("MainTabHighlightColor");
            this.Property(t => t.DocumentTypeHighlightColor).HasColumnName("DocumentTypeHighlightColor");
            this.Property(t => t.IsCustomsActivated).HasColumnName("IsCustomsActivated");
            this.Property(t => t.IsExportActivated).HasColumnName("IsExportActivated");
            this.Property(t => t.QueryFiltersHighlightColor).HasColumnName("QueryFiltersHighlightColor");
            this.Property(t => t.CreateShipmentsWithoutDocs).HasColumnName("CreateShipmentsWithoutDocs");
            this.Property(t => t.CreateOShipmentsWithoutDocs).HasColumnName("CreateOShipmentsWithoutDocs");
            this.Property(t => t.FilingInboxDomain).HasColumnName("FilingInboxDomain");
            this.Property(t => t.DistributorCode).HasColumnName("DistributorCode");
            //this.HasRequired(t => t.GlobalTenant).WithOptional(t => t.TenantManagement);
            //this.HasOptional(t => t.MainLogoId).WithMany().HasForeignKey(d => d.LogoId);
        }
    }
}
