using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class CustomerTenantAccessStatusTypeMap : EntityTypeConfiguration<CustomerTenantAccessStatusType>
    {
        public CustomerTenantAccessStatusTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.Code);

            // Properties
            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(2)
                .IsUnicode(false);

            this.Property(t => t.EnglishName)          
                .HasMaxLength(50)
                .IsUnicode(true);

            this.Property(t => t.LocalName)          
              .HasMaxLength(25)
              .IsUnicode(true);

            this.Property(t => t.SearchFields)
              .HasMaxLength(52)
              .IsUnicode(true);

            // Table & Column Mappings
//#if ORACLE_DB
              string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
              if (dbms == "oracle")
              {
                  this.ToTable("CustomerTenantAccesStatusTypes");
              }
              //#else
              else
              {
                  this.ToTable("CustomerTenantAccessStatusTypes");
              }
//#endif
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.EnglishName).HasColumnName("EnglishName");
            this.Property(t => t.LocalName).HasColumnName("LocalName");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
        }
    }
}

