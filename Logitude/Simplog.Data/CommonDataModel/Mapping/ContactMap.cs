using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class ContactMap : EntityTypeConfiguration<Contact>
    {
        public ContactMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.EnglishName).IsRequired().HasMaxLength(60).IsUnicode(false);
            this.Property(t => t.Email).HasMaxLength(70).IsUnicode(false);
            this.Property(t => t.BusinessPhone).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.Mobile).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.Fax).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.Notes).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.LocalName).HasMaxLength(100).IsUnicode(true);
            this.Property(t => t.FacebookId).HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.SearchFields).HasMaxLength(1000).IsUnicode(true);
            this.Property(t => t.ImageDetailId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.UserType).IsRequired().HasMaxLength(1).IsUnicode(false);
            this.Ignore(t => t.Name); this.Ignore(t => t.Roles);
            this.Property(t => t.ContactDoneMethodCode).HasMaxLength(2).IsUnicode(false);
            this.Property(t => t.IndexColor);
            this.Property(t => t.Position).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.ComputedKey).HasMaxLength(70).IsUnicode(false);
            this.Property(t => t.CompanyName).HasMaxLength(1000).IsUnicode(false);
            this.Property(t => t.ExternalId).HasMaxLength(20).IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("Contacts");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.EnglishName).HasColumnName("EnglishName");
            this.Property(t => t.Email).HasColumnName("Email");        
            this.Property(t => t.BusinessPhone).HasColumnName("BusinessPhone");
            this.Property(t => t.Mobile).HasColumnName("Mobile");
            this.Property(t => t.Fax).HasColumnName("Fax");
            this.Property(t => t.Birthday).HasColumnName("Birthday");
            this.Property(t => t.Anniversary).HasColumnName("Anniversary");
            this.Property(t => t.Notes).HasColumnName("Notes");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.LocalName).HasColumnName("LocalName");
            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t => t.FacebookId).HasColumnName("FacebookId");            
            this.Property(t => t.Signature).HasColumnName("Signature");
            this.Property(t => t.SignatureHtml).HasColumnName("SignatureHtml");   
            
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.DisplayGettingStarted).HasColumnName("DisplayGettingStarted");
            this.Property(t => t.ImageDetailId).HasColumnName("ImageDetailId");
            this.Property(t => t.UserType).HasColumnName("UserType");
            this.Property(t => t.DontShowLocalLabels).HasColumnName("DontShowLocalLabels");
            this.Property(t => t.BirthdayReminder).HasColumnName("BirthdayReminder");
            this.Property(t => t.DigitalPortalLanguage).HasColumnName("DigitalPortalLanguage");
            this.Property(t => t.AnniversaryReminder).HasColumnName("AnniversaryReminder");
            this.Property(t => t.DoneDate).HasColumnName("DoneDate");
            this.Property(t => t.BirthDayOfYear).HasColumnName("BirthDayOfYear");
            this.Property(t => t.ContactDoneMethodCode).HasColumnName("ContactDoneMethodCode");
            this.Property(t => t.IndexColor).HasColumnName("IndexColor");        
            this.Property(t => t.Position).HasColumnName("Position");
            this.Property(t => t.ComputedKey).HasColumnName("ComputedKey");
            this.Property(t => t.CompanyName).HasColumnName("CompanyName");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.ExternalId).HasColumnName("ExternalId");
            this.Property(t => t.AutomaticLastUpdateDate).HasColumnName("AutomaticLastUpdateDate");

            // Relationships
            this.HasOptional(t => t.ColorIndex).WithMany().HasForeignKey(d => d.IndexColor);
        }
    }
}
