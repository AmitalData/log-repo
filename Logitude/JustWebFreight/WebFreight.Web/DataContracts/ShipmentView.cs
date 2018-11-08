using System;
using System.ComponentModel.DataAnnotations;

namespace WebFreight.Web.DataContracts
{
    public class ShipmentView 
    {
      
        public string Id { get; set; }
        public string TransportModeId { get; set; }
        public string DirectionId { get; set; }
        public string ShipmentNumber { get; set; }
        public DateTime OpenDate { get; set; }
        public string Client { get; set; }
        public string ClientAbroad { get; set; }
        public string FromPort { get; set; }
        public string FromPortName { get; set; }
        public string FromPortCountry { get; set; }

        public string ToPort { get; set; }
        public string ToPortName { get; set; }
        public string ToPortCountry { get; set; }

        public string HAWBFBLBL { get; set; }
        public string ShipmentType { get; set; }
        
        public string FollowUpType { get; set; }
        public string FollowUpId { get; set; }
        public DateTime? FollowUpDate { get; set; }
        [Key]
        public string ShipmentViewId { get; set; }
        public string  BasketId { get; set; }
        public bool IsClosed { get; set; }
        public DateTime LastUpdate { get; set; }
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
        public string Field11 { get; set; }
        public string Field12 { get; set; }
        public string Field13 { get; set; }
        public string Field14 { get; set; }
        public string Field15 { get; set; }
        public string Field16 { get; set; }
        public string Field17 { get; set; }
        public string Field18 { get; set; }
        public string Field19 { get; set; }
        public string Field20 { get; set; }
        public string Field21 { get; set; }
        public string Field22 { get; set; }
        public string Field23 { get; set; }
        public string Field24 { get; set; }
        public string Field25 { get; set; }
        public string Field26 { get; set; }
        public string Field27 { get; set; }
        public string Field28 { get; set; }
        public string Field29 { get; set; }
        public string Field30 { get; set; }
        public string Field31 { get; set; }
        public string Field32 { get; set; }
        public string Field33 { get; set; }
        public string Field34 { get; set; }
        public string Field35 { get; set; }
        public string Field36 { get; set; }
        public string Field37 { get; set; }
        public string Field38 { get; set; }
        public string Field39 { get; set; }
        public string Field40 { get; set; }

        public bool NewMessage { get; set; }
        
        public string FollowUpNotes { get; set; }
        public bool IsAnyConversation { get; set; }
        //public Basket Basket { get; set; }
        public int NumberOfShipments { get; set; }
        //public DateTime? MainCarriageETA 
        //{
        //    get 
        //    {
        //        if (Master != null)
        //        {
        //            return Master.MainCarriageETA;
        //        }
        //        else
        //        {
        //            return null;
        //        }

        //    }
            
        //}
        public double? ChargeableWeight { get; set; }
        public string ClientReference1 { get; set; }
        //public CustomField CustomField { get; set; }
        public byte[] LastModified { get; set; }
        public bool hasChanges { get; set; }


        //[Include]
        //[Association("Mohammad","Id","Id",IsForeignKey=true)]
        //public Shipment Shipment { get; set; }



    }
}