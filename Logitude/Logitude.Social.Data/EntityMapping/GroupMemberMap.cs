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
 
    public class GroupMemberMap : EntityTypeConfiguration<GroupMember>
    {
	    string dbms;
        public GroupMemberMap()
        { 
				this.ToTable("GroupMembers");
		
		    this.HasKey(t => new { t.GroupId, t.UserId });
	 
            this.Property(t => t.GroupId).HasColumnName("GroupId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UserId).HasColumnName("UserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.IsCancelled).HasColumnName("IsCancelled");

            this.Property(t => t.CancelledDate).HasColumnName("CancelledDate");
        }
    }
}
	 