using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class QueryExportExecutionLogMap : EntityTypeConfiguration<QueryExportExecutionLog>
    {
        string dbms;
        public QueryExportExecutionLogMap()
        {
            this.ToTable("QueryExportExecutionLogs");

            this.HasKey(t => new { t.Id });

            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.StatusCode).HasColumnName("StatusCode").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.ExceptionMessage).HasColumnName("ExceptionMessage").IsMaxLength().IsUnicode(true);

            this.Property(t => t.DoneDate).HasColumnName("DoneDate");

            this.Property(t => t.QueryFilterXML).HasColumnName("QueryFilterXML").IsMaxLength().IsUnicode(true);

            this.Property(t => t.QueryCode).HasColumnName("QueryCode").HasMaxLength(30).IsUnicode(false);
        }
    }
}
