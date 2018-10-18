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
using Logitude.Social.Data.EntityPOCOs;
using Logitude.Social.Data;
 
namespace Logitude.Social.Data.EntityMapping
{
 
    public class GroupMap : EntityTypeConfiguration<Group>
    {
	    string dbms;
        public GroupMap()
        { 
				this.ToTable("Groups");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.Description).HasColumnName("Description").HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.IsPrivate).HasColumnName("IsPrivate");

            this.Property(t => t.OwnerId).HasColumnName("OwnerId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();
        }
    }
}
	 