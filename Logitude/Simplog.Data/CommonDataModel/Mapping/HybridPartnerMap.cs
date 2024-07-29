using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class HybridPartnerMap: EntityTypeConfiguration<HybridPartner>
    {
        public HybridPartnerMap()
        {
            // property

            this.HasKey(t => t.Id);
            this.Property(t => t.Id)
            .IsRequired()
            .HasMaxLength(15)
            .IsUnicode(false);

            this.Property(t => t.Name)
            .HasMaxLength(50)
            .IsUnicode(true)
            .IsRequired();

            this.Property(t => t.LocalName)
           .HasMaxLength(25)
           .IsUnicode(true);

            this.Property(t => t.LogoId)
           .HasMaxLength(15)
           .IsUnicode(false);
            this.Property(t => t.SmallLogoId)
           .HasMaxLength(15)
           .IsUnicode(false);
           

            this.Property(t => t.SearchFields)
           .HasMaxLength(1000)
           .IsUnicode(true);

            this.Property(t => t.InActive);
            this.Property(t => t.IsMislakaActivated);
            this.Property(t => t.IsExternalPartner);
            this.Property(t => t.ReceiveAllStatuses).IsRequired();
            this.Property(t => t.AllowSendingDocsToAgent).IsRequired();
            // Table & Column Mappings

            this.ToTable("HybridPartners");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PartnerTenant).HasColumnName("PartnerTenant");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.LogoId).HasColumnName("LogoId");
            this.Property(t => t.SmallLogoId).HasColumnName("SmallLogoId");
            this.Property(t => t.LocalName).HasColumnName("LocalName");
  
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t => t.IsMislakaActivated).HasColumnName("IsMislakaActivated");
            this.Property(t => t.IsExternalPartner).HasColumnName("IsExternalPartner");
            this.Property(t => t.ReceiveAllStatuses).HasColumnName("ReceiveAllStatuses");
            this.Property(t => t.AllowSendingDocsToAgent).HasColumnName("AllowSendingDocsToAgent");

            //relationships

            this.HasOptional(t => t.ImageDetail).WithMany().HasForeignKey(d => d.LogoId);
            this.HasOptional(t => t.ImageDetail1).WithMany().HasForeignKey(d => d.SmallLogoId);


        }
    }
}
