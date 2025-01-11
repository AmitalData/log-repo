using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.EntityMapping
{
    public class AutoSignupEmailMap : EntityTypeConfiguration<AutoSignupEmail>
    {
        public AutoSignupEmailMap()
        {
            this.HasKey(t => new { t.Id });

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);



            this.Property(t => t.EmailBody)
                .IsMaxLength()
                .IsUnicode(true);

            this.Property(t => t.Status)
              .IsRequired()
             .HasMaxLength(25)
             .IsUnicode(false);

            this.Property(t => t.EmailSubject)
           .HasMaxLength(200)
           .IsUnicode(false);

            this.Property(t => t.Retries);

            this.Property(t => t.CreateDate)
                 .IsRequired();


            this.ToTable("AutoSignupEmails");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.EmailBody).HasColumnName("EmailBody");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.Retries).HasColumnName("Retries");
            this.Property(t => t.EmailSubject).HasColumnName("EmailSubject");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");


        }
    }

}