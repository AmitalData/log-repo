using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;

namespace AmitalCloud.Infrastructure.Domain.EntityMapping
{
    public class LogitudeLeadsMap : EntityTypeConfiguration<LogitudeLead>
    {
        public LogitudeLeadsMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Id });

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

       

            this.Property(t => t.PhoneNumber)
                .HasMaxLength(40)
                .IsUnicode(false);

                 this.Property(t => t.State)
                .HasMaxLength(40)
                .IsUnicode(false);


               this.Property(t => t.City)
                .HasMaxLength(25)
                .IsUnicode(false);


          this.Property(t => t.Street)
                .HasMaxLength(65)
                .IsUnicode(false);

                this.Property(t => t.ZipCode)
                .HasMaxLength(15)
                .IsUnicode(false);
      


            this.Property(t => t.CreateDate)
                   .IsRequired();


            this.Property(t => t.CompanyName)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(true);

            this.Property(t => t.ContactName)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(true);


            this.Property(t => t.NumberOfBranches)
                .IsRequired();

      

            this.Property(t => t.Country)
                .IsRequired()
                .HasMaxLength(120)
                .IsUnicode(false);

            this.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(70)
                .IsUnicode(false);

            this.Property(t => t.NumberOfUsers)
                .IsRequired();


            this.Property(t => t.Comments)
                .HasMaxLength(500)
                .IsUnicode(true);


            this.Property(t => t.RequestType)
             .HasMaxLength(20)
             .IsUnicode(false);

            this.Property(t => t.IsEmailVerified)
                .IsRequired();

            this.Property(t => t.TenantNumber)
                .IsRequired();

            this.Property(t => t.LastUpdateDate)
               .IsRequired();


            this.Property(t => t.IsSentToCustomer)
               .IsRequired();

            this.Property(t => t.IsUserEmailSent)
               .IsRequired();
            
            this.Property(t => t.StatusCode)
                .HasMaxLength(20)
                .IsUnicode(false);


            this.Property(t => t.IsUserOpened)
              .IsRequired();

            this.Property(t => t.CustomerId)
                .HasMaxLength(15);
          

            this.Property(t => t.OpportunityId)
                .HasMaxLength(15);


            this.Property(t => t.LeadSource)
               .HasMaxLength(60)
               .IsUnicode(false);

            this.Property(t => t.IATACode)
           .HasMaxLength(7)
           .IsUnicode(false);


            this.Property(t => t.CASSCode)
                .HasMaxLength(4)
               .IsUnicode(false);

        this.Property(t => t.PackageCode)
            .HasMaxLength(5)
            .IsUnicode(false);


        this.Property(t => t.VatNumber)
         .HasMaxLength(20)
         .IsUnicode(false);

        this.Property(t => t.ClientId)
         .HasMaxLength(100)
         .IsUnicode(false);


        this.Property(t => t.LeadOrigin)
         .HasMaxLength(100)
         .IsUnicode(false);

        this.Property(t => t.Campaign)
         .HasMaxLength(250)
         .IsUnicode(false);



            this.Property(t => t.SearchFields).HasMaxLength(1000).IsUnicode(true);

            // Table & Column Mappings
            this.ToTable("LogitudeLeads");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.PhoneNumber).HasColumnName("PhoneNumber");
            this.Property(t => t.CompanyName).HasColumnName("CompanyName");
            this.Property(t => t.ContactName).HasColumnName("ContactName");
            this.Property(t => t.NumberOfBranches).HasColumnName("NumberOfBranches");
            this.Property(t => t.Country).HasColumnName("Country");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.NumberOfUsers).HasColumnName("NumberOfUsers");
            this.Property(t => t.Comments).HasColumnName("Comments");

            this.Property(t => t.RequestType).HasColumnName("RequestType");
            this.Property(t => t.IsEmailVerified).HasColumnName("IsEmailVerified");
            this.Property(t => t.TenantNumber).HasColumnName("TenantNumber");
            this.Property(t => t.LastUpdateDate).HasColumnName("LastUpdateDate");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
      


            this.Property(t => t.IsSentToCustomer).HasColumnName("IsSentToCustomer");
            this.Property(t => t.IsUserEmailSent).HasColumnName("IsUserEmailSent");
            this.Property(t => t.StatusCode).HasColumnName("StatusCode");
            this.Property(t => t.IsUserOpened).HasColumnName("IsUserOpened");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.OpportunityId).HasColumnName("OpportunityId");


            this.Property(t => t.LeadSource).HasColumnName("LeadSource");
            this.Property(t => t.IATACode).HasColumnName("IATACode");
            this.Property(t => t.CASSCode).HasColumnName("CASSCode");
            this.Property(t => t.PackageCode).HasColumnName("PackageCode");



            this.Property(t => t.Street).HasColumnName("Street");
            this.Property(t => t.State).HasColumnName("State");
            this.Property(t => t.ZipCode).HasColumnName("ZipCode");
            this.Property(t => t.City).HasColumnName("City");
            this.Property(t => t.VatNumber).HasColumnName("VatNumber");
            this.Property(t => t.ClientId).HasColumnName("ClientId");
            this.Property(t => t.LeadOrigin).HasColumnName("LeadOrigin");
            this.Property(t => t.Campaign).HasColumnName("Campaign");
        }
    }
}
