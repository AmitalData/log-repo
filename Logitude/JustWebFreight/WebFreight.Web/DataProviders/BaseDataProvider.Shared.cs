
using Logitude.BL.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;

namespace WebFreight.Web.DataProviders
{
    public class BaseDataProvider
    {
        public DateTime Today_DateTime { get; set; }
        public byte[] Logo { get; set; }
        public string Address { get; set; }
        public string GeneralAddress { get; set; }
        public string CompanyName { get; set; }
        public string InvoicePrintNotes { get; set; }
        public string InvoicePrintNotesLocal { get; set; }
    }
}