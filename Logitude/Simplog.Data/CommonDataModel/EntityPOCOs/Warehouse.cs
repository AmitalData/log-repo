using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class Warehouse
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public bool AddedManually { get; set; }
        public bool MyWarehouse { get; set; }      
        public string FirmCode { get; set; }
        public string TypeCode { get; set; }
        public string PrimaryContactName { get; set; }
        public string PrimaryContactEmail { get; set; }
        public string PrimaryContactPhone { get; set; }

        [ForeignKey("TypeCode")]
        public virtual WarehouseType WarehouseType { get; set; }

        public virtual Card Card { get; set; }        
    }
}