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
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data;
 
namespace Logitude.Customs.Data.EntityMapping
{
 
    public class ContainerizationMap : EntityTypeConfiguration<Containerization>
    {
	    string dbms;
        public ContainerizationMap()
        { 
			  this.ToTable("Containerizations", "Customs");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.AgentDeclaration).HasColumnName("AgentDeclaration");

            this.Property(t => t.ContainerizationDate).HasColumnName("ContainerizationDate");

            this.Property(t => t.ContainerizationNumber).HasColumnName("ContainerizationNumber").HasMaxLength(35).IsUnicode(false);

            this.Property(t => t.ContainerizationStatus).HasColumnName("ContainerizationStatus").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.HataraStatus).HasColumnName("HataraStatus").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.OperationMode).HasColumnName("OperationMode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.IsChange).HasColumnName("IsChange");
        }
    }
}
	 