using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.QuoteModel.EntityLists
{
    public class QuoteComputedFieldList
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public bool ConnectedToShipment { get; set; }
        public bool ConnectedToTicket { get; set; }
        public string ToLocation { get; set; }
        public string FromLocation { get; set; }
        public string DeliveryTo { get; set; }
        public string PickupFrom { get; set; }
        public double? EstimatedPayablesInSales { get; set; }
        public double? EstimatedPayablesInLocal { get; set; }
        public double? EstimatedReceivablesInLocal { get; set; }
        public double? EstimatedReceivablesInSales { get; set; }
        public double? MarkupPercentage { get; set; }

    }
}
