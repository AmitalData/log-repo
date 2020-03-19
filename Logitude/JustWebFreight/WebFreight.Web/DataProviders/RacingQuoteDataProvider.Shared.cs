using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebFreight.Web.DataProviders
{

    public class RacingQuoteDataProvider : BaseDataProvider
    {
        [Key]
    public int Id { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public List<RacingQuoteItem> Quotes { get; set; }
}

public class RacingQuoteItem
    {
        public string QuoteNumber { get; set; }
        public string Salesman { get; set; }
        public string TransportMode { get; set; }
        public string Direction { get; set; }
        public string Type { get; set; }
        public string CustomerType { get; set; }
        public string Customer { get; set; }
        public string Notify { get; set; }   
        public string OpenedBy { get; set; }
        public DateTime? OpenDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? LastActivityDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public DateTime? SentDate { get; set; }
        public DateTime? AcceptedDate { get; set; }
        public DateTime? DeclinedDate { get; set; }
        public string QuoteNotes { get; set; }
        public string Stage { get; set; }
        public string ClosingReason { get; set; }
        public string QuoteSubject { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string FromPortCountry { get; set; }
        public string ToPortCountry { get; set; }
        public string Counter { get; set; }

        public string QuoteField1 { get; set; }
        public string QuoteField2 { get; set; }
        public string QuoteField3 { get; set; }
        public string QuoteField4 { get; set; }
        public string QuoteField5 { get; set; }
        public string QuoteField6 { get; set; }
        public string QuoteField7 { get; set; }
        public string QuoteField8 { get; set; }
        public string QuoteField9 { get; set; }
        public string QuoteField10 { get; set; }
        public string QuoteField11 { get; set; }
        public string QuoteField12 { get; set; }
        public string QuoteField13 { get; set; }
        public string QuoteField14 { get; set; }
        public string QuoteField15 { get; set; }
        public string QuoteField16 { get; set; }
        public string QuoteField17 { get; set; }
        public string QuoteField18 { get; set; }
        public string QuoteField19 { get; set; }
        public string QuoteField20 { get; set; }

    }
}