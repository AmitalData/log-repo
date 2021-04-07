using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class TermsofUsMap : EntityTypeConfiguration<TermsofUse>
    {
        public TermsofUsMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(null);

            // Table & Column Mappings
            this.ToTable("TermsofUses");


            //#if ORACLE_DB
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
                this.Property(t => t.Id).HasColumnName("Id_");
                this.Property(t => t.Date).HasColumnName("Date_");
                this.Property(t => t.VersionNumber).HasColumnName("VersionNumber_");

            }
            //#else
            else
            {
                this.Property(t => t.Id).HasColumnName("Id");
                this.Property(t => t.Date).HasColumnName("Date");
                this.Property(t => t.VersionNumber).HasColumnName("VersionNumber");

            }
            //#endif

            this.Property(t => t.Tenant).HasColumnName("Tenant");


        }
    }
}
