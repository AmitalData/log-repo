using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.ResponseData
{
    public class ExportDeclarationDataResponseData : ResponseDataBase
    {
        public string DeclarationID { get; set; }
        public string ReshimonNumber { get; set; }
        public string Title { get; set; }
        public string LoadingDate { get; set; }
        public string CalculationDate { get; set; }
        public string AgentCustomerExternalID { get; set; }
        public string DeclarationNumber { get; set; }
        public string FOBNetoNISAmount { get; set; }
        public string FOBNISAmount { get; set; }

        public List<Invoice> InvoiceList { get; set; }
        public List<Request> RequestList { get; set; }
    }

    public class Invoice
    {
        public string SequenceNumber { get; set; }
        public string ExternalID { get; set; }
        public string InvoiceAmountCurrency { get; set; }
        public string InvoiceAmount { get; set; }
        public string InvoiceCurrency { get; set; }
    }

    public class Request
    {
        public string SequenceNumber { get; set; }
        public string CustomsItem { get; set; }
        public string ValueQuantity { get; set; }
        public string ForeignCurrencyAmount { get; set; }
        public string ForeignAmount { get; set; }
        public string ForeignCurrency { get; set; }
        public string OriginCountry { get; set; }
        public string OriginCountryName { get; set; }
        public List<GovernmentProcedure> GovernmentProcedureList { get; set; }
        public List<Vehicle> VehicleList { get; set; }
    }

    public class GovernmentProcedure
    {
        public string ItemGovernmentProcedureType { get; set; }
        public string ItemGovernmentProcedureName { get; set; }
    }

    public class Vehicle
    {
        public string CargoIdentityQualifierID { get; set; }
        public string RichbitNumber { get; set; }
        public string VehicleExternalIDNum { get; set; }
    }

}
