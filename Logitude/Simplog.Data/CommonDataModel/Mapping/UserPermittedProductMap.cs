using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class UserPermittedProductMap: EntityTypeConfiguration<UserPermittedProduct>
    {
        public UserPermittedProductMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.UserId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ProductTypeCode)
                .IsRequired()
                .HasMaxLength(2)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("UserPermittedProducts");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.ProductTypeCode).HasColumnName("ProductTypeCode");            
             
            // Relationships
            this.HasRequired(t => t.User)
                .WithMany()
                .HasForeignKey(d => d.UserId);

            this.HasRequired(t => t.ProductType)
                .WithMany()
                .HasForeignKey(d => d.ProductTypeCode);
        }
    }
}
