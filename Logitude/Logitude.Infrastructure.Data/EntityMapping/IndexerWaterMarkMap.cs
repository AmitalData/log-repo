using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System.Data.Entity.ModelConfiguration;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data;

namespace Logitude.Infrastructure.Data.EntityMapping
{

    public class IndexerWaterMarkMap : EntityTypeConfiguration<IndexerWaterMark>
    {
        public IndexerWaterMarkMap()
        {
            this.ToTable("ElasticIndexerWaterMarks");

            this.HasKey(t => new { t.TableName });

            this.Property(t => t.TableName).HasColumnName("TableName").IsRequired().HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.LastUpdateDate).HasColumnName("LastUpdateDate");
        }
    }
}
