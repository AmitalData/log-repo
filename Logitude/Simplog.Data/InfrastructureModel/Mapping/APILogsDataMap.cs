using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class APILogsDataMap : EntityTypeConfiguration<APILogsData>
    {
        public APILogsDataMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);
            this.Property(t => t.Tenant).IsRequired();
            this.Property(t => t.DiagnosticLog)
                .IsMaxLength()
                .IsUnicode(true);
            this.Property(t => t.RequestData)
                .IsMaxLength()
                .IsUnicode(true);
            this.Property(t => t.ResponseData)
                .IsMaxLength()
                .IsUnicode(true);
            this.Property(t => t.ExceptionsMessage)
                .IsMaxLength()
                .IsUnicode(true);

            // Table & Column Mappings
            this.ToTable("APILogsData");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.DiagnosticLog).HasColumnName("DiagnosticLog");
            this.Property(t => t.RequestData).HasColumnName("RequestData");
            this.Property(t => t.ResponseData).HasColumnName("ResponseData");
            this.Property(t => t.ExceptionsMessage).HasColumnName("ExceptionsMessage");

            this.HasRequired(t => t.APILogs);//.WithRequiredPrincipal(t=>t.APILogsData);

        }
    }
}
