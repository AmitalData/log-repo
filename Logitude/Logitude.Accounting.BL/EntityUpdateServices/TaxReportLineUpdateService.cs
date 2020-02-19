using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.CustomsMessaging.Common.Gen;
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
            JournalAdditionalDataQueryService additionalDataQueryService = new JournalAdditionalDataQueryService(entityPM.Tenant);
            IAccountingContext MyContext = AccountingContext.GetContext(entityPM.Tenant);
            JournalAdditionalDataUpdateService journalAdditionalDataUpdateService = new JournalAdditionalDataUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            SetTaxReportLineStatusCodeAndLineTypeCode(entityPM);
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
            //JournalPM journalPM = journalQuery.GetSingle(EntityPM.JournalId, false, false);
            JournalAdditionalDataPM journalAdditionalDataPM = additionalDataQueryService.GetSingle(EntityPM.JournalId, false, false);
            if (journalAdditionalDataPM != null)
            {
                journalAdditionalDataPM.TaxReportTransmitStatusCode = entityPM.TransmitStatusCode;
                journalAdditionalDataPM.TaxReportId = entityPM.TaxReportId;
                journalAdditionalDataPM.ChangeSetOp = ChangeSetOperation.Update;
                journalAdditionalDataUpdateService.Update(journalAdditionalDataPM, true);
            }

            base.OnUpdating(entityPM, entityPOCO);
        }
        private  void SetTaxReportLineStatusCodeAndLineTypeCode(TaxReportLinePM entityPM)
        {
            TaxReportQueryService taxReportQueryService = new TaxReportQueryService(entityPM.Tenant);
            TaxReportPM taxReport = taxReportQueryService.GetSingle(entityPM.TaxReportId, false, false);
            TenantQuery tenantQuery = new TenantQuery(entityPM.Tenant);           
            string vatNumber = tenantQuery.GetTenantVatNumber(entityPM.Tenant);         
            entityPM.StatusCode = "6";
            entityPM.VatNumber = entityPM.VatNumber != null ? entityPM.VatNumber.Trim() : null;
            string trimmedZeros = entityPM.VatNumber != null ? entityPM.VatNumber.Trim('0') : null;
            bool zerosVatNumber;
            if (entityPM.OutputOrInput == "O")
            {
               
                if(entityPM.ReferenceDate.Value.Month != taxReport.TaxReportMonth.Month)
                {
                    entityPM.StatusCode = "7";
                }
                if (entityPM.VatNumber == vatNumber)
                {
                    entityPM.LineTypeCode = "M";

                }


                if (entityPM.VatNumber == null)
                {
                    entityPM.StatusCode = "1";
                }
                else if (entityPM.VatNumber != null)
                {
                    zerosVatNumber = trimmedZeros == "" ? true : false;
                    if (entityPM.VatNumber.Length > 9 || (zerosVatNumber && entityPM.VatNumber != "000000000"))
                    {
                        entityPM.StatusCode = "2";
                    }
                    else
                    {
                        if (entityPM.LineTypeCode == "K" && entityPM.VatNumber == "000000000")
                        {
                            entityPM.StatusCode = "6";
                            return;
                        }
                        else
                        {
                            var chars = Regex.Matches(entityPM.VatNumber, @"[^\d{9}$]");
                            if (chars.Count == 0)
                            {
                                var digit = LuhnAlgorithm.CalculateLuhnAlgorithm(entityPM.VatNumber);
                                if (digit != 0)
                                {
                                    entityPM.StatusCode = "2";
                                }

                            }
                            else
                            {
                                entityPM.StatusCode = "2";
                            }
                        }

                    }
                }

                if (entityPM.VatableInvoiceAmount != null && entityPM.VatableInvoiceAmount != 0)
                {
                    //  string s = entityPM.VatableInvoiceAmount.ToString();
                    string[] amount = entityPM.VatableInvoiceAmount.ToString().Split('.');
                    if (amount.Count() > 1 && amount[1] != "00")
                    {
                        entityPM.StatusCode = "4";
                    }

                }


                if (entityPM.Reference != null)
                {
                    var chars = Regex.Matches(entityPM.Reference, @"[^\d{9}$]");
                    if (chars.Count != 0)
                    {
                        entityPM.StatusCode = "3";
                    }

                }



            }

            //for input line

            else
            {

                if (entityPM.VatNumber == null)
                {
                    entityPM.StatusCode = "1";
                }
                else if (entityPM.VatNumber != null)
                {
                    zerosVatNumber = trimmedZeros == "" ? true : false;
                    if (entityPM.VatNumber.Length > 9 || (zerosVatNumber && entityPM.VatNumber != "000000000"))
                    {
                        entityPM.StatusCode = "2";
                    }
                    else
                    {

                        if (entityPM.LineTypeCode == "K" && entityPM.VatNumber == "000000000")
                        {
                            entityPM.StatusCode = "6";
                            return;
                        }
                        else
                        {
                            var chars = Regex.Matches(entityPM.VatNumber, @"[^\d{9}$]");
                            if (chars.Count == 0)
                            {
                                var digit = LuhnAlgorithm.CalculateLuhnAlgorithm(entityPM.VatNumber);
                                if (digit != 0)
                                {
                                    entityPM.StatusCode = "2";
                                }

                            }
                            else
                            {
                                entityPM.StatusCode = "2";
                            }
                        }

                    }
                }

                if (entityPM.Reference != null)
                {
                    var chars = Regex.Matches(entityPM.Reference, @"[^\d{9}$]");
                    if (chars.Count != 0)
                    {
                        entityPM.StatusCode = "3";
                    }

                }

                if (entityPM.VatableInvoiceAmount != 0 && entityPM.VatableInvoiceAmount != null)
                {
                    //string s = entityPM.VatableInvoiceAmount.ToString();
                    string[] amount = entityPM.VatableInvoiceAmount.ToString().Split('.');
                    if (amount.Count() > 1 && amount[1] != "00")
                    {
                        entityPM.StatusCode = "4";
                    }

                }
            }


            }
        protected override void Validate(TaxReportLinePM entityPM)
        {
            
           
            base.Validate(entityPM);
        }
       
    }
}
