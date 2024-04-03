using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class CustomChildObjectMap : EntityTypeConfiguration<CustomChildObject>
    {
        public CustomChildObjectMap()
        {
            this.HasKey(t => t.Id); 
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ParentEntityId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ParentObjectTableId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ObjectTableId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.CreatedBy).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.UpdatedBy).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Field1).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field2).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field3).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field4).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field5).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field6).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field7).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field8).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field9).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field10).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field11).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field12).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field13).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field14).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field15).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field16).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field17).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field18).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field19).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field20).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field21).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field22).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field23).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field24).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field25).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field26).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field27).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field28).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field29).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field30).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field31).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field32).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field33).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field34).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field35).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field36).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field37).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field38).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field39).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field40).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field41).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field42).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field43).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field44).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field45).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field46).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field47).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field48).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field49).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Field50).HasMaxLength(2000).IsUnicode(true);

            this.ToTable("CustomChildObjects");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ParentEntityId).HasColumnName("ParentEntityId");
            this.Property(t => t.ParentObjectTableId).HasColumnName("ParentObjectTableId");
            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.UpdatedBy).HasColumnName("UpdatedBy");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.Field1).HasColumnName("Field1");
            this.Property(t => t.Field2).HasColumnName("Field2");
            this.Property(t => t.Field3).HasColumnName("Field3");
            this.Property(t => t.Field4).HasColumnName("Field4");
            this.Property(t => t.Field5).HasColumnName("Field5");
            this.Property(t => t.Field6).HasColumnName("Field6");
            this.Property(t => t.Field7).HasColumnName("Field7");
            this.Property(t => t.Field8).HasColumnName("Field8");
            this.Property(t => t.Field9).HasColumnName("Field9");
            this.Property(t => t.Field10).HasColumnName("Field10");
            this.Property(t => t.Field11).HasColumnName("Field11");
            this.Property(t => t.Field12).HasColumnName("Field12");
            this.Property(t => t.Field13).HasColumnName("Field13");
            this.Property(t => t.Field14).HasColumnName("Field14");
            this.Property(t => t.Field15).HasColumnName("Field15");
            this.Property(t => t.Field16).HasColumnName("Field16");
            this.Property(t => t.Field17).HasColumnName("Field17");
            this.Property(t => t.Field18).HasColumnName("Field18");
            this.Property(t => t.Field19).HasColumnName("Field19");
            this.Property(t => t.Field20).HasColumnName("Field20");
            this.Property(t => t.Field21).HasColumnName("Field21");
            this.Property(t => t.Field22).HasColumnName("Field22");
            this.Property(t => t.Field23).HasColumnName("Field23");
            this.Property(t => t.Field24).HasColumnName("Field24");
            this.Property(t => t.Field25).HasColumnName("Field25");
            this.Property(t => t.Field26).HasColumnName("Field26");
            this.Property(t => t.Field27).HasColumnName("Field27");
            this.Property(t => t.Field28).HasColumnName("Field28");
            this.Property(t => t.Field29).HasColumnName("Field29");
            this.Property(t => t.Field30).HasColumnName("Field30");
            this.Property(t => t.Field31).HasColumnName("Field31");
            this.Property(t => t.Field32).HasColumnName("Field32");
            this.Property(t => t.Field33).HasColumnName("Field33");
            this.Property(t => t.Field34).HasColumnName("Field34");
            this.Property(t => t.Field35).HasColumnName("Field35");
            this.Property(t => t.Field36).HasColumnName("Field36");
            this.Property(t => t.Field37).HasColumnName("Field37");
            this.Property(t => t.Field38).HasColumnName("Field38");
            this.Property(t => t.Field39).HasColumnName("Field39");
            this.Property(t => t.Field40).HasColumnName("Field40");
            this.Property(t => t.Field41).HasColumnName("Field41");
            this.Property(t => t.Field42).HasColumnName("Field42");
            this.Property(t => t.Field43).HasColumnName("Field43");
            this.Property(t => t.Field44).HasColumnName("Field44");
            this.Property(t => t.Field45).HasColumnName("Field45");
            this.Property(t => t.Field46).HasColumnName("Field46");
            this.Property(t => t.Field47).HasColumnName("Field47");
            this.Property(t => t.Field48).HasColumnName("Field48");
            this.Property(t => t.Field49).HasColumnName("Field49");
            this.Property(t => t.Field50).HasColumnName("Field50");

            // Relationships
            this.HasRequired(t => t.CreatedByUser).WithMany().HasForeignKey(d => d.CreatedBy);
            this.HasRequired(t => t.UpdatedByUser).WithMany().HasForeignKey(d => d.UpdatedBy);
        }
    }
}
