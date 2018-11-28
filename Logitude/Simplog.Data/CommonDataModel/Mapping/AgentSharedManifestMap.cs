using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class AgentSharedManifestMap : EntityTypeConfiguration<AgentSharedManifest>
    {
        public AgentSharedManifestMap()
        {
            
            // Primary Key
            this.HasKey(t => t.Id);

            this.Property(t => t.Id)
               .IsRequired()
               .HasMaxLength(15)
               .IsUnicode(false);

            this.Property(t => t.Tenant).IsRequired();

            this.Property(t => t.CreateDate).IsRequired();
            this.Property(t => t.UpdateDate).IsRequired();
            this.Property(t => t.ShipmentTypeId).HasMaxLength(4).IsUnicode(false);


            this.Property(t => t.Master)
               .HasMaxLength(20)
               .IsUnicode(false);

            this.Property(t => t.ManifestXML)
            .IsMaxLength()
            .IsRequired()
            .IsUnicode(true);


            this.Property(t => t.AgentReference).IsRequired().HasMaxLength(15).IsUnicode(false);


            this.Property(t => t.UpdatedByUserId)
             .HasMaxLength(15)
             .IsUnicode(false)
             .IsRequired();

            this.Property(t => t.SearchFields)
           .HasMaxLength(1000)
           .IsUnicode(true);

            this.Property(t => t.TransportModeId).IsRequired().IsFixedLength().HasMaxLength(1).IsUnicode(false);
            this.Property(t => t.AgentId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.DirectionId).IsRequired().IsFixedLength().HasMaxLength(1).IsUnicode(false);
            this.Property(t => t.FromPortId).HasMaxLength(15).IsRequired().IsUnicode(false);
            this.Property(t => t.ToPortId).HasMaxLength(15).IsRequired().IsUnicode(false);
            this.Property(t => t.StatusCode).HasMaxLength(4).IsRequired().IsUnicode(false);
            this.Property(t => t.ShipmentLevelCode).HasMaxLength(1).IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("AgentSharedManifests");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.AgentReference).HasColumnName("AgentReference");
            this.Property(t => t.Master).HasColumnName("Master");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");
            this.Property(t => t.ManifestXML).HasColumnName("ManifestXML");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");

            this.Property(t => t.GrossWeight).HasColumnName("GrossWeight");
            this.Property(t => t.ChargeableWeight).HasColumnName("ChargeableWeight");
            this.Property(t => t.TEU).HasColumnName("TEU");
            this.Property(t => t.PackagesQuantity).HasColumnName("PackagesQuantity");


            this.Property(t => t.TransportModeId).HasColumnName("TransportModeId");
            this.Property(t => t.AgentId).HasColumnName("AgentId");
            this.Property(t => t.DirectionId).HasColumnName("DirectionId");
            this.Property(t => t.FromPortId).HasColumnName("FromPortId");
            this.Property(t => t.ToPortId).HasColumnName("ToPortId");
            this.Property(t => t.StatusCode).HasColumnName("StatusCode");
            this.Property(t => t.ShipmentLevelCode).HasColumnName("ShipmentLevelCode");
            this.Property(t => t.ShipmentTypeId).HasColumnName("ShipmentTypeId");
            this.Property(t => t.CancelledBySenderAgent).HasColumnName("CancelledBySenderAgent");

            
            // Relationships

            this.HasRequired(t => t.UpdatedByUser)
                .WithMany()
                .HasForeignKey(d => d.UpdatedByUserId);

            this.HasRequired(t => t.FromPort)
                .WithMany()
               .HasForeignKey(d => d.FromPortId);


            this.HasRequired(t => t.ToPort)
                .WithMany()
                .HasForeignKey(d => d.ToPortId);



            this.HasRequired(t => t.SharedManifestsStatus)
                .WithMany()
                .HasForeignKey(d => d.StatusCode);


            this.HasOptional(t => t.Agent)
                .WithMany()
                .HasForeignKey(d => d.AgentId);


            this.HasOptional(t => t.ShipmentLevel)
                .WithMany()
                .HasForeignKey(d => d.ShipmentLevelCode);


            this.HasOptional(t => t.ShipmentType).WithMany().HasForeignKey(d => d.ShipmentTypeId);

        }
    }
}
