using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Global.Data.GlobalModel.EntityPOCOs
{
    public class AutoSignupEmail
    {
        [Key]
        public string Id { get; set; }
        public string EmailBody { get; set; }
        public string EmailSubject { get; set; }
        public string Status { get; set; }
        public int Retries { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
