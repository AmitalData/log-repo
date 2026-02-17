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
 
    public class ConversationHeaderMessageMap : EntityTypeConfiguration<ConversationHeaderMessage>
    {
	    string dbms;
        public ConversationHeaderMessageMap()
        { 
				this.ToTable("ConversationHeaderMessages");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.ConversationHeaderId).HasColumnName("ConversationHeaderId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.MessageBody).HasColumnName("MessageBody").IsRequired().HasMaxLength(1000).IsUnicode(true);
        }
    }
}
	 