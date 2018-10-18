using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
  public  class CustomerAccountManagerByProductMap : EntityTypeConfiguration<CustomerAccountManagerByProduct>
    {

      public CustomerAccountManagerByProductMap()
        {
            this.HasKey(d => new { d.ProductTypeCode,  d.CustomerId});

            this.Property(d => d.AccountManagerId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(d => d.ProductTypeCode)
                .HasMaxLength(2)
                .IsRequired()
                .IsUnicode(false);

            this.Property(d => d.CustomerId)
              .HasMaxLength(15)
              .IsUnicode(false);

//#if ORACLE_DB
          string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
          if (dbms == "oracle")
          {
              this.ToTable("CustomerAccManagerByProducts");
          }
          //#else
          else
          {
              this.ToTable("CustomerAccountManagerByProducts");
          }
//#endif
            
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.AccountManagerId).HasColumnName("AccountManagerId");
            this.Property(d => d.ProductTypeCode).HasColumnName("ProductTypeCode");

         
        }
    }
    
}
