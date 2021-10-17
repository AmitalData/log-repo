using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class EntityStatus
    {

        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Name { get; set; }
        public string ObjectTableId { get; set; }
        public int StatusWeight { get; set; }
        public string Code { get; set; }
        public bool InActive { get; set; }
        //public int IndexOrder { get; set; }
        public string SearchFields { get; set; }
        public string DisplayName { get; set; }
        public DateTime? AutomaticLastUpdateDate { get; set; }
        [ForeignKey("ObjectTableId")]
        public virtual ObjectTable ObjectTable { get; set; }
        public int? StatusLocalWeight { get; set; }
        public string EntityStatusTypeCode { get; set; }

        [ForeignKey("EntityStatusTypeCode")]
        public virtual EntityStatusType EntityStatusType { get; set; }
        //   public List<EventType> EventTypes { get; set; }
        // public List<Shipment> Shipments { get; set; }
        // public List<Quote> Quotes { get; set; }
        // public List<ShipmentMasterData> ShipmentMasterDatas { get; set; }

    }
}