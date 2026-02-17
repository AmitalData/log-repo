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
 
    public class PostLikeMap : EntityTypeConfiguration<PostLike>
    {
	    string dbms;
        public PostLikeMap()
        { 
				this.ToTable("PostLikes");
		
		    this.HasKey(t => new { t.PostId, t.UserId });
	 
            this.Property(t => t.PostId).HasColumnName("PostId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UserId).HasColumnName("UserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.IsCancelled).HasColumnName("IsCancelled");
        }
    }
}
	 