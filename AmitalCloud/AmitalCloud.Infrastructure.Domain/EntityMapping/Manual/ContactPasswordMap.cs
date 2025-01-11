using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;

namespace AmitalCloud.Infrastructure.Domain.EntityMapping
{
    public class ContactPasswordMap : EntityTypeConfiguration<ContactPassword>
    {
        public ContactPasswordMap()
        {

            // Primary Key
            this.HasKey(t => t.Email);
             

            this.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(70)
                .IsUnicode(false);

            this.Property(t => t.Password)
                .IsRequired()
                .HasMaxLength(60)
                .IsUnicode(false);

            this.Property(t => t.LockDateTime);

            this.Property(t => t.CaptchaKey)
                .HasMaxLength(40)
                .IsUnicode(false);




            // Table & Column Mappings
            this.ToTable("ContactPasswords");
            
            this.Property(t => t.Email).HasColumnName("Email");
           
            this.Property(t => t.Password).HasColumnName("Password");
            
            this.Property(t => t.MustChangePassword).HasColumnName("MustChangePassword");
            this.Property(t => t.IsLocked).HasColumnName("IsLocked");
            this.Property(t => t.NumberOfRetries).HasColumnName("NumberOfRetries");
            this.Property(t => t.LockDateTime).HasColumnName("LockDateTime");
            this.Property(t => t.CaptchaKey).HasColumnName("CaptchaKey");

            this.Property(t => t.IsSendNotificationForMobile).HasColumnName("IsSendNotificationForMobile");
            this.Property(t => t.PasswordExpirationDate).HasColumnName("PasswordExpirationDate");
            this.Property(t => t.IsBCrypt).HasColumnName("IsBCrypt");
            // #if ORACLE_DB
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
                this.Property(t => t.SharedMobileAppAlertsforFollowedShipment).HasColumnName("MobileAlertforFollowedShipment");
                this.Property(t => t.SharedMobileAppAlertonExceptions).HasColumnName("MobileAppAlertonExceptions");
            }
            //#else
            else
            {
                this.Property(t => t.SharedMobileAppAlertsforFollowedShipment).HasColumnName("SharedMobileAppAlertsforFollowedShipment");
                this.Property(t => t.SharedMobileAppAlertonExceptions).HasColumnName("SharedMobileAppAlertonExceptions");
            }
//#endif


        }
    }
}
