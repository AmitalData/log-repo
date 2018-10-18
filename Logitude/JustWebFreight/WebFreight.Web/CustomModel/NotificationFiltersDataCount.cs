using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebFreight.Web.CustomModel
{
    public class NotificationFiltersDataCount
    {
        [Key]
        public string Id { get; set; }
        public int OpenCount { get; set; }
        public int InfoCount { get; set; }
        public int ActionCount { get; set; }
        public int AllCount { get; set; }
        public int ClosedCount { get; set; }
        public int AllReadCount { get; set; }
        public int AllUnreadCount { get; set; }
    }
}