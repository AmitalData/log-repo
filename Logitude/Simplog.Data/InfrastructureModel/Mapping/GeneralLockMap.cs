using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class GeneralLockMap
        : EntityTypeConfiguration<GeneralLock>
    {
        public GeneralLockMap()
        {
            
            // Primary Key
            this.HasKey(t => new { t.GeneralKey, t.Tenant });

            // Properties
            this.Property(t => t.GeneralKey)
                .IsRequired()
                .HasMaxLength(128)
                .IsUnicode(false);

            this.Property(t => t.Tenant)
                .IsRequired();

			this.Property(t => t.EntityId1)
	            .HasMaxLength(15)
	            .IsUnicode(false);

			this.Property(t => t.ObjectTableId1)
	            .HasMaxLength(15)
	            .IsUnicode(false);

			this.Property(t => t.EntityId2)
                .HasMaxLength(15)
                .IsUnicode(false);

			this.Property(t => t.ObjectTableId2)
                .HasMaxLength(15)
                .IsUnicode(false);

			this.Property(t => t.UserId)
			   .IsRequired()
			   .HasMaxLength(15)
			   .IsUnicode(false);

			this.Property(t => t.SessionId)
				.HasMaxLength(100)
				.IsUnicode(false);

			// Table & Column Mappings
			this.ToTable("GeneralLocks");
            this.Property(t => t.GeneralKey).HasColumnName("GeneralKey");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
			this.Property(t => t.EntityId1).HasColumnName("EntityId1");
			this.Property(t => t.ObjectTableId1).HasColumnName("ObjectTableId1");
			this.Property(t => t.EntityId2).HasColumnName("EntityId1");
			this.Property(t => t.ObjectTableId2).HasColumnName("ObjectTableId1");
			this.Property(t => t.UserId).HasColumnName("UserId");
			this.Property(t => t.SessionId).HasColumnName("SessionId");


			this.HasOptional(t => t.ObjectTable1)
			   .WithMany()
			   .HasForeignKey(d => d.ObjectTableId1);
			this.HasOptional(t => t.ObjectTable2)
			   .WithMany()
			   .HasForeignKey(d => d.ObjectTableId2);
			this.HasRequired(t => t.UsedByUser)
				.WithMany()
				.HasForeignKey(d => d.UserId);

		}
	}
}
