using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.Customs.Data.Repsitories;
using System.Runtime.Remoting.Contexts;
using Logitude.Customs.BL.EntityUpdateServices;
using System.Data.Entity;
using Microsoft.Practices.ObjectBuilder2;
using Logitude.Customs.Data.EntityLists;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class SupplierInvoiceQueryService : EntityQueryService<SupplierInvoice, SupplierInvoiceKeys, SupplierInvoicePM, object, SupplierInvoiceKeys>
    {

        
        private bool onlyParentItem = false;
        public bool OnlyParentItem
        {
            get { return onlyParentItem; }
            set { onlyParentItem = value; }
        }

        public override void GetComposition(EntityKeyFields entityKeys,SupplierInvoicePM entityPM)
        {
            
            ICustomContext context = MainContext as CustomContext;
            SupplierInvoiceKeys supplierInvoiceKeys = entityKeys as SupplierInvoiceKeys;
            SupplierInvoiceItemQueryService supplierInvoiceItemQueryService = new SupplierInvoiceItemQueryService(context);
            
            List<int> listLines = null;
            if (!OnlyParentItem || !entityPM.IsAccumalated)
            {
                
                entityPM.SupplierInvoiceItems = supplierInvoiceItemQueryService.GetMulti(supplierInvoiceKeys, true).OrderBy(d => d.SequenceNumeric).ToList();
            }
            else
            {
                entityPM.SupplierInvoiceItems = supplierInvoiceItemQueryService.GetMultiOnlyParentItem(supplierInvoiceKeys).OrderBy(d => d.SequenceNumeric).ToList();
                listLines=entityPM.SupplierInvoiceItems.Select(r => r.LineNumber).ToList();
            }

            
            
            SupplierInvoiceModificationQueryService supplierInvoiceModificationQueryService = new SupplierInvoiceModificationQueryService(context);
            entityPM.SupplierInvoiceModifications = supplierInvoiceModificationQueryService.GetMulti(supplierInvoiceKeys, true);

            SupplierInvoicePaymentQueryService supplierInvoicePaymentQueryService = new SupplierInvoicePaymentQueryService(context);
            entityPM.SupplierInvoicePayments = supplierInvoicePaymentQueryService.GetMulti(supplierInvoiceKeys, true);


            SupplierInvoiceUCRQueryService supplierInvoiceUCRQueryService = new SupplierInvoiceUCRQueryService(context);
            entityPM.SupplierInvoiceUCRs = supplierInvoiceUCRQueryService.GetMulti(supplierInvoiceKeys, true);




            #region new code for get Composition

            SupplierInvoiceItemsConDeclarQueryService supplierInvoiceItemsConnectedDeclarationService = new SupplierInvoiceItemsConDeclarQueryService(context);
            
            List<SupplierInvoiceItemsConDeclarPM> supplierInvoiceItemsConDeclars = supplierInvoiceItemsConnectedDeclarationService.GetSupplierInvoiceItemsConDeclarPMsForSupplierInvoice(supplierInvoiceKeys.DeclarationId, supplierInvoiceKeys.InvoiceCounterKey, Tenant, listLines);
            SupplierInvoiceItemsModQueryService supplierInvoiceItemsModificationQueryService = new SupplierInvoiceItemsModQueryService(context);
            List<SupplierInvoiceItemsModPM> supplierInvoiceItemsMods = supplierInvoiceItemsModificationQueryService.GetSupplierInvoiceItemsModsForSupplierInvoice(supplierInvoiceKeys.DeclarationId, supplierInvoiceKeys.InvoiceCounterKey, Tenant, listLines);
            SupplierInvoiceItemsTaxQueryService supplierInvoiceItemsTaxQueryService = new SupplierInvoiceItemsTaxQueryService(context);
            List<SupplierInvoiceItemsTaxPM> supplierInvoiceItemsTaxes = supplierInvoiceItemsTaxQueryService.GetSupplierInvoiceItemsTaxesForSupplierInvoice(supplierInvoiceKeys.DeclarationId, supplierInvoiceKeys.InvoiceCounterKey, Tenant, listLines);
            SupplierInvioceItemCertificatQueryService supplierInvioceItemsCertificateQueryService = new SupplierInvioceItemCertificatQueryService(context);
            List<SupplierInvioceItemCertificatPM> supplierInvioceItemCertificates = supplierInvioceItemsCertificateQueryService.GetSupplierInvioceItemCertificatesForSupplierInvoice(supplierInvoiceKeys.DeclarationId, supplierInvoiceKeys.InvoiceCounterKey, Tenant, listLines);
            SupplierInvoiceItemsSerialNumQueryService supplierInvoiceItemsSerialNumberQueryService = new SupplierInvoiceItemsSerialNumQueryService(context);
            List<SupplierInvoiceItemsSerialNumPM> supplierInvoiceItemsSerialNums = supplierInvoiceItemsSerialNumberQueryService.GetSupplierInvoiceItemsSerialNumsForSupplierInvoice(supplierInvoiceKeys.DeclarationId, supplierInvoiceKeys.InvoiceCounterKey, Tenant, listLines);
            SupplierInvoiceItemsProdIdentQueryService supplierInvoiceItemsProductIdentificationQueryService = new SupplierInvoiceItemsProdIdentQueryService(context);
            List<SupplierInvoiceItemsProdIdentPM> supplierInvoiceItemsProdIdents = supplierInvoiceItemsProductIdentificationQueryService.GetSupplierInvoiceItemsProdIdentsForSupplierInvoice(supplierInvoiceKeys.DeclarationId, supplierInvoiceKeys.InvoiceCounterKey, Tenant, listLines);
            SupplierInvoiceItemsDescriptQueryService supplierInvoiceItemsDescriptionQueryService = new SupplierInvoiceItemsDescriptQueryService(context);
            List<SupplierInvoiceItemsDescriptPM> supplierInvoiceItemsDescripts = supplierInvoiceItemsDescriptionQueryService.GetSupplierInvoiceItemsDescriptsForSupplierInvoice(supplierInvoiceKeys.DeclarationId, supplierInvoiceKeys.InvoiceCounterKey, Tenant, listLines);
            SupplierInvoiceItemProcesTypeQueryService supplierInvoiceItemsProcessTypeQueryService = new SupplierInvoiceItemProcesTypeQueryService(context);
            List<SupplierInvoiceItemProcesTypePM> supplierInvoiceItemProcesTypes = supplierInvoiceItemsProcessTypeQueryService.GetSupplierInvoiceItemProcesTypesForSupplierInvoice(supplierInvoiceKeys.DeclarationId, supplierInvoiceKeys.InvoiceCounterKey, Tenant, listLines);
            SupplierInvoiceItemsLevyQueryService supplierInvoiceItemsLevyQueryService = new SupplierInvoiceItemsLevyQueryService(context);
            List<SupplierInvoiceItemsLevyPM> supplierInvoiceItemsLevies = supplierInvoiceItemsLevyQueryService.GetSupplierInvoiceItemsLeviesForSupplierInvoice(supplierInvoiceKeys.DeclarationId, supplierInvoiceKeys.InvoiceCounterKey, Tenant, listLines);
            SupplierInvoiceItemVehicleQueryService supplierInvoiceItemVehicleQueryService = new SupplierInvoiceItemVehicleQueryService(context);
            List<SupplierInvoiceItemVehiclePM> supplierInvoiceItemVehicles = supplierInvoiceItemVehicleQueryService.GetSupplierInvoiceItemVehiclesForSupplierInvoice(supplierInvoiceKeys.DeclarationId, supplierInvoiceKeys.InvoiceCounterKey, Tenant, listLines);
            SupplierInvoiceItemModVehicleQueryService supplierInvoiceItemModVehicleQueryService = new SupplierInvoiceItemModVehicleQueryService(context);
            List<SupplierInvoiceItemModVehiclePM> supplierInvoiceItemModVehicles = supplierInvoiceItemModVehicleQueryService.GetSupplierInvoiceItemModVehiclesForSupplierInvoice(supplierInvoiceKeys.DeclarationId, supplierInvoiceKeys.InvoiceCounterKey, Tenant, listLines);
            SupplierInvoiceItemsPriceQueryService supplierInvoiceItemsPriceQueryService = new SupplierInvoiceItemsPriceQueryService(context);
            List<SupplierInvoiceItemsPricePM> supplierInvoiceItemsPrices = supplierInvoiceItemsPriceQueryService.GetSupplierInvoiceItemsPricesForSupplierInvoiceWithSpecificKeys(supplierInvoiceKeys.DeclarationId, supplierInvoiceKeys.InvoiceCounterKey, listLines, entityPM.Tenant);
            SuppInvoiceItemsAbachStatementQueryService suppInvoiceItemsAbachStatementQueryService = new SuppInvoiceItemsAbachStatementQueryService(context);
            List<SuppInvoiceItemsAbachStatementPM> suppInvoiceItemsAbachStatements = suppInvoiceItemsAbachStatementQueryService.GetSuppInvoiceItemsAbachStatementsForSupplierInvoiceWithSpecificKeys(supplierInvoiceKeys.DeclarationId, supplierInvoiceKeys.InvoiceCounterKey, listLines, entityPM.Tenant);

            SupplierInvoiceItemVehicleModQueryService supplierInvoiceItemVehicleModQueryService = new SupplierInvoiceItemVehicleModQueryService(context); // moran 20.10.15 - Task 17209

            foreach (SupplierInvoiceItemPM supplierInvoiceItem in entityPM.SupplierInvoiceItems)
            {
                supplierInvoiceItem.SupplierInvoiceItemsConDeclars = supplierInvoiceItemsConDeclars.Where(d => d.DeclarationId == supplierInvoiceItem.DeclarationId && d.InvoiceCounterKey == supplierInvoiceItem.CounterKey && d.InvoiceItemLineNumber == supplierInvoiceItem.LineNumber).ToList();
                supplierInvoiceItem.SupplierInvoiceItemsMods = supplierInvoiceItemsMods.Where(d => d.DeclarationId == supplierInvoiceItem.DeclarationId && d.InvoiceCounterKey == supplierInvoiceItem.CounterKey && d.LineNumber == supplierInvoiceItem.LineNumber).ToList();
                supplierInvoiceItem.SupplierInvoiceItemTaxes = supplierInvoiceItemsTaxes.Where(d => d.DeclarationId == supplierInvoiceItem.DeclarationId && d.InvoiceCounterKey == supplierInvoiceItem.CounterKey && d.LineNumber == supplierInvoiceItem.LineNumber).ToList();
                supplierInvoiceItem.SupplierInvioceItemCertificats = supplierInvioceItemCertificates.Where(d => d.DeclarationId == supplierInvoiceItem.DeclarationId && d.InvoiceCounterKey == supplierInvoiceItem.CounterKey && d.LineNumber == supplierInvoiceItem.LineNumber).ToList();
                supplierInvoiceItem.SupplierInvoiceItemsSerialNums = supplierInvoiceItemsSerialNums.Where(d => d.DeclarationId == supplierInvoiceItem.DeclarationId && d.InvoiceCounterKey == supplierInvoiceItem.CounterKey && d.InvoiceItemLineNumber == supplierInvoiceItem.LineNumber).ToList();
                supplierInvoiceItem.SupplierInvoiceItemsProdIdents = supplierInvoiceItemsProdIdents.Where(d => d.DeclarationId == supplierInvoiceItem.DeclarationId && d.InvoiceCounterKey == supplierInvoiceItem.CounterKey && d.InvoiceItemLineNumber == supplierInvoiceItem.LineNumber).ToList();
                supplierInvoiceItem.SupplierInvoiceItemsDescripts = supplierInvoiceItemsDescripts.Where(d => d.DeclarationId == supplierInvoiceItem.DeclarationId && d.InvoiceCounterKey == supplierInvoiceItem.CounterKey && d.InvoiceItemLineNumber == supplierInvoiceItem.LineNumber).ToList();
                supplierInvoiceItem.SupplierInvoiceItemProcesTypes = supplierInvoiceItemProcesTypes.Where(d => d.DeclarationId == supplierInvoiceItem.DeclarationId && d.InvoiceCounterKey == supplierInvoiceItem.CounterKey && d.InvoiceItemLineNumber == supplierInvoiceItem.LineNumber).ToList();
                supplierInvoiceItem.SupplierInvoiceItemLevies = supplierInvoiceItemsLevies.Where(d => d.DeclarationId == supplierInvoiceItem.DeclarationId && d.InvoiceCounterKey == supplierInvoiceItem.CounterKey && d.InvoiceItemLineNumber == supplierInvoiceItem.LineNumber).ToList();
                supplierInvoiceItem.SupplierInvoiceItemVehicles = supplierInvoiceItemVehicles.Where(d => d.DeclarationId == supplierInvoiceItem.DeclarationId && d.InvoiceCounterKey == supplierInvoiceItem.CounterKey && d.InvoiceItemLineNumber == supplierInvoiceItem.LineNumber).ToList();
                supplierInvoiceItem.SupplierInvoiceItemsPrices = supplierInvoiceItemsPrices.Where(d => d.DeclarationId == supplierInvoiceItem.DeclarationId && d.InvoiceCounterKey == supplierInvoiceItem.CounterKey && d.InvoiceItemLineNumber == supplierInvoiceItem.LineNumber).ToList();
                supplierInvoiceItem.SuppInvoiceItemsAbachStatements = suppInvoiceItemsAbachStatements.Where(d => d.DeclarationId == supplierInvoiceItem.DeclarationId && d.InvoiceCounterKey == supplierInvoiceItem.CounterKey && d.InvoiceItemLineNumber == supplierInvoiceItem.LineNumber).ToList();

                // moran 20.10.15 - Task 17209 --> 
                /*                List<SupplierInvoiceItemVehicleModPM> supplierInvoiceItemVehicleMods = supplierInvoiceItemVehicleModQueryService.GetSupplierInvoiceItemVehicleModsForSupplierInvoice(supplierInvoiceKeys.DeclarationId, supplierInvoiceKeys.InvoiceCounterKey, supplierInvoiceItem.LineNumber, Tenant);
                                foreach (SupplierInvoiceItemVehiclePM supplierInvoiceItemVehicle in supplierInvoiceItem.SupplierInvoiceItemVehicles)
                                {
                                    supplierInvoiceItemVehicle.SupplierInvoiceItemVehicleMods = supplierInvoiceItemVehicleMods.Where(d => d.DeclarationId == supplierInvoiceItem.DeclarationId && d.InvoiceCounterKey == supplierInvoiceItem.CounterKey && d.InvoiceItemLineNumber == supplierInvoiceItem.LineNumber && d.VehicleLineNumber == supplierInvoiceItemVehicle.LineNumber).ToList();
                                } */
                // moran 20.10.15 - Task 17209 <--
                supplierInvoiceItem.SupplierInvoiceItemModVehicles = supplierInvoiceItemModVehicles.Where(d => d.DeclarationId == supplierInvoiceItem.DeclarationId && d.InvoiceCounterKey == supplierInvoiceItem.CounterKey && d.InvoiceItemLineNumber == supplierInvoiceItem.LineNumber).ToList();

                if (supplierInvoiceItem.SupplierInvoiceItemsConDeclars != null)
                {
                    if (supplierInvoiceItem.SupplierInvoiceItemsConDeclars.Count > 0)
                    {
                        supplierInvoiceItem.SupplierInvoiceItemsConnectedDeclarationLastLineNumber = supplierInvoiceItem.SupplierInvoiceItemsConDeclars.Max(m => m.LineNumber);
                    }
                }

                if (supplierInvoiceItem.SupplierInvoiceItemsMods != null)
                {
                    if (supplierInvoiceItem.SupplierInvoiceItemsMods.Count > 0)
                    {
                        supplierInvoiceItem.SupplierInvoiceItemsModificationLastLineNumber = supplierInvoiceItem.SupplierInvoiceItemsMods.Max(m => m.LineNumber);
                    }
                }

                if (supplierInvoiceItem.SupplierInvoiceItemTaxes != null)
                {
                    if (supplierInvoiceItem.SupplierInvoiceItemTaxes.Count > 0)
                    {
                        supplierInvoiceItem.SupplierInvoiceItemTaxLastLineNumber = supplierInvoiceItem.SupplierInvoiceItemTaxes.Max(m => m.LineNumber);
                    }
                }

                if (supplierInvoiceItem.SupplierInvioceItemCertificats != null)
                {
                    if (supplierInvoiceItem.SupplierInvioceItemCertificats.Count > 0)
                    {
                        supplierInvoiceItem.SupplierInvioceItemsCertificatLastLineNumber = supplierInvoiceItem.SupplierInvioceItemCertificats.Max(m => m.LineNumber);
                    }
                }

                if (supplierInvoiceItem.SupplierInvoiceItemsSerialNums != null)
                {
                    if (supplierInvoiceItem.SupplierInvoiceItemsSerialNums.Count > 0)
                    {
                        supplierInvoiceItem.SupplierInvoiceItemsSerialNumberLastLineNumber = supplierInvoiceItem.SupplierInvoiceItemsSerialNums.Max(m => m.LineNumber);
                    }
                }

                if (supplierInvoiceItem.SupplierInvoiceItemsProdIdents != null)
                {
                    if (supplierInvoiceItem.SupplierInvoiceItemsProdIdents.Count > 0)
                    {
                        supplierInvoiceItem.SupplierInvoiceItemsProductIdentificationLastLineNumber = supplierInvoiceItem.SupplierInvoiceItemsProdIdents.Max(m => m.LineNumber);
                    }
                }

                if (supplierInvoiceItem.SupplierInvoiceItemsDescripts != null)
                {
                    if (supplierInvoiceItem.SupplierInvoiceItemsDescripts.Count > 0)
                    {
                        supplierInvoiceItem.SupplierInvoiceItemsDescriptionLastLineNumber = supplierInvoiceItem.SupplierInvoiceItemsDescripts.Max(m => m.LineNumber);
                    }
                }

                if (supplierInvoiceItem.SupplierInvoiceItemProcesTypes != null)
                {
                    if (supplierInvoiceItem.SupplierInvoiceItemProcesTypes.Count > 0)
                    {
                        supplierInvoiceItem.SupplierInvoiceItemsProcessTypeLastLineNumber = supplierInvoiceItem.SupplierInvoiceItemProcesTypes.Max(m => m.LineNumber);
                    }
                }

                if (supplierInvoiceItem.SupplierInvoiceItemLevies != null)
                {
                    if (supplierInvoiceItem.SupplierInvoiceItemLevies.Count > 0)
                    {
                        supplierInvoiceItem.SupplierInvoiceItemsLevyLastLineNumber = supplierInvoiceItem.SupplierInvoiceItemLevies.Max(m => m.LineNumber);
                    }
                }

                if (supplierInvoiceItem.SupplierInvoiceItemVehicles != null)
                {
                    if (supplierInvoiceItem.SupplierInvoiceItemVehicles.Count > 0)
                    {
                        supplierInvoiceItem.SupplierInvoiceItemVehicleLastLineNumber = supplierInvoiceItem.SupplierInvoiceItemVehicles.Max(m => m.LineNumber);
                    }
                }
            }

            #endregion

            if (entityPM.SupplierInvoiceItems == null)
            {
                entityPM.SupplierInvoiceItems = new List<SupplierInvoiceItemPM>();
            }

            if (entityPM.SupplierInvoiceItems.Count > 0)
            {
                entityPM.InvoiceItemLastLineNumber = entityPM.SupplierInvoiceItems.Max(m => m.LineNumber);
            }

            SupplierInvoiceFreightAmountQueryService supplierInvoiceFreightAmountQueryService = new SupplierInvoiceFreightAmountQueryService(context);

            entityPM.SupplierInvoiceFreightAmounts = supplierInvoiceFreightAmountQueryService.GetMulti(supplierInvoiceKeys, true);
            if (entityPM.SupplierInvoiceFreightAmounts == null)
            {
                entityPM.SupplierInvoiceFreightAmounts = new List<SupplierInvoiceFreightAmountPM>();
            }
            
            entityPM.FullItemsCount = entityPM.SupplierInvoiceItems.Count();
            entityPM.MaxSequence = entityPM.SupplierInvoiceItems.Max(d => d.SequenceNumeric);

            base.GetComposition(entityKeys, entityPM);
        }

        public int GetSupplierInvoiceToAccumulateCount(int tenant, string declarationId)
        {
            return repository.GetSupplierInvoiceToAccumulateCount(tenant, declarationId);
        }

        public int GetDeclarationCountOfSupplierInvoiceItemsForAccumulation(int tenant, string declarationId)
        {
            List<int> invoicesCounterKeys = repository.GetDeclarationSupplierInvoicesKeysForAccumulation(tenant, declarationId, false);
            var qs = new SupplierInvoiceItemQueryService(tenant);
            return qs.GetDeclarationCountOfSupplierInvoiceItemsForAccumulation(tenant, declarationId, invoicesCounterKeys);
        }

        public int ExistSupplierInvoiceItemsWithoutHashForAccumulation(int tenant, string declarationId, bool onlyAlwaysAccumulate)
        {
            List<int> invoicesCounterKeys = repository.GetDeclarationSupplierInvoicesKeysForAccumulation(tenant, declarationId, onlyAlwaysAccumulate);
            var qs = new SupplierInvoiceItemQueryService(tenant);
            return qs.ExistSupplierInvoiceItemsWithoutHashForAccumulation(tenant, declarationId, invoicesCounterKeys);
        }

        public SupplierInvoicePM GetSupplierInvoiceByNumber(string invoiceNumber, int tenant)
        {
            SupplierInvoicePM invoicePM = null;
            SupplierInvoice invoice = repository.GetSupplierInvoiceByInvoiceNumber(invoiceNumber, tenant);
            if (invoice != null)
            {
                invoicePM = new SupplierInvoicePM()
                {
                     InvoiceNumber = invoice.InvoiceNumber,
                     DeclarationId = invoice.DeclarationId,
                     InvoiceCounterKey = invoice.InvoiceCounterKey,
                     Tenant = invoice.Tenant
                     
                };

                ICustomContext context = MainContext as CustomContext;
                SupplierInvoiceKeys supplierInvoiceKeys = new SupplierInvoiceKeys() { DeclarationId = invoicePM.DeclarationId, InvoiceCounterKey = invoicePM.InvoiceCounterKey };
                SupplierInvoiceItemQueryService supplierInvoiceItemQueryService = new SupplierInvoiceItemQueryService(context);
                invoicePM.SupplierInvoiceItems = supplierInvoiceItemQueryService.GetMulti(supplierInvoiceKeys, true);
          
               
            }


           
            return invoicePM;
        }

        public List<SupplierInvoicePM> GetSupplierInvoicesByCounterKeys(string declarationId, List<int> counterKeys,int tenant)
        {
            List<SupplierInvoice> supplierInvoices = repository.GetSupplierInvoicesByCounterKeys(declarationId, counterKeys, tenant);
            return (from a in supplierInvoices
                    select new SupplierInvoicePM()
                    {
                        DeclarationId = a.DeclarationId,
                        InvoiceCounterKey = a.InvoiceCounterKey,
                        SequenceNumeric = a.SequenceNumeric,
                    }).OrderBy(d => d.SequenceNumeric).ToList();
        }

        public List<SupplierInvoicePM> GetSupplierInvoicesForDeclaration(string declarationId, int tenant, bool getComposition = false)
        {
            List<SupplierInvoice> supplierInvoices = repository.GetSupplierInvoicesForDeclaration(declarationId, tenant);
            SupplierInvoiceDataMapping mappings = new SupplierInvoiceDataMapping();

            List<SupplierInvoicePM> supplierInvoicePMs = new List<SupplierInvoicePM>();
            foreach (SupplierInvoice invoice in supplierInvoices)
            {
                SupplierInvoicePM invoicePM = new SupplierInvoicePM();
                
                mappings.CustomPOCOToPM(invoicePM, invoice);
                mappings.POCOToPM(invoicePM, invoice);
                if (getComposition)
                {
                    GetComposition(new SupplierInvoiceKeys() { DeclarationId = invoicePM.DeclarationId, InvoiceCounterKey = invoicePM.InvoiceCounterKey }, invoicePM);
                }
                supplierInvoicePMs.Add(invoicePM);
            }
            return supplierInvoicePMs.OrderBy(d=>d.SequenceNumeric).ToList();
        }


        public IQueryable<SupplierInvoicePM> GetSupplierInvoicesQueryForDeclaration(string declarationId, int tenant, bool getComposition = false)
        {
            IQueryable<SupplierInvoice> supplierInvoices = repository.GetSupplierInvoicesQueryForDeclaration(declarationId, tenant);


            IQueryable<SupplierInvoicePM> supplierInvoicePMs = from a in supplierInvoices
                                                         where a.DeclarationId == declarationId && a.Tenant == tenant
                                                         select new SupplierInvoicePM()
                                                         {

                                                             DeclarationId = a.DeclarationId,
                                                             InvoiceCounterKey = a.InvoiceCounterKey,
                                                             Tenant = a.Tenant,
                                                             InvoiceNumber = a.InvoiceNumber,
                                                           
                                                         };
           
            return supplierInvoicePMs;
        }
       

        public int GetSupplierInvoiceCountForDeclaration(string declarationId, int tenant)
        {
            return repository.GetSupplierInvoiceCountForDeclaration(declarationId, tenant);
        }

        public int? GetMaxSequenceNumeric(string declarationId, int tenant)
        {
            return repository.GetMaxSequenceNumeric(declarationId, tenant);
        }

        // this is for editing an invoice from the declaration invoice tab.
        public SupplierInvoicePM GetSingleSupplierInvoiceWithLimitedItems(string declarationId, int counterKey, int tenant, int skip, int take, string type)
        {
            SupplierInvoice invoice = repository.GetSingle(new SupplierInvoiceKeys() { DeclarationId = declarationId, InvoiceCounterKey = counterKey });
            SupplierInvoicePM invoicePM = new SupplierInvoicePM();
            SupplierInvoiceDataMapping mapping = new SupplierInvoiceDataMapping();
            mapping.CustomPOCOToPM(invoicePM, invoice);
            mapping.POCOToPM(invoicePM, invoice);
            GetCompositionForLimitedItems(invoicePM, skip, take, type);
            return invoicePM;

        }

        public int GetSupplierInvoiceItemsCount(string declarationId, int counterKey, int tenant)
        {
            SupplierInvoice invoice = repository.GetSingle(new SupplierInvoiceKeys() { DeclarationId = declarationId, InvoiceCounterKey = counterKey });
            SupplierInvoicePM invoicePM = new SupplierInvoicePM();
            SupplierInvoiceDataMapping mapping = new SupplierInvoiceDataMapping();
            mapping.CustomPOCOToPM(invoicePM, invoice);
            mapping.POCOToPM(invoicePM, invoice);
            //entityPM.SupplierInvoiceItems = supplierInvoiceItemQueryService.GetSomeSupplierInvoiceItemsForInvoice(entityPM.DeclarationId, entityPM.InvoiceCounterKey, skip, take, ref fullCount);

            return invoicePM.SupplierInvoiceItems.Count();
        }

        public void GetCompositionForLimitedItems(SupplierInvoicePM entityPM,int skip,int take, string type)
        {
            int fullCount=0;
            int childrenCount = 0;
            ICustomContext context = MainContext as CustomContext;
            SupplierInvoiceKeys supplierInvoiceKeys = new SupplierInvoiceKeys() { DeclarationId = entityPM.DeclarationId, InvoiceCounterKey = entityPM.InvoiceCounterKey };
            SupplierInvoiceItemQueryService supplierInvoiceItemQueryService = new SupplierInvoiceItemQueryService(context);
            entityPM.SupplierInvoiceItems = supplierInvoiceItemQueryService.GetSomeSupplierInvoiceItemsForInvoice(entityPM.IsAccumalated, entityPM.DeclarationId, entityPM.InvoiceCounterKey, skip, take,type, ref fullCount, ref childrenCount);
            List<int> itemsLineNumbers = (from a in entityPM.SupplierInvoiceItems
                                                  select a.LineNumber).ToList();

            entityPM.FullItemsCount = fullCount;
            entityPM.FullChildrenCount = childrenCount;

            entityPM.FullParentsCount = fullCount - childrenCount;
               
            SupplierInvoiceModificationQueryService supplierInvoiceModificationQueryService = new SupplierInvoiceModificationQueryService(context);
            entityPM.SupplierInvoiceModifications = supplierInvoiceModificationQueryService.GetMulti(supplierInvoiceKeys, true);

            SupplierInvoicePaymentQueryService supplierInvoicePaymentQueryService = new SupplierInvoicePaymentQueryService(context);
            entityPM.SupplierInvoicePayments = supplierInvoicePaymentQueryService.GetMulti(supplierInvoiceKeys, true);

            SupplierInvoiceUCRQueryService supplierInvoiceUCRQueryService = new SupplierInvoiceUCRQueryService(context);
            entityPM.SupplierInvoiceUCRs = supplierInvoiceUCRQueryService.GetMulti(supplierInvoiceKeys, true);


            #region new code for get Composition

            SupplierInvoiceItemsConDeclarQueryService supplierInvoiceItemsConnectedDeclarationService = new SupplierInvoiceItemsConDeclarQueryService(context);
            List<SupplierInvoiceItemsConDeclarPM> supplierInvoiceItemsConDeclars = supplierInvoiceItemsConnectedDeclarationService.GetSupplierInvoiceItemsConDeclarPMsForSupplierInvoiceWithSpecificKeys(supplierInvoiceKeys.DeclarationId, supplierInvoiceKeys.InvoiceCounterKey, itemsLineNumbers, Tenant);
            SupplierInvoiceItemsModQueryService supplierInvoiceItemsModificationQueryService = new SupplierInvoiceItemsModQueryService(context);
            List<SupplierInvoiceItemsModPM> supplierInvoiceItemsMods = supplierInvoiceItemsModificationQueryService.GetSupplierInvoiceItemsModsForSupplierInvoiceWithSpecificKeys(supplierInvoiceKeys.DeclarationId, supplierInvoiceKeys.InvoiceCounterKey, itemsLineNumbers,Tenant);
            SupplierInvoiceItemsTaxQueryService supplierInvoiceItemsTaxQueryService = new SupplierInvoiceItemsTaxQueryService(context);
            List<SupplierInvoiceItemsTaxPM> supplierInvoiceItemsTaxes = supplierInvoiceItemsTaxQueryService.GetSupplierInvoiceItemsTaxesForSupplierInvoiceWithSpecificKeys(supplierInvoiceKeys.DeclarationId, supplierInvoiceKeys.InvoiceCounterKey, itemsLineNumbers, Tenant);
            SupplierInvioceItemCertificatQueryService supplierInvioceItemsCertificateQueryService = new SupplierInvioceItemCertificatQueryService(context);
            List<SupplierInvioceItemCertificatPM> supplierInvioceItemCertificates = supplierInvioceItemsCertificateQueryService.GetSupplierInvioceItemCertificatesForSupplierInvoiceWithSpecificKeys(supplierInvoiceKeys.DeclarationId, supplierInvoiceKeys.InvoiceCounterKey,itemsLineNumbers, Tenant);
            SupplierInvoiceItemsSerialNumQueryService supplierInvoiceItemsSerialNumberQueryService = new SupplierInvoiceItemsSerialNumQueryService(context);
            List<SupplierInvoiceItemsSerialNumPM> supplierInvoiceItemsSerialNums = supplierInvoiceItemsSerialNumberQueryService.GetSupplierInvoiceItemsSerialNumsForSupplierInvoiceWithSpecificKeys(supplierInvoiceKeys.DeclarationId, supplierInvoiceKeys.InvoiceCounterKey,itemsLineNumbers, Tenant);
            SupplierInvoiceItemsProdIdentQueryService supplierInvoiceItemsProductIdentificationQueryService = new SupplierInvoiceItemsProdIdentQueryService(context);
            List<SupplierInvoiceItemsProdIdentPM> supplierInvoiceItemsProdIdents = supplierInvoiceItemsProductIdentificationQueryService.GetSupplierInvoiceItemsProdIdentsForSupplierInvoiceWithSpecificKeys(supplierInvoiceKeys.DeclarationId, supplierInvoiceKeys.InvoiceCounterKey,itemsLineNumbers, Tenant);
            SupplierInvoiceItemsDescriptQueryService supplierInvoiceItemsDescriptionQueryService = new SupplierInvoiceItemsDescriptQueryService(context);
            List<SupplierInvoiceItemsDescriptPM> supplierInvoiceItemsDescripts = supplierInvoiceItemsDescriptionQueryService.GetSupplierInvoiceItemsDescriptsForSupplierInvoiceWithSpecificKeys(supplierInvoiceKeys.DeclarationId, supplierInvoiceKeys.InvoiceCounterKey,itemsLineNumbers, Tenant);
            SupplierInvoiceItemProcesTypeQueryService supplierInvoiceItemsProcessTypeQueryService = new SupplierInvoiceItemProcesTypeQueryService(context);
            List<SupplierInvoiceItemProcesTypePM> supplierInvoiceItemProcesTypes = supplierInvoiceItemsProcessTypeQueryService.GetSupplierInvoiceItemProcesTypesForSupplierInvoiceWithSpecificKeys(supplierInvoiceKeys.DeclarationId, supplierInvoiceKeys.InvoiceCounterKey,itemsLineNumbers, Tenant);
            SupplierInvoiceItemsLevyQueryService supplierInvoiceItemsLevyQueryService = new SupplierInvoiceItemsLevyQueryService(context);
            List<SupplierInvoiceItemsLevyPM> supplierInvoiceItemsLevies = supplierInvoiceItemsLevyQueryService.GetSupplierInvoiceItemsLeviesForSupplierInvoiceWithSpecificKeys(supplierInvoiceKeys.DeclarationId, supplierInvoiceKeys.InvoiceCounterKey,itemsLineNumbers, Tenant);
            SupplierInvoiceItemVehicleQueryService supplierInvoiceItemVehicleQueryService = new SupplierInvoiceItemVehicleQueryService(context);
            List<SupplierInvoiceItemVehiclePM> supplierInvoiceItemVehicles = supplierInvoiceItemVehicleQueryService.GetSupplierInvoiceItemVehiclesForSupplierInvoiceWithSpecificKeys(supplierInvoiceKeys.DeclarationId, supplierInvoiceKeys.InvoiceCounterKey,itemsLineNumbers, Tenant);
            SupplierInvoiceItemModVehicleQueryService supplierInvoiceItemModVehicleQueryService = new SupplierInvoiceItemModVehicleQueryService(context);
            List<SupplierInvoiceItemModVehiclePM> supplierInvoiceItemModVehicles = supplierInvoiceItemModVehicleQueryService.GetSupplierInvoiceItemModVehiclesForSupplierInvoiceWithSpecificKeys(supplierInvoiceKeys.DeclarationId, supplierInvoiceKeys.InvoiceCounterKey,itemsLineNumbers, Tenant);
            SupplierInvoiceItemsPriceQueryService supplierInvoiceItemsPriceQueryService = new SupplierInvoiceItemsPriceQueryService(context);
            List<SupplierInvoiceItemsPricePM> supplierInvoiceItemsPrices = supplierInvoiceItemsPriceQueryService.GetSupplierInvoiceItemsPricesForSupplierInvoiceWithSpecificKeys(supplierInvoiceKeys.DeclarationId, supplierInvoiceKeys.InvoiceCounterKey, itemsLineNumbers, entityPM.Tenant);
            SuppInvoiceItemsAbachStatementQueryService suppInvoiceItemsAbachStatementQueryService = new SuppInvoiceItemsAbachStatementQueryService(context);
            List<SuppInvoiceItemsAbachStatementPM> suppInvoiceItemsAbachStatements = suppInvoiceItemsAbachStatementQueryService.GetSuppInvoiceItemsAbachStatementsForSupplierInvoiceWithSpecificKeys(supplierInvoiceKeys.DeclarationId, supplierInvoiceKeys.InvoiceCounterKey, itemsLineNumbers, entityPM.Tenant);
          
            //  SupplierInvoiceItemVehicleModQueryService supplierInvoiceItemVehicleModQueryService = new SupplierInvoiceItemVehicleModQueryService(context); // moran 20.10.15 - Task 17209

            foreach (SupplierInvoiceItemPM supplierInvoiceItem in entityPM.SupplierInvoiceItems)
            {
                supplierInvoiceItem.SupplierInvoiceItemsConDeclars = supplierInvoiceItemsConDeclars.Where(d => d.DeclarationId == supplierInvoiceItem.DeclarationId && d.InvoiceCounterKey == supplierInvoiceItem.CounterKey && d.InvoiceItemLineNumber == supplierInvoiceItem.LineNumber).ToList();
                supplierInvoiceItem.SupplierInvoiceItemsMods = supplierInvoiceItemsMods.Where(d => d.DeclarationId == supplierInvoiceItem.DeclarationId && d.InvoiceCounterKey == supplierInvoiceItem.CounterKey && d.LineNumber == supplierInvoiceItem.LineNumber).ToList();
                supplierInvoiceItem.SupplierInvoiceItemTaxes = supplierInvoiceItemsTaxes.Where(d => d.DeclarationId == supplierInvoiceItem.DeclarationId && d.InvoiceCounterKey == supplierInvoiceItem.CounterKey && d.LineNumber == supplierInvoiceItem.LineNumber).ToList();
                supplierInvoiceItem.SupplierInvioceItemCertificats = supplierInvioceItemCertificates.Where(d => d.DeclarationId == supplierInvoiceItem.DeclarationId && d.InvoiceCounterKey == supplierInvoiceItem.CounterKey && d.LineNumber == supplierInvoiceItem.LineNumber).ToList();
                supplierInvoiceItem.SupplierInvoiceItemsSerialNums = supplierInvoiceItemsSerialNums.Where(d => d.DeclarationId == supplierInvoiceItem.DeclarationId && d.InvoiceCounterKey == supplierInvoiceItem.CounterKey && d.InvoiceItemLineNumber == supplierInvoiceItem.LineNumber).ToList();
                supplierInvoiceItem.SupplierInvoiceItemsProdIdents = supplierInvoiceItemsProdIdents.Where(d => d.DeclarationId == supplierInvoiceItem.DeclarationId && d.InvoiceCounterKey == supplierInvoiceItem.CounterKey && d.InvoiceItemLineNumber == supplierInvoiceItem.LineNumber).ToList();
                supplierInvoiceItem.SupplierInvoiceItemsDescripts = supplierInvoiceItemsDescripts.Where(d => d.DeclarationId == supplierInvoiceItem.DeclarationId && d.InvoiceCounterKey == supplierInvoiceItem.CounterKey && d.InvoiceItemLineNumber == supplierInvoiceItem.LineNumber).ToList();
                supplierInvoiceItem.SupplierInvoiceItemProcesTypes = supplierInvoiceItemProcesTypes.Where(d => d.DeclarationId == supplierInvoiceItem.DeclarationId && d.InvoiceCounterKey == supplierInvoiceItem.CounterKey && d.InvoiceItemLineNumber == supplierInvoiceItem.LineNumber).ToList();
                supplierInvoiceItem.SupplierInvoiceItemLevies = supplierInvoiceItemsLevies.Where(d => d.DeclarationId == supplierInvoiceItem.DeclarationId && d.InvoiceCounterKey == supplierInvoiceItem.CounterKey && d.InvoiceItemLineNumber == supplierInvoiceItem.LineNumber).ToList();
                supplierInvoiceItem.SupplierInvoiceItemVehicles = supplierInvoiceItemVehicles.Where(d => d.DeclarationId == supplierInvoiceItem.DeclarationId && d.InvoiceCounterKey == supplierInvoiceItem.CounterKey && d.InvoiceItemLineNumber == supplierInvoiceItem.LineNumber).ToList();
                // moran 20.10.15 - Task 17209 --> 
                /*                List<SupplierInvoiceItemVehicleModPM> supplierInvoiceItemVehicleMods = supplierInvoiceItemVehicleModQueryService.GetSupplierInvoiceItemVehicleModsForSupplierInvoice(supplierInvoiceKeys.DeclarationId, supplierInvoiceKeys.InvoiceCounterKey, supplierInvoiceItem.LineNumber, Tenant);
                                foreach (SupplierInvoiceItemVehiclePM supplierInvoiceItemVehicle in supplierInvoiceItem.SupplierInvoiceItemVehicles)
                                {
                                    supplierInvoiceItemVehicle.SupplierInvoiceItemVehicleMods = supplierInvoiceItemVehicleMods.Where(d => d.DeclarationId == supplierInvoiceItem.DeclarationId && d.InvoiceCounterKey == supplierInvoiceItem.CounterKey && d.InvoiceItemLineNumber == supplierInvoiceItem.LineNumber && d.VehicleLineNumber == supplierInvoiceItemVehicle.LineNumber).ToList();
                                } */
                // moran 20.10.15 - Task 17209 <--
                supplierInvoiceItem.SupplierInvoiceItemModVehicles = supplierInvoiceItemModVehicles.Where(d => d.DeclarationId == supplierInvoiceItem.DeclarationId && d.InvoiceCounterKey == supplierInvoiceItem.CounterKey && d.InvoiceItemLineNumber == supplierInvoiceItem.LineNumber).ToList();
                supplierInvoiceItem.SupplierInvoiceItemsPrices = supplierInvoiceItemsPrices.Where(d => d.DeclarationId == supplierInvoiceItem.DeclarationId && d.InvoiceCounterKey == supplierInvoiceItem.CounterKey && d.InvoiceItemLineNumber == supplierInvoiceItem.LineNumber).ToList();
                supplierInvoiceItem.SuppInvoiceItemsAbachStatements = suppInvoiceItemsAbachStatements.Where(d => d.DeclarationId == supplierInvoiceItem.DeclarationId && d.InvoiceCounterKey == supplierInvoiceItem.CounterKey && d.InvoiceItemLineNumber == supplierInvoiceItem.LineNumber).ToList();

                if (supplierInvoiceItem.SupplierInvoiceItemsConDeclars != null)
                {
                    if (supplierInvoiceItem.SupplierInvoiceItemsConDeclars.Count > 0)
                    {
                        supplierInvoiceItem.SupplierInvoiceItemsConnectedDeclarationLastLineNumber = supplierInvoiceItem.SupplierInvoiceItemsConDeclars.Max(m => m.LineNumber);
                    }
                }

                if (supplierInvoiceItem.SupplierInvoiceItemsMods != null)
                {
                    if (supplierInvoiceItem.SupplierInvoiceItemsMods.Count > 0)
                    {
                        supplierInvoiceItem.SupplierInvoiceItemsModificationLastLineNumber = supplierInvoiceItem.SupplierInvoiceItemsMods.Max(m => m.LineNumber);
                    }
                }

                if (supplierInvoiceItem.SupplierInvoiceItemTaxes != null)
                {
                    if (supplierInvoiceItem.SupplierInvoiceItemTaxes.Count > 0)
                    {
                        supplierInvoiceItem.SupplierInvoiceItemTaxLastLineNumber = supplierInvoiceItem.SupplierInvoiceItemTaxes.Max(m => m.LineNumber);
                    }
                }

                if (supplierInvoiceItem.SupplierInvioceItemCertificats != null)
                {
                    if (supplierInvoiceItem.SupplierInvioceItemCertificats.Count > 0)
                    {
                        supplierInvoiceItem.SupplierInvioceItemsCertificatLastLineNumber = supplierInvoiceItem.SupplierInvioceItemCertificats.Max(m => m.LineNumber);
                    }
                }

                if (supplierInvoiceItem.SupplierInvoiceItemsSerialNums != null)
                {
                    if (supplierInvoiceItem.SupplierInvoiceItemsSerialNums.Count > 0)
                    {
                        supplierInvoiceItem.SupplierInvoiceItemsSerialNumberLastLineNumber = supplierInvoiceItem.SupplierInvoiceItemsSerialNums.Max(m => m.LineNumber);
                    }
                }

                if (supplierInvoiceItem.SupplierInvoiceItemsProdIdents != null)
                {
                    if (supplierInvoiceItem.SupplierInvoiceItemsProdIdents.Count > 0)
                    {
                        supplierInvoiceItem.SupplierInvoiceItemsProductIdentificationLastLineNumber = supplierInvoiceItem.SupplierInvoiceItemsProdIdents.Max(m => m.LineNumber);
                    }
                }

                if (supplierInvoiceItem.SupplierInvoiceItemsDescripts != null)
                {
                    if (supplierInvoiceItem.SupplierInvoiceItemsDescripts.Count > 0)
                    {
                        supplierInvoiceItem.SupplierInvoiceItemsDescriptionLastLineNumber = supplierInvoiceItem.SupplierInvoiceItemsDescripts.Max(m => m.LineNumber);
                    }
                }

                if (supplierInvoiceItem.SupplierInvoiceItemProcesTypes != null)
                {
                    if (supplierInvoiceItem.SupplierInvoiceItemProcesTypes.Count > 0)
                    {
                        supplierInvoiceItem.SupplierInvoiceItemsProcessTypeLastLineNumber = supplierInvoiceItem.SupplierInvoiceItemProcesTypes.Max(m => m.LineNumber);
                    }
                }

                if (supplierInvoiceItem.SupplierInvoiceItemLevies != null)
                {
                    if (supplierInvoiceItem.SupplierInvoiceItemLevies.Count > 0)
                    {
                        supplierInvoiceItem.SupplierInvoiceItemsLevyLastLineNumber = supplierInvoiceItem.SupplierInvoiceItemLevies.Max(m => m.LineNumber);
                    }
                }

                if (supplierInvoiceItem.SupplierInvoiceItemVehicles != null)
                {
                    if (supplierInvoiceItem.SupplierInvoiceItemVehicles.Count > 0)
                    {
                        supplierInvoiceItem.SupplierInvoiceItemVehicleLastLineNumber = supplierInvoiceItem.SupplierInvoiceItemVehicles.Max(m => m.LineNumber);
                    }
                }
            }

            #endregion

            if (entityPM.SupplierInvoiceItems == null)
            {
                entityPM.SupplierInvoiceItems = new List<SupplierInvoiceItemPM>();
            }

            if (entityPM.SupplierInvoiceItems.Count > 0)
            {
                //entityPM.SupplierInvoiceItemLastLineNumber = entityPM.SupplierInvoiceItems.Max(m => m.LineNumber);
                entityPM.InvoiceItemLastLineNumber = supplierInvoiceItemQueryService.GetMaxLineNumber(entityPM.DeclarationId, entityPM.InvoiceCounterKey);
            }

            SupplierInvoiceFreightAmountQueryService supplierInvoiceFreightAmountQueryService = new SupplierInvoiceFreightAmountQueryService(context);

            entityPM.SupplierInvoiceFreightAmounts = supplierInvoiceFreightAmountQueryService.GetMulti(supplierInvoiceKeys, true);
            if (entityPM.SupplierInvoiceFreightAmounts == null)
            {
                entityPM.SupplierInvoiceFreightAmounts = new List<SupplierInvoiceFreightAmountPM>();
            }

        }

        //this is for opening the invoice from the customs answers.
        public SupplierInvoicePM GetSupplierInvoiceBySequenceNumber(string declarationId, int invoiceSequence,int skip, int take)
        {
            SupplierInvoicePM invoicePM = null;
            SupplierInvoice invoice = repository.GetSupplierInvoiceBySequenceNumeric(declarationId, invoiceSequence);
            if (invoice != null)
            {
                invoicePM = new SupplierInvoicePM();
                SupplierInvoiceDataMapping mapping = new SupplierInvoiceDataMapping();
                mapping.CustomPOCOToPM(invoicePM, invoice);
                mapping.POCOToPM(invoicePM, invoice);
                GetCompositionForLimitedItems(invoicePM, skip, take, null);
            }
            return invoicePM;
        }


        public SupplierInvoicePM GetSupplierInvoiceWithSpecificItemByCounterKey(string declarationId, int counterKey, int itemSequence, string type=null)
        {
            SupplierInvoice invoice = repository.GetSupplierInvoiceByCounterKey(declarationId, counterKey);
            SupplierInvoicePM invoicePM = new SupplierInvoicePM();
            SupplierInvoiceDataMapping mapping = new SupplierInvoiceDataMapping();
            mapping.CustomPOCOToPM(invoicePM, invoice);
            mapping.POCOToPM(invoicePM, invoice);
            GetCompositionForInvoiceWithSpecificItem(invoicePM, itemSequence,type);
            return invoicePM;
        }


        // this one is for opening the certificates from customs answers.
        public SupplierInvoicePM GetSupplierInvoiceWithSpecificItemBySequenceNumber(string declarationId, int invoiceSequence, int itemSequence, string type=null)
        {
            SupplierInvoice invoice = repository.GetSupplierInvoiceBySequenceNumeric(declarationId, invoiceSequence);
            if (invoice != null)
            {

                SupplierInvoicePM invoicePM = new SupplierInvoicePM();
                SupplierInvoiceDataMapping mapping = new SupplierInvoiceDataMapping();
                mapping.CustomPOCOToPM(invoicePM, invoice);
                mapping.POCOToPM(invoicePM, invoice);
                
                GetCompositionForInvoiceWithSpecificItem(invoicePM, itemSequence, type);
               
                return invoicePM;
            }
            else
            {
                return null;
            }
        }

        public void GetCompositionForInvoiceWithSpecificItem(SupplierInvoicePM entityPM, int sequenceNumeric, string type= null)
        {
            ICustomContext context = MainContext as CustomContext;
            SupplierInvoiceItemQueryService supplierInvoiceItemQueryService = new SupplierInvoiceItemQueryService(context);
            supplierInvoiceItemQueryService.LoadComposition = true;
            SupplierInvoiceKeys supplierInvoiceKeys = new SupplierInvoiceKeys() { DeclarationId = entityPM.DeclarationId, InvoiceCounterKey = entityPM.InvoiceCounterKey };


            SupplierInvoicePaymentQueryService supplierInvoicePaymentQueryService = new SupplierInvoicePaymentQueryService(context);
            entityPM.SupplierInvoicePayments = supplierInvoicePaymentQueryService.GetMulti(supplierInvoiceKeys, true);

            SupplierInvoiceUCRQueryService supplierInvoiceUCRQueryService = new SupplierInvoiceUCRQueryService(context);
            entityPM.SupplierInvoiceUCRs = supplierInvoiceUCRQueryService.GetMulti(supplierInvoiceKeys, true);

            SupplierInvoiceModificationQueryService supplierInvoiceModificationQueryService = new SupplierInvoiceModificationQueryService(context);
            entityPM.SupplierInvoiceModifications = supplierInvoiceModificationQueryService.GetMulti(supplierInvoiceKeys, true);

            SupplierInvoiceFreightAmountQueryService supplierInvoiceFreightAmountQueryService = new SupplierInvoiceFreightAmountQueryService(context);
            entityPM.SupplierInvoiceFreightAmounts = supplierInvoiceFreightAmountQueryService.GetMulti(supplierInvoiceKeys, true);


            if (entityPM.SupplierInvoiceFreightAmounts == null)
            {
                entityPM.SupplierInvoiceFreightAmounts = new List<SupplierInvoiceFreightAmountPM>();
            }
            SupplierInvoiceItemPM invoiceItem = new SupplierInvoiceItemPM();
            if (entityPM.IsAccumalated && type != "certificate")
            {
                invoiceItem = supplierInvoiceItemQueryService.GetParentSingleSupplierInvoicePMBySequence(entityPM.DeclarationId, entityPM.InvoiceCounterKey, sequenceNumeric);

                if (invoiceItem != null)
                {
                    int lineNumber = invoiceItem.LineNumber;
                    entityPM.SupplierInvoiceItems = supplierInvoiceItemQueryService.GetSupplierInvoiceItemsByParent(entityPM.DeclarationId, entityPM.InvoiceCounterKey, lineNumber, entityPM.Tenant);

                }

            }
            else
            {
                invoiceItem = supplierInvoiceItemQueryService.GetSingleSupplierInvoicePMBySequence(entityPM.DeclarationId, entityPM.InvoiceCounterKey, sequenceNumeric);
           }

            if (invoiceItem != null)
            {
                entityPM.SupplierInvoiceItems.Add(invoiceItem);
                entityPM.FullChildrenCount = entityPM.SupplierInvoiceItems.Where(d => !d.IsParent).Count();
                entityPM.FullParentsCount = entityPM.SupplierInvoiceItems.Where(d => d.IsParent).Count();

            }
            if (entityPM.SupplierInvoiceItems.Count > 0)
            {
                entityPM.InvoiceItemLastLineNumber = entityPM.SupplierInvoiceItems.Max(m => m.LineNumber);
            }

         

        }

        public SupplierInvoice CheckIfInvoiceNumberExists(string declarationId,string invoiceNumber,int invoiceCounterKey, int tenant)
        {
            return repository.CheckIfInvoiceNumberExists(declarationId, invoiceNumber, invoiceCounterKey, tenant);
        }

        //public int GetTotalFreightInInvoiceCurrencyForDeclarationInvoices(List<string> Ids, List<CustomsExchangeRatePM> rates)
        //{
        //    decimal? TotalFreightAmountInInvoiceCurrency;
        //    CustomsExchangeRateQueryService customsExchangeRateQueryService = new CustomsExchangeRateQueryService(context);

        //    SupplierInvoiceQueryService supplierInvoiceQueryService = new SupplierInvoiceQueryService(context);
        //    foreach (string id in Ids)
        //    {
        //        decimal? totalFreightInNIS = 0;
        //        decimal? totalFreightInInvoice = 0;
        //        CustomsExchangeRatePM rate;
        //        SupplierInvoiceFreightAmountQueryService supplierInvoiceFreightAmountQueryService = new SupplierInvoiceFreightAmountQueryService(context);

        //        SupplierInvoicePM invoice = supplierInvoiceQueryService.GetSingle(id, false, false);

        //        invoice.SupplierInvoiceFreightAmounts = supplierInvoiceFreightAmountQueryService.GetMulti(id, true);

        //        foreach (SupplierInvoiceFreightAmountPM item in invoice.SupplierInvoiceFreightAmounts)
        //        {



        //            decimal? amountInNIS;

        //            if (item.CurrencyTypeCode == "ILS")
        //            {
        //                amountInNIS = item.Amount;
        //                totalFreightInNIS = totalFreightInNIS + amountInNIS;

        //            }

        //            else
        //            {
        //                rate = rates.Where(d => d.CurrencyTypeCode == item.CurrencyTypeCode).FirstOrDefault();
        //                if (rate != null)
        //                {

        //                    amountInNIS = item.Amount * rate.ExchangeRate;
        //                    totalFreightInNIS = totalFreightInNIS + amountInNIS;


        //                }
        //            }
        //        }

        //        if (invoice.FreightCurrencyTypeCode == "ILS")
        //        {
        //            totalFreightInInvoice = totalFreightInNIS;
        //        }

        //        else
        //        {
        //            rate = rates.Where(d => d.CurrencyTypeCode == invoice.FreightCurrencyTypeCode).FirstOrDefault();
        //            if (rate != null)
        //            {

        //                totalFreightInInvoice = totalFreightInNIS / rate.ExchangeRate;

        //            }

        //        }



        //        if (this.EntityPM.TotalFreightInFreightCurrency != totalFreightInInvoice)
        //        {
        //            TotalFreightAmountInInvoiceCurrency = totalFreightInInvoice;

        //        }

        //    }

        //    return 1;
        //}


        //public SupplierInvoicePM GetSupplierInvoicWithoutComposition(string declarationId, int counterKey, int tenant)
        //{
        //    SupplierInvoice invoice = repository.GetSupplierInvoiceByCounterKey(declarationId, counterKey);
        //    SupplierInvoicePM invoicePM = new SupplierInvoicePM();
        //    SupplierInvoiceDataMapping mapping = new SupplierInvoiceDataMapping();
        //    mapping.CustomPOCOToPM(invoicePM, invoice);
        //    mapping.POCOToPM(invoicePM, invoice);
        //    return invoicePM;
        //}

        public bool DoesAnyInvoiceHasFreight(string declarationId,int tenant)
        {
            return repository.DoesAnyInvoiceHasFreight(declarationId, tenant);
        }

        public List<SupplierInvoicePM> GetSupplierInvoicesForDeclarationWithFreightsOnly(string declarationId, int tenant)
        {
            List<SupplierInvoice> supplierInvoices = repository.GetSupplierInvoicesForDeclaration(declarationId, tenant);
            SupplierInvoiceDataMapping mappings = new SupplierInvoiceDataMapping();

            List<SupplierInvoicePM> supplierInvoicePMs = new List<SupplierInvoicePM>();
            
            foreach (SupplierInvoice invoice in supplierInvoices)
            {
                SupplierInvoicePM invoicePM = new SupplierInvoicePM();

                mappings.CustomPOCOToPM(invoicePM, invoice);
                mappings.POCOToPM(invoicePM, invoice);
                SupplierInvoiceFreightAmountQueryService supplierInvoiceFreightAmountQueryService = new SupplierInvoiceFreightAmountQueryService(context);
                SupplierInvoiceKeys supplierInvoiceKeys = new Data.EntityKeys.SupplierInvoiceKeys() { DeclarationId = declarationId, InvoiceCounterKey = invoice.InvoiceCounterKey };
                invoicePM.SupplierInvoiceFreightAmounts = supplierInvoiceFreightAmountQueryService.GetMulti(supplierInvoiceKeys, true);
                if (invoicePM.SupplierInvoiceFreightAmounts == null)
                {
                    invoicePM.SupplierInvoiceFreightAmounts = new List<SupplierInvoiceFreightAmountPM>();
                }
                supplierInvoicePMs.Add(invoicePM);
            }
            return supplierInvoicePMs.OrderBy(d => d.SequenceNumeric).ToList();
        }


        public bool UpdateSupplierInvoiceByOcrDefaults(string declarationId, string supplierInvocieList,  SupplierInvioceItemCertificatPM[] supplierInvioceItemCertificats, SupplierInvioceExportDefaultPM supplierInvioceExportDefault,int tenant)
        {
            var context = CustomContext.GetContext(tenant);
            var arrSupplierInvocie = supplierInvocieList.Split(',');
            var myDeclarationQueryService = new DeclarationQueryService(context);
            DeclarationPM declarationPM = myDeclarationQueryService.GetSingle(declarationId, true, false);
            if (declarationPM == null)
                return false;
           

            var supplierInvoiceQueryServices = new SupplierInvoiceQueryService(context);
            var supplierInvoices = supplierInvoiceQueryServices.GetSupplierInvoicesForDeclaration(declarationId, tenant,true).Where(i=> arrSupplierInvocie.Contains((i.InvoiceCounterKey).ToString(), StringComparer.OrdinalIgnoreCase));

            foreach (var supplierInvoice in supplierInvoices)
            {
                UpdateSupplierInvoices(supplierInvoice, supplierInvioceExportDefault, context,  supplierInvioceItemCertificats);
            }
            // var supplierInvoiceItemUpdateService = new SupplierInvoiceUpdateService(context, new Dictionary<string, IContext>(), tenant);
                            return true;
        }
        private void UpdateSupplierInvoices(SupplierInvoicePM supplierInvoice, SupplierInvioceExportDefaultPM supplierInvioceExportDefault,ICustomContext context, SupplierInvioceItemCertificatPM[] supplierInvioceItemCertificats)
        {
            var supplierInvioceItemCertificatUpdateService = new SupplierInvioceItemCertificatUpdateService(context, new Dictionary<string, IContext>(), supplierInvoice.Tenant); 
            var supplierInvoiceItemUpdateService = new SupplierInvoiceUpdateService(context, new Dictionary<string, IContext>(), supplierInvioceExportDefault.Tenant);

            if (supplierInvioceExportDefault != null) {
                supplierInvoice.AccountTypeCode = supplierInvioceExportDefault.AccountTypeCode != "non" ? supplierInvioceExportDefault.AccountTypeCode : supplierInvoice.AccountTypeCode;
                supplierInvoice.PartyRelationshipCode = supplierInvioceExportDefault.PartyRelationshipCode!="non" ? supplierInvioceExportDefault.PartyRelationshipCode: supplierInvoice.PartyRelationshipCode;
                supplierInvoice.BuyerRoleCode = supplierInvioceExportDefault.BuyerRoleCode!="non"? supplierInvioceExportDefault.BuyerRoleCode : supplierInvoice.BuyerRoleCode;
                supplierInvoice.ChangeSetOp = ChangeSetOperation.Update;
            }
           
            foreach (var supplierInvoiceItem in supplierInvoice.SupplierInvoiceItems)
            {
                if (supplierInvioceExportDefault != null)
                {
                    supplierInvoiceItem.TransactionNatureCode = supplierInvioceExportDefault.TransactionNatureCode!="non"? supplierInvioceExportDefault.TransactionNatureCode: supplierInvoiceItem.TransactionNatureCode;
                    supplierInvoiceItem.ClaimReasonCode = supplierInvioceExportDefault.ClaimReasonCode!="non"? supplierInvioceExportDefault.ClaimReasonCode: supplierInvoiceItem.ClaimReasonCode;
                    supplierInvoiceItem.ChangeSetOp = ChangeSetOperation.Update;

                    if (supplierInvioceExportDefault.ProcessTypeCode != "non")
                    {
                        foreach (var supplierInvoiceItemProcesType in supplierInvoiceItem.SupplierInvoiceItemProcesTypes)
                        {
                            supplierInvoiceItemProcesType.ProcessTypeCode = supplierInvioceExportDefault.ProcessTypeCode;
                            supplierInvoiceItemProcesType.ChangeSetOp = ChangeSetOperation.Update;

                        }
                    }
                }

                if (supplierInvioceItemCertificats?.Length>0)
                {

                    supplierInvoiceItem.SupplierInvioceItemCertificats.ForEach(x => x.ChangeSetOp = ChangeSetOperation.Delete);
                    foreach (var Certificat in supplierInvioceItemCertificats)
                    {
                        Certificat.ChangeSetOp = ChangeSetOperation.Insert;
                        
                        supplierInvoiceItem.SupplierInvioceItemCertificats.Add(Certificat);
                            //CertificatUpdateService.Update(MyCertificat,true);
                    }
                   
                }
            }

            supplierInvoiceItemUpdateService.Update(supplierInvoice, true);

        }

    }
}
