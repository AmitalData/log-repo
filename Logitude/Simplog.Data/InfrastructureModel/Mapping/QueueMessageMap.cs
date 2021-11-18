using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class QueueMessageMap : EntityTypeConfiguration<QueueMessage>
    {
        public QueueMessageMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired();
               

            this.Property(t => t.QueueDefinitionCode)
                .IsRequired()
                .HasMaxLength(265)
                .IsUnicode(false);

            this.Property(t => t.MessageBody)
               .IsRequired()
               .HasMaxLength(1000)
               .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("QueueMessages");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.QueueDefinitionCode).HasColumnName("QueueDefinitionCode");
            this.Property(t => t.MessageBody).HasColumnName("MessageBody");
            this.Property(t => t.CreateDateTime).HasColumnName("CreateDateTime");
            this.Property(t => t.NextRunDateTime).HasColumnName("NextRunDateTime");
            this.Property(t => t.ProcessingDateTime).HasColumnName("ProcessingDateTime");
            this.Property(t => t.CompleteDateTime).HasColumnName("CompleteDateTime");
            this.Property(t => t.RetryNumber).HasColumnName("RetryNumber");
            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.HasRequired(t => t.QueueDefinition).WithMany().HasForeignKey(d => d.QueueDefinitionCode);
            //this.HasRequired(t => t.QueueMessageMoreDetails).WithRequiredPrincipal(d => d.QueueMessage);

            this.Property(t => t.TenantPriority).HasColumnName("TenantPriority");

        }
    }
}