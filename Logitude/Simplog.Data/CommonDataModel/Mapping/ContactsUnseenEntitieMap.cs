using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
 public   class ContactsUnseenEntitieMap : EntityTypeConfiguration<ContactsUnseenEntitie> 
    {

     public ContactsUnseenEntitieMap()
     {



         this.HasKey(t => t.Id);

         // Properties
         this.Property(t => t.Id)
             .IsRequired()
             .HasMaxLength(40)
             .IsUnicode(false);


         this.Property(t => t.Tenant)
        .IsRequired();




         this.Property(t => t.EntityId)
             .IsRequired()
             .HasMaxLength(15)
             .IsUnicode(false);

         this.Property(t => t.ContactId)
             .IsRequired()
             .HasMaxLength(15)
             .IsUnicode(false);


         this.Property(t => t.ObjectTableId)
             .IsRequired()
             .HasMaxLength(15)
             .IsUnicode(false);

            this.Property(t => t.CreateDate).IsRequired();



            //         public string Id { get; set; }
            //public int Tenant { get; set; }
            // public string ContactId { get; set; }
            // public string  EntityId { get; set; }
            //  public string   ObjectTableId  { get; set; }

            // Table & Column Mappings
            this.ToTable("ContactsUnseenEntities");
         this.Property(t => t.Id).HasColumnName("Id");
         this.Property(t => t.Tenant).HasColumnName("Tenant");

         this.Property(t => t.EntityId).HasColumnName("EntityId");
         this.Property(t => t.ContactId).HasColumnName("ContactId");

         this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId");
         this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            
       //  this.Property(t => t.IsException).HasColumnName("IsException");


            // Relationships
            this.HasRequired(t => t.Contact)
             .WithMany()
             .HasForeignKey(d => d.ContactId);


         this.HasRequired(t => t.ObjectTable)
             .WithMany()
            .HasForeignKey(d => d.ObjectTableId);

     }


    }
}
