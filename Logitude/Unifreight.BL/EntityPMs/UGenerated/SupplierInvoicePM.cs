using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial class SupplierInvoicePM : EntityPM
    {      
        public int FILENO { get; set; }

        public int LINENO { get; set; }

        public string ACCOUNTTYPE { get; set; }
        
        public double? CHANGINGVALUE { get; set; }
        
        public double? COMMISSION { get; set; }
        
        public string COUNTRYID { get; set; }
        
        public string CURRENCYID { get; set; }
        
        public string DECLARATIONNO { get; set; }
        
        public string INCOTERMID { get; set; }

        public bool? MAINACCOUNT { get; set; }
        
        public string SUPPLIERACCOUNT { get; set; }
        
        public string SUPPLIERID { get; set; }
        
        public double? VALUE { get; set; }

        public string SUPPLIERACCOUNTN { get; set; }

        public double? COMMISSIONPERCENT { get; set; }
        public int Tenant { get; set; }


        public bool IS_SYNCH { get; set; }

        public DateTime? LAST_UPDATE_DT { get; set; }
        public string COUNTRYIDN { get; set; }
        public string CURRENCYIDN { get; set; }
        public string INCOTERMIDN { get; set; }


        #region Ext


        private List<SupplierInvoiceItem103PM> _DeletedSupplierInvoiceItems103s;

        public List<SupplierInvoiceItem103PM> DeletedSupplierInvoiceItems103s
        {
            get { return _DeletedSupplierInvoiceItems103s = _DeletedSupplierInvoiceItems103s?? new List<SupplierInvoiceItem103PM>(); }
            set { _DeletedSupplierInvoiceItems103s = value; }
        }

        List<SupplierInvoiceItem103PM> _SupplierInvoiceItems103s;

        public List<SupplierInvoiceItem103PM> SupplierInvoiceItems103s
        {
            get { return _SupplierInvoiceItems103s = _SupplierInvoiceItems103s??new List<SupplierInvoiceItem103PM> () ; }
            set { _SupplierInvoiceItems103s = value; }
        }

        public int SupplierInvoiceItems103LastLine { get; set; }

        #endregion

        public LastLine105PM LastLine105PM { get; set; }
    }
}
