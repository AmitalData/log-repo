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
            this.HasKey(t => t.Version);

            // Properties
            this.Property(t => t.Version)
                .HasDatabaseGeneratedOption(null);

            // Table & Column Mappings
            this.ToTable("TermsofUses");

//#if ORACLE_DB
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
                this.Property(t => t.Version).HasColumnName("Version_");
                this.Property(t => t.Date).HasColumnName("Date_");
            }
            //#else
            else
            {
                this.Property(t => t.Version).HasColumnName("Version");
                this.Property(t => t.Date).HasColumnName("Date");
            }
//#endif
           
        }
    }
}
