using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class SupplierInvoiceItemsTaxQueryService
    {
        public override void GetComposition(Simplog.Server.Infrastructure.EntityKeyFields entityKeys, SupplierInvoiceItemsTaxPM entityPM)
        {
            ICustomContext context = MainContext as CustomContext;
            SupplierInvoiceItemsTaxKeys supplierInvoiceItemTaxKeys = entityKeys as SupplierInvoiceItemsTaxKeys;

            //SupplierInvoiceItemsTaxesModQueryService supplierInvoiceItemsTaxesModificationQueryService = new SupplierInvoiceItemsTaxesModQueryService(context);
            //entityPM.SupplierInvoiceItemsTaxesMods = supplierInvoiceItemsTaxesModificationQueryService.GetMulti(supplierInvoiceItemTaxKeys, false);
            base.GetComposition(entityKeys, entityPM);
        }

        public List<SupplierInvoiceItemsTaxPM> GetDeclarationTaxesForDeclarationId(string declarationId, int tenant)
        {
            ICustomContext context = MainContext as CustomContext;
            IQueryable<SupplierInvoiceItemsTax> taxes = repository.GetSupplierInvoiceItemTaxesForDeclarationId(declarationId, tenant);
            List<SupplierInvoiceItemsTaxPM> taxpms = (from a in taxes
                                                      select new SupplierInvoiceItemsTaxPM()
                                             {
                                                 DeclarationId = a.DeclarationId,
                                                 AlternateDefinedPerUnitMeasure = a.AlternateDefinedPerUnitMeasure,
                                                 TaxBaseAmount = a.TaxBaseAmount,
                                                 TaxTypeCode = a.TaxTypeCode,
                                                 Tenant = a.Tenant,
                                                 AlternateDefinedPerUnitQuant = a.AlternateDefinedPerUnitQuant,
                                                 TaxTypeName = a.ParagraphType.LocalName,
                                                 AlternateMeasurementUnitCode = a.AlternateMeasurementUnitCode,
                                                 AlternateRate = a.AlternateRate,
                                                 DeferedTaxAmount = a.DeferedTaxAmount,
                                                 DefinedPerUnitMeasure = a.DefinedPerUnitMeasure,
                                                 DefinedPerUnitQuantity = a.DefinedPerUnitQuantity,
                                                 InvoiceCounterKey = a.InvoiceCounterKey,
                                                 LineNumber = a.LineNumber,
                                                 TaxAmount = a.TaxAmount,
                                                 MeasurementUnitCode = a.MeasurementUnitCode,
                                                 TaxRate = a.TaxRate,
                                                 TotalBtlCoverageNIS = a.TotalBtlCoverageNIS,
                                                 TradeAgreementTypeCode = a.TradeAgreementTypeCode,
                                                 TradeLevyNumber = a.TradeLevyNumber,

                                             }).ToList();
            //SupplierInvoiceItemsTaxesModQueryService supplierInvoiceItemsTaxesModificationQueryService = new SupplierInvoiceItemsTaxesModQueryService(context);
            //List<SupplierInvoiceItemsTaxesModPM> modifications = supplierInvoiceItemsTaxesModificationQueryService.GetSupplierInvoiceItemTaxModificationsForDeclaration(declarationId, tenant);
            //foreach (SupplierInvoiceItemsTaxPM tax in taxpms)
            //{
            //    tax.SupplierInvoiceItemsTaxesMods = modifications.Where(d => d.InvoiceCounterKey == tax.InvoiceCounterKey && d.LineNumber == tax.LineNumber && d.TaxTypeCode == tax.TaxTypeCode).ToList();
            //}

            return taxpms;
        }


        public List<SupplierInvoiceItemsTaxPM> GetSupplierInvoiceItemsTaxForInvoiceItem(string declarationId, int invoiceCounterKey, int LineNumber,int tenant)
        {
            ICustomContext context = MainContext as CustomContext;
            IQueryable<SupplierInvoiceItemsTax> taxes = repository.GetSupplierInvoiceItemTaxesForInvoiceItem(declarationId,invoiceCounterKey,LineNumber, tenant);
            List<SupplierInvoiceItemsTaxPM> taxpms = (from a in taxes
                                                      select new SupplierInvoiceItemsTaxPM()
                                                      {
                                                          DeclarationId = a.DeclarationId,
                                                          AlternateDefinedPerUnitMeasure = a.AlternateDefinedPerUnitMeasure,
                                                          TaxBaseAmount = a.TaxBaseAmount,
                                                          TaxTypeCode = a.TaxTypeCode,
                                                          Tenant = a.Tenant,
                                                          AlternateDefinedPerUnitQuant = a.AlternateDefinedPerUnitQuant,
                                                          TaxTypeName = a.ParagraphType.LocalName,
                                                          AlternateMeasurementUnitCode = a.AlternateMeasurementUnitCode,
                                                          AlternateRate = a.AlternateRate,
                                                          DeferedTaxAmount = a.DeferedTaxAmount,
                                                          DefinedPerUnitMeasure = a.DefinedPerUnitMeasure,
                                                          DefinedPerUnitQuantity = a.DefinedPerUnitQuantity,
                                                          InvoiceCounterKey = a.InvoiceCounterKey,
                                                          LineNumber = a.LineNumber,
                                                          TaxAmount = a.TaxAmount,
                                                          MeasurementUnitCode = a.MeasurementUnitCode,
                                                          TaxRate = a.TaxRate,
                                                          TotalBtlCoverageNIS = a.TotalBtlCoverageNIS,
                                                          TradeAgreementTypeCode = a.TradeAgreementTypeCode,
                                                          TradeLevyNumber = a.TradeLevyNumber,
                                                           
                                                      }).ToList();
            //SupplierInvoiceItemsTaxesModQueryService supplierInvoiceItemsTaxesModificationQueryService = new SupplierInvoiceItemsTaxesModQueryService(context);
            //List<SupplierInvoiceItemsTaxesModPM> modifications = supplierInvoiceItemsTaxesModificationQueryService.GetSupplierInvoiceItemTaxModificationsForDeclaration(declarationId, tenant);
            //foreach (SupplierInvoiceItemsTaxPM tax in taxpms)
            //{
            //    tax.SupplierInvoiceItemsTaxesMods = modifications.Where(d => d.InvoiceCounterKey == tax.InvoiceCounterKey && d.LineNumber == tax.LineNumber && d.TaxTypeCode == tax.TaxTypeCode).ToList();
            //}

            return taxpms;
        }


        public List<SupplierInvoiceItemsTaxPM> GetSupplierInvoiceItemsTaxesForSupplierInvoice(string declarationId, int invoiceCounterKey, int tenant, List<int> FilterLine = null)
        {
            List<SupplierInvoiceItemsTax> SupplierInvoiceItemsTaxes = repository.GetSupplierInvoiceItemsTaxesForSupplierInvoice(declarationId, invoiceCounterKey, tenant, FilterLine);
            List<SupplierInvoiceItemsTaxPM> SupplierInvoiceItemsTaxPMs = (from a in SupplierInvoiceItemsTaxes
                                                                          select new SupplierInvoiceItemsTaxPM()
                                                                          {
                                                                              DeclarationId = a.DeclarationId,
                                                                              AlternateDefinedPerUnitMeasure = a.AlternateDefinedPerUnitMeasure,
                                                                              AlternateDefinedPerUnitQuant = a.AlternateDefinedPerUnitQuant,
                                                                              AlternateMeasurementUnitCode = a.AlternateMeasurementUnitCode,
                                                                              AlternateRate = a.AlternateRate,
                                                                              DeferedTaxAmount = a.DeferedTaxAmount,
                                                                              DefinedPerUnitMeasure = a.DefinedPerUnitMeasure,
                                                                              DefinedPerUnitQuantity = a.DefinedPerUnitQuantity,
                                                                              MeasurementUnitCode = a.MeasurementUnitCode,
                                                                              TaxAmount = a.TaxAmount,
                                                                              TaxBaseAmount = a.TaxBaseAmount,
                                                                              TaxRate = a.TaxRate,
                                                                              TaxTypeCode = a.TaxTypeCode,
                                                                              TaxTypeName = a.ParagraphType != null ? a.ParagraphType.LocalName : null,
                                                                              TotalBtlCoverageNIS = a.TotalBtlCoverageNIS,
                                                                              TradeAgreementTypeCode = a.TradeAgreementTypeCode,
                                                                              TradeAgreementTypeName = a.TradeAgreement != null ? a.TradeAgreement.LocalName : null,
                                                                              TradeLevyNumber = a.TradeLevyNumber,
                                                                              InvoiceCounterKey = a.InvoiceCounterKey,
                                                                              LineNumber = a.LineNumber,
                                                                              Tenant = a.Tenant,
                                                                              AlternateMeasurementUnitName = a.AlternateMeasurmentUnit != null? a.AlternateMeasurmentUnit.LocalName : null,
                                                                              MeasurementUnitName = a.MeasurmentUnit != null? a.MeasurmentUnit.LocalName : null,
                                                                              
                                                                          }).ToList();
            return SupplierInvoiceItemsTaxPMs;
        }

        public List<SupplierInvoiceItemsTaxPM> GetSupplierInvoiceItemsTaxesForSupplierInvoiceWithSpecificKeys(string declarationId, int invoiceCounterKey,List<int> itemsLineNumbers, int tenant)
        {
            List<SupplierInvoiceItemsTax> SupplierInvoiceItemsTaxes = repository.GetSupplierInvoiceItemsTaxesForSupplierInvoiceWithSpecificKeys(declarationId, invoiceCounterKey, itemsLineNumbers,tenant);
            List<SupplierInvoiceItemsTaxPM> SupplierInvoiceItemsTaxPMs = (from a in SupplierInvoiceItemsTaxes
                                                                          select new SupplierInvoiceItemsTaxPM()
                                                                          {
                                                                              DeclarationId = a.DeclarationId,
                                                                              AlternateDefinedPerUnitMeasure = a.AlternateDefinedPerUnitMeasure,
                                                                              AlternateDefinedPerUnitQuant = a.AlternateDefinedPerUnitQuant,
                                                                              AlternateMeasurementUnitCode = a.AlternateMeasurementUnitCode,
                                                                              AlternateRate = a.AlternateRate,
                                                                              DeferedTaxAmount = a.DeferedTaxAmount,
                                                                              DefinedPerUnitMeasure = a.DefinedPerUnitMeasure,
                                                                              DefinedPerUnitQuantity = a.DefinedPerUnitQuantity,
                                                                              MeasurementUnitCode = a.MeasurementUnitCode,
                                                                              TaxAmount = a.TaxAmount,
                                                                              TaxBaseAmount = a.TaxBaseAmount,
                                                                              TaxRate = a.TaxRate,
                                                                              TaxTypeCode = a.TaxTypeCode,
                                                                              TaxTypeName = a.ParagraphType != null ? a.ParagraphType.LocalName : null,
                                                                              TotalBtlCoverageNIS = a.TotalBtlCoverageNIS,
                                                                              TradeAgreementTypeCode = a.TradeAgreementTypeCode,
                                                                              TradeAgreementTypeName = a.TradeAgreement != null ? a.TradeAgreement.LocalName : null,
                                                                              TradeLevyNumber = a.TradeLevyNumber,
                                                                              InvoiceCounterKey = a.InvoiceCounterKey,
                                                                              LineNumber = a.LineNumber,
                                                                              Tenant = a.Tenant,
                                                                              AlternateMeasurementUnitName = a.AlternateMeasurmentUnit != null ? a.AlternateMeasurmentUnit.LocalName : null,
                                                                              MeasurementUnitName = a.MeasurmentUnit != null ? a.MeasurmentUnit.LocalName : null,

                                                                          }).ToList();
            return SupplierInvoiceItemsTaxPMs;
        }

    }
}
