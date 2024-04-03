using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace Simplog.Data.InfrastructureModel.Mapping
{

        public class EventRemarkMap : EntityTypeConfiguration<EventRemark>
        {
            string dbms;
            public EventRemarkMap()
            {
                this.ToTable("EventRemarks");

                this.HasKey(t => new { t.Id });

                this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

                this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

                this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

                this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

                this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

                this.Property(t => t.EventTypeId).HasColumnName("EventTypeId").HasMaxLength(15).IsUnicode(false);

                this.Property(t => t.PartnerTypeId).HasColumnName("PartnerTypeId").HasMaxLength(2).IsFixedLength();

                this.Property(t => t.IsChoose).HasColumnName("IsChoose");
            }
        }
}



