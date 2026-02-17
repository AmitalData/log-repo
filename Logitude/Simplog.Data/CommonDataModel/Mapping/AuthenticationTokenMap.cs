using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class AuthenticationTokenMap : EntityTypeConfiguration<AuthenticationToken>
    {
        public AuthenticationTokenMap()
        {
            // Primary Key
            this.HasKey(t => t.Token);

            // Properties
            this.Property(t => t.Token)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            this.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(70)
                .IsUnicode(false);


            this.Property(t => t.Password)
                .IsRequired()
                .HasMaxLength(60)
                .IsUnicode(false);

            this.Property(t => t.APICredentialID)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ClientType)
                .HasMaxLength(70)
                .IsUnicode(false);

            this.Property(t => t.InActiveReason)
                .HasMaxLength(250)
                .IsUnicode(false);



            // Table & Column Mappings
            this.ToTable("AuthenticationTokens");
            this.Property(t => t.Token).HasColumnName("Token");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.Password).HasColumnName("Password");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.APICredentialID).HasColumnName("APICredentialID");


            this.Property(t => t.InActiveReason).HasColumnName("InActiveReason");
            this.Property(t => t.InActiveDate).HasColumnName("InActiveDate");
            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t => t.ClientType).HasColumnName("ClientType");
            this.Property(t => t.ExpirationDate).HasColumnName("ExpirationDate");

        }
    }
}
