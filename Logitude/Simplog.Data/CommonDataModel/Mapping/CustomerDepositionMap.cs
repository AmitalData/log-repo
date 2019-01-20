using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
   
    public class CustomerDepositionMap : EntityTypeConfiguration<CustomerDeposition>
    {
        public CustomerDepositionMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.CustomsShipperId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.DepositionNumber).IsRequired().HasMaxLength(20).IsUnicode(false);


            // Table & Column Mappings
            this.ToTable("CustomerDepositions");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.CustomsShipperId).HasColumnName("CustomsShipperId");
            this.Property(t => t.DepositionNumber).HasColumnName("DepositionNumber");
            this.Property(t => t.ValidityStartDate).HasColumnName("ValidityStartDate");
            this.Property(t => t.ValidityEndDate).HasColumnName("ValidityEndDate");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            
            this.HasRequired(t => t.CustomsShipper).WithMany().HasForeignKey(d => d.CustomsShipperId);


        }
    }

}
