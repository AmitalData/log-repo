using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityQueryServicesExt;

namespace Logitude.Customs.BL.Validators
{
    public class CustomsRequiredFieldsValidator
    {

        public static CustomsRequiredFieldErrors GetRequiredFieldErrorsForDeclaration(string declarationId, int tenant, DeclarationPM declarationPM = null)
        {
            DeclarationPM declaration = null;
            CustomsRequiredFieldErrors requiredErrors = new CustomsRequiredFieldErrors() { RequiredFields = new List<CustomsRequiredFieldsErrorItem>(), };
            ICustomContext context = CustomContext.GetContext(tenant);
            CustomsRequiredFieldQueryService customsRequiredFieldQueryService = new CustomsRequiredFieldQueryService(context);
            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(0);
            DeclarationQueryService declarationQueryService = new DeclarationQueryService(context);
            SupplierInvoiceQueryService invoiceQuery = new SupplierInvoiceQueryService(context);
            SupplierInvoiceItemQueryService invoiceItemQuery = new SupplierInvoiceItemQueryService(context);

            List<SupplierInvoicePM> invoicePMs = invoiceQuery.GetSupplierInvoicesForDeclaration(declarationId, tenant);//mohammad fix wi 20751
            ///List<SupplierInvoiceItemPM> invoiceItemPMs = invoiceItemQuery.GetSupplierInvoiceItemsForDeclaration(declarationId, tenant);//mohammad fix wi 20751
            var fromCache = true;
            if (fromCache)
            {
                var cacheKey = "DeclarationPM.RequiredVldAfterUpdate" + declarationId;
                declaration = CacheManager.CacheWrapper.Remove(cacheKey) as DeclarationPM;

            }
            if (declaration == null)
            {
                declarationQueryService.LoadSupplierInvoicesWithItems = false;
                declaration = declarationQueryService.GetSingle(declarationId, true, false);
            }

            if (declaration == null)
            {
                return null;
            }

            string isExport = declaration.Direction == "E" ? "E" : "I";

            DeclarationPaymentQueryService DeclarationPaymentQuery = new DeclarationPaymentQueryService(context);
            DeclarationPaymentPM payment = DeclarationPaymentQuery.GetSingle(declarationId, true, fromCache);


            #region declaration entity
            ObjectTable declarationTable = objectTabelRepository.GetObjectTableByName("Customs.Declaration", 0, fromCache);

            List<CustomsRequiredFieldPM> declarationRequiredFields = customsRequiredFieldQueryService.GetCustomRequiredFieldsByObjectTableFromCache(declarationTable.Id, tenant , isExport);
            List<PropertyInfo> properties = GetPropertiesForEntity("DeclarationPM");

            foreach (PropertyInfo info in properties)
            {
                bool required = (from a in declarationRequiredFields
                                 where a.ObjectFieldName == info.Name
                                 select a).Any();
                if (required)
                {
                    if (info.GetValue(declaration) == null || info.GetValue(declaration) == "")
                    {
                        requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { FieldName = info.Name, TableName = "Customs.Declaration" });
                    }
                }
            }


            if(declaration.Direction=="E" &&(declaration.DeclarationExportRecipients==null || declaration.DeclarationExportRecipients.Count()==0))
            {
                requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { FieldName = "שורת פרטי מקבל", TableName = "Customs.Declaration" });

            }


            #endregion

            #region SupplierInvoice

            List<SupplierInvoicePM> supplierInvoices = invoiceQuery.GetSupplierInvoicesForDeclaration(declarationId, tenant, true); //declaration.SupplierInvoices;//mohammad fix wi 20751
            List<SupplierInvoiceItemPM> allInvoiceItems = invoiceItemQuery.GetSupplierInvoiceItemsForDeclaration(declarationId, tenant);
            List<SupplierInvoiceItemPM> supplierInvoiceItems = new List<SupplierInvoiceItemPM>();
            List<SupplierInvoiceModificationPM> supplierInvoiceModifications = new List<SupplierInvoiceModificationPM>();
            List<SupplierInvoiceFreightAmountPM> supplierInvoicFreightAmounts = new List<SupplierInvoiceFreightAmountPM>();
            List<SupplierInvoicePaymentPM> supplierInvoicePayments = new List<SupplierInvoicePaymentPM>();

            ObjectTable supplierInvoiceTable = objectTabelRepository.GetObjectTableByName("Customs.SupplierInvoice", 0, true);
            List<CustomsRequiredFieldPM> supplierInvoiceRequiredFields = customsRequiredFieldQueryService.GetCustomRequiredFieldsByObjectTable(supplierInvoiceTable.Id, tenant, isExport);
            List<PropertyInfo> SupplierInvoiceProperties = GetPropertiesForEntity("SupplierInvoicePM");
            //string[] supplierInvoiceArray = new string[declaration.SupplierInvoices.Count() + 1]; // Alaa: array index out of bounds problem
            //supplierInvoiceArray[0] = "";


