using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Amital.QuoteOPM.Def.EntityPMs
{
    public class QuoteStatus
    {
        public string QuoteNumber { get; set; }
        public bool? IsQuoteCancel { get; set; }
        public string QuoteCancelNote { get; set; }
        public string QuoteAcceptNote { get; set; }
        public string QuoteDeclineNote { get; set; }
        public DateTime? QuoteCancelDate { get; set; }
        public DateTime? QuoteAcceptDate { get; set; }
        public DateTime? QuoteDeclineDate { get; set; }

        public QuoteDeclineReason QuoteDeclineReason { get; set; }
        public Stage Stage { get; set; }
        public DateTime? DueDate { get; set; }

    }

    public class QuoteDeclineReason
    {
        [XmlAttribute]
        public string Code { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
    }


    public class Stage
    {
        [XmlAttribute]
        public string Id { get; set; }

        [XmlAttribute]
        public string Code { get; set; }

        public string Name { get; set; }
        public DateTime? StageDate { get; set; }


    }
}