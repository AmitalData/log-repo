using Simplog.Data.QuoteModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.QuoteModel.Mapping
{
    public class BorderTypeMap : EntityTypeConfiguration<BorderType>
    {
       public BorderTypeMap()
       {

           this.HasKey(t => t.Code);
           // Properties
           this.Property(t => t.Code)
               .IsRequired()
               .HasMaxLength(40)
               .IsUnicode(false);

           this.Property(t => t.Name)
               .IsRequired()
               .HasMaxLength(120)
               .IsUnicode(true);



           // Table & Column Mappings
           this.ToTable("BorderTypes");
           this.Property(t => t.Code).HasColumnName("Code");
           this.Property(t => t.Name).HasColumnName("Name");

       }


    }
}
