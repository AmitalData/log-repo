using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Server.Tools.Counters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WebFreight.Web.Controllers.WebServices.Services
{
    public class AllocateInvoiceService
    {
        ShaamService shaamService = new ShaamService();

        public HttpClienResponse CreateConfirmationNumber(string invoiceJson, int tenant, bool testEnvironment)
        {
            ConfirmationNumberTokenLogRepository confirmationNumberTokenLogRepository = new ConfirmationNumberTokenLogRepository(tenant);
            InvoiceRequest invoice = JsonConvert.DeserializeObject<InvoiceRequest>(invoiceJson);
            string confirmationTokenLogId = IdCounter.GetNumber("Customs.ConfirmationNumberTokenLog", tenant);
            string communicationLogId = IdCounter.GetNumber("CommunicationLog", tenant);

            confirmationNumberTokenLogRepository.Add(new ConfirmationNumberTokenLog()
            {
                Id = confirmationTokenLogId,
                Tenant = tenant,
                CreateDate = DateTime.Now,
                InvoiceNumber = invoice.Invoice_ID,
                CallType = "confirmation",
                CommunicationType = 1,
                CompanyIdInvoiceProducer = invoice.Vat_Number.ToString(),
                CompanyIdInvoiceRecipient = invoice.Customer_VAT_Number.ToString(),
                CommunicationLogId = communicationLogId,
                SearchFields = invoice.Vat_Number.ToString() + "," + invoice.Customer_VAT_Number.ToString() + "," + invoice.Invoice_ID
            });
            confirmationNumberTokenLogRepository.SubmitChanges();

            HttpClienResponse apiToShaamRes = shaamService.CreateConfirmationNumber(invoiceJson, tenant, confirmationTokenLogId, communicationLogId, testEnvironment);

            return apiToShaamRes;
        }
    }

    public class InvoiceRequest
    {
        public string Invoice_ID { get; set; }
        public int Invoice_Type { get; set; }
        public int Vat_Number { get; set; }
        public int Union_Vat_Number { get; set; }
        public string Invoice_Reference_Number { get; set; }
        public int Customer_VAT_Number { get; set; }
        public string Customer_Name { get; set; }
        public string Invoice_Date { get; set; }
        public string Invoice_Issuance_Date { get; set; }
        public string Branch_ID { get; set; }
        public int Accounting_Software_Number { get; set; }
        public string Client_Software_Key { get; set; }
        public double Amount_Before_Discount { get; set; }
        public double Discount { get; set; }
        public double Payment_Amount { get; set; }
        public double VAT_Amount { get; set; }
        public double Payment_Amount_Including_VAT { get; set; }
        public string Invoice_Note { get; set; }
        public int Action { get; set; }
        public int Vehicle_License_Number { get; set; }
        public string Phone_Of_Driver { get; set; }
        public string Arrival_Date { get; set; }
        public string Estimated_Arrival_Time { get; set; }
        public int Transition_Location { get; set; }
        public string Delivery_Address { get; set; }
        public int Additional_Information { get; set; }
        public List<Item> Items { get; set; } = new List<Item>();

        public class Item
        {
            public int Index { get; set; }
            public string Catalog_ID { get; set; }
            public int Category { get; set; }
            public string Description { get; set; }
            public string Measure_Unit_Description { get; set; }
            public double Quantity { get; set; }
            public double Price_Per_Unit { get; set; }
            public double Discount { get; set; }
            public double Total_Amount { get; set; }
            public double VAT_Rate { get; set; }
            public double VAT_Amount { get; set; }
        }
    }
}