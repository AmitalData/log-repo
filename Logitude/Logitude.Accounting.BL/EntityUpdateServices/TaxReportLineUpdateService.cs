using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class TaxReportLineUpdateService
    {

        protected override void OnCreating(TaxReportLinePM entityPM, TaxReportPM entityParentPM)
        {
            entityPM.TaxReportId = entityParentPM.Id;
            entityParentPM.TaxReportLineLastLine += 1;
            entityPM.Line = entityParentPM.TaxReportLineLastLine;
            entityPM.IsManuallyChanged = true;
        }

        protected override void OnUpdating(TaxReportLinePM entityPM, TaxReportLine entityPOCO)
        {
            Validate(entityPM);
            // TASK 43057
            //if (entityPM.ChangeSetOp == ChangeSetOperation.Update)
            //{
                if (entityPM.IsManuallyChanged == true)
                {
                    JournalQueryService journalQuery = new JournalQueryService(EntityPM.Tenant);
                    JournalMoreDataQueryService moreDataQueryService = new JournalMoreDataQueryService(entityPM.Tenant);
                    IAccountingContext MyContext = AccountingContext.GetContext(entityPM.Tenant);
                    JournalMoreDataUpdateService journalMoreDataUpdateService = new JournalMoreDataUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);

                    JournalPM journalPM = journalQuery.GetSingle(EntityPM.JournalId, false, false);
                    JournalMoreDataPM journalMoreDataPM = moreDataQueryService.GetSingleJournalMorData(journalPM.Id, entityPM.Tenant);
                    if (journalPM != null)
                    {
                        if (entityPM.TransmitStatusCode == "1") // 1- For transmit
                            journalMoreDataPM.TaxReportId = entityPM.TaxReportId;
                        else if (entityPM.TransmitStatusCode == "3") // 3- Not for transmit at all
                            journalMoreDataPM.TaxReportId = "1111";

                        //update

                        journalMoreDataPM.ChangeSetOp = ChangeSetOperation.Update;
                        journalMoreDataUpdateService.Update(journalMoreDataPM, true);
                        entityPM.IsManuallyChanged = false;

                    }

                //}
            }

          
            base.OnUpdating(entityPM, entityPOCO);
        }

        protected override void Validate(TaxReportLinePM entityPM)
        {
            //for output lines
            CardRepository cardRepository = new CardRepository(entityPM.Tenant);
            TenantQuery tenantQuery = new TenantQuery(entityPM.Tenant);
            TenantPM tenantPM = tenantQuery.GetSinglePM(entityPM.Tenant);
            //ARInvoiceRepository aRInvoiceRepository = new ARInvoiceRepository(entityPM.Tenant);

            if (entityPM.OutputOrInput == "O")
            {
                //ARInvoice invoice = aRInvoiceRepository.GetSingleARInvoice(a.AccountingEntityId, a.Tenant);

                //Simplog.Data.CommonDataModel.EntityPOCOs.Card card = cardRepository.GetSingleCard(invoice.BillToId, tenant);



                if (entityPM.VatNumber == tenantPM.VatNumber)
                {
                    entityPM.LineTypeCode = "M";

                }
                //else if (card.IsAutonomy)
                //{
                //    entityPM.LineTypeCode = "I";
                //}
                //else
                //{
                //    entityPM.LineTypeCode = "S";
                //}

                if (entityPM.VatNumber == null)
                {
                    entityPM.StatusCode = "1";
                }
               
                //               else if (!string.IsNullOrEmpty(line.VatNumber))
                //               {
                //                   //var hasChars = Regex.Matches(line.VatNumber, @"[a-zA-Z]");


                //                   //if (line.VatNumber.Length != 9 || hasChars.Count != 0)
                //                   //{
                //                   //    line.StatusCode = "2";
                //                   //}

                //                   //else if (line.VatNumber.Length == 9)
                //                   //{
                //                   //    var wrongDigit = LuhnAlgorithm.CalculateLuhnAlgorithm(line.VatNumber);
                //                   //    var digit = line.VatNumber.ToString().Substring(8);
                //                   //    if (digit == wrongDigit.ToString())
                //                   //    {
                //                   //        line.StatusCode = "6";
                //                   //    }
                //                   //    else
                //                   //    {
                //                   //        line.StatusCode = "2";
                //                   //    }
                //                   //}

                //}

                 if (entityPM.Reference != null)
                {
                    var chars = Regex.Matches(entityPM.Reference, @"[a-zA-Z]");
                    if (chars.Count != 0)
                    {
                        entityPM.StatusCode = "3";
                    }
                    else { entityPM.StatusCode = "6"; }
                }

                else
                {
                    entityPM.StatusCode = "6";
                }

            }

            //for input line

           else  {

                if (entityPM.VatNumber == null)
                {
                    entityPM.StatusCode = "1";
                }

                if (entityPM.Reference != null)
                {
                    var chars = Regex.Matches(entityPM.Reference, @"[a-zA-Z]");
                    if (chars.Count != 0)
                    {
                        entityPM.StatusCode = "3";
                    }
                    else
                    {
                        entityPM.StatusCode = "6";
                    }

                }

                else
                {
                    entityPM.StatusCode = "6";
                }



            }
            base.Validate(entityPM);
        }

    }
}
