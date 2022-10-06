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
 
    public class SignStationMap : EntityTypeConfiguration<SignStation>
    {
	    string dbms;
        public SignStationMap()
        { 
			  this.ToTable("SignStations", "Customs");
		
		    this.HasKey(t => new { t.CustomsAgentId, t.PersonId });
	 
            this.Property(t => t.CustomsAgentId).HasColumnName("CustomsAgentId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.PersonId).HasColumnName("PersonId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SignCertificate).HasColumnName("SignCertificate").IsRequired().HasMaxLength(512).IsUnicode(false);

            this.Property(t => t.UserName).HasColumnName("UserName").HasMaxLength(32).IsUnicode(false);

            this.Property(t => t.MachineName).HasColumnName("MachineName").HasMaxLength(32).IsUnicode(false);

            this.Property(t => t.IsPersonalSignOn).HasColumnName("IsPersonalSignOn");

            this.Property(t => t.IsCompanySignOn).HasColumnName("IsCompanySignOn");

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.Status).HasColumnName("Status").HasMaxLength(32).IsUnicode(false);

            this.Property(t => t.VersionByFeatures).HasColumnName("VersionByFeatures").HasMaxLength(32).IsUnicode(false);

            this.Property(t => t.LastAccessedAt).HasColumnName("LastAccessedAt");
        }
    }
}
	 