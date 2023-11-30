using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class HTSCodeMap : EntityTypeConfiguration<HTSCode>
    {
        public HTSCodeMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ItemId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.DestinationCountryId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Code).IsRequired().HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.OtherDuties).HasMaxLength(200).IsUnicode(false);            
            this.Property(t => t.Remarks).HasMaxLength(1000).IsUnicode(true);

            // Table & Column Mappings
            this.ToTable("HTSCodes");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ItemId).HasColumnName("ItemId");
            this.Property(t => t.DestinationCountryId).HasColumnName("DestinationCountryId");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.ApprovedByCustomer).HasColumnName("ApprovedByCustomer");
            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t => t.LineNumber).HasColumnName("LineNumber");
            this.Property(t => t.VATPercentage).HasColumnName("VATPercentage");
            this.Property(t => t.DutiesPercentage).HasColumnName("DutiesPercentage");
            this.Property(t => t.OtherDuties).HasColumnName("OtherDuties");
            this.Property(t => t.Remarks).HasColumnName("Remarks");

            this.HasRequired(t => t.Country).WithMany().HasForeignKey(d => d.DestinationCountryId);
            this.HasRequired(t => t.Item).WithMany().HasForeignKey(d => d.ItemId);
        }
    }
}
