using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class CardSearch
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public DateTime RecordDate { get; set; }
        public string Keyword { get; set; }
        public string CardId { get; set; }
        public int Weight { get; set; }
        public string PartnerTypeId { get; set; }
        public bool InActive { get; set; }
        public bool IsCustomer { get; set; }


        [ForeignKey("CardId")]
        public Card Card { get; set; }


    }
}
