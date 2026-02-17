using Logitude.SystemLogs.POCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.SystemLogs.Mapping
{
    public class BatchServicesLogMap : EntityTypeConfiguration<BatchServicesLog>
    {

        public BatchServicesLogMap()
        {
            this.HasKey(t => t.Id);

            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.BatchServiceCode)
                 .IsRequired()
                .HasMaxLength(40)
                
                .IsUnicode(false);

            this.Property(t => t.LastActivity);
           


            this.Property(t => t.CPU);

            this.Property(t => t.CreateDate);

            this.Property(t => t.NumberOfDoneItems);

            this.ToTable("BatchServicesLogs");

            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.BatchServiceCode).HasColumnName("BatchServiceCode");
            this.Property(t => t.LastActivity).HasColumnName("LastActivity");
            this.Property(t => t.NumberOfDoneItems).HasColumnName("NumberOfDoneItems");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.CPU).HasColumnName("CPU");
            this.Property(t => t.DoneItemsInOneMinute).HasColumnName("DoneItemsInOneMinute");
            this.Property(t => t.DoneItemsInOneHour).HasColumnName("DoneItemsInOneHour");
            this.Property(t => t.DoneItemsInFiveMinutes).HasColumnName("DoneItemsInFiveMinutes");







        }

          


    }
}
