using System.Data.Entity.ModelConfiguration;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;

namespace AmitalCloud.Infrastructure.Domain.EntityMapping
{
    public class SessionPolicyMap : EntityTypeConfiguration<SessionPolicy>
    {
        public SessionPolicyMap()
        {

            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                 .IsUnicode(false);


            // Table & Column Mappings
            this.ToTable("SessionPolicies");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.WebTokenLifeTimeInMinutes).HasColumnName("WebTokenLifeTimeInMinutes");

            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
                this.Property(t => t.WebTokenExpirationWarningInMinutes).HasColumnName("WebTokenExpirationWarningInMin");

            }
            else
            {
                this.Property(t => t.WebTokenExpirationWarningInMinutes).HasColumnName("WebTokenExpirationWarningInMinutes");

            }


        }
    }
}
