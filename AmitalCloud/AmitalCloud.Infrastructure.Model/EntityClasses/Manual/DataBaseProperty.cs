using System;
using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class DataBaseProperty
    {
        [Key]
        public int DataBaseNumber { get; set; }

        public DateTime? LastBackupDate { get; set; }
    }
}