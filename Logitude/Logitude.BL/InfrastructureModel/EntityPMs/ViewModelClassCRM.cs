using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{
    public class ViewModelClassCRM
    {
        private static int counter = 0;
        public ViewModelClassCRM()
        {
            linePrimary = ++counter;
        }

        [Key]
        public int linePrimary { get; set; }

        public int total { get; set; }
        public int month { get; set; }
        public int year { get; set; }
        public string transportMode { get; set; }        
        public string country { get; set; }
        public string direction { get; set; }

        public string shipmentTypeId { get; set; }
        public double? sumChargeableWeight { get; set; }
        public double? sumGrossWeight { get; set; }

        public string CustomerName { get; set; }
        public double sumOfTotals { get; set; }
    }


    public class DashBoardClass
    {
        private static int counter = 0;
        public DashBoardClass()
        {
            linePrimary = ++counter;
        }

        [Key]
        public int linePrimary { get; set; }
        public int day { get; set; }
        public int month { get; set; }
        public int year { get; set; }
        public string directionID { get; set; }
        public string transportModeID { get; set; }
        public DateTime? FullDate { get; set; }
        public string countryCode { get; set; }
        public string countryName { get; set; }
        public string country { get; set; }

        public string shipmentTypeId { get; set; }

        public double total { get; set; }
        public double? sumChargeableWeight { get; set; }
        public double? sumGrossWeight { get; set; }

        public double totalLastMonth { get; set; }
        public double? sumChargeableWeightLastMonth { get; set; }
        public double? sumGrossWeightLastMonth { get; set; }

        public string CustomerID { get; set; }
        public string CustomerName { get; set; }

        public string XField { get; set; }
        public int YField { get; set; }
        public DateTime Date { get; set; }
        public bool IsExport { get; set; }
        public string TransportModeName { get; set; }
        public string DirectionName { get; set; }
        public DateTime StartOfTheWeek { get; set; }
        public DateTime EndOfTheWeek { get; set; }
        public string DateRange { get; set; }
        public double? totalProfitInLocalCurrency { get; set; }
        public double? totalProfitInProfitCurrency { get; set; }
        public double? ReceivablesInLocalCurrency { get; set; }
        public double? ReceivablesInProfitCurrency { get; set; }
        public double GeneralTotal { get; set; }
    }

    public class MoneyStatusClass
    {
        private static int counter = 0;
        public MoneyStatusClass()
        {
            linePrimary = ++counter;
        }
        [Key]
        public int linePrimary { get; set; }
        public int day { get; set; }
        public int month { get; set; }
        public int year { get; set; }
        public string DateRange { get; set; }
        public double? TotalAmount { get; set; }
        public string TotalAmountLabel { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Quarter { get; set; }
        public string DataType { get; set; }
        public DateTime? FullDate { get; set; }


    }

    public class SharedManifestsStatusClass
    {
        private static int counter = 0;
        public SharedManifestsStatusClass()
        {
            linePrimary = ++counter;
        }
        [Key]
        public int linePrimary { get; set; }
        public int day { get; set; }
        public int month { get; set; }
        public int year { get; set; }
        public string DateRange { get; set; }
        public double? TotalAmount { get; set; }
        public string TotalAmountLabel { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Quarter { get; set; }
        public string DataType { get; set; }

    }

    //public class BalanceData
    //{
    //    private static int counter = 0;
    //    public BalanceData()
    //    {
    //        linePrimary = ++counter;
    //    }

    //    [Key]
    //    public int linePrimary { get; set; }

    //    public string TitleTypeID { get; set; }
    //    public DateTime? CreateDateTime { get; set; }
    //    public string directionID { get; set; }
    //    public string transportID { get; set; }

    //}

    public class QuoteBalanceData
    {
         private static int counter = 0;
         public QuoteBalanceData()
        {
            linePrimary = ++counter;
        }

        [Key]
        public int linePrimary { get; set; }
        public string StatusCode { get; set; }
        public DateTime? StatusDate { get; set; }
      

    }

    public class OpenShipmentsStatisticsView
    {
        private static int counter = 0;
        public OpenShipmentsStatisticsView()
        {
            linePrimary = ++counter;
        }

        [Key]
        public int linePrimary { get; set; }

        public string directionID { get; set; }
        public string transportID { get; set; }
        public double YField { get; set; }
    }

    public class Top10GroupClass
    {
        private static int counter = 0;
        public Top10GroupClass()
        {
            linePrimary = ++counter;
        }

        [Key]
        public int linePrimary { get; set; }
        public string XField { get; set; }
        public double YField1 { get; set; }
        public double YField2 { get; set; }

        public string directionID { get; set; }
        public string transportID { get; set; }

    }



    public class ViewModelOpenFilesCRM
    {

        [Key]
        public int linePrimary { get; set; }
        public int total { get; set; }
        public string transportMode { get; set; }
        public string direction { get; set; }
        public string header { get; set; }
    }

    public class DebtorsClass
    {
        private static int counter = 0;
        public DebtorsClass()
        {
            linePrimary = ++counter;
        }

        [Key]
        public int linePrimary { get; set; }
        public string DebtorName { get; set; }
        public double? Amount { get; set; }
        public string AmountLabel { get; set; }
        public double? Outstanding { get; set; }
        public double? Overdue { get; set; }
        public string DebtorId { get; set; }
        public string DebtorType { get; set; }
    }

    public class CreditorsClass
    {
        private static int counter = 0;
        public CreditorsClass()
        {
            linePrimary = ++counter;
        }

        [Key]
        public int linePrimary { get; set; }
        public string CreditorName { get; set; }
        public double? Amount { get; set; }
        public double? Outstanding { get; set; }
        public double? Overdue { get; set; }
        public string CreditorId { get; set; }
        public string CreditorType { get; set; }
    }

    public class CRMDataCounts
    {
        private static int counter = 0;
        public CRMDataCounts()
        {
            linePrimary = ++counter;
        }

        [Key]
        public int linePrimary { get; set; }
        public int AllQuotes { get; set; }
        public int OpenQuotes { get; set; }
        public int AllShipments { get; set; }
        public int OpenShipments { get; set; }
    }

    public class CRMMoneyInformation
    {
        private static int counter = 0;
        public CRMMoneyInformation()
        {
            linePrimary = ++counter;
        }

        [Key]
        public int linePrimary { get; set; }
        public double OpenARInvoices { get; set; }
        public double InvoicesDue { get; set; }
        public double OpenReceivables { get; set; }
        public double ARPayments { get; set; }
    }

}