using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools;
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

        protected override void OnCreating(TaxReportLinePM entityPM, EntityPM entityParentPM)
        {
            entityPM.IsManuallyChanged = true;

            base.OnCreating(entityPM, entityParentPM);
        }

        protected override void OnUpdating(TaxReportLinePM entityPM, TaxReportLine entityPOCO)
        {
            JournalQueryService journalQuery = new JournalQueryService(EntityPM.Tenant);
            JournalAdditionalDataQueryService additionalDataQueryService = new JournalAdditionalDataQueryService(entityPM.Tenant);
            IAccountingContext MyContext = AccountingContext.GetContext(entityPM.Tenant);
            JournalAdditionalDataUpdateService journalAdditionalDataUpdateService = new JournalAdditionalDataUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);


            Validate(entityPM);

            // TASK 43057
            if (this.EntityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                if (entityPM.IsManuallyChanged == true)
                    entityPM.IsManuallyChanged = false;
            }
            else
            {
                entityPM.IsManuallyChanged = true;
            }

            // Update Journal
            JournalPM journalPM = journalQuery.GetSingle(EntityPM.JournalId, false, false);
            JournalAdditionalDataPM journalAdditionalDataPM = additionalDataQueryService.GetSingle(journalPM.Id, false, false);
            if (journalPM != null)
            {
                journalAdditionalDataPM.TaxReportTransmitStatusCode = entityPM.TransmitStatusCode;
                journalAdditionalDataPM.TaxReportId = entityPM.TaxReportId;
                journalAdditionalDataPM.ChangeSetOp = ChangeSetOperation.Update;
                journalAdditionalDataUpdateService.Update(journalAdditionalDataPM, true);
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
