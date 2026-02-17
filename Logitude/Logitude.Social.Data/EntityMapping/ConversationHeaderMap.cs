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
 
    public class ConversationHeaderMap : EntityTypeConfiguration<ConversationHeader>
    {
	    string dbms;
        public ConversationHeaderMap()
        { 
				this.ToTable("ConversationHeaders");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.EntityId).HasColumnName("EntityId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.IsWaitingForResponse).HasColumnName("IsWaitingForResponse");

            this.Property(t => t.EntityDescription).HasColumnName("EntityDescription").HasMaxLength(250).IsUnicode(true);
        }
    }
}
	 