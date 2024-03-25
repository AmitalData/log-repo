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
 
    public class ClientItemMap : EntityTypeConfiguration<ClientItem>
    {
	    string dbms;
        public ClientItemMap()
        { 
			  this.ToTable("ClientItems", "Customs");
		
		    this.HasKey(t => new { t.ItemCode, t.ClientCode, t.Id });
	 
            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.ItemDescription).HasColumnName("ItemDescription").HasMaxLength(100).IsUnicode(true);

            this.Property(t => t.ClassificationCode).HasColumnName("ClassificationCode").HasMaxLength(50).IsUnicode(false);

            this.Property(t => t.ItemCode).HasColumnName("ItemCode").IsRequired().HasMaxLength(30).IsUnicode(false);

            this.Property(t => t.OriginCountryCode).HasColumnName("OriginCountryCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.ClientCode).HasColumnName("ClientCode").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ItemKey).HasColumnName("ItemKey").HasMaxLength(131).IsUnicode(true);
        }
    }
}
	 