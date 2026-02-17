using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer;
using Logitude.Customs.BL.Messaging.U2L.FritzDeclaration.FritzDeclarationCls;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.U2L.FritzDeclaration
{
    public class FritzDeclarationMapping
    {
        ICustomContext context;
        public DeclarationFritz Get(DeclarationPM declarationPM)
        {
            if (context == null) context = CustomContext.GetContext(declarationPM.Tenant); 
            //CardRepository rep = new CardRepository(declarationPM.Tenant);
            //Card customerCard = rep.GetSingleCardByCode(declarationPM.CustomerCode, declarationPM.Tenant, false);
            
            var declarationFritz = new DeclarationFritz();
            
            declarationFritz.CustomerVatNo = declarationPM.ImporterCode;
            declarationFritz.CustomFileNo = declarationPM.CustomFileNo;
            declarationFritz.DeclarationStatusTypeCode = declarationPM.DeclarationStatusTypeCode; 
             
            declarationFritz.Customer = new Logitude.Customs.BL.Messaging.U2L.FritzDeclaration.FritzDeclarationCls.Customer() { CustomerName = declarationPM.CustomerName, CustomerNumber = declarationPM.CustomerCode };
            declarationFritz.AgentId = declarationPM.AgentId;
           
            declarationFritz.DeclarationNumber = declarationPM.DeclarationNumber; 
            declarationFritz.VersionId = declarationPM.VersionId; 
            declarationFritz.DeclarationOfficeCode = declarationPM.DeclarationOfficeCode;   
            declarationFritz.PaymentDate = declarationPM.PaymentDate;
            declarationFritz.HatraDate = declarationPM.HatraDate;
            if (declarationPM.ProcedureCurrentCode != null)
            {
                GovernmentProcedureTypeQueryService procedureQuery = new GovernmentProcedureTypeQueryService(context);
                GovernmentProcedureTypePM procedure = procedureQuery.GetSingle(declarationPM.ProcedureCurrentCode, true, false);
                if (procedure != null)
                {
                    if (!String.IsNullOrWhiteSpace(procedure.LocalName))
                    {
                        declarationFritz.ProcedureCurrentName = procedure.LocalName;
                    }
                    else if (!String.IsNullOrWhiteSpace(procedure.EnglishName))
                    {
                        declarationFritz.ProcedureCurrentName = procedure.EnglishName;
                    }
                }
                declarationFritz.ProcedureCurrentCode = declarationPM.ProcedureCurrentCode;
            }
            declarationFritz.LoadingFactor = declarationPM.LoadingFactor;
            declarationFritz.DealValue = declarationPM.DealValue;
            declarationFritz.CIFValue = declarationPM.CIFValue;
            declarationFritz.DealValueWithoutFactor = declarationPM.DealValueWithoutFactor;
            declarationFritz.TotalTax = declarationPM.TotalTax;
            declarationFritz.PaymentOrderNumber = declarationPM.PaymentOrderNumber;
            declarationFritz.PaymentStatusCode = declarationPM.PaymentStatusCode;
            
            if (declarationPM.TaxationDateTime.HasValue)
            {
                declarationFritz.TaxationDateTime = (DateTime)declarationPM.TaxationDateTime; 
            }
            declarationFritz.AutonomyRegionTypeCode = declarationPM.AutonomyRegionTypeCode;

            declarationFritz.Consignment = GetConsignment(declarationPM);


            declarationFritz.SupplierInvoice = GetSupplierInvoice(declarationPM);

            declarationFritz.DeclarationTax = GetDeclarationTax(declarationPM);
                
            
            return declarationFritz;
        }


        private DeclarationConsignment[] GetConsignment(DeclarationPM declarationPM)
        {
            
            if (context == null) context = CustomContext.GetContext(declarationPM.Tenant);
            var DeclarationConsignmentList = new List<DeclarationConsignment>();

            foreach (var consignment in declarationPM.Consignments)
            {
                var DeclarationConsignment = new DeclarationConsignment();
                DeclarationConsignment.CargoTypeCode = consignment.CargoTypeCode;
                
                DeclarationConsignment.ManifestNumber = consignment.ManifestNumber;
                DeclarationConsignment.SecondCargoID = consignment.SecondCargoID;
                DeclarationConsignment.ThirdCargoID = consignment.ThirdCargoID; 
                if (consignment.ManifestDate.HasValue)
                {
                    DeclarationConsignment.ManifestDate = consignment.ManifestDate; 
                }
                if (consignment.UnloadDate.HasValue)
                {
                    DeclarationConsignment.UnloadDate = consignment.UnloadDate;
                }
                DeclarationConsignment.OriginCountryCode = consignment.OriginCountryCode;
                DeclarationConsignment.LoadingPortCode = consignment.LoadingPortCode; 
                
                DeclarationConsignment.UnloadPortCode = consignment.UnloadPortCode;
                
                
                DeclarationConsignment.StorageSiteCode = consignment.StorageSiteCode;

                DeclarationConsignment.ReceiverWarehouseCode = consignment.ReceiverWarehouseCode; 
                
                DeclarationConsignment.IsLastReleaseFromWarehous = consignment.IsLastReleaseFromWarehous;
                 
                DeclarationConsignment.CargoDescription = consignment.CargoDescription; 
                
                DeclarationConsignment.SequenceNumeric = (int)consignment.SequenceNumeric;

                var DeclarationConsignmentPackageList = new List<DeclarationConsignmentConsignmentPackage>();
                for (int consignmentPackageSeq = 0; consignmentPackageSeq < consignment.ConsignmentPackages.Count(); consignmentPackageSeq++)
                {
                    var consignmentPackagePM = consignment.ConsignmentPackages[consignmentPackageSeq];
                    var DeclarationConsignmentpackage = new DeclarationConsignmentConsignmentPackage();
                    DeclarationConsignmentpackage.SequenceNumeric = consignmentPackagePM.SequenceNumeric;
                    DeclarationConsignmentpackage.PackageMeasureQualifierCode = consignmentPackagePM.PackageMeasureQualifierCode;
                    DeclarationConsignmentpackage.PackageMeasureQualifierName = consignmentPackagePM.PackageMeasureQualifierName;
                    
                    if (consignmentPackagePM.PackageQuantity.HasValue)
                    {
                        DeclarationConsignmentpackage.PackageQuantity = consignmentPackagePM.PackageQuantity.Value; 
                    }
                    if (consignmentPackagePM.GrossMassMeasure.HasValue)
                    {
                        DeclarationConsignmentpackage.GrossMassMeasure = consignmentPackagePM.GrossMassMeasure; 
                    }
                    if (consignmentPackagePM.PackageTypeCode != null)
                    {
                        DeclarationConsignmentpackage.PackageTypeCode = consignmentPackagePM.PackageTypeCode;
                        PackingTypeQueryService packingTypeQuery = new PackingTypeQueryService(context);
                        PackingTypePM packingType = packingTypeQuery.GetSingle(consignmentPackagePM.PackageTypeCode, true, false);
                        if (packingType != null)
                        {
                            if (!String.IsNullOrWhiteSpace(packingType.LocalName))
                            {
                                DeclarationConsignmentpackage.PackageTypeName = packingType.LocalName;
                            }
                            else if (!String.IsNullOrWhiteSpace(packingType.EnglishName))
                            {
                                DeclarationConsignmentpackage.PackageTypeName = packingType.EnglishName;
                            }
                        }
                    }
                    DeclarationConsignmentPackageList.Add(DeclarationConsignmentpackage);
                }
                if (DeclarationConsignmentPackageList != null && DeclarationConsignmentPackageList.Count() > 0)
                {
                    DeclarationConsignment.ConsignmentPackage = DeclarationConsignmentPackageList.ToArray();
                }

                DeclarationConsignmentList.Add(DeclarationConsignment);
            }
            if (DeclarationConsignmentList != null && DeclarationConsignmentList.Count() > 0)
            {
                return DeclarationConsignmentList.ToArray();
            }
            return null;

        }

        private DeclarationSupplierInvoice[] GetSupplierInvoice(DeclarationPM declarationPM)
        {
            
            if (context == null) context = CustomContext.GetContext(declarationPM.Tenant);
            var SupplierInvoiceList = new List<DeclarationSupplierInvoice>();

            foreach (var supplierInvoice in declarationPM.SupplierInvoices)
            {
                var SupplierInvoice = new DeclarationSupplierInvoice();
                SupplierInvoice.SequenceNumeric = supplierInvoice.SequenceNumeric;
                SupplierInvoice.InvoiceNumber = supplierInvoice.InvoiceNumber;
                SupplierInvoice.IssueDate = supplierInvoice.IssueDate;
                
                if (!String.IsNullOrWhiteSpace(supplierInvoice.VendorId))
                {
                    CustomsVendorQueryService vendorQuery = new CustomsVendorQueryService(context);
                    CustomsVendorPM vendor = vendorQuery.GetSingle(supplierInvoice.VendorId, true, false);
                    if (vendor != null)
                    {
                        SupplierInvoice.VendorNumber = vendor.VendorNumber;
                    }
                }
                
                SupplierInvoice.AccountTypeCode = supplierInvoice.AccountTypeCode;
                
                SupplierInvoice.IssueCountryCode = supplierInvoice.IssueCountryCode;
                
                SupplierInvoice.IncotermCode = supplierInvoice.IncotermCode;
                if (supplierInvoice.InvoiceAmount.HasValue)
                {
                    SupplierInvoice.InvoiceAmount = supplierInvoice.InvoiceAmount;
                    SupplierInvoice.InvoiceCurrencyTypeCode = supplierInvoice.InvoiceCurrencyTypeCode;
                }
                
                SupplierInvoice.TotalFreightInNIS = supplierInvoice.TotalFreightInNIS;
                SupplierInvoice.PreferenceDocumentTypeCode = supplierInvoice.PreferenceDocumentTypeCode;
                SupplierInvoice.IsPreference = supplierInvoice.IsPreference;
                SupplierInvoice.TotalFreightInFreightCurrency = supplierInvoice.TotalFreightInFreightCurrency;
                SupplierInvoice.FreightCurrencyTypeCode = supplierInvoice.FreightCurrencyTypeCode;
                SupplierInvoice.ExchangeRate = supplierInvoice.ExchangeRate;
                SupplierInvoice.InsruanceCurrencyTypeCode = supplierInvoice.InsruanceCurrencyTypeCode;
                SupplierInvoice.InsuranceAmount = supplierInvoice.InsuranceAmount;
                SupplierInvoice.InsruancePercentage = supplierInvoice.InsruancePercentage;

                SupplierInvoice.SupplierInvoiceItem = GetSupplierInvoiceItem(supplierInvoice);

                var supplierInvoiceModificationList = new List<DeclarationSupplierInvoiceSupplierInvoiceModification>();
                foreach (var supplierInvoiceModificationPM in supplierInvoice.SupplierInvoiceModifications)
                {
                    var supplierInvoiceModification = new DeclarationSupplierInvoiceSupplierInvoiceModification();
                    
                    supplierInvoiceModification.TypeCode = supplierInvoiceModificationPM.TypeCode;
                    supplierInvoiceModification.CurrencyTypeCode = supplierInvoiceModificationPM.CurrencyTypeCode;
                    supplierInvoiceModification.Amount = supplierInvoiceModificationPM.Amount;
                    supplierInvoiceModificationList.Add(supplierInvoiceModification);
                }

                if (supplierInvoiceModificationList != null && supplierInvoiceModificationList.Count() > 0)
                {
                    SupplierInvoice.SupplierInvoiceModification = supplierInvoiceModificationList.ToArray();
                }
                

                SupplierInvoiceList.Add(SupplierInvoice);
            }
            if (SupplierInvoiceList != null && SupplierInvoiceList.Count() > 0)
            {
                return SupplierInvoiceList.ToArray();
            }
            return null;
        }


        private DeclarationSupplierInvoiceSupplierInvoiceItem[] GetSupplierInvoiceItem(SupplierInvoicePM supplierInvoice)
        {
         
            var supplierInvoiceItemList = new List<DeclarationSupplierInvoiceSupplierInvoiceItem>();

            foreach (var supplierInvoiceItem in supplierInvoice.SupplierInvoiceItems)
            {
                var SupplierInvoiceItem = new DeclarationSupplierInvoiceSupplierInvoiceItem();
                SupplierInvoiceItem.SequenceNumeric = supplierInvoice.SequenceNumeric;
                SupplierInvoiceItem.ClassificationCode = supplierInvoiceItem.ClassificationCode;
                SupplierInvoiceItem.TradeAgreemenCode = supplierInvoiceItem.TradeAgreementCode;
                if (supplierInvoiceItem.StatisticQuantity.HasValue)
                {
                    SupplierInvoiceItem.StatisticQuantity = supplierInvoiceItem.StatisticQuantity;
                    if (!String.IsNullOrWhiteSpace(supplierInvoiceItem.StatisticQuantityType))
                    {
                        SupplierInvoiceItem.StatisticQuantityType = supplierInvoiceItem.StatisticQuantityType;
                        /*
                        MeasurmentUnitQueryService MeasurmentUnitQuery = new MeasurmentUnitQueryService(context);
                        MeasurmentUnitPM MeasurmentUnit = MeasurmentUnitQuery.GetSingle(supplierInvoiceItem.StatisticQuantityType, true, false);
                        if (MeasurmentUnit != null)
                        {
                            if (!String.IsNullOrWhiteSpace(MeasurmentUnit.LocalName))
                            {
                                SupplierInvoiceItem.StatisticQuantityType = MeasurmentUnit.LocalName;
                            }
                            else if (!String.IsNullOrWhiteSpace(MeasurmentUnit.EnglishName))
                            {
                                SupplierInvoiceItem.StatisticQuantityType = MeasurmentUnit.EnglishName;
                            }
                        }
                         * */
                    }
                }
                if (supplierInvoiceItem.InvoiceQuantity.HasValue)
                {
                    SupplierInvoiceItem.InvoiceQuantity = supplierInvoiceItem.InvoiceQuantity;
                    SupplierInvoiceItem.InvoiceQuantityType = supplierInvoiceItem.InvoiceQuantityType;
                    if (!String.IsNullOrWhiteSpace(supplierInvoiceItem.InvoiceQuantityType))
                    {
                        SupplierInvoiceItem.InvoiceQuantityType = supplierInvoiceItem.InvoiceQuantityType;
                        /*
                        MeasurmentUnitQueryService MeasurmentUnitQuery = new MeasurmentUnitQueryService(context);
                        MeasurmentUnitPM MeasurmentUnit = MeasurmentUnitQuery.GetSingle(supplierInvoiceItem.InvoiceQuantityType, true, false);
                        if (MeasurmentUnit != null)
                        {
                            if (!String.IsNullOrWhiteSpace(MeasurmentUnit.LocalName))
                            {
                                SupplierInvoiceItem.InvoiceQuantityType = MeasurmentUnit.LocalName;
                            }
                            else if (!String.IsNullOrWhiteSpace(MeasurmentUnit.EnglishName))
                            {
                                SupplierInvoiceItem.InvoiceQuantityType = MeasurmentUnit.EnglishName;
                            }
                        }
                         * */
                    }
                }
                if (supplierInvoiceItem.ItemPrice.HasValue)
                {
                    SupplierInvoiceItem.ItemPrice = supplierInvoiceItem.ItemPrice;
                }

                SupplierInvoiceItem.OriginCountryCode = supplierInvoiceItem.OriginCountryCode;
                
                var supplierInvoiceItemProcesTypes = new List<DeclarationSupplierInvoiceSupplierInvoiceItemSupplierInvioceItemProcessType>();
                if (supplierInvoiceItem.SupplierInvoiceItemProcesTypes != null && supplierInvoiceItem.SupplierInvoiceItemProcesTypes.Count() > 0)
                {
                    foreach (var supplierInvoiceItemProcesTypePM in supplierInvoiceItem.SupplierInvoiceItemProcesTypes.Where(d => d.DeclarationId == supplierInvoiceItem.DeclarationId && d.InvoiceCounterKey == supplierInvoiceItem.CounterKey && d.InvoiceItemLineNumber == supplierInvoiceItem.LineNumber).ToList())
                    {
                        if(!String.IsNullOrWhiteSpace(supplierInvoiceItemProcesTypePM.ProcessTypeCode))
                        {
                            var supplierInvoiceItemProcesType = new DeclarationSupplierInvoiceSupplierInvoiceItemSupplierInvioceItemProcessType();
                            supplierInvoiceItemProcesType.ProcessTypeCode = supplierInvoiceItemProcesTypePM.ProcessTypeCode;
                            supplierInvoiceItemProcesTypes.Add(supplierInvoiceItemProcesType);
                        }
                    }
                }
                if (supplierInvoiceItemProcesTypes != null && supplierInvoiceItemProcesTypes.Count() > 0)
                {
                    SupplierInvoiceItem.SupplierInvioceItemProcessType = supplierInvoiceItemProcesTypes.ToArray();
                }
                SupplierInvoiceItem.SupplierInvoiceItemTax = GetSupplierInvoiceItemTax(supplierInvoiceItem);
                
                SupplierInvoiceItem.SupplierInvioceItemCertificate = GetSupplierInvoiceItemCertificate(supplierInvoiceItem); 

                supplierInvoiceItemList.Add(SupplierInvoiceItem);
            }
            if (supplierInvoiceItemList != null && supplierInvoiceItemList.Count() > 0)
            {
                return supplierInvoiceItemList.ToArray();
            }
            return null;
        }

        

        private DeclarationSupplierInvoiceSupplierInvoiceItemSupplierInvoiceItemTax[] GetSupplierInvoiceItemTax(SupplierInvoiceItemPM supplierInvoiceItem)
        {

            if (context == null) context = CustomContext.GetContext(supplierInvoiceItem.Tenant);

            var supplierInvoiceItemTaxList = new List<DeclarationSupplierInvoiceSupplierInvoiceItemSupplierInvoiceItemTax>();


            foreach (var tax in supplierInvoiceItem.SupplierInvoiceItemTaxes)
            {
                var supplierInvoiceItemTax = new DeclarationSupplierInvoiceSupplierInvoiceItemSupplierInvoiceItemTax();

                supplierInvoiceItemTax.TaxTypeCode = tax.TaxTypeCode;

                if (tax.TaxBaseAmount.HasValue)
                {
                    supplierInvoiceItemTax.TaxBaseAmount = tax.TaxBaseAmount;
                }
                if (tax.TaxAmount.HasValue)
                {
                    supplierInvoiceItemTax.TaxAmount = tax.TaxAmount;
                }
                if (tax.DeferedTaxAmount.HasValue)
                {
                    supplierInvoiceItemTax.DeferedTaxAmount = tax.DeferedTaxAmount;
                }
                if (tax.TaxRate.HasValue)
                {
                    supplierInvoiceItemTax.TaxRate = tax.TaxRate;
                }
                supplierInvoiceItemTax.DefinedPerUnitMeasure = tax.DefinedPerUnitMeasure;
                supplierInvoiceItemTax.AlternateDefinedPerUnitMeasure = tax.AlternateDefinedPerUnitMeasure;
                supplierInvoiceItemTax.DefinedPerUnitQuantity = tax.DefinedPerUnitQuantity;
                supplierInvoiceItemTax.AlternateDefinedPerUnitQuant = tax.AlternateDefinedPerUnitQuant;
                supplierInvoiceItemTax.AlternateRate = tax.AlternateRate;
                supplierInvoiceItemTax.AlternateMeasurementUnitCode = tax.AlternateMeasurementUnitCode;
                supplierInvoiceItemTax.MeasurementUnitCode = tax.MeasurementUnitCode;
                supplierInvoiceItemTaxList.Add(supplierInvoiceItemTax);
            }
            if (supplierInvoiceItemTaxList != null && supplierInvoiceItemTaxList.Count() > 0)
            {
                return supplierInvoiceItemTaxList.ToArray();
            }
            return null;
        }


        private DeclarationSupplierInvoiceSupplierInvoiceItemSupplierInvioceItemCertificate[] GetSupplierInvoiceItemCertificate(SupplierInvoiceItemPM supplierInvoiceItem)
        {
            
            if (context == null) context = CustomContext.GetContext(supplierInvoiceItem.Tenant);
            var supplierInvioceItemCertificateList = new List<DeclarationSupplierInvoiceSupplierInvoiceItemSupplierInvioceItemCertificate>();
            if (supplierInvoiceItem.SupplierInvioceItemCertificats != null && supplierInvoiceItem.SupplierInvioceItemCertificats.Count() > 0)
            {
                foreach (var supplierInvoiceItemCertificatePM in supplierInvoiceItem.SupplierInvioceItemCertificats.Where(d => d.DeclarationId == supplierInvoiceItem.DeclarationId && d.InvoiceCounterKey == supplierInvoiceItem.CounterKey && d.LineNumber == supplierInvoiceItem.LineNumber).ToList())
                {
                    var supplierInvioceItemCertificate = new DeclarationSupplierInvoiceSupplierInvoiceItemSupplierInvioceItemCertificate();

                    supplierInvioceItemCertificate.ReqConfirmationTypeCode = supplierInvoiceItemCertificatePM.ReqConfirmationTypeCode;
                    supplierInvioceItemCertificate.SequenceNumeric = supplierInvoiceItemCertificatePM.SequenceNumeric;

                    supplierInvioceItemCertificate.CertificateNumber = supplierInvoiceItemCertificatePM.CertificateNumber;

                    supplierInvioceItemCertificate.CertificateExemptionTypeCode = supplierInvoiceItemCertificatePM.CertificateExemptionTypeCode;
                    supplierInvioceItemCertificate.AttachmentTypeCode = supplierInvoiceItemCertificatePM.AttachmentTypeCode;
                    
                    supplierInvioceItemCertificate.ResponseConfirmationTypeCode = supplierInvoiceItemCertificatePM.ResConfirmationTypeCode;

                    supplierInvioceItemCertificateList.Add(supplierInvioceItemCertificate);
                }
                if (supplierInvioceItemCertificateList != null && supplierInvioceItemCertificateList.Count() > 0)
                {
                    return supplierInvioceItemCertificateList.ToArray();
                }
            }
            return null;
        }

        
        private DeclarationDeclarationTax[] GetDeclarationTax(DeclarationPM declarationPM)
        {

            if (context == null) context = CustomContext.GetContext(declarationPM.Tenant);

            var declarationTaxList = new List<DeclarationDeclarationTax>();

            foreach (var Tax in declarationPM.DeclarationTaxes)
            {
                var declarationTax = new DeclarationDeclarationTax();

                declarationTax.TaxTypeCode = Tax.TaxTypeCode;
                
                if (Tax.TaxBaseAmount.HasValue)
                {
                    declarationTax.TaxBaseAmount = Tax.TaxBaseAmount;
                }
                if (Tax.TotalAmount.HasValue)
                {
                    declarationTax.TotalAmount = Tax.TotalAmount;
                    declarationTax.TaxToPay = Tax.TotalAmount;
                }
                if (Tax.DeferredTaxAmount.HasValue)
                {
                    declarationTax.DeferredTaxAmount = Tax.DeferredTaxAmount;
                }
                declarationTaxList.Add(declarationTax);
            }
            if (declarationTaxList != null && declarationTaxList.Count() > 0)
            {
                return declarationTaxList.ToArray();
            }
            return null;
        }

        public DeclarationFritz GetSingle(int tenant, string declarationId)
        {
            DeclarationPM MyDeclarationPM;
            var context = CustomContext.GetContext(tenant);
            var myQueryService = new DeclarationQueryService(context);
            MyDeclarationPM = myQueryService.GetSingle(
                declarationId
                , true, false); 
            var myFile = Get(MyDeclarationPM);
            return myFile;
         
        }

        public string GetXml(int tenant, string declarationId)
        {

            var myFile = GetSingle(tenant, declarationId);
            var xml = XmlGenericUtil<DeclarationFritz>.SerializeObject(myFile);
            return xml;
        }

        public string GetXml(DeclarationPM myDeclarationPM)
        {

            var myFile = Get(myDeclarationPM);
            var xml = XmlGenericUtil<DeclarationFritz>.SerializeObject(myFile);
            return xml;
        }

    }
}
