using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class EntityCasualDataMap : EntityTypeConfiguration<EntityCasualData>
    {
        public EntityCasualDataMap()
        { 
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.CasualTypeCode).HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.ObjectTableId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.LocalName).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.EnglishName).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.City).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.ZipCode).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.FaxNumber).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.PhoneNumber).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.Address1).HasMaxLength(150).IsUnicode(false);
            this.Property(t => t.Address2).HasMaxLength(150).IsUnicode(false);
            this.Property(t => t.SearchFields).IsUnicode(false);
            this.Property(t => t.EntityId).HasMaxLength(15).IsUnicode(false);

            this.ToTable("EntityCasualData");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CasualTypeCode).HasColumnName("CasualTypeCode");
            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId");
            this.Property(t => t.LocalName).HasColumnName("LocalName");
            this.Property(t => t.EnglishName).HasColumnName("EnglishName");
            this.Property(t => t.City).HasColumnName("City");
            this.Property(t => t.ZipCode).HasColumnName("ZipCode");
            this.Property(t => t.FaxNumber).HasColumnName("FaxNumber");
            this.Property(t => t.PhoneNumber).HasColumnName("PhoneNumber");
            this.Property(t => t.Address1).HasColumnName("Address1");
            this.Property(t => t.Address2).HasColumnName("Address2");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.EntityId).HasColumnName("EntityId");
             
        }
    }
}
