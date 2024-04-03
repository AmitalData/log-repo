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

            this.Property(t => t.VersionDocumentId).HasMaxLength(15);

            this.Property(t => t.PrivateLabelId).HasMaxLength(15);

            // Table & Column Mappings
            this.ToTable("TermsofUses");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.VersionNumber).HasColumnName("VersionNumber");
            this.Property(t => t.VersionDocumentId).HasColumnName("VersionDocumentId");
            this.Property(t => t.PrivateLabelId).HasColumnName("PrivateLabelId");
            //#if ORACLE_DB
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
               
                this.Property(t => t.Date).HasColumnName("Date_");
                this.Property(t => t.VersionNumber).HasColumnName("VersionNumber_");
                this.Property(t => t.VersionDocumentId).HasColumnName("VersionDocumentId_");
                this.Property(t => t.PrivateLabelId).HasColumnName("PrivateLabelId_");
                this.Property(t => t.IsNew).HasColumnName("IsNew_");

            }
            //#else
            else
            {
                
                this.Property(t => t.Date).HasColumnName("Date");
                this.Property(t => t.VersionNumber).HasColumnName("VersionNumber");
                this.Property(t => t.VersionDocumentId).HasColumnName("VersionDocumentId");
                this.Property(t => t.PrivateLabelId).HasColumnName("PrivateLabelId");
                this.Property(t => t.IsNew).HasColumnName("IsNew");

            }
            //#endif

            this.Property(t => t.Tenant).HasColumnName("Tenant");


        }
    }
}
