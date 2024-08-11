using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class ReportGroupMap : EntityTypeConfiguration<ReportGroup>
    {

       public ReportGroupMap()
        {
            this.HasKey(t => t.Id);

            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false);

            this.Property(t => t.EnglishName)
                .IsRequired()
                .HasMaxLength(80)
                .IsUnicode(true);

            this.Property(t => t.LocalName)
                .HasMaxLength(40)
                .IsUnicode(true);

            this.Property(t => t.OrderNumber);


            this.Property(t => t.OrderNumber);
              



            this.ToTable("ReportGroups");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.EnglishName).HasColumnName("EnglishName");
            this.Property(t => t.LocalName).HasColumnName("LocalName");
            this.Property(t => t.OrderNumber).HasColumnName("OrderNumber");




        }
    }
}
