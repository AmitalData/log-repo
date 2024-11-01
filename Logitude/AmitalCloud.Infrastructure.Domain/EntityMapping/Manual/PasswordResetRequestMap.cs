using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;

namespace AmitalCloud.Infrastructure.Domain.EntityMapping
{
    public class PasswordResetRequestMap : EntityTypeConfiguration<PasswordResetRequest>
    {
        public PasswordResetRequestMap()
        {
            // Primary Key
            this.HasKey(t => t.RequestNumber);

            // Properties
            this.Property(t => t.RequestNumber)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.Email)
                .HasMaxLength(70)
                .IsUnicode(false);

            this.Property(t => t.VerificationCode)
                .HasMaxLength(5)
               .IsUnicode(false);


            this.Property(t => t.Type)
                .HasMaxLength(20)
                .IsUnicode(false);

 


            // Table & Column Mappings
            this.ToTable("PasswordResetRequests");
            this.Property(t => t.RequestNumber).HasColumnName("RequestNumber");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.IsDone).HasColumnName("IsDone");

            this.Property(t => t.IsMobileOnly).HasColumnName("IsMobileOnly");
            this.Property(t => t.ExpirationDate).HasColumnName("ExpirationDate");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.DoneDate).HasColumnName("DoneDate");
            
            this.Property(t => t.VerificationCode).HasColumnName("VerificationCode");
            this.Property(t => t.Type).HasColumnName("Type");







        }
    }
}
