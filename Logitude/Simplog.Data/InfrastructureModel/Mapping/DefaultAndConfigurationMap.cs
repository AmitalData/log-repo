using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class DefaultAndConfigurationMap : EntityTypeConfiguration<DefaultAndConfiguration>
    {
        public DefaultAndConfigurationMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired();
               
            this.Property(t => t.SetKey)
               .IsRequired()
               .HasMaxLength(36)
               .IsUnicode(false);

            this.Property(t => t.AdditionalKey)
               .HasMaxLength(36)
               .IsUnicode(false);

            this.Property(t => t.SetValueType1)
                .IsRequired()
               .HasMaxLength(128)
               .IsUnicode(false);

            this.Property(t => t.Value1)
                .IsRequired()
               .HasMaxLength(4000)
               .IsUnicode(false);

            this.Property(t => t.SetValueType2)
                .IsRequired()
               .HasMaxLength(128)
               .IsUnicode(false);

            this.Property(t => t.Value2)
                .IsRequired()
               .HasMaxLength(4000)
               .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("DefaultAndConfigurations");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Id).HasColumnName("Tenant");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.Is_Active).HasColumnName("Is_Active");
            this.Property(t => t.StoreInCache).HasColumnName("StoreInCache");
            this.Property(t => t.SetKey).HasColumnName("SetKey");
            this.Property(t => t.AdditionalKey).HasColumnName("AdditionalKey ");
            this.Property(t => t.SortOrder).HasColumnName("AdditionalKey");
            this.Property(t => t.SetValueType1).HasColumnName("SetValueType1"); 
            this.Property(t => t.Value1).HasColumnName("Value1");
            this.Property(t => t.SetValueType2).HasColumnName("SetValueType2"); 
            this.Property(t => t.Value2).HasColumnName("Value2");
            this.Property(t => t.AllowInheritance).HasColumnName("AllowInheritance");
 
        }
    }
}