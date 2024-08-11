using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Simplog.Data.InvoiceModel.Mapping
{
    public class QuickbooksSyncRequestTicketMap : EntityTypeConfiguration<QuickbooksSyncRequestTicket>
    {
        public QuickbooksSyncRequestTicketMap()
        {
            this.HasKey(d => d.Ticket);

            this.Property(d => d.Ticket)
                .HasMaxLength(50)
                .IsRequired()
                .IsUnicode(false);

            this.Property(d => d.ReferenceNumber)
                .HasMaxLength(20)
                .IsUnicode(false);

            this.Property(d => d.UserName)
                .HasMaxLength(140)
                .IsUnicode(true);

            this.Property(d => d.Password)
                .HasMaxLength(40)
                .IsUnicode(false);


            this.ToTable("QuickbooksSyncRequestTickets");
            this.Property(t => t.Ticket).HasColumnName("Ticket");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.Password).HasColumnName("Password");
            this.Property(t => t.ExternalTablesRequestCount).HasColumnName("ExternalTablesRequestCount");
            this.Property(t => t.RequestCount).HasColumnName("RequestCount");
            this.Property(t => t.ReferenceNumber).HasColumnName("ReferenceNumber");
            this.Property(t => t.IsCurrentInvoiceChecked).HasColumnName("IsCurrentInvoiceChecked");



                

                
        }
    }
}
