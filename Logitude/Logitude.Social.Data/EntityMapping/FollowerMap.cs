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
 
    public class FollowerMap : EntityTypeConfiguration<Follower>
    {
	    string dbms;
        public FollowerMap()
        { 
				this.ToTable("Followers");
		
		    this.HasKey(t => new { t.FolloweeUserId, t.FollowerUserId });
	 
            this.Property(t => t.FolloweeUserId).HasColumnName("FolloweeUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.FollowerUserId).HasColumnName("FollowerUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.IsCancelled).HasColumnName("IsCancelled");

            this.Property(t => t.CancelledDate).HasColumnName("CancelledDate");
        }
    }
}
	 