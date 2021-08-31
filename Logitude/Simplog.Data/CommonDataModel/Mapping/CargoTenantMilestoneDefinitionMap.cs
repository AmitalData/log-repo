using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Data.Entity.ModelConfiguration;

namespace Simplog.Data.CommonDataModel.Mapping
{
  public  class CargoTenantMilestoneDefinitionMap : EntityTypeConfiguration<CargoTenantMilestoneDefinition>
    {

      public CargoTenantMilestoneDefinitionMap()
        {
            this.HasKey(d => d.Id);

            this.Property(d => d.Code)
                .HasMaxLength(2)
                .IsUnicode(false);
   
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.IsCustomerView).HasColumnName("IsCustomerView");
        }
    }
}
