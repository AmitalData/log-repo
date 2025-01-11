using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.EntityMapping
{
    public class CaptchaKeyMap : EntityTypeConfiguration<CaptchaKey>
    {
        
        public CaptchaKeyMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);


            this.Property(t => t.IP)
        .HasMaxLength(15)
        .IsUnicode(false);

            this.Property(t => t.Code)
            .IsRequired()
            .HasMaxLength(6)
            .IsUnicode(false);

            this.Property(t => t.Email)
            .HasMaxLength(70)
            .IsUnicode(false);

            this.Property(t => t.Activity)
                .HasMaxLength(70)
                .IsUnicode(false);



            // Table & Column Mappings
            this.ToTable("CaptchaKeys");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t =>t.Code).HasColumnName("Code");
            this.Property(t => t.IP).HasColumnName("IP");
            this.Property(t => t.Activity).HasColumnName("Activity");
            this.Property(t => t.IsUsed).HasColumnName("IsUsed");
            
        }
    }
}
