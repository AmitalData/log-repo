 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Customs.Data.DataContracts;
using System.Data.Entity.Infrastructure;
namespace Logitude.Customs.Data.Repsitories
{
   public partial class SupplierInvioceItemCertificatRepository:IRepository<SupplierInvioceItemCertificat>
   {
        
		public List<SupplierInvioceItemCertificat> GetMulti(EntityKeyFields entityKeys)
        {

            SupplierInvoiceItemKeys supplierInvoiceItemKeys = entityKeys as SupplierInvoiceItemKeys;

            return (from a in context.SupplierInvioceItemCertificats
                    where a.DeclarationId == supplierInvoiceItemKeys.DeclarationId && a.InvoiceCounterKey == supplierInvoiceItemKeys.CounterKey && a.LineNumber == supplierInvoiceItemKeys.LineNumber
                    select a).ToList();
        }
        public List<SupplierInvioceItemCertificat> GetSupplierInvioceItemCertificatesForSupplierInvoice(string declarationId, int invoiceCounterKey,int tenant, List<int> FilterLine = null)
        {
            //return (from a in context.SupplierInvioceItemCertificats.Include("ResponseConfirmationType").Include("RequestConfirmationType").Include("CertificateExemptionType").Include("AttachmentType")
            //        where a.DeclarationId == declarationId && a.InvoiceCounterKey == invoiceCounterKey 
            //        select a).ToList();

            var q = (from a in context.SupplierInvioceItemCertificats.Include("ResponseConfirmationType").Include("RequestConfirmationType").Include("CertificateExemptionType").Include("AttachmentType")
                     where a.DeclarationId == declarationId && a.InvoiceCounterKey == invoiceCounterKey
                     select a);
            if (FilterLine != null)
            {
                q = q.Where(r => FilterLine.Contains(r.LineNumber));
            }
            return q.ToList();
        }

        public List<SupplierInvioceItemCertificat> GetSupplierInvioceItemCertificatesForSupplierInvoiceWithSpecificKeys(string declarationId, int invoiceCounterKey, List<int> itemsLineNumbers, int tenant)
        {
            return (from a in context.SupplierInvioceItemCertificats.Include("ResponseConfirmationType").Include("RequestConfirmationType").Include("CertificateExemptionType").Include("AttachmentType")
                    where a.DeclarationId == declarationId && a.InvoiceCounterKey == invoiceCounterKey && itemsLineNumbers.Contains(a.LineNumber)
                    select a).OrderBy(d => d.SequenceNumeric).ToList();
        }
        public List<SupplierInvioceItemCertificat> GetCertificatesBySearchFields(string declarationId, int invoiceCounterKey,string externalRequestTypeCode, string approvalRequestNumber, int tenant)
        {
            return (from a in context.SupplierInvioceItemCertificats.Include("ResponseConfirmationType").Include("RequestConfirmationType").Include("CertificateExemptionType").Include("AttachmentType")
                    where  a.DeclarationId == declarationId
                        && a.InvoiceCounterKey == invoiceCounterKey
                        && a.ExternalRequestTypeCode == externalRequestTypeCode 
                        && a.ApprovalRequestNumber == approvalRequestNumber
                        && a.Tenant == tenant
                    select a).ToList();
        }

        public void FastDeleteMulti(DeclarationKeys entityKeyFields)
        {

            (context as DbContextBase)
                .DeleteWhere<SupplierInvioceItemCertificat>(rec => rec.DeclarationId == entityKeyFields.Id);
        }

        public IQueryable<SupplierInvioceItemCertificat> GetTicketCertificates(string declarationId, string attachmentTypeCode, string reqConfirmationTypeCode, string CertificateExemptionTypeCode, string CertificateNumber, string ResConfirmationTypeCode, int tenant)
        {

            IQueryable<SupplierInvioceItemCertificat> certificates = (from a in context.SupplierInvioceItemCertificats.Include("ResponseConfirmationType").Include("RequestConfirmationType").Include("CertificateExemptionType").Include("AttachmentType")
                                                                      where a.DeclarationId == declarationId && a.AttachmentTypeCode == attachmentTypeCode && a.ReqConfirmationTypeCode == reqConfirmationTypeCode && a.CertificateExemptionTypeCode == CertificateExemptionTypeCode && a.CertificateNumber == CertificateNumber && a.ResConfirmationTypeCode == ResConfirmationTypeCode && a.Tenant == tenant
                                                                      select a);
            return certificates;
        }

