using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class AgentSharedDocumentMap : EntityTypeConfiguration<AgentSharedDocument>
    {
        public AgentSharedDocumentMap()
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
            this.Property(t => t.DocumentXML).IsMaxLength().IsRequired().IsUnicode(true);
            this.Property(t => t.AgentReference).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.AgentId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.StatusCode).HasMaxLength(4).IsRequired().IsUnicode(false);
            this.Property(t => t.ShipmentLevelCode).HasMaxLength(1).IsUnicode(false);
            this.Property(t => t.AgentSharedManifestRef).HasMaxLength(20).IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("AgentSharedDocuments");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.AgentReference).HasColumnName("AgentReference");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.DocumentXML).HasColumnName("DocumentXML");
            this.Property(t => t.AgentId).HasColumnName("AgentId");
            this.Property(t => t.StatusCode).HasColumnName("StatusCode");
            this.Property(t => t.ShipmentLevelCode).HasColumnName("ShipmentLevelCode");
            this.Property(t => t.AgentSharedManifestRef).HasColumnName("AgentSharedManifestRef");
            

            // Relationships

            this.HasRequired(t => t.SharedManifestsStatus)
                .WithMany()
                .HasForeignKey(d => d.StatusCode);


            this.HasOptional(t => t.Agent)
                .WithMany()
                .HasForeignKey(d => d.AgentId);


            this.HasOptional(t => t.ShipmentLevel)
                .WithMany()
                .HasForeignKey(d => d.ShipmentLevelCode);



        }
    }
}
