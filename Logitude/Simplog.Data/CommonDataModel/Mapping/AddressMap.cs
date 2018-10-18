using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class AddressMap : EntityTypeConfiguration<Address>
    {
        public AddressMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(70)
                .IsUnicode(true);

            this.Property(t => t.City)
                .HasMaxLength(25)
                .IsUnicode(true);

            this.Property(t => t.ZipCode)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.FaxNumber)
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.PhoneNumber)
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.ATTN)
                .HasMaxLength(40)
                .IsUnicode(true);

            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.CardId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.AddressTypeId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Address1)
                .HasMaxLength(65)
                .IsUnicode(true);

            this.Property(t => t.Address2)
                .HasMaxLength(65)
                .IsUnicode(true);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(70)
                .IsUnicode(true);


            this.Property(t => t.CountryId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.StateId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.SearchFields)
                .HasMaxLength(1000)
                .IsUnicode(true);

            this.Property(t => t.ExternalId)
                .HasMaxLength(10)
                .IsUnicode(false);


            

            // Table & Column Mappings
            this.ToTable("Addresses");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.City).HasColumnName("City");
            this.Property(t => t.ZipCode).HasColumnName("ZipCode");
            this.Property(t => t.FaxNumber).HasColumnName("FaxNumber");
            this.Property(t => t.PhoneNumber).HasColumnName("PhoneNumber");
            this.Property(t => t.ATTN).HasColumnName("ATTN");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CardId).HasColumnName("CardId");
            this.Property(t => t.AddressTypeId).HasColumnName("AddressTypeId");
            this.Property(t => t.Address1).HasColumnName("Address1");
            this.Property(t => t.Address2).HasColumnName("Address2");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.CountryId).HasColumnName("CountryId");
            this.Property(t => t.StateId).HasColumnName("StateId");
            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t => t.IsLocalLanguage).HasColumnName("IsLocalLanguage");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.ExternalId).HasColumnName("ExternalId");

            // Relationships
            this.HasOptional(t => t.Card)
                .WithMany()
                .HasForeignKey(d => d.CardId);
            this.HasOptional(t => t.Country)
                .WithMany()
                .HasForeignKey(d => d.CountryId);
            this.HasOptional(t => t.State)
                .WithMany()
                .HasForeignKey(d => d.StateId);
            //this.HasRequired(t => t.AddressType)
            //    .WithMany(t => t.Addresses)
            //    .HasForeignKey(d => d.AddressTypeId);

        }
    }
}
