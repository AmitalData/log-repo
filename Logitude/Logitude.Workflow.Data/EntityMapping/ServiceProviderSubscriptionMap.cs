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
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.Data;
 
namespace Logitude.Workflow.Data.EntityMapping
{
 
    public class ServiceProviderSubscriptionMap : EntityTypeConfiguration<ServiceProviderSubscription>
    {
	    string dbms;
        public ServiceProviderSubscriptionMap()
        { 
				this.ToTable("ServiceProviderSubscriptions");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate").IsRequired();

            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.UserEmail).HasColumnName("UserEmail").IsRequired().HasMaxLength(250).IsUnicode(true);

            this.Property(t => t.AccessToken).HasColumnName("AccessToken").IsRequired().IsMaxLength().IsUnicode(true);

            this.Property(t => t.RefreshToken).HasColumnName("RefreshToken").IsRequired().IsMaxLength().IsUnicode(true);

            this.Property(t => t.EmailProvider).HasColumnName("EmailProvider").IsRequired().HasMaxLength(50).IsUnicode(false);

            this.Property(t => t.WorkflowNumber).HasColumnName("WorkflowNumber").IsRequired().HasMaxLength(100).IsUnicode(true);

            this.Property(t => t.AdditionalSettings).HasColumnName("AdditionalSettings").IsMaxLength().IsUnicode(true);

            this.Property(t => t.SubscriptionExpirationDateTime).HasColumnName("SubscriptionExpirationDateTime").IsRequired();

            this.Property(t => t.AccessTokenExpirationDateTime).HasColumnName("AccessTokenExpirationDateTime").IsRequired();

            this.Property(t => t.WebhookParams).HasColumnName("WebhookParams").IsRequired().IsMaxLength().IsUnicode(true);
        }
    }
}
	 