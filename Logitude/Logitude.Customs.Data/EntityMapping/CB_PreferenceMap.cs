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
 
    public class CB_PreferenceMap : EntityTypeConfiguration<CB_Preference>
    {
	    string dbms;
        public CB_PreferenceMap()
        { 
			  this.ToTable("CB_Preferences", "Customs");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.BackgroundColor).HasColumnName("BackgroundColor").HasMaxLength(128).IsUnicode(true);

            this.Property(t => t.TextColor).HasColumnName("TextColor").HasMaxLength(128).IsUnicode(true);

            this.Property(t => t.UserId).HasColumnName("UserId").HasMaxLength(128).IsUnicode(true);
        }
    }
}
	 