using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.Tools.Initializers;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.Behaviours.APInvoiceBehaviours
{
    public class APInvoiceTotalVatsBehavior : IServiceBehaviour
    {
        private bool isHandling;

        private APInvoicePM entityPM;

        private APInvoiceServiceInitializer initializer;

        public void Handle(IServiceInitializer initializer)
        {
            this.initializer = (APInvoiceServiceInitializer)initializer;
            this.entityPM = this.initializer.EntityPM;
            this.HandleBehaviour();
        }

        private void HandleBehaviour()
        {
            this.CheckHandling();

            if (isHandling)
            {
                this.UpdateTotalVats();
            }
        }

        private void CheckHandling()
        {
            if (initializer.IsNewEntity)
            {
                isHandling = true;
            }

            else if (entityPM.TotalVATOnly != initializer.EntityPOCO.TotalVATOnly)
            {
                isHandling = true;
            }

            else if (entityPM.TotalVATOnly && initializer.Flags.IsInvoiceTotalVATsChanged)
            {
                isHandling = true;
            }

            else if(initializer.Flags.IsInvoiceLinesChanged)
            {
                isHandling = true;
            }
        }


        private List<InvoiceTotalsClass> group_Source;
        private List<InvoiceTotalsClass> group_TotalVATs;
        private List<VATTypesGroup> allVATTypesGroup;
        private void UpdateTotalVats()
        {
            this.group_Source = new List<InvoiceTotalsClass>();
            this.group_TotalVATs = new List<InvoiceTotalsClass>();
            this.allVATTypesGroup = (from d in initializer.CommonContext.VATTypesGroups where d.Tenant == initializer.Tenant select d).ToList();

            this.CalculateSubTotals();
            this.CalculateTotalVATs();
            this.CalculateInvoiceAmounts();
            this.InitializeAmountDueFields();
        }

        private void CalculateSubTotals()
        {
            double? subTotal = 0;
            double? subTotal_Local = 0;
            List<APInvoiceLinePM> myDataLines = entityPM.InvoiceLines.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).ToList();

            if (myDataLines.Count > 0)
            {
                subTotal = MethodHelper.Round(myDataLines.Sum(s => s.InvoiceCurrencyAmount), 2);
                subTotal_Local = MethodHelper.Round(myDataLines.Sum(s => s.LocalCurrencyAmount), 2);
            }

            entityPM.SubTotalInInvoiceCurrency = subTotal;
            entityPM.SubTotalInLocalCurrency = subTotal_Local;
        }
        private void CalculateTotalVATs()
        {
            this.BuildTotalVATsDataSource();
            this.BuildTotalVATsDataGroups();
            this.GenerateTotalVATs();
        }
        private void BuildTotalVATsDataSource()
        {
            if (entityPM.TotalVATOnly)
            {
                this.BuildTotalVATsFromVATsOnly();
            }

            else
            {
                if (entityPM.TotalVATOnly != initializer.EntityPOCO.TotalVATOnly)
                {
                    this.BuildTotalVATsFromLines();
                }

                else if (initializer.Flags.IsInvoiceLinesChanged)
                {
                    this.BuildTotalVATsFromLines();
                }
            }
        }
        private void BuildTotalVATsFromVATsOnly()
        {
            List<APInvoiceTotalVATPM> items = entityPM.TotalVATs.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete && d.VatTypeId != null).ToList();

            if (items.Count > 0)
            {
                foreach (APInvoiceTotalVATPM item in items)
                {
                    VatType lineVatType = initializer.AllVatTypes.Where(d => d.Id == item.VatTypeId).FirstOrDefault();

                    if (lineVatType != null)
                    {
                        if (!lineVatType.IsMultiPercentage)
                        {
                            InvoiceTotalsClass newItem = new InvoiceTotalsClass()
                            {
                                Id = item.VatTypeId,
                                VatTypeId = item.VatTypeId,
                                VatTypePercentage = MethodHelper.GetValue(item.VatPercent),
                                LocalCurrencyAmount = item.LocalVATAmount,
                                InvoiceCurrencyAmount = item.InvoiceCurrencyVATAmount,
                                ProfitCurrencyAmount = MethodHelper.GetValue(item.ProfitCurrencyVATAmount),
                                ExternalVatCard = item.ExternalVATCard,
                                ExternalTAXItemId = lineVatType.ExternalTAXItemId,
                            };

                            if (string.IsNullOrEmpty(newItem.ExternalVatCard))
                            {
                                if (initializer.AccountingSetting.AccountingSystemCode == "HV" || this.initializer.AccountingSetting.AccountingSystemCode == "RH")
                                {
                                    newItem.ExternalVatCard = this.initializer.AccountingSetting.PayableVATCard;
                                }
                                else
                                {
                                    newItem.ExternalVatCard = lineVatType.PayablesExternalId;
                                }
                            }

                            group_Source.Add(newItem);
                        }

                        else
                        {
                            List<VATTypesGroup> vatTypesGroup = allVATTypesGroup.Where(d => d.GroupVATTypeId == item.VatTypeId).ToList();

                            foreach (VATTypesGroup itemGroup in vatTypesGroup)
                            {
                                InvoiceTotalsClass newItem = new InvoiceTotalsClass()
                                {
                                    Id = itemGroup.SingleVATTypeId,
                                    VatTypeId = itemGroup.SingleVATTypeId,
                                    LocalCurrencyAmount = item.LocalVATAmount,
                                    InvoiceCurrencyAmount = item.InvoiceCurrencyVATAmount,
                                    ProfitCurrencyAmount = MethodHelper.GetValue(item.ProfitCurrencyVATAmount),
                                };

                                VatType vatType = initializer.AllVatTypes.Where(d => d.Id == itemGroup.SingleVATTypeId).FirstOrDefault();
                                if (vatType != null)
                                {
                                    newItem.ExternalTAXItemId = vatType.ExternalTAXItemId;
                                }

                                if (initializer.AccountingSetting != null)
                                {
                                    newItem.ExternalVatCard = initializer.AccountingSetting.PayableVATCard;
                                }

                                VatTypePercentagePM myPercentagePM = initializer.AllVatPercentages.Where(d => d.VatTypeId == itemGroup.SingleVATTypeId).FirstOrDefault();
                                if (myPercentagePM != null)
                                {
                                    newItem.VatTypePercentage = MethodHelper.GetValue(myPercentagePM.Percentage);
                                }

                                if (string.IsNullOrEmpty(newItem.ExternalVatCard))
                                {
                                    if (initializer.AccountingSetting.AccountingSystemCode == "HV" || initializer.AccountingSetting.AccountingSystemCode == "RH")
                                    {
                                        newItem.ExternalVatCard = initializer.AccountingSetting.PayableVATCard;
                                    }
                                    else if (vatType != null)
                                    {
                                        newItem.ExternalVatCard = vatType.PayablesExternalId;
                                    }
                                }

                                group_Source.Add(newItem);
                            }
                        }
                    }
                }
            }
        }
        private void BuildTotalVATsFromLines()
        {
            List<APInvoiceLinePM> items = entityPM.InvoiceLines.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete && d.VatTypeId != null).ToList();

            if (items.Count > 0)
            {
                foreach (APInvoiceLinePM item in items)
                {
                    VatType lineVatType = initializer.AllVatTypes.Where(d => d.Id == item.VatTypeId).FirstOrDefault();

                    if (lineVatType != null)
                    {
                        if (!lineVatType.IsMultiPercentage)
                        {
                            InvoiceTotalsClass newItem = new InvoiceTotalsClass()
                            {
                                Id = item.VatTypeId,
                                VatTypeId = item.VatTypeId,
                                VatTypePercentage = MethodHelper.GetValue(item.VatPercentage),
                                LocalCurrencyAmount = MethodHelper.GetValue(item.LocalCurrencyAmount),
                                InvoiceCurrencyAmount = MethodHelper.GetValue(item.InvoiceCurrencyAmount),
                                ProfitCurrencyAmount = MethodHelper.GetValue(item.ProfitCurrencyAmount),
                                ExternalVatCard = item.ExternalVATCard,
                                ExternalTAXItemId = lineVatType.ExternalTAXItemId,
                            };

                            if (string.IsNullOrEmpty(newItem.ExternalVatCard))
                            {
                                if (initializer.AccountingSetting.AccountingSystemCode == "HV" || initializer.AccountingSetting.AccountingSystemCode == "RH")
                                {
                                    newItem.ExternalVatCard = initializer.AccountingSetting.PayableVATCard;
                                }
                                else
                                {
                                    newItem.ExternalVatCard = lineVatType.PayablesExternalId;
                                }
                            }

                            group_Source.Add(newItem);
                        }

                        else
                        {
                            List<VATTypesGroup> vatTypesGroup = allVATTypesGroup.Where(d => d.GroupVATTypeId == item.VatTypeId).ToList();

                            foreach (VATTypesGroup itemGroup in vatTypesGroup)
                            {
                                InvoiceTotalsClass newItem = new InvoiceTotalsClass()
                                {
                                    Id = itemGroup.SingleVATTypeId,
                                    VatTypeId = itemGroup.SingleVATTypeId,
                                    LocalCurrencyAmount = MethodHelper.GetValue(item.LocalCurrencyAmount),
                                    InvoiceCurrencyAmount = MethodHelper.GetValue(item.InvoiceCurrencyAmount),
                                    ProfitCurrencyAmount = MethodHelper.GetValue(item.ProfitCurrencyAmount),
                                };

                                VatType vatType = initializer.AllVatTypes.Where(d => d.Id == itemGroup.SingleVATTypeId).FirstOrDefault();
                                if (vatType != null)
                                {
                                    newItem.ExternalTAXItemId = vatType.ExternalTAXItemId;
                                }

                                if (initializer.AccountingSetting != null)
                                {
                                    newItem.ExternalVatCard = initializer.AccountingSetting.PayableVATCard;
                                }

                                VatTypePercentagePM myPercentagePM = initializer.AllVatPercentages.Where(d => d.VatTypeId == itemGroup.SingleVATTypeId).FirstOrDefault();
                                if (myPercentagePM != null)
                                {
                                    newItem.VatTypePercentage = MethodHelper.GetValue(myPercentagePM.Percentage);
                                }

                                if (string.IsNullOrEmpty(newItem.ExternalVatCard))
                                {
                                    if (initializer.AccountingSetting.AccountingSystemCode == "HV" || initializer.AccountingSetting.AccountingSystemCode == "RH")
                                    {
                                        newItem.ExternalVatCard = initializer.AccountingSetting.PayableVATCard;
                                    }
                                    else if (vatType != null)
                                    {
                                        newItem.ExternalVatCard = vatType.PayablesExternalId;
                                    }
                                }

                                group_Source.Add(newItem);
                            }
                        }
                    }
                }
            }
        }
        private void BuildTotalVATsDataGroups()
        {
            if (this.group_Source.Count > 0)
            {
                this.group_TotalVATs = (from items in group_Source
                                        group items by new { items.VatTypeId, items.VatTypePercentage, items.ExternalVatCard, items.ExternalTAXItemId, items.VatRecognizedPercentage } into g
                                        select new InvoiceTotalsClass()
                                        {
                                            Id = g.Key.VatTypeId,
                                            VatTypeId = g.Key.VatTypeId,
                                            VatTypePercentage = g.Key.VatTypePercentage,
                                            LocalCurrencyAmount = g.Sum(s => s.LocalCurrencyAmount),
                                            InvoiceCurrencyAmount = g.Sum(s => s.InvoiceCurrencyAmount),
                                            ProfitCurrencyAmount = g.Sum(s => s.ProfitCurrencyAmount),
                                            ExternalVatCard = g.Key.ExternalVatCard,
                                            ExternalTAXItemId = g.Key.ExternalTAXItemId,
                                            VatRecognizedPercentage = g.Key.VatRecognizedPercentage
                                        }).ToList();

                foreach (InvoiceTotalsClass item in group_TotalVATs)
                {
                    item.VatTypePercentage = MethodHelper.Roundd(item.VatTypePercentage, 3);

                    if (entityPM.TotalVATOnly)
                    {
                        item.LocalCurrencyVATAmount = MethodHelper.Roundd(item.LocalCurrencyAmount, 2);
                        item.ProfitCurrencyVATAmount = MethodHelper.Roundd(item.ProfitCurrencyAmount, 2);
                        item.InvoiceCurrencyVATAmount = MethodHelper.Roundd(item.InvoiceCurrencyAmount, 2);
                        item.LocalCurrencyAmount = 0;
                        item.ProfitCurrencyAmount = 0;
                        item.InvoiceCurrencyAmount = 0;
                    }

                    else
                    {
                        item.LocalCurrencyAmount = MethodHelper.Roundd(item.LocalCurrencyAmount, 2);
                        item.ProfitCurrencyAmount = MethodHelper.Roundd(item.ProfitCurrencyAmount, 2);
                        item.InvoiceCurrencyAmount = MethodHelper.Roundd(item.InvoiceCurrencyAmount, 2);
                        item.LocalCurrencyVATAmount = MethodHelper.Roundd((item.LocalCurrencyAmount * item.VatTypePercentage / 100), 2);
                        item.ProfitCurrencyVATAmount = MethodHelper.Roundd((item.ProfitCurrencyAmount * item.VatTypePercentage / 100), 2);
                        item.InvoiceCurrencyVATAmount = MethodHelper.Roundd((item.InvoiceCurrencyAmount * item.VatTypePercentage / 100), 2);
                    }
                }
            }
        }
        private void CalculateInvoiceAmounts()
        {
            double? sumOfVATsAmounts = group_TotalVATs.Sum(s => s.InvoiceCurrencyVATAmount);
            double? sumOfVATsAmounts_Local = group_TotalVATs.Sum(s => s.LocalCurrencyVATAmount);
            double? sumOfVATsAmounts_Profit = group_TotalVATs.Sum(s => s.ProfitCurrencyVATAmount);
            double? Amount = MethodHelper.Round(entityPM.SubTotalInInvoiceCurrency + sumOfVATsAmounts, 2);
            double? Amount_Local = MethodHelper.Round(entityPM.SubTotalInLocalCurrency + sumOfVATsAmounts_Local, 2);
            double? Amount_Profit = 0;

            if (entityPM.ProfitCurrencyId == entityPM.InvoiceCurrencyId)
            {
                Amount_Profit = Amount;
            }

            else
            {
                Amount_Profit = MethodHelper.Round(Amount_Local / entityPM.ProfitCurrencyExchangeRate, 2);
            }

            entityPM.AmountInInvoiceCurrency = Amount;
            entityPM.AmountInLocalCurrency = Amount_Local;
            entityPM.AmountInProfitCurrency = Amount_Profit;
        }
        APInvoiceTotalVATRepository invoiceTotalVatRepository;
        private void GenerateTotalVATs()
        {
             invoiceTotalVatRepository = new APInvoiceTotalVATRepository(initializer.Context);

            List<APInvoiceTotalVAT> dbTotalVats = invoiceTotalVatRepository.GetInvoiceTotalVatsByInvoiceId(entityPM.Id, entityPM.Tenant).ToList();

            foreach (APInvoiceTotalVAT item in dbTotalVats)
            {
                invoiceTotalVatRepository.Remove(item);
            }

            foreach (InvoiceTotalsClass item in group_TotalVATs)
            {
                APInvoiceTotalVAT itemPOCO = new APInvoiceTotalVAT()
                {
                    Id = IdCounter.GetNumber("APInvoiceTotalVAT", entityPM.Tenant).ToString(),
                    Tenant = entityPM.Tenant,
                    APInvoiceId = entityPM.Id,
                    VatTypeId = item.Id,
                    ExternalVATCard = item.ExternalVatCard,
                    ExternalTAXItemId = item.ExternalTAXItemId,
                    VatPercent = MethodHelper.GetValue(item.VatTypePercentage),
                    LocalVatableAmount = MethodHelper.GetValue(item.LocalCurrencyAmount),
                    InvoiceCurrencyVatableAmount = MethodHelper.GetValue(item.InvoiceCurrencyAmount),
                    ProfitVatableAmount = item.ProfitCurrencyAmount,
                    LocalVATAmount = item.LocalCurrencyVATAmount,
                    ProfitCurrencyVATAmount = item.ProfitCurrencyVATAmount,
                    InvoiceCurrencyVATAmount = item.InvoiceCurrencyVATAmount,
                };

                invoiceTotalVatRepository.Add(itemPOCO);
            }
            SubmitChangesForFullAccountingInvoices();

        }
        private bool GetAccountingActivated()
        {
            TenantQuery tenantQuery = new TenantQuery(entityPM.Tenant);
            TenantPM tenantPM = tenantQuery.GetSinglePM(entityPM.Tenant);
            return tenantPM.AccountingActivated;
        }
        private void SubmitChangesForFullAccountingInvoices()
        {
            if (GetAccountingActivated())
            {
                if (!initializer.IsNewEntity) invoiceTotalVatRepository.SubmitChanges();
            }
        }
        private void InitializeAmountDueFields()
        {
            entityPM.AmountDue = entityPM.AmountInInvoiceCurrency == null ? 0 : entityPM.AmountInInvoiceCurrency.Value;
            entityPM.AmountDueInLocalCurrency = entityPM.AmountInLocalCurrency == null ? 0 : entityPM.AmountInLocalCurrency.Value;
            entityPM.AmountDueInProfitCurrency = entityPM.AmountInProfitCurrency == null ? 0 : entityPM.AmountInProfitCurrency.Value;
        }
    }
}
