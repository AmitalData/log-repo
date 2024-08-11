using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class APILogsMap : EntityTypeConfiguration<APILogs>
    {
        public APILogsMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Tenant).IsRequired();
            

            this.Property(t => t.Id)
                 .IsRequired()
                 .HasMaxLength(15)
                 .IsUnicode(false);

            this.Property(t => t.Direction)
                .IsRequired()
                .HasMaxLength(1)
                .IsUnicode(false);

            this.Property(t => t.Status)
                .IsRequired()
                .HasMaxLength(1)
                .IsUnicode(false);

            this.Property(t => t.Subject)
                .HasMaxLength(100)
                .IsUnicode(false);

            this.Property(t => t.EntityId)
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.ObjectTableId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.PartnerName)
           .HasMaxLength(200)
           .IsUnicode(true);

            this.Property(t => t.Refrence)
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.SearchFields)
                .HasMaxLength(1000)
                .IsUnicode(true);

            this.Property(t => t.LastExceptionMessage)
                .HasMaxLength(250)
                .IsUnicode(true);

            this.Property(t => t.CorrelationId)
                .HasMaxLength(64)
                .IsUnicode(false);

            this.Property(t => t.BatchNumber)
                .HasMaxLength(15)
                .IsUnicode(false);
            this.Property(t => t.QueueType)
               .HasMaxLength(100)
               .IsUnicode(false);
            this.Property(t => t.QueueMessage)
               .HasMaxLength(400)
               .IsUnicode(false);

            this.Property(t => t.QueueMessageMoreDetailsId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.CustomerId)
              .HasMaxLength(15)
              .IsUnicode(false);



            // Table & Column Mappings
            this.ToTable("APILogs");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Direction).HasColumnName("Direction");
            this.Property(t => t.Status).HasColumnName("Status");

            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.CreateDateUTC).HasColumnName("CreateDateUTC");
            this.Property(t => t.LastUpdateDate).HasColumnName("LastUpdateDate");
            this.Property(t => t.LastUpdateDateUTC).HasColumnName("LastUpdateDateUTC");

            this.Property(t => t.NumberOfRetries).HasColumnName("NumberOfRetries");
            this.Property(t => t.ExpirationDate).HasColumnName("ExpirationDate");
            this.Property(t => t.Subject).HasColumnName("Subject");
            this.Property(t => t.EntityId).HasColumnName("EntityId");
            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId");
            this.Property(t => t.PartnerName).HasColumnName("PartnerName");
            this.Property(t => t.Refrence).HasColumnName("Refrence");

            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.LastExceptionMessage).HasColumnName("LastExceptionMessage");
            this.Property(t => t.CorrelationId).HasColumnName("CorrelationId");
            this.Property(t => t.BatchNumber).HasColumnName("BatchNumber");
            this.Property(t => t.QueueMessageMoreDetailsId).HasColumnName("QueueMessageMoreDetailsId");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.QueueMessage).HasColumnName("QueueMessage");
            this.Property(t => t.QueueType).HasColumnName("QueueType");


            this.HasRequired(t => t.APILogsData).WithRequiredPrincipal(d => d.APILogs);
        }
    }
}
