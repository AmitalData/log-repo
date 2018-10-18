using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataContracts
{
    public class BookingsDataCounts
    {
        [Key]
        public int Id { get; set; }
        public int CreatedBookingsCount { get; set; }
        public int WaitingForResponseCount { get; set; }
        public int ConfirmedBookingsCount { get; set; }
        public int RejectedBookingsCount { get; set; }
        public int InProgressBookingsCount { get; set; }
        public int AllBookingsCount { get; set; }
        public int CancelledBookingsCount { get; set; }
    }
}