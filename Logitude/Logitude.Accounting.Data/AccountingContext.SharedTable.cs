using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.Data
{
    public partial class AccountingContext : DbContextBase, IAccountingContext
    {
        public IDbSet<VatTypePercentage> VatTypePercentages { get; set; }

        public IDbSet<Card> Cards { get; set; }
        public IDbSet<Customer> Customers { get; set; }
        public IDbSet<Contact> Contacts { get; set; }
        public IDbSet<Tenant> Tenants { get; set; }
    }
    public partial interface IAccountingContext : IContext
    {
        IDbSet<VatTypePercentage> VatTypePercentages { get; set; }

        IDbSet<Card> Cards { get; set; }
        IDbSet<Customer> Customers { get; set; }
        IDbSet<Tenant> Tenants { get; set; }
        IDbSet<Contact> Contacts { get; set; }
    }
}
