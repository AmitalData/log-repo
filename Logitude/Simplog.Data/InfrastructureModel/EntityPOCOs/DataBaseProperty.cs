using System;
using System.ComponentModel.DataAnnotations;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class DataBaseProperty
    {
        [Key]
        public int DataBaseNumber { get; set; }

        public DateTime? LastBackupDate { get; set; }
    }
}