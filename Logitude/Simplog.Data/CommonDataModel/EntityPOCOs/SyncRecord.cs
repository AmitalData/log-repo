using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    [Table("SyncRecord")]
    public class SyncRecord
    {
        [Key]
        [Column("Id")]
        public string Id { get; set; }
        [Column("Tenant")]
        public int Tenant { get; set; }
        [Column("Entname")]
        public string Entname { get; set; }
        [Column("KeyVal")]
        public string KeyVal { get; set; }
        [Column("FileNo")]
        public string FileNo { get; set; }
        [Column("TrigAction")]
        public string TrigAction { get; set; }
        [Column("CreateDate")]
        public DateTime CreateDate { get; set; }
        [Column("SyncDT")]
        public DateTime? SyncDT { get; set; }
        [Column("IsSync")]
        public int IsSync { get; set; }
    }
}