using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataContracts
{
    public class ContactSummary
    {
        [Key]
        public int Id { get; set; }

        public int UpcomingEventsCount { get; set; }
        public int WithoutRemindersCount { get; set; }
        public int AllContactsCount { get; set; }
    }
}