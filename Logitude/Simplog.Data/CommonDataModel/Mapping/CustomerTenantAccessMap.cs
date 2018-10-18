using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class CustomerTenantAccessMap : EntityTypeConfiguration<CustomerTenantAccess>
    {
        public CustomerTenantAccessMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id)
            .IsRequired()
            .HasMaxLength(15)
            .IsUnicode(false);

            this.Property(t => t.ContactName)      
           .HasMaxLength(60)
           .IsUnicode(false);

            this.Property(t => t.CompanyVat)
           .HasMaxLength(20)
           .IsUnicode(false);

            this.Property(t => t.CompanyName)
           .HasMaxLength(100)
           .IsUnicode(false);

            this.Property(t => t.CompanyEmail)
           .HasMaxLength(70)
           .IsUnicode(false);

            this.Property(t => t.ContactPhone)         
           .HasMaxLength(25)
           .IsUnicode(false);

            this.Property(t => t.ContactMobile)
           .HasMaxLength(25)
           .IsUnicode(false);

            this.Property(t => t.Status)
           .HasMaxLength(2)
           .IsUnicode(false);

            this.Property(t => t.UpdatedByUserId)         
           .HasMaxLength(15)
           .IsUnicode(false);

            this.Property(t => t.SearchFields)
             .HasMaxLength(1000)
             .IsUnicode(true);


            this.Property(t => t.Tenant).IsRequired();
            this.Property(t => t.CustomerTenant).IsRequired();

            this.Property(t => t.StockTypeCode)
          .HasMaxLength(15)
          .IsUnicode(false);


            // Table & Column Mappings

            this.ToTable("CustomerTenantAccesses");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.CustomerTenant).HasColumnName("CustomerTenant");
            this.Property(t => t.ContactName).HasColumnName("ContactName");
            this.Property(t => t.CompanyVat).HasColumnName("CompanyVat");
            this.Property(t => t.CompanyName).HasColumnName("CompanyName");
            this.Property(t => t.CompanyEmail).HasColumnName("CompanyEmail");
            this.Property(t => t.ContactPhone).HasColumnName("ContactPhone");
            this.Property(t => t.ContactMobile).HasColumnName("ContactMobile");
            this.Property(t => t.RequestDateTime).HasColumnName("RequestDateTime");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");
            this.Property(t => t.LastUpdateDate).HasColumnName("LastUpdateDate");
            this.Property(t => t.LastShipmentDate).HasColumnName("LastShipmentDate");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.IsPrivateLabelCustomer).HasColumnName("IsPrivateLabelCustomer");
            this.Property(t => t.StockTypeCode).HasColumnName("StockTypeCode");


            //relationships

            this.HasOptional(t => t.UpdatedByUser).WithMany().HasForeignKey(d => d.UpdatedByUserId);
            this.HasOptional(t => t.StatusCode).WithMany().HasForeignKey(d => d.Status);






        }
    }
}
