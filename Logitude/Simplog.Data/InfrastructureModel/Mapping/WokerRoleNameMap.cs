using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Mapping
{


    public class WorkerRoleNameMap : EntityTypeConfiguration<WorkerRoleName>
    {
        public WorkerRoleNameMap()
        {
            this.HasKey(t => t.Name);
            this.Property(t => t.Name).IsRequired().HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.WaitingStatus).IsRequired();
            this.Property(t => t.CreateDate);


            this.ToTable("WorkerRoleNames");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.WaitingStatus).HasColumnName("WaitingStatus");
        }
    }


}