        public IQueryable<CertificateConnectedItems> GetCertificateConnectedItems(string declarationId, string attachmentTypeCode, string reqConfirmationTypeCode, string CertificateExemptionTypeCode, string CertificateNumber, string ResConfirmationTypeCode, int tenant, string invoiceNumber, int skip, int take, bool getAll)
        {
            IQueryable<SupplierInvoiceItemsProdIdent> prodIdents = context.SupplierInvoiceItemsProdIdents.Where(d => d.TypeCode == "MN");
            IQueryable<SupplierInvoice> supplierInvoices = null;
            if (invoiceNumber != null)
            {
                supplierInvoices = (from a in context.SupplierInvoices
                                    where a.InvoiceNumber == invoiceNumber
                                    select a);
            }
            else {
                supplierInvoices = context.SupplierInvoices;
            }

           

            ICustomContext customContext = CustomContext.GetContext(tenant);
            CustomContext activeContext = customContext.GetActiveDbContext() as CustomContext;
            object[] parameters = new object[] { };
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            DbRawSqlQuery<CertificateConnectedItems> iQueryable = null;
            string cmd = "";
            if (dbms == "oracle")
            {

                string wherestring = " where SupplierInvoiceItems.DeclarationId = '" + declarationId + "'" + " and  SupplierInvoiceItems.IsParent = 0 ";
                if (!string.IsNullOrEmpty(attachmentTypeCode) && attachmentTypeCode != "null")
                {
                    wherestring += " and SupplierInvioceItemCertificats.AttachmentTypeCode = '" + attachmentTypeCode + "'";
                }
                else
                {
                    wherestring += " and SupplierInvioceItemCertificats.AttachmentTypeCode  is null ";
                }
                
                if (!string.IsNullOrEmpty(reqConfirmationTypeCode) && reqConfirmationTypeCode != "null")
                {
                    wherestring += " and SupplierInvioceItemCertificats.ReqConfirmationTypeCode = '" + reqConfirmationTypeCode + "'";
                }
                if (!string.IsNullOrEmpty(CertificateExemptionTypeCode) && CertificateExemptionTypeCode != "null")
                {
                    wherestring += " and SupplierInvioceItemCertificats.CertificateExemptionTypeCode = '" + CertificateExemptionTypeCode + "'";
                }
                if (!string.IsNullOrEmpty(CertificateNumber) && CertificateNumber != "null")
                {
                    wherestring += " and SupplierInvioceItemCertificats.CertificateNumber = '" + CertificateNumber + "'";
                }
                if (!string.IsNullOrEmpty(ResConfirmationTypeCode) && ResConfirmationTypeCode != "null")
                {
                    wherestring += " and SupplierInvioceItemCertificats.ResConfirmationTypeCode = '" + ResConfirmationTypeCode + "'";
                }
                if (!string.IsNullOrEmpty(invoiceNumber) && invoiceNumber != "null")
                {
                    wherestring += " and SupplierInvoices.InvoiceNumber = '" + invoiceNumber + "'";
                }

                cmd = (@"select SYS_GUID() as Id, SupplierInvoices.InvoiceNumber,SupplierInvoiceItems.ItemCode,SupplierInvoiceItems.ClassificationCode,
SupplierInvoiceItems.TradeAgreementCode,SupplierInvoiceItems.OriginCountryCode,
TradeAgreements.LocalName AS TradeAgreementName,
CustomsCountries.LocalName AS OriginCountryName,
SupplierInvioceItemCertificats.ItemCertificateCounterKey,SupplierInvoices.InvoiceCounterKey,SupplierInvoices.DeclarationId,SupplierInvoiceItems.LineNumber,SupplierInvoiceItems.SequenceNumeric
,SupplierInvoiceItemsProdIdents.Identification AS CatalogNumber,SupplierInvioceItemCertificats.ReqConfirmationTypeCode,SupplierInvioceItemCertificats.AttachmentTypeCode,
SupplierInvioceItemCertificats.CertificateExemptionTypeCode,SupplierInvioceItemCertificats.CertificateNumber,SupplierInvioceItemCertificats.ResConfirmationTypeCode
from supplierInvoiceItems
LEFT OUTER JOIN CustomsCountries ON SupplierInvoiceItems.OriginCountryCode = CustomsCountries.Code
 LEFT OUTER JOIN TradeAgreements ON SupplierInvoiceItems.TradeAgreementCode = TradeAgreements.Code

LEFT OUTER JOIN SupplierInvoiceItemsProdIdents on SupplierInvoiceItems.DeclarationId=SupplierInvoiceItemsProdIdents.DeclarationId 
and 
SupplierInvoiceItems.CounterKey = SupplierInvoiceItemsProdIdents.InvoiceCounterKey and SupplierInvoiceItems.LineNumber =SupplierInvoiceItemsProdIdents.InvoiceItemLineNumber


INNER JOIN SupplierInvioceItemCertificats ON SupplierInvoiceItems.DeclarationId = SupplierInvioceItemCertificats.DeclarationId AND SupplierInvoiceItems.CounterKey=SupplierInvioceItemCertificats.InvoiceCounterKey
AND SupplierInvoiceItems.LineNumber =SupplierInvioceItemCertificats. LineNumber
INNER JOIN SupplierInvoices ON SupplierInvoiceItems.DeclarationId = SupplierInvoices.DeclarationId AND SupplierInvoiceItems.CounterKey = SupplierInvoices.InvoiceCounterKey" + wherestring);//+ " Order By SupplierInvoiceItems.SequenceNumeric skip("+skip.ToString()+") "+"take("+take.ToString()+")");

            }
            else
            {
                string wherestring = " where Customs.SupplierInvoiceItems.DeclarationId = '" + declarationId + "'" + " and  Customs.SupplierInvoiceItems.IsParent = 0 "; // and Customs.SupplierInvioceItemCertificats.AttachmentTypeCode = '" + attachmentTypeCode + "'";

                if (!string.IsNullOrEmpty(attachmentTypeCode) && attachmentTypeCode!= "null")
                {
                    wherestring += " and SupplierInvioceItemCertificats.AttachmentTypeCode = '" + attachmentTypeCode + "'";
                }
                
                if (!string.IsNullOrEmpty(reqConfirmationTypeCode) && reqConfirmationTypeCode !="null")
                {
                    wherestring += " and Customs.SupplierInvioceItemCertificats.ReqConfirmationTypeCode = '" + reqConfirmationTypeCode + "'";
                }
                if (!string.IsNullOrEmpty(CertificateExemptionTypeCode) && CertificateExemptionTypeCode != "null")
                {
                    wherestring += " and Customs.SupplierInvioceItemCertificats.CertificateExemptionTypeCode = '" + CertificateExemptionTypeCode + "'";
                }
                if (!string.IsNullOrEmpty(CertificateNumber) && CertificateNumber != "null")
                {
                    wherestring += " and Customs.SupplierInvioceItemCertificats.CertificateNumber = '" + CertificateNumber + "'";
                }
                if (!string.IsNullOrEmpty(ResConfirmationTypeCode) && ResConfirmationTypeCode != "null")
                {
                    wherestring += " and Customs.SupplierInvioceItemCertificats.ResConfirmationTypeCode = '" + ResConfirmationTypeCode + "'";
                }
                if (!string.IsNullOrEmpty(invoiceNumber) && invoiceNumber != "null")
                {
                    wherestring += " and Customs.SupplierInvoices.InvoiceNumber = '" + invoiceNumber + "'";
                }
                cmd = (@"select NEWID() as Id , Customs.SupplierInvoices.InvoiceNumber,Customs.SupplierInvoiceItems.ItemCode,Customs.SupplierInvoiceItems.ClassificationCode,
Customs.SupplierInvoiceItems.TradeAgreementCode,SupplierInvoiceItems.OriginCountryCode, Customs.CustomsCountries.LocalName AS OriginCountryName,
Customs.TradeAgreements.LocalName AS TradeAgreementName,
Customs.SupplierInvioceItemCertificats.ItemCertificateCounterKey,Customs.SupplierInvoices.InvoiceCounterKey,Customs.SupplierInvoices.DeclarationId,
Customs.SupplierInvoiceItems.LineNumber,Customs.SupplierInvoiceItems.SequenceNumeric
,Customs.SupplierInvoiceItemsProdIdents.Identification AS CatalogNumber,Customs.SupplierInvioceItemCertificats.ReqConfirmationTypeCode,SupplierInvioceItemCertificats.AttachmentTypeCode,
Customs.SupplierInvioceItemCertificats.CertificateExemptionTypeCode,Customs.SupplierInvioceItemCertificats.CertificateNumber,Customs.SupplierInvioceItemCertificats.ResConfirmationTypeCode
from Customs.supplierInvoiceItems

LEFT OUTER JOIN Customs.CustomsCountries ON Customs.SupplierInvoiceItems.OriginCountryCode = Customs.CustomsCountries.Code
 LEFT OUTER JOIN Customs.TradeAgreements ON Customs.SupplierInvoiceItems.TradeAgreementCode = Customs.TradeAgreements.Code
 LEFT OUTER JOIN Customs.SupplierInvoiceItemsProdIdents on Customs.SupplierInvoiceItems.DeclarationId=Customs.SupplierInvoiceItemsProdIdents.DeclarationId and 




Customs.SupplierInvoiceItems.CounterKey = Customs.SupplierInvoiceItemsProdIdents.InvoiceCounterKey and Customs.SupplierInvoiceItems.LineNumber =Customs.SupplierInvoiceItemsProdIdents.InvoiceItemLineNumber
INNER JOIN Customs.SupplierInvioceItemCertificats ON Customs.SupplierInvoiceItems.DeclarationId = Customs.SupplierInvioceItemCertificats.DeclarationId AND Customs.SupplierInvoiceItems.CounterKey=Customs.SupplierInvioceItemCertificats.InvoiceCounterKey
AND Customs.SupplierInvoiceItems.LineNumber = Customs.SupplierInvioceItemCertificats. LineNumber
INNER JOIN Customs.SupplierInvoices ON Customs.SupplierInvoiceItems.DeclarationId = Customs.SupplierInvoices.DeclarationId AND Customs.SupplierInvoiceItems.CounterKey = Customs.SupplierInvoices.InvoiceCounterKey" + wherestring);
               
            }
            iQueryable = activeContext.Database.SqlQuery<CertificateConnectedItems>(cmd, parameters);
            IQueryable<CertificateConnectedItems> result;
            if (getAll)
            {
                result = iQueryable.OrderBy(d => d.SequenceNumeric).AsQueryable();
            }
            else
            {
                result = iQueryable.OrderBy(d => d.InvoiceNumber).ThenBy(d => d.InvoiceCounterKey).ThenBy(d => d.SequenceNumeric).Skip(skip).Take(take).AsQueryable();
            }
            //foreach (CertificateConnectedItems item in result)
            //{
            //    item.Id = Guid.NewGuid().ToString();
            //}
            return result;



        }

        public int GetCertificateConnectedItemsCount(string declarationId, string attachmentTypeCode, string reqConfirmationTypeCode, string CertificateExemptionTypeCode, string CertificateNumber, string ResConfirmationTypeCode, int tenant, string invoiceNumber)
        {


            IQueryable<SupplierInvoiceItemsProdIdent> prodIdents = context.SupplierInvoiceItemsProdIdents.Where(d => d.TypeCode == "MN");
            IQueryable<SupplierInvoice> supplierInvoices = null;
            if (invoiceNumber != null)
            {
                supplierInvoices = (from a in context.SupplierInvoices
                                    where a.InvoiceNumber == invoiceNumber
                                    select a);
            }
            else
            {
                supplierInvoices = context.SupplierInvoices;
            }



            ICustomContext customContext = CustomContext.GetContext(tenant);
            CustomContext activeContext = customContext.GetActiveDbContext() as CustomContext;
            object[] parameters = new object[] { };
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            DbRawSqlQuery<CertificateConnectedItems> iQueryable = null;
            string cmd = "";
            if (dbms == "oracle")
            {

                string wherestring = " where SupplierInvoiceItems.DeclarationId = '" + declarationId + "'" + " and  SupplierInvoiceItems.IsParent = 0 ";
                if (!string.IsNullOrEmpty(attachmentTypeCode) && attachmentTypeCode != "null")
                {
                    wherestring += " and SupplierInvioceItemCertificats.AttachmentTypeCode = '" + attachmentTypeCode + "'";
                }
                else
                {
                    wherestring += " and SupplierInvioceItemCertificats.AttachmentTypeCode  is null ";
                }

                if (!string.IsNullOrEmpty(reqConfirmationTypeCode) && reqConfirmationTypeCode != "null")
                {
                    wherestring += " and SupplierInvioceItemCertificats.ReqConfirmationTypeCode = '" + reqConfirmationTypeCode + "'";
                }
                if (!string.IsNullOrEmpty(CertificateExemptionTypeCode) && CertificateExemptionTypeCode != "null")
                {
                    wherestring += " and SupplierInvioceItemCertificats.CertificateExemptionTypeCode = '" + CertificateExemptionTypeCode + "'";
                }
                if (!string.IsNullOrEmpty(CertificateNumber) && CertificateNumber != "null")
                {
                    wherestring += " and SupplierInvioceItemCertificats.CertificateNumber = '" + CertificateNumber + "'";
                }
                if (!string.IsNullOrEmpty(ResConfirmationTypeCode) && ResConfirmationTypeCode != "null")
                {
                    wherestring += " and SupplierInvioceItemCertificats.ResConfirmationTypeCode = '" + ResConfirmationTypeCode + "'";
                }
                if (!string.IsNullOrEmpty(invoiceNumber)  && invoiceNumber != "null")
                {
                    wherestring += " and SupplierInvoices.InvoiceNumber = '" + invoiceNumber + "'";
                }

                cmd = (@"select SYS_GUID() as Id, SupplierInvoices.InvoiceNumber,SupplierInvoiceItems.ItemCode,SupplierInvoiceItems.ClassificationCode,SupplierInvoiceItems.TradeAgreementCode,SupplierInvoiceItems.OriginCountryCode,
SupplierInvioceItemCertificats.ItemCertificateCounterKey,SupplierInvoices.InvoiceCounterKey,SupplierInvoices.DeclarationId,SupplierInvoiceItems.LineNumber,SupplierInvoiceItems.SequenceNumeric
,SupplierInvoiceItemsProdIdents.Identification,SupplierInvioceItemCertificats.ReqConfirmationTypeCode,SupplierInvioceItemCertificats.AttachmentTypeCode,
SupplierInvioceItemCertificats.CertificateExemptionTypeCode,SupplierInvioceItemCertificats.CertificateNumber,SupplierInvioceItemCertificats.ResConfirmationTypeCode
from supplierInvoiceItems LEFT OUTER JOIN SupplierInvoiceItemsProdIdents on SupplierInvoiceItems.DeclarationId=SupplierInvoiceItemsProdIdents.DeclarationId and 
SupplierInvoiceItems.CounterKey = SupplierInvoiceItemsProdIdents.InvoiceCounterKey and SupplierInvoiceItems.LineNumber =SupplierInvoiceItemsProdIdents.InvoiceItemLineNumber
INNER JOIN SupplierInvioceItemCertificats ON SupplierInvoiceItems.DeclarationId = SupplierInvioceItemCertificats.DeclarationId AND SupplierInvoiceItems.CounterKey=SupplierInvioceItemCertificats.InvoiceCounterKey
AND SupplierInvoiceItems.LineNumber =SupplierInvioceItemCertificats. LineNumber
INNER JOIN SupplierInvoices ON SupplierInvoiceItems.DeclarationId = SupplierInvoices.DeclarationId AND SupplierInvoiceItems.CounterKey = SupplierInvoices.InvoiceCounterKey" + wherestring);//+ " Order By SupplierInvoiceItems.SequenceNumeric skip("+skip.ToString()+") "+"take("+take.ToString()+")");

            }
            else
            {
                string wherestring = " where Customs.SupplierInvoiceItems.DeclarationId = '" + declarationId + "'"+ " and  Customs.SupplierInvoiceItems.IsParent = 0 "; // and Customs.SupplierInvioceItemCertificats.AttachmentTypeCode = '" + attachmentTypeCode + "'";

                if (!string.IsNullOrEmpty(attachmentTypeCode) && attachmentTypeCode != "null")
                {
                    wherestring += " and SupplierInvioceItemCertificats.AttachmentTypeCode = '" + attachmentTypeCode + "'";
                }

                if (!string.IsNullOrEmpty(reqConfirmationTypeCode) && reqConfirmationTypeCode != "null")
                {
                    wherestring += " and Customs.SupplierInvioceItemCertificats.ReqConfirmationTypeCode = '" + reqConfirmationTypeCode + "'";
                }
                if (!string.IsNullOrEmpty(CertificateExemptionTypeCode) && CertificateExemptionTypeCode != "null")
                             {
                    wherestring += " and Customs.SupplierInvioceItemCertificats.CertificateExemptionTypeCode = '" + CertificateExemptionTypeCode + "'";
                }
                if (!string.IsNullOrEmpty(CertificateNumber) && CertificateNumber != "null")
                {
                    wherestring += " and Customs.SupplierInvioceItemCertificats.CertificateNumber = '" + CertificateNumber + "'";
                }
                if (!string.IsNullOrEmpty(ResConfirmationTypeCode) && ResConfirmationTypeCode != "null")
                {
                    wherestring += " and Customs.SupplierInvioceItemCertificats.ResConfirmationTypeCode = '" + ResConfirmationTypeCode + "'";
                }
                if (!string.IsNullOrEmpty(invoiceNumber) && invoiceNumber != "null")
                {
                    wherestring += " and Customs.SupplierInvoices.InvoiceNumber = '" + invoiceNumber + "'";
                }
                cmd = (@"select NEWID() as Id , Customs.SupplierInvoices.InvoiceNumber,Customs.SupplierInvoiceItems.ItemCode,Customs.SupplierInvoiceItems.ClassificationCode,
Customs.SupplierInvoiceItems.TradeAgreementCode,SupplierInvoiceItems.OriginCountryCode, Customs.CustomsCountries.LocalName AS OriginCountryName,
Customs.TradeAgreements.LocalName AS TradeAgreementName,
Customs.SupplierInvioceItemCertificats.ItemCertificateCounterKey,Customs.SupplierInvoices.InvoiceCounterKey,Customs.SupplierInvoices.DeclarationId,
Customs.SupplierInvoiceItems.LineNumber,Customs.SupplierInvoiceItems.SequenceNumeric
,Customs.SupplierInvoiceItemsProdIdents.Identification,Customs.SupplierInvioceItemCertificats.ReqConfirmationTypeCode,SupplierInvioceItemCertificats.AttachmentTypeCode,
Customs.SupplierInvioceItemCertificats.CertificateExemptionTypeCode,Customs.SupplierInvioceItemCertificats.CertificateNumber,Customs.SupplierInvioceItemCertificats.ResConfirmationTypeCode
from Customs.supplierInvoiceItems

LEFT OUTER JOIN Customs.CustomsCountries ON Customs.SupplierInvoiceItems.OriginCountryCode = Customs.CustomsCountries.Code
 LEFT OUTER JOIN Customs.TradeAgreements ON Customs.SupplierInvoiceItems.TradeAgreementCode = Customs.TradeAgreements.Code
 LEFT OUTER JOIN Customs.SupplierInvoiceItemsProdIdents on Customs.SupplierInvoiceItems.DeclarationId=Customs.SupplierInvoiceItemsProdIdents.DeclarationId and 




Customs.SupplierInvoiceItems.CounterKey = Customs.SupplierInvoiceItemsProdIdents.InvoiceCounterKey and Customs.SupplierInvoiceItems.LineNumber =Customs.SupplierInvoiceItemsProdIdents.InvoiceItemLineNumber
INNER JOIN Customs.SupplierInvioceItemCertificats ON Customs.SupplierInvoiceItems.DeclarationId = Customs.SupplierInvioceItemCertificats.DeclarationId AND Customs.SupplierInvoiceItems.CounterKey=Customs.SupplierInvioceItemCertificats.InvoiceCounterKey
AND Customs.SupplierInvoiceItems.LineNumber = Customs.SupplierInvioceItemCertificats. LineNumber
INNER JOIN Customs.SupplierInvoices ON Customs.SupplierInvoiceItems.DeclarationId = Customs.SupplierInvoices.DeclarationId AND Customs.SupplierInvoiceItems.CounterKey = Customs.SupplierInvoices.InvoiceCounterKey" + wherestring);

            }
            iQueryable = activeContext.Database.SqlQuery<CertificateConnectedItems>(cmd, parameters);


            return iQueryable.Count();
        }



        public IQueryable<CertificateConnectedItems> GetCertificateConnectedItems2(string declarationId, string attachmentTypeCode, string  reqConfirmationTypeCode, string CertificateExemptionTypeCode,string CertificateNumber,string ResConfirmationTypeCode,   int tenant)
        {


            IQueryable<SupplierInvoiceItemsProdIdent> prodIdents = context.SupplierInvoiceItemsProdIdents.Where(d => d.TypeCode == "MN");

            IQueryable<CertificateConnectedItems> result = (from a in context.SupplierInvoiceItems
                          join d in prodIdents
                          on new { p1 = a.DeclarationId, p2 = a.CounterKey, p3 = a.LineNumber } equals new { p1 = d.DeclarationId, p2 = d.InvoiceCounterKey, p3 = d.InvoiceItemLineNumber } into j
                          from modificationJoin in j.DefaultIfEmpty()

                          join c in context.SupplierInvioceItemCertificats 
                          on new { p1 = a.DeclarationId, p2 = a.CounterKey, p3 = a.LineNumber } equals new { p1 = c.DeclarationId, p2 = c.InvoiceCounterKey, p3 = c.LineNumber }
                          join v in context.SupplierInvoices
                          on new { p1 = a.DeclarationId, p2 = a.CounterKey } equals new { p1 = v.DeclarationId, p2 = v.InvoiceCounterKey }
                          where a.DeclarationId == declarationId && c.AttachmentTypeCode == attachmentTypeCode && c.ReqConfirmationTypeCode == reqConfirmationTypeCode && c.CertificateExemptionTypeCode == CertificateExemptionTypeCode && c.CertificateNumber == CertificateNumber && c.ResConfirmationTypeCode == ResConfirmationTypeCode
                          select new  CertificateConnectedItems()
                          { 
                              Id= Guid.NewGuid(),
                              InvoiceNumber = v.InvoiceNumber, 
                              ItemCode = a.ItemCode, 
                              ClassificationCode = a.ClassificationCode, 
                              TradeAgreementCode = a.TradeAgreementCode, 
                              OriginCountryCode = a.OriginCountryCode,
                              ItemCertificateCounterKey = c.ItemCertificateCounterKey,
                              InvoiceCounterKey = a.CounterKey, 
                              DeclarationId = a.DeclarationId,
                              LineNumber = a.LineNumber,
                            SequenceNumeric = a.SequenceNumeric,
                              CatalogNumber =modificationJoin!=null? modificationJoin.Identification:null,
                              ReqConfirmationTypeCode = c.ReqConfirmationTypeCode,
                              AttachmentTypeCode = c.AttachmentTypeCode,
                              CertificateExemptionTypeCode = c.CertificateExemptionTypeCode,
                              CertificateNumber = c.CertificateNumber,
                              ResConfirmationTypeCode = c.ResConfirmationTypeCode
                          });


            

            return result;
                 


        }

        public void FastDeleteMultiParents(SupplierInvoiceKeys entityKeyFields, List<int> supplierInvoiceItemsParentsLines)
        {
            (context as DbContextBase)
                .DeleteWhere<SupplierInvioceItemCertificat>(rec => rec.DeclarationId == entityKeyFields.DeclarationId && rec.InvoiceCounterKey == entityKeyFields.InvoiceCounterKey && supplierInvoiceItemsParentsLines.Contains(rec.LineNumber));

        }

        public List<CertificateTicket> GetDeclarationCertificateTicket(string declarationId,string reqConfirmationType, string invoiceNumber,int? invoiceCounterKey, string demandState, int tenant)
        {
            IQueryable<SupplierInvioceItemCertificat> certificates;
          

            certificates = (from a in context.SupplierInvioceItemCertificats//.Include("ResponseConfirmationType").Include("RequestConfirmationType").Include("CertificateExemptionType").Include("AttachmentType")
                            join s in context.SupplierInvoiceItems on new { DeclarationId = a.DeclarationId, LineNumber = a.LineNumber, CounterKey = a.InvoiceCounterKey } equals new { DeclarationId = s.DeclarationId, LineNumber = s.LineNumber, CounterKey = s.CounterKey }
                            where a.DeclarationId == declarationId && !s.IsParent && a.Tenant == tenant
                            select a);//.ToList();

            if (invoiceNumber != null && invoiceCounterKey != null)
            {
                certificates = certificates.Where(d =>  d.InvoiceCounterKey == invoiceCounterKey);

                                //where a.DeclarationId == declarationId && a.InvoiceCounterKey == invoiceCounterKey && a.Tenant == tenant && a.ReqConfirmationTypeCode== reqConfirmationType
                                //select a).ToList();
            }

            if (reqConfirmationType != null)
            {
                certificates = certificates.Where(d => d.ReqConfirmationTypeCode == reqConfirmationType);
                //certificates = (from a in context.SupplierInvioceItemCertificats.Include("ResponseConfirmationType").Include("RequestConfirmationType").Include("CertificateExemptionType").Include("AttachmentType")

                //                where a.DeclarationId == declarationId &&  a.Tenant == tenant && a.ReqConfirmationTypeCode == reqConfirmationType
                //                select a).ToList();
            }

            IQueryable<ConfirmationType> ConfirmationTypes = from a in context.ConfirmationTypes select a;
            IQueryable<AttachmentType> AttachmentTypes = from a in context.AttachmentTypes select a;
            IQueryable<CertificateExemptionType> CertificateExemptionTypes = from a in context.CertificateExemptionTypes select a;

            List < SupplierInvioceItemCertificat > certificatesList = certificates.ToList();
            List<CertificateTicket> ticketsQuery = (from a in certificatesList
                                                          where a.DeclarationId == declarationId
                                                          group a by new { a.ReqConfirmationTypeCode, a.AttachmentTypeCode, a.CertificateExemptionTypeCode, a.CertificateNumber, a.ResConfirmationTypeCode }
                                                         into c
                                                          select new CertificateTicket()
                                                          {
                                                              Id = Guid.NewGuid().ToString(),
                                                              AttachmentTypeCode = c.Key.AttachmentTypeCode,
                                                              CertificateNumber = c.Key.CertificateNumber,
                                                              ResConfirmationTypeCode = c.Key.ResConfirmationTypeCode,
                                                              CertificateExemptionTypeCode = c.Key.CertificateExemptionTypeCode,
                                                              ReqConfirmationTypeCode = c.Key.ReqConfirmationTypeCode,
                                                              CustomsAttachmentId = c.FirstOrDefault().CustomsAttachmentID,
                                                              ExternalCertificatCode = c.FirstOrDefault().ExternalCertificatCode,
                                                              //ResConfirmationTypeName = c.FirstOrDefault().ResponseConfirmationType != null ? c.FirstOrDefault().ResponseConfirmationType.LocalName : null,
                                                              //ReqConfirmationTypeName = c.FirstOrDefault().RequestConfirmationType != null ? c.FirstOrDefault().RequestConfirmationType.LocalName : null,
                                                              //AttachmentTypeName = c.FirstOrDefault().AttachmentType != null ? c.FirstOrDefault().AttachmentType.LocalName : null,
                                                              //CertificateExemptionTypeName = c.FirstOrDefault().CertificateExemptionType != null ? c.FirstOrDefault().CertificateExemptionType.LocalName : null,
                                                              DeclarationId = c.FirstOrDefault().DeclarationId,
                                                          }).ToList();
            foreach (CertificateTicket item in ticketsQuery)
            {
                if(item.AttachmentTypeCode != null)
                {
                    item.AttachmentTypeName = AttachmentTypes.Where(d => d.Code == item.AttachmentTypeCode).FirstOrDefault().LocalName;
                }
                if(item.ResConfirmationTypeCode != null)
                {
                    item.ResConfirmationTypeName = ConfirmationTypes.Where(d => d.Code == item.ResConfirmationTypeCode).FirstOrDefault().LocalName;
                }
                if(item.ReqConfirmationTypeCode != null)
                {
                    item.ReqConfirmationTypeName = ConfirmationTypes.Where(d => d.Code == item.ReqConfirmationTypeCode).FirstOrDefault().LocalName;
                }
                if(item.CertificateExemptionTypeCode != null)
                {
                    item.CertificateExemptionTypeName = CertificateExemptionTypes.Where(d => d.Code == item.CertificateExemptionTypeCode).FirstOrDefault().LocalName;
                }
            }

            if (demandState == "withResponse")
            {
                ticketsQuery = ticketsQuery.Where(d => d.AttachmentTypeCode != null && (d.CertificateExemptionTypeCode != null || d.CertificateNumber != null)).ToList();
            }
            else if (demandState == "withoutResponse")
            {
                ticketsQuery = ticketsQuery.Where(d => !(d.AttachmentTypeCode != null && (d.CertificateExemptionTypeCode != null || d.CertificateNumber != null))).ToList();
            }

            return ticketsQuery.OrderBy(d => d.ReqConfirmationTypeCode).ThenBy(d => d.AttachmentTypeCode).ToList();
        }

        public IQueryable<CertificateTicket> GetDeclarationCertificateTicket2(string declarationId,string demandState, int tenant)
        {

            IQueryable<SupplierInvioceItemCertificat> certificates = (from a in context.SupplierInvioceItemCertificats.Include("ResponseConfirmationType").Include("RequestConfirmationType").Include("CertificateExemptionType").Include("AttachmentType")
                                                                      where a.DeclarationId == declarationId && a.Tenant == tenant
                                                                      select a);

            IQueryable<CertificateTicket> tickets = (from a in certificates
                                                     where a.DeclarationId == declarationId
                                                     group a by new { a.ReqConfirmationTypeCode, a.AttachmentTypeCode, a.CertificateExemptionTypeCode, a.CertificateNumber, a.ResConfirmationTypeCode }
                                                         into c
                                                         select new CertificateTicket()
                                                         {
                                                             Id = Guid.NewGuid().ToString(),
                                                             AttachmentTypeCode = c.Key.AttachmentTypeCode,
                                                             CertificateNumber = c.Key.CertificateNumber,
                                                             ResConfirmationTypeCode = c.Key.ResConfirmationTypeCode,
                                                             CertificateExemptionTypeCode = c.Key.CertificateExemptionTypeCode,
                                                             ReqConfirmationTypeCode = c.Key.ReqConfirmationTypeCode,
                                                         });

            if (demandState == "withResponse")
            {
                tickets = tickets.Where(d => d.AttachmentTypeCode != null && (d.CertificateExemptionTypeCode != null || d.CertificateNumber != null));
            }
            else if (demandState == "withoutResponse")
            {
                tickets = tickets.Where(d => !(d.AttachmentTypeCode != null && (d.CertificateExemptionTypeCode != null || d.CertificateNumber != null)));
            }

            return tickets;
        }



        public List<CertificateConnectedItems> GetDeclarationInvoicesNumbers(string declarationId, int tenant)
        {
            List<SupplierInvoice> supplierInvoices=(from a in context.SupplierInvoices
                                                              where a.DeclarationId == declarationId && a.Tenant == tenant
                                                             select a).ToList();


            List<CertificateConnectedItems> invoiceNumbers = (from a in supplierInvoices
                                                              select new CertificateConnectedItems()
                                                              {
                                                                  Id= Guid.NewGuid(),
                                                                  InvoiceNumber = a.InvoiceNumber,
                                                                  InvoiceCounterKey = a.InvoiceCounterKey,
                                                              }).ToList();

            return invoiceNumbers;
                                         
        }

        public List<CertificateGroupItems> GetCertificateGroupForDeclaration(string declarationId, int tenant) 
        {
            //Get all items (supplierInvoiceItems) that do not have certificates
            var supplierInvoiceItems = (from a in context.SupplierInvoiceItems
                                        where a.DeclarationId == declarationId && a.Tenant == tenant && a.CertificatesStatusCode != "1"
                                        join d in context.SupplierInvoices
                                        on new
                                        {
                                            a.DeclarationId,
                                            a.CounterKey,
                                        }
                                        equals new
                                        {
                                            DeclarationId = d.DeclarationId,
                                            CounterKey = d.InvoiceCounterKey
                                        }
                                        join customsVendor in context.CustomsVendors on d.VendorId equals customsVendor.Id
                                        select new { a, customsVendor.VendorNumber }
                                        into itemsGroups
                                        group itemsGroups by new { itemsGroups.a.ClassificationCode, itemsGroups.a.ItemCode, itemsGroups.a.OriginCountryCode, itemsGroups.VendorNumber }).ToList();


            List<CertificateGroupItems> certificateGroupItems = new List<CertificateGroupItems>();
            foreach (var groupItem in supplierInvoiceItems)
            {
                CertificateGroupItems certificateItem = new CertificateGroupItems();
                certificateItem.Id = Guid.NewGuid();
                certificateItem.ClassificationCode = groupItem.FirstOrDefault().a.ClassificationCode;
                certificateItem.ItemCode = groupItem.FirstOrDefault().a.ItemCode;
                certificateItem.OriginCountryCode = groupItem.FirstOrDefault().a.OriginCountryCode;
                certificateItem.VendorNumber = groupItem.FirstOrDefault().VendorNumber;
                certificateItem.SupplierInvoiceItems = new List<SupplierInvoiceItemKeys>();
                foreach (var item in groupItem)
                {
                    SupplierInvoiceItemKeys supplierInvoiceItem = new SupplierInvoiceItemKeys();
                    supplierInvoiceItem.DeclarationId = item.a.DeclarationId;
                    supplierInvoiceItem.CounterKey = item.a.CounterKey;
                    supplierInvoiceItem.LineNumber = item.a.LineNumber;
                    certificateItem.SupplierInvoiceItems.Add(supplierInvoiceItem);
                }
                certificateGroupItems.Add(certificateItem);
            }

            return certificateGroupItems;   
        }

        public int? GetMaxCounterKey(string declarationId,int invoiceCounterKey,int invoiceItemLineNum, int tenant)
        {
            return (from a in context.SupplierInvioceItemCertificats
                    where a.DeclarationId == declarationId && a.InvoiceCounterKey==invoiceCounterKey&&a.LineNumber==invoiceItemLineNum && a.Tenant == tenant
                    select a).Max(d => (int?)d.ItemCertificateCounterKey) ?? 0;
        }

    }

}
   