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
    public class QueueMessageMoreDetailsMap : EntityTypeConfiguration<QueueMessageMoreDetails>
    {
        public QueueMessageMoreDetailsMap()
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

            this.Property(t => t.Field1)
               .IsRequired()
               .HasMaxLength(15)
               .IsUnicode(false);

            this.Property(t => t.Field2)
               .IsRequired()
               .HasMaxLength(15)
               .IsUnicode(false);

            this.Property(t => t.Field3)
               .IsRequired()
               .HasMaxLength(15)
               .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("QueueMessageMoreDetails");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.QueueDefinitionCode).HasColumnName("QueueDefinitionCode");
            this.Property(t => t.MessageBody).HasColumnName("MessageBody");
            this.Property(t => t.CreateDateTime).HasColumnName("CreateDateTime");
            this.Property(t => t.NextRunDateTime).HasColumnName("NextRunDateTime");
            this.Property(t => t.ProcessingDateTime).HasColumnName("ProcessingDateTime");
            this.Property(t => t.CompleteDateTime).HasColumnName("CompleteDateTime");
            this.Property(t => t.RetryNumber).HasColumnName("RetryNumber");
            this.Property(t => t.Field1).HasColumnName("Field1"); 
            this.Property(t => t.Field2).HasColumnName("Field2");
            this.Property(t => t.Field3).HasColumnName("Field3");

            this.HasRequired(t => t.QueueDefinition).WithMany().HasForeignKey(d => d.QueueDefinitionCode);
            //this.HasRequired(t => t.QueueMessage); 
 
        }
    }
}