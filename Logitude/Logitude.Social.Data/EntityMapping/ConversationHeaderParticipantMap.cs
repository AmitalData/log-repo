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
 
    public class ConversationHeaderParticipantMap : EntityTypeConfiguration<ConversationHeaderParticipant>
    {
	    string dbms;
        public ConversationHeaderParticipantMap()
        { 
				this.ToTable("ConversationHeaderParticipants");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.ConversationHeaderId).HasColumnName("ConversationHeaderId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ParticipantUserId).HasColumnName("ParticipantUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.LeaveDate).HasColumnName("LeaveDate");

            this.Property(t => t.IsLeft).HasColumnName("IsLeft").IsRequired();

            this.Property(t => t.Replied).HasColumnName("Replied").IsRequired();

            this.Property(t => t.IsRead).HasColumnName("IsRead").IsRequired();

            this.Property(t => t.LastReadDate).HasColumnName("LastReadDate");

            this.Property(t => t.IsDelete).HasColumnName("IsDelete");

            this.Property(t => t.DeleteDate).HasColumnName("DeleteDate");
        }
    }
}
	 