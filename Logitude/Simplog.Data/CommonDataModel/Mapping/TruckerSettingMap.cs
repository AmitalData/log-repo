using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class TruckerSettingMap : EntityTypeConfiguration<TruckerSetting>
    {
        public TruckerSettingMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Tenant).IsRequired();
            this.Property(t => t.SearchFields).HasMaxLength(1000).IsUnicode(false);
            this.Property(t => t.AddressId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.FromAddressCityId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ToAddressCityId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ShipmentType).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.TruckerId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Responsibility).HasMaxLength(15).IsUnicode(false);




            // Table & Column Mappings
            this.ToTable("TruckerSettings");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.AddressId).HasColumnName("AddressId");
            this.Property(t => t.FromAddressCityId).HasColumnName("FromAddressCityId");
            this.Property(t => t.ToAddressCityId).HasColumnName("ToAddressCityId");
            this.Property(t => t.ShipmentType).HasColumnName("ShipmentType");
            this.Property(t => t.TruckerId).HasColumnName("TruckerId");
            this.Property(t => t.Responsibility).HasColumnName("Responsibility");

            this.HasRequired(t => t.Address);







        }
    }
}
