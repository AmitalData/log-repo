using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
   public class CustomerOpenFilesAmountMap : EntityTypeConfiguration<CustomerOpenFilesAmount>
    {
        public CustomerOpenFilesAmountMap()
        {
            this.HasKey(t => t.CustomerId);
            this.Property(t => t.TotalOpenFilesAmount).HasColumnName("TotalOpenFilesAmount").HasPrecision(18, 2);

            // Table & Column Mappings
            this.ToTable("CustomerOpenFilesAmounts");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.TotalOpenFilesAmount).HasColumnName("TotalOpenFilesAmount");


        }


    }
}
