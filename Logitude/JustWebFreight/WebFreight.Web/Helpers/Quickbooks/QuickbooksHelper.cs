using Logitude.BL.InvoiceModel.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace WebFreight.Web.Helpers.Quickbooks
{
    public class QuickbooksHelper
    {

        private string Tenant;
        private ARInvoiceHelper service;


       public QuickbooksHelper(string Tenant) {
            this.Tenant = Tenant;

            int tenantInt = 0;
            Int32.TryParse(Tenant, out tenantInt);
            service = new ARInvoiceHelper(tenantInt);
        }

        public List<Intuit.Ipp.Data.Customer> GetQuickBooksOnlineCustomersByText(String sql)
        {
            try
            {
                var myResult = service.GetQuickBooksOnlineCustomersByText(sql, Tenant);
                return myResult;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }

        public string QBOSQL(string CardName, string SearchField, string SearchText, bool ReceivableCard, bool PayableCard, string LogitudeCardName)
        {
            string query = "";

            if (SearchText.Contains('&'))
            {
                SearchText.Replace('&', '%');
            }

            if (ReceivableCard)
                query = "Select * from Customer where displayName like '%" + SearchText + "%' STARTPOSITION 1 MAXRESULTS 21 ";
            else if (PayableCard)
                query = "Select * from Vendor where displayName like '%" + SearchText + "%' STARTPOSITION 1 MAXRESULTS 21 ";
            else if (LogitudeCardName == "ChargesType")
            {
                query = "Select * from " + CardName + " where " + SearchField + " like '%" + SearchText + "%' STARTPOSITION 1 MAXRESULTS 21 ";

            }
            else if (CardName == "AccountBankCredit")
            {
                query = "Select * from account where AccountType IN('Bank' ,'Credit Card') And Name like '%" + SearchText + "%' STARTPOSITION 1 MAXRESULTS 21 ";
            }
            else if (CardName == "PaymentMethod")
            {
                query = "Select * from PaymentMethod where Name like '%" + SearchText + "%' STARTPOSITION 1 MAXRESULTS 21 ";
            }
            else
            {
                query = "Select * from " + CardName;
            }
            return query;
        }

        public List<Intuit.Ipp.Data.TaxCode> GetQuickBooksOnlineVatTypesByText(String sql)
        {
            try
            {
                var myResult = service.GetQuickBooksOnlineVatTypesByText(sql, Tenant);
                return myResult;
            }

            catch (Exception ex)
            {

                throw new ApplicationException(ex.ToString());

            }


        }


        public List<Intuit.Ipp.Data.Item> GetQuickBooksOnlineReceivableChargesTypesByText(String sql)
        {
            try
            {
                var myResult = service.GetQuickBooksOnlineReceivableChargesTypesByText(sql, Tenant);
                return myResult;
            }

            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }


        }


        public List<Intuit.Ipp.Data.Account> GetQuickBooksOnlinePayablesChargesTypesByText(String sql)
        {
            try
            {
                var myResult = service.GetQuickBooksOnlinePayablesChargesTypesByText(sql, Tenant);
                return myResult;
           }

            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }



        public List<Intuit.Ipp.Data.CompanyCurrency> GetQuickBooksOnlineCurrenciesByText(String sql)
        {
            try
            {
                var myResult = service.GetQuickBooksOnlineCurrenciesByText(sql,Tenant);
                return myResult;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }



        public List<Intuit.Ipp.Data.Term> GetQuickBooksOnlinePaymentTermsByText(String sql)
        {
            try
            {
                var myResult = service.GetQuickBooksOnlinePaymentTermsByText(sql,Tenant);
                return myResult;
            }

            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }


        public List<Intuit.Ipp.Data.Vendor> GetQuickBooksOnlineVendorByText(String sql)
        {
            try
            {
                var myResult = service.GetQuickBooksOnlineVendorByText(sql,Tenant);
                return myResult;
            }

            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }


        public List<Intuit.Ipp.Data.PaymentMethod> GetQuickBooksOnlinePaymentMethodsByText(String sql)
        {
            try
            {
                var myResult = service.GetQuickBooksOnlinePaymentMethodByText(sql,Tenant);
                return myResult;

            }

            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }
        
    }
}