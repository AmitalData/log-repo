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
 
    public class NotificationReplyMap : EntityTypeConfiguration<NotificationReply>
    {
	    string dbms;
        public NotificationReplyMap()
        { 
			  this.ToTable("NotificationReplies", "Customs");
		
		    this.HasKey(t => new { t.NotificationId, t.Line });
	 
            this.Property(t => t.NotificationId).HasColumnName("NotificationId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Line).HasColumnName("Line").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.ResponseToCustoms).HasColumnName("ResponseToCustoms").IsMaxLength().IsUnicode(true);

            this.Property(t => t.RepliedByUserId).HasColumnName("RepliedByUserId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ReplyDateTime).HasColumnName("ReplyDateTime");
        }
    }
}
	 