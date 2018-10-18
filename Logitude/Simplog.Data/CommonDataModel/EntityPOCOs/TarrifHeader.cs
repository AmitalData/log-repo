using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class TarrifHeader
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string CardId { get; set; }
        public string TarrifTypeCode { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool InActive { get; set; }
        public string Notes { get; set; }
        public string TransitTimeNotes { get; set; }
        public DateTime CreateDate { get; set; }

        [ForeignKey("CardId")]
        public virtual Card Card { get; set; }
        [ForeignKey("TarrifTypeCode")]
        public virtual TarrifType TarrifType { get; set; }

        //public List<TarrifCharge> TarrifCharges { get; set; }

        //public List<TarrifFromTo> TarrifFromToes { get; set; }
        //public List<TarrifStep> TarrifSteps { get; set; }
    }
}