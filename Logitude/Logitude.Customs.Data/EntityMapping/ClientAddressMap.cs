using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System.Data.Entity.ModelConfiguration;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data;
 
namespace Logitude.Customs.Data.EntityMapping
{
 
    public class ClientAddressMap : EntityTypeConfiguration<ClientAddress>
    {
	    string dbms;
        public ClientAddressMap()
        { 
			  this.ToTable("ClientAddresses", "Customs");
		
		    this.HasKey(t => new { t.ClientId, t.AddressId });
	 
            this.Property(t => t.ClientId).HasColumnName("ClientId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.AddressId).HasColumnName("AddressId").HasMaxLength(9).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.ContactStateCode).HasColumnName("ContactStateCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.AddressTypeCode).HasColumnName("AddressTypeCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.AddressPurposeCode).HasColumnName("AddressPurposeCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.IsPalestinianCity).HasColumnName("IsPalestinianCity").IsRequired();

            this.Property(t => t.IsHebrewAddress).HasColumnName("IsHebrewAddress").IsRequired();

            this.Property(t => t.BranchName).HasColumnName("BranchName").HasMaxLength(25).IsUnicode(true);

            this.Property(t => t.ContactIdentifier).HasColumnName("ContactIdentifier").HasMaxLength(9).IsUnicode(false);

            this.Property(t => t.ContactFirstName).HasColumnName("ContactFirstName").HasMaxLength(15).IsUnicode(true);

            this.Property(t => t.ContactLastName).HasColumnName("ContactLastName").HasMaxLength(19).IsUnicode(true);

            this.Property(t => t.ContactRoleTypeCode).HasColumnName("ContactRoleTypeCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.AuthorizedSignerPermit1).HasColumnName("AuthorizedSignerPermit1").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.AuthorizedSignerPermit2).HasColumnName("AuthorizedSignerPermit2").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.AuthorizedSignerPermit3).HasColumnName("AuthorizedSignerPermit3").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.LocalCityCode).HasColumnName("LocalCityCode").HasMaxLength(5).IsUnicode(false);

            this.Property(t => t.LocalSecondLine).HasColumnName("LocalSecondLine").HasMaxLength(25).IsUnicode(true);

            this.Property(t => t.LocalStreetName).HasColumnName("LocalStreetName").HasMaxLength(20).IsUnicode(true);

            this.Property(t => t.LocalHouseLetter).HasColumnName("LocalHouseLetter").HasMaxLength(1).IsUnicode(true);

            this.Property(t => t.LocalEntrance).HasColumnName("LocalEntrance").HasMaxLength(2).IsUnicode(true);

            this.Property(t => t.EnglishCountryCode).HasColumnName("EnglishCountryCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.EnglishSubCountryCode).HasColumnName("EnglishSubCountryCode").HasMaxLength(6).IsUnicode(false);

            this.Property(t => t.EnglishCityName).HasColumnName("EnglishCityName").HasMaxLength(35).IsUnicode(false);

            this.Property(t => t.EnglishMainAddressLine).HasColumnName("EnglishMainAddressLine").HasMaxLength(70).IsUnicode(false);

            this.Property(t => t.EnglishPostalCode).HasColumnName("EnglishPostalCode").HasMaxLength(9).IsUnicode(false);

            this.Property(t => t.LocalApartment).HasColumnName("LocalApartment").HasPrecision(4, 0);

            this.Property(t => t.LocalPOBox).HasColumnName("LocalPOBox").HasMaxLength(9).IsUnicode(false);

            this.Property(t => t.LocalPostalCode).HasColumnName("LocalPostalCode").HasMaxLength(7).IsUnicode(false);

            this.Property(t => t.LocalHouseNumber).HasColumnName("LocalHouseNumber").HasMaxLength(50).IsUnicode(false);

            this.Property(t => t.CustomAddressCode).HasColumnName("CustomAddressCode").HasMaxLength(9).IsUnicode(false);
        }
    }
}
	 