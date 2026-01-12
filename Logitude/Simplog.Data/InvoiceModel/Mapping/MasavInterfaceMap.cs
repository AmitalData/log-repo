using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Simplog.Data.InvoiceModel.Mapping
{
    public class MasavInterfaceMap : EntityTypeConfiguration<MasavInterface>
    {

        public MasavInterfaceMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);           

            this.Property(t => t.CreatedByUserId)
                .HasMaxLength(15)
                .IsUnicode(false);
            this.Property(t => t.UpdatedByUserId)
                .HasMaxLength(15)
                .IsUnicode(false);
            this.Property(t => t.SearchFields)
                .HasMaxLength(1000)
                .IsUnicode(true);
            this.Property(t => t.StatusCode)
                .HasMaxLength(3)
                .IsUnicode(false);



            // Table & Column Mappings
            this.ToTable("MasavInterfaces");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.FromDate).HasColumnName("FromDate");
            this.Property(t => t.ToDate).HasColumnName("ToDate");
            this.Property(t => t.PaymentDate).HasColumnName("PaymentDate");
            this.Property(t => t.StatusCode).HasColumnName("StatusCode");

            this.HasRequired(t => t.Status).WithMany().HasForeignKey(d => d.StatusCode);

        }

    }
}
