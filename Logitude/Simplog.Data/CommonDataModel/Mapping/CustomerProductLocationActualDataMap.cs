using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class CustomerProductLocationActualDataMap : EntityTypeConfiguration<CustomerProductLocationActualData>
    {
        public CustomerProductLocationActualDataMap()
        {
            this.HasKey(d => new { d.CustomerId, d.ProductTypeCode, d.Month, d.Year, d.CountryId });

            // Properties
            this.Property(t => t.CustomerId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ProductTypeCode)
               .IsRequired()
               .HasMaxLength(2)
               .IsUnicode(false);

            this.Property(t => t.CountryId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);
                                    
            // Table & Column Mappings
//#if ORACLE_DB
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
                this.ToTable("CustomerProdLocatActualDatas"); ;
            }
            //#else
            else
            {
                this.ToTable("CustomerProductLocationActualDatas");
            }
//#endif
            
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.ProductTypeCode).HasColumnName("ProductTypeCode");
            this.Property(t => t.Month).HasColumnName("Month");
            this.Property(t => t.Year).HasColumnName("Year");
            this.Property(t => t.CountryId).HasColumnName("CountryId");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.TEU).HasColumnName("TEU");
            this.Property(t => t.NumberOfShipments).HasColumnName("NumberOfShipments");
            this.Property(t => t.ChargeableWeight).HasColumnName("ChargeableWeight");
            this.Property(t => t.Revenue).HasColumnName("Revenue");

            // Relationships
            this.HasRequired(t => t.Customer)
                .WithMany()
                .HasForeignKey(d => d.CustomerId);

            this.HasRequired(t => t.ProductType)
                .WithMany()
                .HasForeignKey(d => d.ProductTypeCode);

            this.HasRequired(t => t.Country)
                .WithMany()
                .HasForeignKey(d => d.CountryId);
        }
    }
}