            List<string> errors = CheckSupplierInvoice(declarationId, tenant);
            foreach (string error in errors)
            {
                requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { CustomMessageError = error, });
            }

            foreach (SupplierInvoicePM supplierInvoice in supplierInvoices)//declaration.SupplierInvoices)//mohammad fix wi 20751
            {
                foreach (PropertyInfo info in SupplierInvoiceProperties)
                {
                    bool required = (from a in supplierInvoiceRequiredFields
                                     where a.ObjectFieldName == info.Name
                                     select a).Any();
                    var value = info.GetValue(supplierInvoice);

                    if (required)
                    {
                        if (info.GetValue(supplierInvoice) == null)
                        {
                            requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { EntityReference = supplierInvoice.InvoiceNumber, FieldName = info.Name, TableName = "Customs.SupplierInvoice" });
                        }
                    }
                }

                //supplierInvoiceArray[supplierInvoice.InvoiceCounterKey] = supplierInvoice.InvoiceNumber;



                supplierInvoiceItems = allInvoiceItems.Where(d => d.DeclarationId == supplierInvoice.DeclarationId && d.CounterKey == supplierInvoice.InvoiceCounterKey).ToList();//supplierInvoice.SupplierInvoiceItems;//mohammad fix wi 20751
                supplierInvoiceModifications = supplierInvoice.SupplierInvoiceModifications;
                supplierInvoicFreightAmounts = supplierInvoice.SupplierInvoiceFreightAmounts;
                supplierInvoicePayments = supplierInvoice.SupplierInvoicePayments;


                #region SupplierInvoiceItem
                //List<SupplierInvoiceItemsQuantityPM> supplierInvoiceItemsQuantities = new List<SupplierInvoiceItemsQuantityPM>();
                List<SupplierInvoiceItemsConDeclarPM> supplierInvoiceItemsConnectedDeclarations = new List<SupplierInvoiceItemsConDeclarPM>();
                List<SupplierInvioceItemCertificatPM> supplierInvioceItemsCertificates = new List<SupplierInvioceItemCertificatPM>();
                List<SupplierInvoiceItemsModPM> supplierInvoiceItemsModifications = new List<SupplierInvoiceItemsModPM>();
                List<SupplierInvoiceItemsDescriptPM> supplierInvoiceItemsDescripts = new List<SupplierInvoiceItemsDescriptPM>();
                List<SupplierInvoiceItemsSerialNumPM> supplierInvoiceItemsSerialNumbers = new List<SupplierInvoiceItemsSerialNumPM>();
                List<SupplierInvoiceItemsProdIdentPM> supplierInvoiceItemsProdIdents = new List<SupplierInvoiceItemsProdIdentPM>();
                List<SupplierInvoiceItemProcesTypePM> supplierInvoiceItemsProcessTypes = new List<SupplierInvoiceItemProcesTypePM>();
                List<SupplierInvoiceItemsTaxPM> supplierInvoiceItemsTaxes = new List<SupplierInvoiceItemsTaxPM>();

                ObjectTable supplierInvoiceItemTable = objectTabelRepository.GetObjectTableByName("Customs.SupplierInvoiceItem", 0, fromCache);
                List<CustomsRequiredFieldPM> supplierInvoiceItemRequiredFields = customsRequiredFieldQueryService.GetCustomRequiredFieldsByObjectTable(supplierInvoiceItemTable.Id, tenant, isExport);
                List<PropertyInfo> SupplierInvoiceItemProperties = GetPropertiesForEntity("SupplierInvoiceItemPM");
                foreach (SupplierInvoiceItemPM supplierInvoiceItem in supplierInvoiceItems)
                {
                    foreach (PropertyInfo info in SupplierInvoiceItemProperties)
                    {
                        bool required = (from a in supplierInvoiceItemRequiredFields
                                         where a.ObjectFieldName == info.Name
                                         select a).Any();
                        if (required)
                        {
                            if (info.GetValue(supplierInvoiceItem) == null || info.GetValue(supplierInvoiceItem) == "")
                            {
                                requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { EntityReference = supplierInvoiceItem.SequenceNumeric.ToString(), FieldName = info.Name, TableName = "Customs.SupplierInvoiceItem", EntityReference2 = supplierInvoice.InvoiceNumber });
                            }
                        }
                    }
                    //supplierInvoiceItemsQuantities = supplierInvoiceItemsQuantities.Concat(supplierInvoiceItem.SupplierInvoiceItemsQuantities).ToList();
                    supplierInvoiceItemsConnectedDeclarations = supplierInvoiceItemsConnectedDeclarations.Concat(supplierInvoiceItem.SupplierInvoiceItemsConDeclars).ToList();
                    supplierInvioceItemsCertificates = supplierInvioceItemsCertificates.Concat(supplierInvoiceItem.SupplierInvioceItemCertificats).ToList();
                    supplierInvoiceItemsModifications = supplierInvoiceItemsModifications.Concat(supplierInvoiceItem.SupplierInvoiceItemsMods).ToList();
                    supplierInvoiceItemsDescripts = supplierInvoiceItemsDescripts.Concat(supplierInvoiceItem.SupplierInvoiceItemsDescripts).ToList();
                    supplierInvoiceItemsSerialNumbers = supplierInvoiceItemsSerialNumbers.Concat(supplierInvoiceItem.SupplierInvoiceItemsSerialNums).ToList();
                    supplierInvoiceItemsProdIdents = supplierInvoiceItemsProdIdents.Concat(supplierInvoiceItem.SupplierInvoiceItemsProdIdents).ToList();
                    supplierInvoiceItemsProcessTypes = supplierInvoiceItemsProcessTypes.Concat(supplierInvoiceItem.SupplierInvoiceItemProcesTypes).ToList();
                    supplierInvoiceItemsTaxes = supplierInvoiceItemsTaxes.Concat(supplierInvoiceItem.SupplierInvoiceItemTaxes).ToList();

                    #region SupplierInvoiceItemsConnectedDeclaration

                    ObjectTable supplierInvoiceItemsConnectedDeclarationTable = objectTabelRepository.GetObjectTableByName("Customs.SupplierInvoiceItemsConDeclar", 0, fromCache);

                    List<CustomsRequiredFieldPM> SupplierInvoiceItemsConnectedDeclarationRequiredFields = customsRequiredFieldQueryService.GetCustomRequiredFieldsByObjectTableFromCache(supplierInvoiceItemsConnectedDeclarationTable.Id, tenant, isExport);
                    List<PropertyInfo> supplierInvoiceItemsConnectedDeclarationProperties = GetPropertiesForEntity("SupplierInvoiceItemsConDeclarPM");
                    foreach (SupplierInvoiceItemsConDeclarPM supplierInvoiceItemsConnectedDeclaration in supplierInvoiceItemsConnectedDeclarations)
                    {
                        foreach (PropertyInfo info in supplierInvoiceItemsConnectedDeclarationProperties)
                        {
                            bool required = (from a in SupplierInvoiceItemsConnectedDeclarationRequiredFields
                                             where a.ObjectFieldName == info.Name
                                             select a).Any();
                            if (required)
                            {
                                if (info.GetValue(supplierInvoiceItemsConnectedDeclaration) == null || info.GetValue(supplierInvoiceItemsConnectedDeclaration) == "")
                                {
                                    requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { EntityReference = supplierInvoiceItemsConnectedDeclaration.LineNumber.ToString(), FieldName = info.Name, TableName = "Customs.SupplierInvoiceItemsConDeclar" });
                                }
                            }
                        }

                    }

                    #endregion


                    #region SupplierInvioceItemsCertificates

                    ObjectTable supplierInvioceItemsCertificateTable = objectTabelRepository.GetObjectTableByName("Customs.SupplierInvioceItemCertificat", 0, fromCache);
                    List<CustomsRequiredFieldPM> supplierInvioceItemsCertificateRequiredFields = customsRequiredFieldQueryService.GetCustomRequiredFieldsByObjectTable(supplierInvioceItemsCertificateTable.Id, tenant , isExport);
                    List<PropertyInfo> supplierInvioceItemsCertificateProperties = GetPropertiesForEntity("SupplierInvioceItemCertificatPM");
                    foreach (SupplierInvioceItemCertificatPM supplierInvioceItemsCertificate in supplierInvioceItemsCertificates)
                    {
                        foreach (PropertyInfo info in supplierInvioceItemsCertificateProperties)
                        {
                            bool required = (from a in supplierInvioceItemsCertificateRequiredFields
                                             where a.ObjectFieldName == info.Name
                                             select a).Any();
                            if (required)
                            {
                                if (info.GetValue(supplierInvioceItemsCertificate) == null || info.GetValue(supplierInvioceItemsCertificate) == "")
                                {
                                    requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { EntityReference = supplierInvioceItemsCertificate.CertificateNumber, FieldName = info.Name, TableName = "Customs.SupplierInvioceItemCertificat" });
                                }
                            }
                        }

                    }
                    #endregion

                }







                #region SupplierInvoiceItemsModifications
                ObjectTable supplierInvoiceItemsModificationTable = objectTabelRepository.GetObjectTableByName("Customs.SupplierInvoiceItemsMod", 0, fromCache);
                List<CustomsRequiredFieldPM> supplierInvoiceItemsModificationRequiredFields = customsRequiredFieldQueryService.GetCustomRequiredFieldsByObjectTable(supplierInvoiceItemsModificationTable.Id, tenant , isExport);
                List<PropertyInfo> supplierInvoiceItemsModificationProperties = GetPropertiesForEntity("SupplierInvoiceItemsModPM");
                foreach (SupplierInvoiceItemsModPM supplierInvoiceItemsModification in supplierInvoiceItemsModifications)
                {
                    foreach (PropertyInfo info in supplierInvoiceItemsModificationProperties)
                    {
                        bool required = (from a in supplierInvoiceItemsModificationRequiredFields
                                         where a.ObjectFieldName == info.Name
                                         select a).Any();
                        if (required)
                        {
                            if (info.GetValue(supplierInvoiceItemsModification) == null || info.GetValue(supplierInvoiceItemsModification) == "")
                            {
                                requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { EntityReference = supplierInvoiceItemsModification.TypeCode, FieldName = info.Name, TableName = "Customs.SupplierInvoiceItemsMod" });
                            }
                        }
                    }

                }
                #endregion

                #region SupplierInvoiceItemsDescript
                ObjectTable SupplierInvoiceItemsDescriptTable = objectTabelRepository.GetObjectTableByName("Customs.SupplierInvoiceItemsDescript", 0, fromCache);
                List<CustomsRequiredFieldPM> supplierInvoiceItemsDescriptRequiredFields = customsRequiredFieldQueryService.GetCustomRequiredFieldsByObjectTable(SupplierInvoiceItemsDescriptTable.Id, tenant, isExport);
                List<PropertyInfo> supplierInvoiceItemsDescriptProperties = GetPropertiesForEntity("SupplierInvoiceItemsDescriptPM");
                foreach (SupplierInvoiceItemsDescriptPM supplierInvoiceItemsDescript in supplierInvoiceItemsDescripts)
                {
                    foreach (PropertyInfo info in supplierInvoiceItemsDescriptProperties)
                    {
                        bool required = (from a in supplierInvoiceItemsDescriptRequiredFields
                                         where a.ObjectFieldName == info.Name
                                         select a).Any();
                        if (required)
                        {
                            if (info.GetValue(supplierInvoiceItemsDescript) == null || info.GetValue(supplierInvoiceItemsDescript) == "")
                            {
                                requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { EntityReference = supplierInvoiceItemsDescript.InvoiceCounterKey.ToString(), FieldName = info.Name, TableName = "Customs.SupplierInvoiceItemsDescript" });
                            }
                        }
                    }

                }
                #endregion

                #region supplierInvoiceItemsSerialNumbers

                ObjectTable supplierInvoiceItemsSerialNumberTable = objectTabelRepository.GetObjectTableByName("Customs.SupplierInvoiceItemsSerialNum", 0, fromCache);
                List<CustomsRequiredFieldPM> supplierInvoiceItemsSerialNumberRequiredFields = customsRequiredFieldQueryService.GetCustomRequiredFieldsByObjectTable(supplierInvoiceItemsSerialNumberTable.Id, tenant, isExport);
                List<PropertyInfo> supplierInvoiceItemsSerialNumberProperties = GetPropertiesForEntity("SupplierInvoiceItemsSerialNumPM");
                foreach (SupplierInvoiceItemsSerialNumPM supplierInvoiceItemsSerialNumber in supplierInvoiceItemsSerialNumbers)
                {
                    foreach (PropertyInfo info in supplierInvoiceItemsSerialNumberProperties)
                    {
                        bool required = (from a in supplierInvoiceItemsSerialNumberRequiredFields
                                         where a.ObjectFieldName == info.Name
                                         select a).Any();
                        if (required)
                        {
                            if (info.GetValue(supplierInvoiceItemsSerialNumber) == null || info.GetValue(supplierInvoiceItemsSerialNumber) == "")
                            {
                                requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { EntityReference = supplierInvoiceItemsSerialNumber.LineNumber.ToString(), FieldName = info.Name, TableName = "Customs.SupplierInvoiceItemsSerialNum" });
                            }
                        }
                    }

                }
                #endregion

                #region SupplierInvoiceItemsProductIdentification

                ObjectTable supplierInvoiceItemsProductIdentificationTable = objectTabelRepository.GetObjectTableByName("Customs.SupplierInvoiceItemsProdIdent", 0, fromCache);
                List<CustomsRequiredFieldPM> supplierInvoiceItemsProductIdentificationRequiredFields = customsRequiredFieldQueryService.GetCustomRequiredFieldsByObjectTable(supplierInvoiceItemsProductIdentificationTable.Id, tenant , isExport);
                List<PropertyInfo> supplierInvoiceItemsProductIdentificationProperties = GetPropertiesForEntity("SupplierInvoiceItemsProdIdentPM");
                foreach (SupplierInvoiceItemsProdIdentPM supplierInvoiceItemsProductIdentification in supplierInvoiceItemsProdIdents)
                {
                    foreach (PropertyInfo info in supplierInvoiceItemsProductIdentificationProperties)
                    {
                        bool required = (from a in supplierInvoiceItemsProductIdentificationRequiredFields
                                         where a.ObjectFieldName == info.Name
                                         select a).Any();
                        if (required)
                        {
                            if (info.GetValue(supplierInvoiceItemsProductIdentification) == null || info.GetValue(supplierInvoiceItemsProductIdentification) == "")
                            {
                                requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { EntityReference = supplierInvoiceItemsProductIdentification.LineNumber.ToString(), FieldName = info.Name, TableName = "Customs.SupplierInvoiceItemsProdIdent" });
                            }
                        }
                    }

                }

                #endregion

                #region supplierInvoiceItemsProcessTypes
                ObjectTable supplierInvoiceItemsProcessTypeTable = objectTabelRepository.GetObjectTableByName("Customs.SupplierInvoiceItemProcesType", 0, fromCache);
                List<CustomsRequiredFieldPM> supplierInvoiceItemsProcessTypeRequiredFields = customsRequiredFieldQueryService.GetCustomRequiredFieldsByObjectTable(supplierInvoiceItemsProcessTypeTable.Id, tenant, isExport);
                List<PropertyInfo> supplierInvoiceItemsProcessTypeProperties = GetPropertiesForEntity("SupplierInvoiceItemProcesTypePM");
                foreach (SupplierInvoiceItemProcesTypePM supplierInvoiceItemsProcessType in supplierInvoiceItemsProcessTypes)
                {
                    foreach (PropertyInfo info in supplierInvoiceItemsProcessTypeProperties)
                    {
                        bool required = (from a in supplierInvoiceItemsProcessTypeRequiredFields
                                         where a.ObjectFieldName == info.Name
                                         select a).Any();
                        if (required)
                        {
                            if (info.GetValue(supplierInvoiceItemsProcessType) == null || info.GetValue(supplierInvoiceItemsProcessType) == "")
                            {
                                requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { EntityReference = supplierInvoiceItemsProcessType.LineNumber.ToString(), FieldName = info.Name, TableName = "Customs.SupplierInvoiceItemProcesType" });
                            }
                        }
                    }

                }


                #endregion

                #region supplierInvoiceItemsTaxes
                ObjectTable supplierInvoiceItemsTaxTable = objectTabelRepository.GetObjectTableByName("Customs.SupplierInvoiceItemsTax", 0, fromCache);
                List<CustomsRequiredFieldPM> supplierInvoiceItemsTaxRequiredFields = customsRequiredFieldQueryService.GetCustomRequiredFieldsByObjectTable(supplierInvoiceItemsTaxTable.Id, tenant, isExport);
                List<PropertyInfo> supplierInvoiceItemsTaxProperties = GetPropertiesForEntity("SupplierInvoiceItemsTaxPM");
                foreach (SupplierInvoiceItemsTaxPM supplierInvoiceItemsTax in supplierInvoiceItemsTaxes)
                {
                    foreach (PropertyInfo info in supplierInvoiceItemsTaxProperties)
                    {
                        bool required = (from a in supplierInvoiceItemsTaxRequiredFields
                                         where a.ObjectFieldName == info.Name
                                         select a).Any();
                        if (required)
                        {
                            if (info.GetValue(supplierInvoiceItemsTax) == null || info.GetValue(supplierInvoiceItemsTax) == "")
                            {
                                requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { EntityReference = supplierInvoiceItemsTax.TaxTypeCode, FieldName = info.Name, TableName = "Customs.SupplierInvoiceItemsTax" });
                            }
                        }
                    }

                }

                #endregion





                #endregion

                #region SupplierInvoiceModification

                ObjectTable supplierInvoiceModificationTable = objectTabelRepository.GetObjectTableByName("Customs.SupplierInvoiceModification", 0, fromCache);
                List<CustomsRequiredFieldPM> supplierInvoiceModificationRequiredFields = customsRequiredFieldQueryService.GetCustomRequiredFieldsByObjectTable(supplierInvoiceModificationTable.Id, tenant, isExport);
                List<PropertyInfo> SupplierInvoiceModificationProperties = GetPropertiesForEntity("SupplierInvoiceModificationPM");
                foreach (SupplierInvoiceModificationPM supplierInvoiceModification in supplierInvoiceModifications)
                {
                    foreach (PropertyInfo info in SupplierInvoiceModificationProperties)
                    {
                        bool required = (from a in supplierInvoiceModificationRequiredFields
                                         where a.ObjectFieldName == info.Name
                                         select a).Any();
                        if (required)
                        {
                            if (info.GetValue(supplierInvoiceModification) == null || info.GetValue(supplierInvoiceModification) == "")
                            {
                                requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { EntityReference = supplierInvoiceModification.ModificationCounterKey.ToString(), FieldName = info.Name, TableName = "Customs.SupplierInvoiceModification" });
                            }
                        }
                    }
                }



                #endregion

                #region SupplierInvoiceFreightAmount

                ObjectTable supplierInvoiceFreightAmountTable = objectTabelRepository.GetObjectTableByName("Customs.SupplierInvoiceFreightAmount", 0, fromCache);
                List<CustomsRequiredFieldPM> supplierInvoiceFreightAmountRequiredFields = customsRequiredFieldQueryService.GetCustomRequiredFieldsByObjectTable(supplierInvoiceFreightAmountTable.Id, tenant, isExport);
                List<PropertyInfo> supplierInvoiceFreightAmountProperties = GetPropertiesForEntity("SupplierInvoiceFreightAmountPM");
                foreach (SupplierInvoiceFreightAmountPM supplierInvoiceFreightAmount in supplierInvoicFreightAmounts)
                {
                    foreach (PropertyInfo info in supplierInvoiceFreightAmountProperties)
                    {
                        bool required = (from a in supplierInvoiceFreightAmountRequiredFields
                                         where a.ObjectFieldName == info.Name
                                         select a).Any();
                        if (required)
                        {
                            if (info.GetValue(supplierInvoiceFreightAmount) == null || info.GetValue(supplierInvoiceFreightAmount) == "")
                            {
                                requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { EntityReference = supplierInvoiceFreightAmount.CurrencyTypeCode, FieldName = info.Name, TableName = "Customs.SupplierInvoiceFreightAmount" });
                            }
                        }
                    }
                }


                #endregion


                #region SupplierInvoiceFreightAmount


                //if ((supplierInvoicePayments == null || supplierInvoicePayments.Count() == 0) && isExport == "E")
                //{
                //    requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { EntityReference = supplierInvoice.InvoiceCounterKey.ToString(), FieldName = "פרטי תשלום", TableName = "Customs.SupplierInvoice" });

                //}


                ObjectTable supplierInvoicePaymentObjectTable = objectTabelRepository.GetObjectTableByName("Customs.SupplierInvoicePayment", 0, fromCache);
                List<CustomsRequiredFieldPM> supplierInvoicePaymentRequiredFields = customsRequiredFieldQueryService.GetCustomRequiredFieldsByObjectTable(supplierInvoicePaymentObjectTable.Id, tenant, isExport);
                List<PropertyInfo> supplierInvoicePaymentProperties = GetPropertiesForEntity("SupplierInvoicePaymentPM");
                foreach (SupplierInvoicePaymentPM supplierInvoicePayment in supplierInvoicePayments)
                {
                    foreach (PropertyInfo info in supplierInvoicePaymentProperties)
                    {
                        bool required = (from a in supplierInvoicePaymentRequiredFields
                                         where a.ObjectFieldName == info.Name
                                         select a).Any();
                        if (required)
                        {
                            if (info.GetValue(supplierInvoicePayment) == null || info.GetValue(supplierInvoicePayment) == "")
                            {
                                requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { EntityReference = supplierInvoicePayment.SequenceNumeric.ToString(), FieldName = info.Name, TableName = "Customs.SupplierInvoicePayment" });
                            }
                        }
                    }
                }


                #endregion
            }







            #endregion

            #region Consignment
            if (!declaration.ExcludeConsignment)
            {
                List<ConsignmentInternalTransitionPM> ConsignmentInternalTransitions = new List<ConsignmentInternalTransitionPM>();
                List<ConsignmentPackagePM> ConsignmentPackages = new List<ConsignmentPackagePM>();

                ObjectTable consignmentTable = objectTabelRepository.GetObjectTableByName("Customs.Consignment", 0, fromCache);
                List<CustomsRequiredFieldPM> consignmentRequiredFields = customsRequiredFieldQueryService.GetCustomRequiredFieldsByObjectTable(consignmentTable.Id, tenant, isExport);
                List<PropertyInfo> ConsignmentProperties = GetPropertiesForEntity("ConsignmentPM");
                foreach (ConsignmentPM Consignment in declaration.Consignments)
                {
                    foreach (PropertyInfo info in ConsignmentProperties)
                    {
                        bool required = (from a in consignmentRequiredFields
                                         where a.ObjectFieldName == info.Name
                                         select a).Any();
                        if (required)
                        {

                            if (info.GetValue(Consignment) == null || info.GetValue(Consignment) == "")
                            {
                                requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem()
                                {
                                    EntityReference =
                                        Consignment.ManifestNumber ?? "" //.ToString()
                                    ,
                                    FieldName = info.Name,
                                    TableName = "Customs.Consignment"
                                });
                            }
                        }
                    }
                    ConsignmentInternalTransitions = ConsignmentInternalTransitions.Concat(Consignment.ConsignmentInternalTransitions).ToList();
                    ConsignmentPackages = ConsignmentPackages.Concat(Consignment.ConsignmentPackages).ToList();
                }

                #region ConsignmentPackages
                ObjectTable ConsignmentPackageTable = objectTabelRepository.GetObjectTableByName("Customs.ConsignmentPackage", 0, fromCache);
                List<CustomsRequiredFieldPM> ConsignmentPackageRequiredFields = customsRequiredFieldQueryService.GetCustomRequiredFieldsByObjectTable(ConsignmentPackageTable.Id, tenant, isExport);
                List<PropertyInfo> ConsignmentPackageProperties = GetPropertiesForEntity("ConsignmentPackagePM");
                foreach (ConsignmentPackagePM ConsignmentPackage in ConsignmentPackages)
                {
                    foreach (PropertyInfo info in ConsignmentPackageProperties)
                    {
                        bool required = (from a in ConsignmentPackageRequiredFields
                                         where a.ObjectFieldName == info.Name
                                         select a).Any();
                        if (required)
                        {
                            if (info.GetValue(ConsignmentPackage) == null)
                            {
                                requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { EntityReference = ConsignmentPackage.LineNumber.ToString(), FieldName = info.Name, TableName = "Customs.ConsignmentPackage" });
                            }
                        }
                    }
                }

                #endregion

                #region ConsignmentInternalTransition
                ObjectTable ConsignmentInternalTransitionTable = objectTabelRepository.GetObjectTableByName("Customs.ConsignmentInternalTransition", 0, fromCache);
                List<CustomsRequiredFieldPM> ConsignmentInternalTransitionRequiredFields = customsRequiredFieldQueryService.GetCustomRequiredFieldsByObjectTable(ConsignmentInternalTransitionTable.Id, tenant, isExport);
                List<PropertyInfo> ConsignmentInternalTransitionProperties = GetPropertiesForEntity("ConsignmentInternalTransitionPM");
                foreach (ConsignmentInternalTransitionPM ConsignmentInternalTransition in ConsignmentInternalTransitions)
                {
                    foreach (PropertyInfo info in ConsignmentInternalTransitionProperties)
                    {
                        bool required = (from a in ConsignmentInternalTransitionRequiredFields
                                         where a.ObjectFieldName == info.Name
                                         select a).Any();
                        if (required)
                        {
                            if (info.GetValue(ConsignmentInternalTransition) == null)
                            {
                                requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { EntityReference = ConsignmentInternalTransition.LineNumber.ToString(), FieldName = info.Name, TableName = "Customs.ConsignmentInternalTransition" });
                            }
                        }
                    }
                }

                #endregion

            }
            #endregion

            #region DeclarationTaxes

            ObjectTable declarationTaxTable = objectTabelRepository.GetObjectTableByName("Customs.DeclarationTax", 0, fromCache);
            List<CustomsRequiredFieldPM> declarationTaxRequiredFields = customsRequiredFieldQueryService.GetCustomRequiredFieldsByObjectTable(declarationTaxTable.Id, tenant);
            List<PropertyInfo> DeclarationTaxProperties = GetPropertiesForEntity("DeclarationTaxPM");
            foreach (DeclarationTaxPM DeclarationTax in declaration.DeclarationTaxes)
            {
                foreach (PropertyInfo info in DeclarationTaxProperties)
                {
                    bool required = (from a in declarationTaxRequiredFields
                                     where a.ObjectFieldName == info.Name
                                     select a).Any();
                    if (required)
                    {
                        if (info.GetValue(DeclarationTax) == null)
                        {
                            requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { EntityReference = declaration.DeclarationNumber, FieldName = info.Name, TableName = "Customs.DeclarationTax" });
                        }
                    }
                }
            }


            #endregion

            #region DeclarationConstraint
            ObjectTable declarationConstraintTable = objectTabelRepository.GetObjectTableByName("Customs.DeclarationConstraint", 0, fromCache);
            List<CustomsRequiredFieldPM> declarationConstraintRequiredFields = customsRequiredFieldQueryService.GetCustomRequiredFieldsByObjectTable(declarationConstraintTable.Id, tenant, isExport);
            List<PropertyInfo> DeclarationConstraintProperties = GetPropertiesForEntity("DeclarationConstraintPM");
            foreach (DeclarationConstraintPM DeclarationConstraint in declaration.DeclarationConstraints)
            {
                foreach (PropertyInfo info in DeclarationConstraintProperties)
                {
                    bool required = (from a in declarationConstraintRequiredFields
                                     where a.ObjectFieldName == info.Name
                                     select a).Any();
                    if (required)
                    {
                        if (info.GetValue(DeclarationConstraint) == null)
                        {
                            requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { EntityReference = declaration.DeclarationNumber, FieldName = info.Name, TableName = "Customs.DeclarationConstraint" });
                        }
                    }
                }
            }

            #endregion


            #region DeclarationExportRecipient

            //if ((declaration.DeclarationExportRecipients == null || declaration.DeclarationExportRecipients.Count() == 0) && isExport=="E")
            //{
            //    requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { EntityReference = declaration.DeclarationNumber, FieldName = "פרטי מקבל", TableName = "Customs.DeclarationExportRecipient" });

      
            //}

            ObjectTable declarationExportRecipientTable = objectTabelRepository.GetObjectTableByName("Customs.DeclarationExportRecipient", 0, fromCache);
            List<CustomsRequiredFieldPM> declarationExportRecipientRequiredFields = customsRequiredFieldQueryService.GetCustomRequiredFieldsByObjectTable(declarationExportRecipientTable.Id, tenant, isExport);
            List<PropertyInfo> DeclarationExportRecipientProperties = GetPropertiesForEntity("DeclarationExportRecipientPM");
            foreach (DeclarationExportRecipientPM declarationExportRecipient in declaration.DeclarationExportRecipients)
            {
                foreach (PropertyInfo info in DeclarationExportRecipientProperties)
                {
                    bool required = (from a in declarationExportRecipientRequiredFields
                                     where a.ObjectFieldName == info.Name
                                     select a).Any();
                    if (required)
                    {
                        if (info.GetValue(declarationExportRecipient) == null)
                        {
                            requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { EntityReference = declaration.DeclarationNumber, FieldName = info.Name, TableName = "Customs.DeclarationExportRecipient" });
                        }
                    }
                }
            }

            #endregion






            return requiredErrors;
        }

        public static CustomsRequiredFieldErrors GetRequiredFieldErrorsForDeclarationPayment(string declarationId, int tenant)
        {
            CustomsRequiredFieldErrors requiredErrors = new CustomsRequiredFieldErrors() { RequiredFields = new List<CustomsRequiredFieldsErrorItem>(), };
            ICustomContext context = CustomContext.GetContext(tenant);
            CustomsRequiredFieldQueryService customsRequiredFieldQueryService = new CustomsRequiredFieldQueryService(context);
            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(0);
            // moran 6.4.16 - AMI-55700 -->
            #region Declaration
            DeclarationQueryService DeclarationQuery = new DeclarationQueryService(context);
            DeclarationPM declaration = DeclarationQuery.GetSingle(declarationId, true, false);
            if (!declaration.IsSignedVersion)
            {
                string user = AuthenticationUtil.ResolveUserId(tenant);
                var repository = new UserRepository(tenant);
                var myUserCard = repository.GetSingleUser(user, tenant, false);
                if (myUserCard == null)
                {
                    myUserCard = repository.GetSingleUser(user, 0, false);
                }
                //bool IsCustomerCare = myUserCard.Tenant == 0 && !myUserCard.IsDistributor;
                bool IsCustomerCare = !myUserCard.IsDistributor; //Yuval Chalup 09.11.2016 TASK-22337 (Removed myUserCard.Tenant == 0)
                var customsSetting = CustomsSettingQueryService.GetSettingByTenant(tenant) ?? new CustomsSettingPM();
                string entityReference = "";
                var listCustomsAgentId = new List<string>() { "550221105", "511487241" };
                if (IsCustomerCare
                    //Anat Friz unable to sign +2Month
                    || (listCustomsAgentId.Contains(customsSetting.CustomsAgentId) && DateTime.Now < new DateTime(2016, 07, 24))
                    )
                {

                    //entityReference = TranslateTextsClass.Translate("Customs.Declaration.O.IsSignedVersionErrorForCustomerCare", tenant);
                }
                else
                {
                    entityReference = TranslateTextsClass.Translate("Customs.Declaration.O.IsSignedVersionError", tenant);
                    requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { FieldName = "IsSignedVersion", TableName = "Customs.Declaration", EntityReference = entityReference, EntityReference2 = "OTHER" });
                }

            }
            #endregion
            // moran 6.4.16 - AMI-55700 <--
            DeclarationPaymentQueryService DeclarationPaymentQuery = new DeclarationPaymentQueryService(context);
            DeclarationPaymentPM payment = DeclarationPaymentQuery.GetSingle(declarationId, true, false);

            #region DeclarationPayment
            ObjectTable declarationPaymentTable = objectTabelRepository.GetObjectTableByName("Customs.DeclarationPayment", 0, false);
            List<CustomsRequiredFieldPM> declarationPaymentRequiredFields = customsRequiredFieldQueryService.GetCustomRequiredFieldsByObjectTable(declarationPaymentTable.Id, tenant);
            List<PropertyInfo> DeclarationPaymentProperties = GetPropertiesForEntity("DeclarationPaymentPM");

            foreach (PropertyInfo info in DeclarationPaymentProperties)
            {
                bool required = (from a in declarationPaymentRequiredFields
                                 where a.ObjectFieldName == info.Name
                                 select a).Any();
                if (required)
                {
                    if (info.GetValue(payment) == null || info.GetValue(payment) == "")
                    {
                        requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { FieldName = info.Name, TableName = "Customs.DeclarationPayment" });
                    }
                }

            }

            #endregion


            #region DeclarationPaymentMethod
            ObjectTable declarationPaymentMethodTable = objectTabelRepository.GetObjectTableByName("Customs.DeclarationPaymentMethod", 0, false);
            List<CustomsRequiredFieldPM> declarationPaymentMethodRequiredFields = customsRequiredFieldQueryService.GetCustomRequiredFieldsByObjectTable(declarationPaymentMethodTable.Id, tenant);
            List<PropertyInfo> DeclarationPaymentMethodProperties = GetPropertiesForEntity("DeclarationPaymentMethodPM");
            foreach (DeclarationPaymentMethodPM method in payment.DeclarationPaymentMethods)
            {
                foreach (PropertyInfo info in DeclarationPaymentMethodProperties)
                {
                    bool required = (from a in declarationPaymentMethodRequiredFields
                                     where a.ObjectFieldName == info.Name
                                     select a).Any();
                    if (required)
                    {
                        if (info.GetValue(method) == null)
                        {
                            requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { EntityReference = method.AccountNumber, FieldName = info.Name, TableName = "Customs.DeclarationPaymentMethod" });
                        }
                    }
                }
            }

            #endregion

            #region DeclarationPaymentProtest
            ObjectTable declarationPaymentProtestTable = objectTabelRepository.GetObjectTableByName("Customs.DeclarationPaymentProtest", 0, false);
            List<CustomsRequiredFieldPM> declarationPaymentProtestRequiredFields = customsRequiredFieldQueryService.GetCustomRequiredFieldsByObjectTable(declarationPaymentProtestTable.Id, tenant);
            List<PropertyInfo> DeclarationPaymentProtestProperties = GetPropertiesForEntity("DeclarationPaymentProtestPM");
            foreach (DeclarationPaymentProtestPM protest in payment.DeclarationPaymentProtests)
            {
                foreach (PropertyInfo info in DeclarationPaymentProtestProperties)
                {
                    bool required = (from a in declarationPaymentProtestRequiredFields
                                     where a.ObjectFieldName == info.Name
                                     select a).Any();
                    if (required)
                    {
                        if (info.GetValue(protest) == null)
                        {
                            requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { EntityReference = protest.ProtestTypeCode, FieldName = info.Name, TableName = "Customs.DeclarationPaymentProtest" });
                        }
                    }
                }
            }

            #endregion

            // moran 6.4.16 - AMI-55700 -->
            decimal? TotalAmount = payment.DeclarationPaymentMethods.Sum(s => s.Amount);
            decimal? TotalTax = declaration.TotalTax;
            double? totalAmount = (double?)Math.Round((decimal)TotalAmount, 2);
            if (TotalTax == null)
            {
                TotalTax = 0;
            }
            double? totalTax = (double?)Math.Round((decimal)TotalTax, 2);
            if (totalTax == null)
            {
                totalTax = 0;
            }
            if (totalAmount != totalTax)
            {
                requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { FieldName = "TotalTax", TableName = "Customs.Declaration", EntityReference = TranslateTextsClass.Translate("Customs.Declaration.O.Totalmustbeequaltototaltax", tenant), EntityReference2 = "OTHER" });
            }
            // moran 6.4.16 - AMI-55700 <--
            return requiredErrors;
        }

        public static CustomsRequiredFieldErrors GetRequiredFieldErrorsForClaim(string claimId, int tenant)
        {
            CustomsRequiredFieldErrors requiredErrors = new CustomsRequiredFieldErrors() { RequiredFields = new List<CustomsRequiredFieldsErrorItem>(), };
            ICustomContext context = CustomContext.GetContext(tenant);
            CustomsRequiredFieldQueryService customsRequiredFieldQueryService = new CustomsRequiredFieldQueryService(context);
            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(0);


            ClaimQueryService claimQueryService = new ClaimQueryService(context);
            ClaimPM claimPM = claimQueryService.GetSingle(claimId, true, false);

            ObjectTable claimTable = objectTabelRepository.GetObjectTableByName("Customs.Claim", 0, false);
            List<CustomsRequiredFieldPM> claimRequiredFields = customsRequiredFieldQueryService.GetCustomRequiredFieldsByObjectTable(claimTable.Id, tenant);
            List<PropertyInfo> claimProperties = GetPropertiesForEntity("ClaimPM");

            foreach (PropertyInfo info in claimProperties)
            {
                bool required = (from a in claimRequiredFields
                                 where a.ObjectFieldName == info.Name
                                 select a).Any();
                if (required)
                {
                    if (info.GetValue(claimPM) == null)
                    {
                        requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { FieldName = info.Name, TableName = "Customs.Claim" });
                    }
                }
            }

            #region ClaimsRelatedEntities

            if (claimPM.ClaimsRelatedEntities == null || (claimPM.ClaimsRelatedEntities != null && claimPM.ClaimsRelatedEntities.Count == 0))
            {
                requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { CustomMessageError = "Customs.Claim.G.NoRelatedEntityForClaim", });
            }

            ObjectTable ClaimsRelatedEntityTable = objectTabelRepository.GetObjectTableByName("Customs.ClaimsRelatedEntity", 0, false);
            List<CustomsRequiredFieldPM> ClaimsRelatedEntityRequiredFields = customsRequiredFieldQueryService.GetCustomRequiredFieldsByObjectTable(ClaimsRelatedEntityTable.Id, tenant);
            List<PropertyInfo> ClaimsRelatedEntityProperties = GetPropertiesForEntity("ClaimsRelatedEntityPM");

            foreach (ClaimsRelatedEntityPM claimsRelatedEntityItem in claimPM.ClaimsRelatedEntities)
            {
                foreach (PropertyInfo info in ClaimsRelatedEntityProperties)
                {
                    bool required = (from a in ClaimsRelatedEntityRequiredFields
                                     where a.ObjectFieldName == info.Name
                                     select a).Any();
                    if (required)
                    {

                        if (info.GetValue(claimsRelatedEntityItem) == null)
                        {
                            requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem()
                            {
                                EntityReference = claimsRelatedEntityItem.ClaimEntityNumber ?? "",
                                FieldName = info.Name,
                                TableName = "Customs.ClaimsRelatedEntity"
                            });
                        }
                    }
                }
                if (claimsRelatedEntityItem.ClaimsRelatedEntitiesReasons == null || (claimsRelatedEntityItem.ClaimsRelatedEntitiesReasons != null && claimsRelatedEntityItem.ClaimsRelatedEntitiesReasons.Count() == 0))
                {
                    requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { CustomMessageError = "Customs.Claim.G.NoClaimsRelatedEntityReasons", });
                }
                else
                {
                    foreach (ClaimsRelatedEntitiesReasonPM claimsRelatedEntitiesReasonItem in claimsRelatedEntityItem.ClaimsRelatedEntitiesReasons)
                    {
                        if (string.IsNullOrWhiteSpace(claimsRelatedEntitiesReasonItem.ReasonListTypeCode))
                        {
                            requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { CustomMessageError = "Customs.Claim.G.NoClaimsRelatedEntityReasons", });
                        }
                        if (claimsRelatedEntitiesReasonItem.ClaimsRelatedEntsReasonsExps == null || (claimsRelatedEntitiesReasonItem.ClaimsRelatedEntsReasonsExps != null && claimsRelatedEntitiesReasonItem.ClaimsRelatedEntsReasonsExps.Count == 0))
                        {
                            requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { CustomMessageError = "Customs.Claim.G.NoClaimsRelatedEntityReasonExps", });
                        }
                        else
                        {
                            foreach (ClaimsRelatedEntsReasonsExpPM claimsRelatedEntsReasonsExpsItem in claimsRelatedEntitiesReasonItem.ClaimsRelatedEntsReasonsExps)
                            {
                                if (string.IsNullOrWhiteSpace(claimsRelatedEntsReasonsExpsItem.ClaimExplanationTypeCode))
                                {
                                    requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { CustomMessageError = "Customs.Claim.G.NoClaimsRelatedEntityReasonExps", });
                                }
                            }
                        }
                    }
                }
            }

            #endregion

            #region ClaimImporterDeclarsPage3

            if (claimPM.ClaimImporterDeclarsPage3 == null || (claimPM.ClaimImporterDeclarsPage3 != null && claimPM.ClaimImporterDeclarsPage3.Count == 0))
            {
                //requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { CustomMessageError = "Customs.Claim.G.NoImporterDeclarsPage3", });
            }
            else
            {
                foreach (var claimImporterDeclarsPage3Item in claimPM.ClaimImporterDeclarsPage3)
                {
                    if (string.IsNullOrWhiteSpace(claimImporterDeclarsPage3Item.ImporterLoiDeclarationTypeCode) ||
                        claimImporterDeclarsPage3Item.ClaimImporterDeclarsP3Loi == null ||
                        (claimImporterDeclarsPage3Item.ClaimImporterDeclarsP3Loi != null && claimImporterDeclarsPage3Item.ClaimImporterDeclarsP3Loi.Count == 0))
                    {
                        requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { CustomMessageError = "Customs.Claim.G.NoImporterDeclarsPage3", });
                    }
                    foreach (var claimImporterDeclarsP3LoiItem in claimImporterDeclarsPage3Item.ClaimImporterDeclarsP3Loi)
                    {
                        if (string.IsNullOrWhiteSpace(claimImporterDeclarsP3LoiItem.DeclarationNumber))
                        {
                            requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { CustomMessageError = "Customs.Claim.G.NoDeclarationForPage3", });
                        }
                    }
                }
            }
            #endregion

            #region ClaimImporterDeclarsPage3A

            if (claimPM.ClaimImporterDeclarsPage3A == null || (claimPM.ClaimImporterDeclarsPage3A != null && claimPM.ClaimImporterDeclarsPage3A.Count == 0))
            {
                //requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { CustomMessageError = "Customs.Claim.G.NoImporterDeclarsPage3A", });
            }
            #endregion

            ClaimImporterDeclarsPage3PM _ClaimImporterDeclarsPage3PM = null;
            if (claimPM.ClaimImporterDeclarsPage3 != null && claimPM.ClaimImporterDeclarsPage3.Count == 1)
            {
                _ClaimImporterDeclarsPage3PM = (from a in claimPM.ClaimImporterDeclarsPage3
                                               where (a.ImporterLoiDeclarationTypeCode == "1")
                                                select a).FirstOrDefault();
            }

            if (_ClaimImporterDeclarsPage3PM != null)
            {
                if (string.IsNullOrEmpty(claimPM.ImporterAffidavit))
                {
                    requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { FieldName = "ImporterAffidavit", TableName = "Customs.Claim" });
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(claimPM.ImporterAffidavit)
                    || (claimPM.ClaimImporterDeclarsPage3 != null && claimPM.ClaimImporterDeclarsPage3.Count > 0)
                    || (claimPM.ClaimImporterDeclarsPage3A != null && claimPM.ClaimImporterDeclarsPage3A.Count > 0))
                {

                    if (string.IsNullOrEmpty(claimPM.ImporterAffidavit)
                    || (claimPM.ClaimImporterDeclarsPage3 == null || (claimPM.ClaimImporterDeclarsPage3 != null && claimPM.ClaimImporterDeclarsPage3.Count == 0))
                    || (claimPM.ClaimImporterDeclarsPage3A == null || (claimPM.ClaimImporterDeclarsPage3A != null && claimPM.ClaimImporterDeclarsPage3A.Count == 0)))
                    {
                        requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { CustomMessageError = "Customs.Claim.G.MustAllImporterDeclarsPage3", });
                    }
                }
            }

            return requiredErrors;
        }

        public static List<PropertyInfo> GetPropertiesForEntity(string entityName)
        {
            string className = "Logitude.Customs.Def.EntityPMs." + entityName;
            Assembly assembly = Assembly.Load("Logitude.Customs.Def");
            Type entityType = assembly.GetType(className);
            List<PropertyInfo> props = entityType.GetProperties().ToList();
            return props;
        }

        //Check if there is at least one Supplier Invoice AND each Supplier Invoice has at least one Item
        public static List<string> CheckSupplierInvoice(string declarationId, int tenant)
        {
            var errorMessage = "";
            List<string> errors = new List<string>();
            ICustomContext context = CustomContext.GetContext(tenant);
            SupplierInvoiceQueryService invoiceQuery = new SupplierInvoiceQueryService(context);
            SupplierInvoiceItemQueryService invoiceItemQuery = new SupplierInvoiceItemQueryService(context);

            List<SupplierInvoicePM> invoicePMs = invoiceQuery.GetSupplierInvoicesForDeclaration(declarationId, tenant);
            List<SupplierInvoiceItemPM> invoiceItemPMs = invoiceItemQuery.GetSupplierInvoiceItemsForDeclaration(declarationId, tenant);
            //Check if there is at least one Supplier Invoice
            if (invoicePMs == null)
            {
                errorMessage = "Customs.General.O.NoSupplierInvoiceForDeclaration";
            }
            else
            {
                if (invoicePMs.Count == 0)
                {
                    errorMessage = "Customs.General.O.NoSupplierInvoiceForDeclaration";
                }
                //If there is at least one Supplier Invoice
                else
                {
                    //Check that each Supplier Invoice has at least one Item

                    foreach (var supplierInvoice in invoicePMs)
                    {
                        List<SupplierInvoiceItemPM> invoiceItems = invoiceItemPMs.Where(d => d.CounterKey == supplierInvoice.InvoiceCounterKey).ToList();
                        if (invoiceItems == null)
                        {
                            errorMessage ="specialerror,"+ "חשבון  " + supplierInvoice.SequenceNumeric+"- ," + "Customs.General.O.NoSupplierInvoiceItemForInvoice";
                        }
                        else
                        {
                            if (invoiceItems.Count == 0)
                            {
                                errorMessage = "specialerror," + "חשבון  " + supplierInvoice.SequenceNumeric + "- ," + "Customs.General.O.NoSupplierInvoiceItemForInvoice";
                            }
                        }
                    }
                }
            }


            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                errors.Add(errorMessage);
            }

            return errors;
        }


        public static CustomsRequiredFieldErrors GetRequiredFieldErrorsForCourierDeclaration(string declarationId, int tenant, DeclarationPM paramDeclarationPM = null)
        {
            DeclarationPM declaration = null;
            CustomsRequiredFieldErrors requiredErrors = new CustomsRequiredFieldErrors() { RequiredFields = new List<CustomsRequiredFieldsErrorItem>(), };
            ICustomContext context = CustomContext.GetContext(tenant);
            CustomsRequiredFieldQueryService customsRequiredFieldQueryService = new CustomsRequiredFieldQueryService(context);
            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(0);
            DeclarationQueryService declarationQueryService = new DeclarationQueryService(context);
            SupplierInvoiceQueryService invoiceQuery = new SupplierInvoiceQueryService(context);
          
            CourierDeclarationQueryService courierDeclarationQueryService = new CourierDeclarationQueryService(context);
            CourierMasterQueryService courierMasterQueryService = new CourierMasterQueryService(context);
            List<SupplierInvoicePM> invoicePMs = invoiceQuery.GetSupplierInvoicesForDeclaration(declarationId, tenant);//mohammad fix wi 20751

            CourierDeclarationPM courierDeclaration = courierDeclarationQueryService.GetCourierDeclarationByDeclarationId(declarationId, tenant);
            //CourierMasterPM courierMaster = null;
            
            ClientAddressRepository clientAddressRep = new ClientAddressRepository(context);

            CustomsVendorQueryService vendorQueryService = new CustomsVendorQueryService(context);

            var fromCache = true;
            declaration = paramDeclarationPM;
            
            if (declaration == null)
            {
                
                if (fromCache)
                {
                    var cacheKey = "DeclarationPM.RequiredVldAfterUpdate" + declarationId;
                    declaration = CacheManager.CacheWrapper.Remove(cacheKey) as DeclarationPM;

                }
                if (declaration == null)
                {
                    declarationQueryService.LoadSupplierInvoicesWithItems = false;
                    declaration = declarationQueryService.GetSingle(declarationId, true, false);
                }
            }

            DeclarationPaymentQueryService DeclarationPaymentQuery = new DeclarationPaymentQueryService(context);
            DeclarationPaymentPM payment = DeclarationPaymentQuery.GetSingle(declarationId, true, fromCache);


            #region declaration entity
            ObjectTable declarationTable = objectTabelRepository.GetObjectTableByName("Customs.Declaration", 0, fromCache);
          //  List<CustomsRequiredFieldPM> declarationRequiredFields = customsRequiredFieldQueryService.GetCustomRequiredFieldsByObjectTable(declarationTable.Id, tenant);
            List<PropertyInfo> properties = GetPropertiesForEntity("DeclarationPM");

            bool IsImporterCodeNull = false;
            foreach (PropertyInfo info in properties)
            {
                //if(info.Name == "AgentId" || info.Name == "WeightValue")
                if (info.Name == "AgentId")//task 46459
                { 
                    if (info.GetValue(declaration) == null )
                    {
                        requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { FieldName = info.Name, TableName = "Customs.Declaration" });
                    }
                }


                //if (info.Name == "ImporterCode")
                //{
                //    if (info.GetValue(declaration) == null)
                //    {
                //        IsImporterCodeNull = true;
                       
                //    }
                //    else
                //    {
                //        IsImporterCodeNull = false;

                //    }
                //}

                if (info.Name == "ImporterName")
                {
                  
                        if (declaration.ImporterCode == null) { 
                        if (info.GetValue(declaration) == null)
                        {
                            requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { FieldName = info.Name, TableName = "Customs.Declaration" });

                        }
                     }
                   
                }

                if (info.Name == "ImporterAddress")
                {
                    if (declaration.ImporterCode == null)
                    {
                        if (info.GetValue(declaration) == null)
                        {
                            requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { FieldName = info.Name, TableName = "Customs.Declaration" });

                        }
                    }

                }


            }


            #endregion

            #region CourierMaster entity
            //if (courierDeclaration != null)
            //{
            //    CustomsRequiredFieldErrors courierMasterRequiredErrors = GetCourierMasterRequiredFieldErrorsForCourierDeclaration(courierDeclaration.CourierMasterId, tenant);
            //    if(courierMasterRequiredErrors != null)
            //    {
            //        foreach(var item in courierMasterRequiredErrors.RequiredFields)
            //        {
            //            requiredErrors.RequiredFields.Add(item);
            //        }
            //    }
            //}

            #endregion

            #region CourierDeclaration entity
            if (courierDeclaration != null)
            {
                ObjectTable courierDeclarationTable = objectTabelRepository.GetObjectTableByName("Customs.CourierDeclaration", 0, fromCache);
                List<PropertyInfo> courierDeclarationproperties = GetPropertiesForEntity("CourierDeclarationPM");


                foreach (PropertyInfo info in courierDeclarationproperties)
                {
                    if (info.Name == "SequenceNumeric")
                    {
                        if (info.GetValue(courierDeclaration) == null)
                        {
                            requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { FieldName = info.Name, TableName = "Customs.CourierDeclaration" });
                        }
                    }



                }
            }

            #endregion

            #region SupplierInvoice

            List<SupplierInvoicePM> supplierInvoices = invoiceQuery.GetSupplierInvoicesForDeclaration(declarationId, tenant); //declaration.SupplierInvoices;//mohammad fix wi 20751
   
            ObjectTable supplierInvoiceTable = objectTabelRepository.GetObjectTableByName("Customs.SupplierInvoice", 0, true);
           // List<CustomsRequiredFieldPM> supplierInvoiceRequiredFields = customsRequiredFieldQueryService.GetCustomRequiredFieldsByObjectTable(supplierInvoiceTable.Id, tenant);
            List<PropertyInfo> SupplierInvoiceProperties = GetPropertiesForEntity("SupplierInvoicePM");
            //string[] supplierInvoiceArray = new string[declaration.SupplierInvoices.Count() + 1]; // Alaa: array index out of bounds problem
            //supplierInvoiceArray[0] = "";


            //List<string> errors = CheckSupplierInvoice(declarationId, tenant);
            //foreach (string error in errors)
            //{
            //    requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { CustomMessageError = error, });
            //}

            //foreach (SupplierInvoicePM supplierInvoice in supplierInvoices)//declaration.SupplierInvoices)//mohammad fix wi 20751
            SupplierInvoicePM supplierInvoice = supplierInvoices.FirstOrDefault();
            bool vendorFromFirstSI = false;
            if (supplierInvoice!=null)
            {
                foreach (PropertyInfo info in SupplierInvoiceProperties)
                {
                  if(info.Name == "VendorId")
                    {
                        if (info.GetValue(supplierInvoice) != null)
                        {
                            string vendorId = info.GetValue(supplierInvoice).ToString();
                            CustomsVendorPM vendor = vendorQueryService.GetSingle(vendorId, false, false);
                            if(vendor != null)
                            {
                                vendorFromFirstSI = true;
                                if (vendor.VendorName == null)
                                {
                                    PropertyInfo prop = properties.Where(d => d.Name == "CasualSupplierName").FirstOrDefault();

                                    //if (prop.GetValue(declaration) == null)
                                    {
                                        requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { FieldName = prop.Name, TableName = "Customs.Declaration" });
                                    }

                                }

                                if (vendor.MainAddressLine == null)
                                {
                                    PropertyInfo prop = properties.Where(d => d.Name == "CasualSupplierAddress").FirstOrDefault();

                                    //if (prop.GetValue(declaration) == null)
                                    {
                                        requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { FieldName = prop.Name, TableName = "Customs.Declaration" });
                                    }

                                }
                            }

                        }

                    }

                    if (info.Name == "IncotermCode")
                    {
                        if (info.GetValue(supplierInvoice) == null)
                        {
                            requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { EntityReference = supplierInvoice.InvoiceNumber, FieldName = info.Name, TableName = "Customs.SupplierInvoice" });
                        }
                    }

                }
                

            }
            if (!vendorFromFirstSI)
            {


                PropertyInfo propCasualSupplierName = properties.Where(d => d.Name == "CasualSupplierName").FirstOrDefault();

                if (string.IsNullOrWhiteSpace(toString(propCasualSupplierName.GetValue(declaration))))
                {
                    requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { FieldName = propCasualSupplierName.Name, TableName = "Customs.Declaration" });
                }





                PropertyInfo propCasualSupplierAddress = properties.Where(d => d.Name == "CasualSupplierAddress").FirstOrDefault();

                if (string.IsNullOrWhiteSpace(toString(propCasualSupplierAddress.GetValue(declaration))))
                {
                    requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { FieldName = propCasualSupplierAddress.Name, TableName = "Customs.Declaration" });
                }



            }

            #endregion

            #region Consignment

            List<ConsignmentPackagePM> ConsignmentPackages = new List<ConsignmentPackagePM>();

            ObjectTable consignmentTable = objectTabelRepository.GetObjectTableByName("Customs.Consignment", 0, fromCache);
         //   List<CustomsRequiredFieldPM> consignmentRequiredFields = customsRequiredFieldQueryService.GetCustomRequiredFieldsByObjectTable(consignmentTable.Id, tenant);
            List<PropertyInfo> ConsignmentProperties = GetPropertiesForEntity("ConsignmentPM");
            foreach (ConsignmentPM Consignment in declaration.Consignments)
            {
                foreach (PropertyInfo info in ConsignmentProperties)
                {
                    //if(info.Name == "StorageSiteCode" || info.Name == "LoadingPortCode" || info.Name == "ThirdCargoID"|| info.Name == "ManifestNumber" || info.Name== "UnloadDate" || info.Name== "CargoDescription" || info.Name == "DeliveryPlaceName")
                    if (info.Name == "StorageSiteCode"  || info.Name == "ThirdCargoID" || info.Name == "ManifestNumber" || info.Name == "CargoDescription")//task 46459 // task 47157
                    {
                        if (info.GetValue(Consignment) == null)
                        {
                            requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem()
                            {
                                EntityReference =
                                    Consignment.ManifestNumber ?? "" //.ToString()
                                ,
                                FieldName = info.Name,
                                TableName = "Customs.Consignment"
                            });
                        }
                    }

                  


                }
             
                ConsignmentPackages = ConsignmentPackages.Concat(Consignment.ConsignmentPackages).ToList();
            }

            #endregion
            #region ConsignmentPackages
            ObjectTable ConsignmentPackageTable = objectTabelRepository.GetObjectTableByName("Customs.ConsignmentPackage", 0, fromCache);
         //   List<CustomsRequiredFieldPM> ConsignmentPackageRequiredFields = customsRequiredFieldQueryService.GetCustomRequiredFieldsByObjectTable(ConsignmentPackageTable.Id, tenant);
            List<PropertyInfo> ConsignmentPackageProperties = GetPropertiesForEntity("ConsignmentPackagePM");
            foreach (ConsignmentPackagePM ConsignmentPackage in ConsignmentPackages)
            {
                foreach (PropertyInfo info in ConsignmentPackageProperties)
                {
                  if(info.Name == "PackageQuantity" || info.Name== "LineNumber" || info.Name== "GrossMassMeasure" || info.Name== "PackageTypeCode")
                    {
                        if (info.GetValue(ConsignmentPackage) == null)
                        {
                            requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { EntityReference = ConsignmentPackage.LineNumber.ToString(), FieldName = info.Name, TableName = "Customs.ConsignmentPackage" });
                        }
                    }
                    
                }
            }

            #endregion

         
            return requiredErrors;
        }

        private static string toString(object obj)
        {
            if (obj==null)
            {
                return "";
            }
            return obj.ToString();
        }

        public static CustomsRequiredFieldErrors GetCourierMasterRequiredFieldErrorsForCourierDeclaration(string courierMasterId, int tenant)
        {
            CustomsRequiredFieldErrors requiredErrors = new CustomsRequiredFieldErrors() { RequiredFields = new List<CustomsRequiredFieldsErrorItem>(), };
            ICustomContext context = CustomContext.GetContext(tenant);
            CustomsRequiredFieldQueryService customsRequiredFieldQueryService = new CustomsRequiredFieldQueryService(context);
            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(0);

            CourierMasterQueryService courierMasterQueryService = new CourierMasterQueryService(context);
            CourierMasterPM courierMaster = courierMasterQueryService.GetSingle(courierMasterId, false, false);
            if (courierMaster == null)
            {
                return null;
            }

            #region CourierMaster entity
            if (courierMaster != null)
            {
                ObjectTable courierMasterTable = objectTabelRepository.GetObjectTableByName("Customs.CourierMaster", 0, false);
                List<PropertyInfo> courierMasterproperties = GetPropertiesForEntity("CourierMasterPM");


                foreach (PropertyInfo info in courierMasterproperties)
                {
                    if (info.Name == "OriginPortCode")
                    {
                        if (info.GetValue(courierMaster) == null)
                        {
                            requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { FieldName = info.Name, TableName = "Customs.CourierMaster" });
                        }
                    }
                    if (info.Name == "MAWB")
                    {
                        if (info.GetValue(courierMaster) == null)
                        {

                            requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { FieldName = info.Name, TableName = "Customs.CourierMaster" });

                        }
                    }


                    if (info.Name == "CourierMaster")
                    {
                        if (info.GetValue(courierMaster) == null)
                        {

                            requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { FieldName = info.Name, TableName = "Customs.CourierMaster" });

                        }
                    }

                    if (info.Name == "MAWBTypeCode")
                    {
                        if (info.GetValue(courierMaster) == null)
                        {

                            requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { FieldName = info.Name, TableName = "Customs.CourierMaster" });

                        }
                    }

                    if (info.Name == "AirlineId")
                    {
                        if (info.GetValue(courierMaster) == null)
                        {
                            requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { FieldName = info.Name, TableName = "Customs.CourierMaster" });
                        }
                    }

                    if (info.Name == "WeightValueCode")//task 46459
                    {
                        if (info.GetValue(courierMaster) == null)
                        {
                            requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { FieldName = info.Name, TableName = "Customs.CourierMaster" });
                        }
                    }

                    if (info.Name == "GatewayPortCode")
                    {
                        if (info.GetValue(courierMaster) == null)
                        {
                            requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { FieldName = info.Name, TableName = "Customs.CourierMaster" });
                        }
                    }
                }
            }

            #endregion

            return requiredErrors;
        }

        public static CustomsRequiredFieldErrors GetRequiredFieldsForCourierMaster(string courierMasterId, int tenant, bool isIncludeRequiredFieldsForManifest = false)
        {
            CustomsRequiredFieldErrors requiredErrors = new CustomsRequiredFieldErrors() { RequiredFields = new List<CustomsRequiredFieldsErrorItem>(), };
            ICustomContext context = CustomContext.GetContext(tenant);
            CustomsRequiredFieldQueryService customsRequiredFieldQueryService = new CustomsRequiredFieldQueryService(context);
            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(0);

            CourierMasterQueryService courierMasterQueryService = new CourierMasterQueryService(context);
            CourierMasterPM courierMasterPM = courierMasterQueryService.GetSingle(courierMasterId, true, false);

            ObjectTable courierMasterTable = objectTabelRepository.GetObjectTableByName("Customs.CourierMaster", 0, false);
            List<CustomsRequiredFieldPM> courierMasterRequiredFields = customsRequiredFieldQueryService.GetCustomRequiredFieldsByObjectTable(courierMasterTable.Id, tenant);
            List<PropertyInfo> courierMasterProperties = GetPropertiesForEntity("CourierMasterPM");

            foreach (PropertyInfo info in courierMasterProperties)
            {
                bool required = (from a in courierMasterRequiredFields
                                 where a.ObjectFieldName == info.Name
                                 select a).Any();
                if (required)
                {
                    if (info.GetValue(courierMasterPM) == null)
                    {
                        requiredErrors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { FieldName = info.Name, TableName = "Customs.CourierMaster" });
                    }
                }
            }

            if (isIncludeRequiredFieldsForManifest)
            {
                CustomsRequiredFieldErrors courierMasterRequiredErrors = GetCourierMasterRequiredFieldErrorsForCourierDeclaration(courierMasterPM.Id, tenant);
                if (courierMasterRequiredErrors != null)
                {
                    foreach (var item in courierMasterRequiredErrors.RequiredFields)
                    {
                        requiredErrors.RequiredFields.Add(item);
                    }
                }
            }

            return requiredErrors;
        }

    }
}
