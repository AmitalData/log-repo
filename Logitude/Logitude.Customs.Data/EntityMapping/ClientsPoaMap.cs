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
 
    public class ClientsPoaMap : EntityTypeConfiguration<ClientsPoa>
    {
	    string dbms;
        public ClientsPoaMap()
        { 
			  this.ToTable("ClientsPoas", "Customs");
		
		    this.HasKey(t => new { t.Id, t.ClientId });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.ClientId).HasColumnName("ClientId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.PoaID).HasColumnName("PoaID").IsRequired().HasMaxLength(9).IsUnicode(false);

            this.Property(t => t.AuthorizedExternalId).HasColumnName("AuthorizedExternalId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.AuthorizerExternalId).HasColumnName("AuthorizerExternalId").HasMaxLength(9).IsUnicode(false);

            this.Property(t => t.AuthorizerPassportNumber).HasColumnName("AuthorizerPassportNumber").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.AuthorizerPassportCountry).HasColumnName("AuthorizerPassportCountry").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.AuthorizerPassportType).HasColumnName("AuthorizerPassportType").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.StartDate).HasColumnName("StartDate");

            this.Property(t => t.EndDate).HasColumnName("EndDate");

            this.Property(t => t.PoaStatus).HasColumnName("PoaStatus").HasMaxLength(7).IsUnicode(false);

            this.Property(t => t.PoaAuthorizationType).HasColumnName("PoaAuthorizationType").HasMaxLength(3).IsUnicode(false);
        }
    }
}
	 