using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.SystemLogs.POCOs
{
  public  class FailedLoginLog
    {
        [Key]
        public string Id { get; set; }
        public string IP { get; set; }
        public string Browser { get; set; }
        public string Email { get; set; }
        public DateTime? GMTDateTime { get; set; }
        public string UserAgent { get; set; }
        public string Reason { get; set; }
        

    }
}
