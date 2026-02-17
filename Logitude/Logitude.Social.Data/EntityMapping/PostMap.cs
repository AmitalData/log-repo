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
 
    public class PostMap : EntityTypeConfiguration<Post>
    {
	    string dbms;
        public PostMap()
        { 
				this.ToTable("Posts");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreatedById).HasColumnName("CreatedById").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.GroupId).HasColumnName("GroupId").HasMaxLength(15).IsUnicode(false);

            dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
              this.Property(t => t.BodyText).HasMaxLength(2000);
			}
            else
            {
              this.Property(t => t.BodyText).HasMaxLength(4000);
			}


            this.Property(t => t.BodyText).HasColumnName("BodyText").IsRequired().IsUnicode(true);

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.ParentPostId).HasColumnName("ParentPostId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.NumberOfLikes).HasColumnName("NumberOfLikes");

            this.Property(t => t.IsPrivate).HasColumnName("IsPrivate");

            this.Property(t => t.IsCancelled).HasColumnName("IsCancelled");

            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.EntityId).HasColumnName("EntityId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.IsAutomatic).HasColumnName("IsAutomatic");

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");

            this.Property(t => t.EntityDescription).HasColumnName("EntityDescription").HasMaxLength(250).IsUnicode(true);

            this.Property(t => t.NumberOfComments).HasColumnName("NumberOfComments");
        }
    }
}
	 