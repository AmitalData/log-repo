using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class MultiEntityUpdateLogMap : EntityTypeConfiguration<MultiEntityUpdateLog>
    {
        public MultiEntityUpdateLogMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ObjectTableId)
                 .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.CreatedByUserId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.StatusCode)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false);

            this.Property(t => t.ExceptionMessage)
                .HasMaxLength(8000)
                .IsUnicode(true);

            this.Property(t => t.XMLData)
                .IsMaxLength()
                .IsUnicode(true);


            // Table & Column Mappings
            this.ToTable("MultiEntityUpdateLogs");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.StatusCode).HasColumnName("StatusCode");
            this.Property(t => t.ExceptionMessage).HasColumnName("ExceptionMessage");
            this.Property(t => t.XMLData).HasColumnName("XMLData");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.DoneDate).HasColumnName("DoneDate");
            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId");
            this.Property(t => t.RetryNumber).HasColumnName("RetryNumber");
            this.Property(t => t.UpdatedEntitiesNumber).HasColumnName("UpdatedEntitiesNumber");
            this.Property(t => t.StartDate).HasColumnName("StartDate");

        }
    }
}
