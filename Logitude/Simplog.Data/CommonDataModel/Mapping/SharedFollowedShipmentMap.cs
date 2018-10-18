using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class SharedFollowedShipmentMap : EntityTypeConfiguration<SharedFollowedShipment> 
    {

        public SharedFollowedShipmentMap()
        {


            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

 
               this.Property(t => t.Tenant)
                   .IsRequired();



            this.Property(t => t.ShipmentId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ContactId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);


            this.Property(t => t.TrackDate)
                .IsRequired();
          



                     // Table & Column Mappings
            this.ToTable("SharedFollowedShipments");
         this.Property(t => t.Id).HasColumnName("Id");
         this.Property(t => t.Tenant).HasColumnName("Tenant");

         this.Property(t => t.ShipmentId).HasColumnName("ShipmentId");
         this.Property(t => t.ContactId).HasColumnName("ContactId");

         this.Property(t => t.TrackDate).HasColumnName("TrackDate");


         // Relationships
         this.HasRequired(t => t.Contact)
             .WithMany()
             .HasForeignKey(d => d.ContactId);

         this.HasRequired(t => t.Shipment)
             .WithMany()
            .HasForeignKey(d => d.ShipmentId);


        }

    }
}
