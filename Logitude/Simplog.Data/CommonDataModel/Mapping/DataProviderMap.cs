using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
  public  class DataProviderMap : EntityTypeConfiguration<DataProvider>
    {

      public DataProviderMap()
        {
            this.HasKey(t => t.Code);

          
            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false);

          this.Property(t => t.Name)
              .IsRequired()
              .HasMaxLength(100)
              .IsUnicode(false);

   
          
            // Table & Column Mappings
            this.ToTable("DataProviders");
            
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");


        }
    }
}
