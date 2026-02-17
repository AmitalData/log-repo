using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.Interfaces;
using Logitude.BL.Resolvers;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.AccountingModel.Reports.Journal
{
    class JournalPrintService
    {
        public void BuildJournalReport(string entityId, int tenant, string documentOutId)
        {
            // 1 
            // Fill DataProvider
            JournalDataProvider journalDP = LoadDataProvider(entityId, tenant);


            // 2
            // Get byte[] of DataProvider
            XmlSerializer serializer = new XmlSerializer(typeof(JournalDataProvider));
            MemoryStream memstream = new MemoryStream();
            serializer.Serialize(memstream, journalDP);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();


            // 3
            // Get StiObject
            StiBusinessObject currentBusinessObject = new StiBusinessObject() { Category = "JRPR", Name = "JournalDataProvider", BusinessObjectValue = journalDP };


            // 4
            //Build report
            Byte[] templatedata = null;
            StiReport report = new StiReport();
            DocumentTypeTemplateRepository documentTypeTemplaterep = new DocumentTypeTemplateRepository(tenant);
            DocumentOutRepository documentOutRepository = new DocumentOutRepository(tenant);


            DocumentOut documentOut = documentOutRepository.GetSingleDocumentOut(documentOutId, tenant);
            DocumentTypeTemplate defaulttemplate = documentTypeTemplaterep.GetSingleDocumentTypeTemplate(documentOut.DocumentTemplateId);

            if (defaulttemplate != null)
                templatedata = defaulttemplate.TemplateBody;


            if (templatedata != null)
            {
                if (templatedata.Length != 0)
                {
                    ExportDocumentHelper exportDocumentHelper = new ExportDocumentHelper();
                    report = exportDocumentHelper.LoadandRender(report, templatedata, defaulttemplate, currentBusinessObject, documentTypeTemplaterep, tenant);
                }
            }



        }

        public JournalDataProvider LoadDataProvider(string entityId, int tenant)
        {
            JournalDataProvider journalDP = new JournalDataProvider();
            JournalQueryService journalQuery = new JournalQueryService(tenant);
            CurrencyQuery currencyQuery = new CurrencyQuery(tenant);
            TenantQuery tenantQuery = new TenantQuery(tenant);

            JournalPM journalPM = journalQuery.GetSingle(entityId, true, false);
   
            if(journalPM != null)
            {
                journalDP.JournalNumber = journalPM.JournalNumber == null ? "" : journalPM.JournalNumber;
                journalDP.AccountingEntityCode = journalPM.AccountingEntityName == null ? "" : journalPM.AccountingEntityName;
                journalDP.AccountingEntityReference = journalPM.AccountingEntityReference == null ? "" : journalPM.AccountingEntityReference;
                journalDP.AccountingDate = journalPM.AccountingDate;

                ContactPM loggedContact = GetLoggedContact(tenant);
                journalDP.PrintedByUserName = loggedContact.LocalName == null ? "" : loggedContact.LocalName;

                TenantPM tenantPM = tenantQuery.GetSinglePM(tenant);
                journalDP.TenantCurrency = tenantPM.CurrencyCode;

                // map lines
                List<JournalLine> lines = journalPM.JournalLines.Select(d => new JournalLine()
                {
                    Line = d.Line,
                    JournalActionCode = d.ActionCode,
                    JournalActionName = d.ActionName,
                    JournalActionTypeCode = d.ActionTypeCode,

                    DocumentDate = d.DocumentDate,
                    DueDate = d.DueDate,

                    CreditAccountNumber = d.CreditAccountNumber,
                    CreditAccountName = d.CreditAccountName,

                    DebitAccountNumber = d.DebitAccountNumber,
                    DebitAccountName = d.DebitAccountName,

                    CurrencyCode = d.CurrencyCode,
                    CurrencyName = d.CurrencyName,

                    LocalAmount = d.LocalAmount,
                    ForeignAmount = d.ForeignAmount,

                    Reference1 = d.Reference1,
                    Reference2 = d.Reference2,
                    Reference3 = d.Reference3,
                }).ToList();

                journalDP.JournalLines = lines;

            }

            //try
            //{
            //    Type journalDPType = journalDP.GetType();
            //    PropertyInfo[] properties = journalDPType.GetProperties();
            //    foreach (PropertyInfo pi in properties)
            //    {
            //        if (pi.GetValue(journalDP, null) == null || pi.GetValue(journalDP, null).ToString() == "0" || pi.GetValue(journalDP, null).ToString() == "00.00" || pi.GetValue(journalDP, null).ToString() == "0.00")
            //        {
            //            pi.SetValue(journalDP, "", null);
            //        }
            //    }
            //}
            //catch
            //{

            //}

            return journalDP;
        }
        private ContactPM GetLoggedContact(int tenant)
        {
            //ILoggedContactUtil loggedContactUtil = ContainerAccessor.Container.Resolve(typeof(ILoggedContactUtil), "LoggedContactUtil", new ParameterOverride("", tenant)) as ILoggedContactUtil;
            //ContactPM loggedcontact = loggedContactUtil.GetLoggedContact(tenant);

            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }


    }
}
