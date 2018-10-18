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
 
    public class ClientMap : EntityTypeConfiguration<Client>
    {
	    string dbms;
        public ClientMap()
        { 
			  this.ToTable("Clients", "Customs");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.Code).HasColumnName("Code").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.FullName).HasColumnName("FullName").HasMaxLength(55).IsUnicode(true);

            this.Property(t => t.ClientTypeSpecificCode).HasColumnName("ClientTypeSpecificCode").HasMaxLength(6).IsUnicode(false);

            this.Property(t => t.IsActive).HasColumnName("IsActive");

            this.Property(t => t.LocalFirstName).HasColumnName("LocalFirstName").HasMaxLength(15).IsUnicode(true);

            this.Property(t => t.LocalLastName).HasColumnName("LocalLastName").HasMaxLength(19).IsUnicode(true);

            this.Property(t => t.LocalCorporationName).HasColumnName("LocalCorporationName").HasMaxLength(55).IsUnicode(true);

            this.Property(t => t.EnglishFirstName).HasColumnName("EnglishFirstName").HasMaxLength(41).IsUnicode(false);

            this.Property(t => t.EnglishLastName).HasColumnName("EnglishLastName").HasMaxLength(30).IsUnicode(false);

            this.Property(t => t.EnglishCorporationName).HasColumnName("EnglishCorporationName").HasMaxLength(55).IsUnicode(false);

            this.Property(t => t.BirthDate).HasColumnName("BirthDate");

            this.Property(t => t.GenderCode).HasColumnName("GenderCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.DunsNumber).HasColumnName("DunsNumber").HasMaxLength(9).IsUnicode(false);

            this.Property(t => t.PassportNumber).HasColumnName("PassportNumber").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.PassportCountryCode).HasColumnName("PassportCountryCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.PassportTypeCode).HasColumnName("PassportTypeCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.PassportFirstName).HasColumnName("PassportFirstName").HasMaxLength(41).IsUnicode(false);

            this.Property(t => t.PassportLastName).HasColumnName("PassportLastName").HasMaxLength(30).IsUnicode(false);

            this.Property(t => t.EnglishBirthPlace).HasColumnName("EnglishBirthPlace").HasMaxLength(35).IsUnicode(false);

            this.Property(t => t.EnglishFatherName).HasColumnName("EnglishFatherName").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.PassportExpirationDate).HasColumnName("PassportExpirationDate");

            this.Property(t => t.PassportIssueDate).HasColumnName("PassportIssueDate");

            this.Property(t => t.IsImporter).HasColumnName("IsImporter");

            this.Property(t => t.IsExporter).HasColumnName("IsExporter");

            this.Property(t => t.ConcurrencyGUID).HasColumnName("ConcurrencyGUID").HasMaxLength(40).IsUnicode(false);

            this.Property(t => t.FacilitationTypeCode).HasColumnName("FacilitationTypeCode").HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 