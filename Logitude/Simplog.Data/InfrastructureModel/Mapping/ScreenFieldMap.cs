using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class ScreenFieldMap : EntityTypeConfiguration<ScreenField>
    {
        public ScreenFieldMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ScreenId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ObjectFieldId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

          

            // Table & Column Mappings
            this.ToTable("ScreenFields");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
          
            this.Property(t => t.ScreenId).HasColumnName("ScreenId");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ObjectFieldId).HasColumnName("ObjectFieldId");
             
            //#if ORACLE_DB
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
             if (dbms == "oracle")
             {
                 this.Property(t => t.Column).HasColumnName("Column_");
                 this.Property(t => t.Row).HasColumnName("Row_");
             }
             //#else
             else
             {
                 this.Property(t => t.Column).HasColumnName("Column");
                 this.Property(t => t.Row).HasColumnName("Row");
             }

            
            //#endif
            // Relationships
            //this.HasRequired(t => t.ObjectField)
            //    .WithMany(t => t.ScreenFields)
            //    .HasForeignKey(d => d.ObjectFieldId);
            //this.HasRequired(t => t.Screen)
            //    .WithMany(t => t.ScreenFields)
            //    .HasForeignKey(d => d.ScreenId);

        }
    }
}
