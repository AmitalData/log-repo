using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.DataContract
{
   public class OpenFormatReportData
    {

        public List<B100Data> B100DataList;
        public List<B110Data> B100Data;
        public List<AddressData> AddressDataList;


    }

    public class B100Data
    {
        public int Counter { get; set; }
        public string JournalNumber { get; set; }
        public int JournalLineNumber { get; set; }
        public string AccountingEntityReference { get; set; }
        public string AccountingEntityCode { get; set; }
        public string Reference2 { get; set; }
        public string Notes { get; set; }
        public DateTime AccountingDate { get; set; }
        public DateTime DocumentDate { get; set; }
        public string GLAccountDisplayNumber { get; set; }
        public decimal LocalAmountDebit { get; set; }
        public string CurrencyId { get; set; }
        public decimal LocalAmountCredit { get; set; }
        public decimal ForeignAmountDebit { get; set; }
        public decimal ForeignAmountCredit { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedByUser { get; set; }




    }

    public class B110Data
    {

        public string ChartOfAccountsCode { get; set; }
        public string DisplayNumber { get; set; }
        public string LocalName { get; set; }
        public string EnglishName { get; set; }
        public string ChartOfAccountsName { get; set; }
        public string AccountTypeCode { get; set; }
        public string CurrencyCode { get; set; }
        public string GLAccountId { get; set; }
        public string CardId { get; set; }
        public string VatNumber { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string AddressType { get; set; }
        public string BillingAddress { get; set; }
        public string MainAddress { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public decimal? OpeningBalance { get; set; }
        public decimal? TotalDebit { get; set; }
        public decimal? TotalCredit { get; set; }
        public bool? IsMultiCurrency { get; set; }
        public string CurrecnyId { get; set; }
        public decimal? OpeningBalanceInForegnCurrency { get; set; }



    }

    public class AddressData
    {
        public string GLAccountId { get; set; }
        public string CardId { get; set; }
        public string VatNumber { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string AddressType { get; set; }
        public string MainAddress { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
    }

}
