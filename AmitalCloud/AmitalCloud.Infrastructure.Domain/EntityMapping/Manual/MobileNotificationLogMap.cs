using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.EntityMapping
{
    public class MobileNotificationLogMap : EntityTypeConfiguration<MobileNotificationLog>
    {

        public MobileNotificationLogMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);


            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(40)
                 .IsUnicode(false);


            this.Property(t => t.Email)
                   .IsRequired()
                .HasMaxLength(70)
                 .IsUnicode(false);



            this.Property(t => t.NotificationMessage)
                .IsRequired()
                .HasMaxLength(500)
                .IsUnicode(true);

            this.Property(t => t.NumberOfRetriesAndroid);
            this.Property(t => t.NumberOfRetriesIOS);

            this.Property(t => t.Tenant)
            .IsRequired();


                 this.Property(t => t.EntityId)
             
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ObjectTableId)
               
                .HasMaxLength(15)
                .IsUnicode(false);


            this.Property(t => t.Exception)
                .HasMaxLength(8000)
                .IsUnicode(true);



            this.Property(t => t.Log)
             
                .IsMaxLength()
                . IsUnicode(false);


           this.Property(t => t.IOSStatus)
               .IsRequired()
               .HasMaxLength(1)
               .IsUnicode(false);


            this.Property(t => t.AndroidStatus)
           .IsRequired()
           .HasMaxLength(1)
           .IsUnicode(false);


            this.Property(t => t.DoneDate);
            this.Property(t => t.SourceEventDate);



            this.Property(t => t.XML)
                .IsMaxLength()
                .IsUnicode(true);

            this.Property(t => t.NotificationMessageAndroid)
             
                .HasMaxLength(500)
                .IsUnicode(true);


            this.Property(t => t.NotificationMessageIOS)

                .HasMaxLength(500)
                .IsUnicode(true);


            // Table & Column Mappings
            this.ToTable("MobileNotificationLogs");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.NotificationMessage).HasColumnName("NotificationMessage");
            this.Property(t => t.EntityId).HasColumnName("EntityId");
            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId");
            this.Property(t => t.Exception).HasColumnName("Exception");
            this.Property(t => t.Log).HasColumnName("Log");
            this.Property(t => t.NumberOfRetriesAndroid).HasColumnName("NumberOfRetriesAndroid");
            this.Property(t => t.NumberOfRetriesIOS).HasColumnName("NumberOfRetriesIOS");
            
            this.Property(t => t.IOSStatus).HasColumnName("IOSStatus");
            this.Property(t => t.AndroidStatus).HasColumnName("AndroidStatus");
            this.Property(t => t.IsException).HasColumnName("IsException");
                        

            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.IsRead).HasColumnName("IsRead");
            this.Property(t => t.IsDelete).HasColumnName("IsDelete");



            this.Property(t => t.DoneDate).HasColumnName("DoneDate");
            this.Property(t => t.SourceEventDate).HasColumnName("SourceEventDate");

            this.Property(t => t.XML).HasColumnName("XML");
            this.Property(t => t.NotificationMessageIOS).HasColumnName("NotificationMessageIOS");
            this.Property(t => t.NotificationMessageAndroid).HasColumnName("NotificationMessageAndroid");
        }
    }
}
