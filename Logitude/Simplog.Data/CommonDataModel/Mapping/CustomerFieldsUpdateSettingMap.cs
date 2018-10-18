using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class CustomerFieldsUpdateSettingMap : EntityTypeConfiguration<CustomerFieldsUpdateSetting>
    {
        public CustomerFieldsUpdateSettingMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ObjectFieldId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.UpdateDirection).HasMaxLength(20).IsUnicode(false);
            

            // Table & Column Mappings
            this.ToTable("CustomerFieldsUpdateSettings");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ObjectFieldId).HasColumnName("ObjectFieldId");
            this.Property(t => t.UpdateDirection).HasColumnName("UpdateDirection");
           
            this.HasRequired(t => t.ObjectField).WithMany().HasForeignKey(d => d.ObjectFieldId);
           
        }
    }
}
