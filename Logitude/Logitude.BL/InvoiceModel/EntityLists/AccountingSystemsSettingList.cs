using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Logitude.BL.InvoiceModel.EntityLists
{
    public class AccountingSystemsSettingList
    {
        [Key]

        public string Id { get; set; }
        public int Tenant { get; set; }
        public int GetExternalCodeInterval { get; set; }
        public bool UpdateOnNextRequest { get; set; }

    }
}