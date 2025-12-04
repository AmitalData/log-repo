
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.QuoteModel.APIDataContract.ApiV1; 
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using System.Xml.Serialization;

namespace Logitude.Accounting.BL.APIDataContract.ApiV1
{

    public partial class JournalLine
    {


        public string JournalNumber { get; set; }

        public int Line { get; set; }

        public int Tenant { get; set; }

        public string DebitControlAccount { get; set; }

        public string DebitAccount { get; set; }

        public string CreditControlAccount { get; set; }

        public string CreditAccount { get; set; }

        public DateTime DocumentDate { get; set; }

        public DateTime AccountingDate { get; set; }

        public DateTime DueDate { get; set; }

        public decimal LocalAmount { get; set; }

        public Currency Currency { get; set; }

        public decimal ForeignAmount { get; set; }

        public decimal? ExchangeRate { get; set; }

        public string Reference1 { get; set; }

        public string Reference2 { get; set; }

        public string Reference3 { get; set; }

        public string Notes { get; set; }

        public decimal? ExternalOpenAmount { get; set; }

        public string ActionCode { get; set; }

        public string ExternalReconcileNumber { get; set; }

        public string ConfirmationNumber { get; set; }

        public bool? ExcludeFromTaxReport { get; set; }
    }
} 