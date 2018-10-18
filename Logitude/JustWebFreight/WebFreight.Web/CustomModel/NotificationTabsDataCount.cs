using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebFreight.Web.CustomModel
{
    public class NotificationTabsDataCount
    {
        [Key]
        public string Id { get; set; }
        public int AllCount {get; set;}
        public int CreateDateCount {get; set;}
        public int DueDateCount { get; set; }
       
    }
}