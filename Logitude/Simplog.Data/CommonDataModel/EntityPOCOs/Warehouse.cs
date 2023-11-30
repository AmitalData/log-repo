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

        public bool ChargeStorage { get; set; }
        public string CurrencyId { get; set; }
        public string AirWeightMeasurementCode { get; set; }
        public string OceanWeightMeasurementCode { get; set; }
        public string InlandWeightMeasurementCode { get; set; }
        public string AirWeightRoundingCode { get; set; }
        public string OceanWeightRoundingCode { get; set; }
        public string InlandWeightRoundingCode { get; set; }
        
        [ForeignKey("TypeCode")]
        public virtual WarehouseType WarehouseType { get; set; }

        public virtual Card Card { get; set; }

        public virtual Currency Currency { get; set; }
        public virtual WarehouseWeightMeasurement AirWeightMeasurement { get; set; }
        public virtual WarehouseWeightMeasurement OceanWeightMeasurement { get; set; }
        public virtual WarehouseWeightMeasurement InlandWeightMeasurement { get; set; }
        public virtual WarehouseWeightRounding AirWeightRounding { get; set; }
        public virtual WarehouseWeightRounding OceanWeightRounding { get; set; }
        public virtual WarehouseWeightRounding InlandWeightRounding { get; set; } 
        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public string Field3 { get; set; }
        public string Field4 { get; set; }
        public string Field5 { get; set; }
        public string Field6 { get; set; }
        public string Field7 { get; set; }
        public string Field8 { get; set; }
        public string Field9 { get; set; }
        public string Field10 { get; set; } 

    }
}