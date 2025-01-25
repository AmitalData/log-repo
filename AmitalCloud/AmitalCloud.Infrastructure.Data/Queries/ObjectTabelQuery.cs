using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Data.Repositories;
using AmitalCloud.Infrastructure.Domain.EntityLists;
using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using AmitalCloud.Infrastructure.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;

namespace AmitalCloud.Infrastructure.Data.Queries
{
    public class ObjectTableQuery
    {
        ObjectTableRepository repository;

        public ObjectTableQuery(int tenant) => repository = new ObjectTableRepository(tenant);
        public ObjectTableQuery(ObjectTableRepository objectTabelRepository) => repository = objectTabelRepository;

        public IQueryable<ObjectTablePM> GetObjectPMsByTenant(int tenant)
        {
            List<ObjectTablePM> currentObjectTables = new List<ObjectTablePM>();
            List<ObjectTablePM> zeroObjectTables = new List<ObjectTablePM>();
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                currentObjectTables = repository.GetMulti(a => a.Tenant == tenant, a => new ObjectTablePM(a)
                {
                    FullNameTextCodeDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.DefaultText : a.Name,
                }
                , "HeaderScreen,DescriptionTextCode,NewButtonTextCode,FullNameTextCode").ToList();
            }
            if (tenant != 0)
            {
                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    zeroObjectTables = GetTenantZeroObjectTables();
                }
            }
            return currentObjectTables.Concat(zeroObjectTables).AsQueryable<ObjectTablePM>();
        }
        private List<ObjectTablePM> GetTenantZeroObjectTables()
        {
            string tenantZeroObjectTablesCacheKeyName = "tenantZeroObjectTables";
            if (HttpContext.Current != null && CacheManager.CacheWrapper.Get(tenantZeroObjectTablesCacheKeyName) != null)
            {
                return (List<ObjectTablePM>)CacheManager.CacheWrapper.Get(tenantZeroObjectTablesCacheKeyName);
            }
            List<ObjectTablePM> zeroObjectTables = 
                repository.GetMulti(a => a.Tenant == 0, a => new ObjectTablePM(a)
                {
                    FullNameTextCodeDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.DefaultText : a.Name,
                }, "HeaderScreen,DescriptionTextCode,NewButtonTextCode,FullNameTextCode").ToList();    

            CacheManager.CacheWrapper.Insert(tenantZeroObjectTablesCacheKeyName, zeroObjectTables, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
            return zeroObjectTables;
        }
        public static List<ObjectTablePM> GetObjectTablesWithTenantZero(int tenant)
        {
            string entityKeyString = $"GetObjectTablesWithTenantZero({tenant})";
            List<ObjectTablePM> myres = CacheManager.GetOrInsertNewObject<List<ObjectTablePM>>(entityKeyString, () =>
            {
                return GetObjectTablesWithTenantZeroBadCache(tenant);
            });
            return myres;
        }
        static List<ObjectTablePM> GetObjectTablesWithTenantZeroBadCache(int tenant)
        {
            string listName = "tenantzerotextobjecttablepms";
            string tenantListName = "tenantobjecttablepms" + tenant;
            List<ObjectTablePM> result = new List<ObjectTablePM>();
            List<ObjectTablePM> currentTenantTables = new List<ObjectTablePM>();
            List<ObjectTablePM> zeroTenantTables = new List<ObjectTablePM>();
            if (tenant != 0)
            {
                if (HttpContext.Current != null)
                {
                    currentTenantTables = (List<ObjectTablePM>)CacheManager.CacheWrapper.Get(tenantListName);
                    if (currentTenantTables == null)
                    {
                        currentTenantTables = GetCurrentTenantTables(tenant);
                        CacheManager.CacheWrapper.Insert(tenantListName, currentTenantTables, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                    }
                }
                else
                {
                    currentTenantTables = GetCurrentTenantTables(tenant);
                }
            }

            if (HttpContext.Current != null)
            {
                zeroTenantTables = (List<ObjectTablePM>)CacheManager.CacheWrapper.Get(listName);

                if (zeroTenantTables == null)
                {
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        IAmitalCloudContext context = AmitalCloudContext.GetContext(tenant);
                        zeroTenantTables = (from a in context.ObjectTables.Include("HeaderScreen").Include("DescriptionTextCode").Include("NewButtonTextCode").Include("FullNameTextCode")
                                            where (a.Tenant == 0 && a.InActive == false)
                                            select new ObjectTablePM(a)
                                            {
                                                FullNameTextCodeDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.DefaultText : a.Name,
                                            }).ToList();
                        scope.Complete();
                    }
                    CacheManager.CacheWrapper.Insert(listName, zeroTenantTables, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                }

            }
            else
            {
                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    IAmitalCloudContext context = AmitalCloudContext.GetContext(tenant);
                    zeroTenantTables = (from a in context.ObjectTables.Include("HeaderScreen").Include("DescriptionTextCode").Include("NewButtonTextCode").Include("FullNameTextCode")
                                        where (a.Tenant == 0 && a.InActive == false)
                                        select new ObjectTablePM(a )
                                        {
                                            FullNameTextCodeDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.DefaultText : a.Name,
                                        }).ToList();


                    scope.Complete();
                }

            }
            zeroTenantTables = zeroTenantTables == null ? new List<ObjectTablePM>() : zeroTenantTables;
            currentTenantTables = currentTenantTables == null ? new List<ObjectTablePM>() : currentTenantTables;

            result = zeroTenantTables.Concat(currentTenantTables).ToList();

            return result;
        }

        private static List<ObjectTablePM> GetCurrentTenantTables(int tenant)
        {
            List<ObjectTablePM> currentTenantTables;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                currentTenantTables = new ObjectTableRepository(tenant).GetMulti(a => a.Tenant == tenant && a.InActive == false,
                    a => new ObjectTablePM(a)
                    {
                        FullNameTextCodeDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.DefaultText : a.Name,
                    }, "FullNameTextCode").ToList();
                scope.Complete();
            }

            return currentTenantTables;
        }

        public static ObjectTablePM GetObjectTableByCode(string name, int tenant)
        {
            ObjectTablePM table = null;
            if (!string.IsNullOrEmpty(name))
            {
                table = GetObjectTablesWithTenantZero(tenant).Where(t => t.Name.ToLower() == name.ToLower()).FirstOrDefault();
            }

            return table;
        }
        public ObjectTablePM GetSinglePM(string id, int tenant)
        {
            ObjectTablePM table = null;
            if (!string.IsNullOrEmpty(id))
            {
                table = GetObjectTablesWithTenantZero(tenant).Where(t => t.Id == id).FirstOrDefault();
            }

            return table;
        }
        public static ObjectTablePM GetSingleObjectTableById(string id, int tenant)
        {
            ObjectTablePM table = null;
            if (!string.IsNullOrEmpty(id))
            {
                table = GetObjectTablesWithTenantZero(tenant).Where(t => t.Id == id).FirstOrDefault();
            }
            return table;
        }
        public ObjectTablePM GetObjectTableByName(string name, int tenant)
        {
            ObjectTablePM table = null;
            if (!string.IsNullOrEmpty(name))
            {
                table = GetObjectTablesWithTenantZero(tenant).Where(t => t.Name.ToLower() == name.ToLower()).FirstOrDefault();
            }
            return table;
        }
        public ObjectTablePM GetObjectTablePMById(string id, int tenant)
        {
            ObjectTablePM table = null;
            if (!string.IsNullOrEmpty(id))
            {
                table = GetObjectTablesWithTenantZero(tenant).Where(t => t.Id == id).FirstOrDefault();
            }
            return table;
        }
        public List<ObjectTablePM> GetSomeObjectTables(int tenant)
        {
            List<ObjectTablePM> tables = null;
            tables = GetObjectTablesWithTenantZero(tenant).Where(t => t.Name == "Customs.Declaration" ||
                t.Name == "Customs.Consignment" ||
                t.Name == "Customs.ConsignmentPackage" ||
                t.Name == "Customs.SupplierInvioceItemCertificat" ||
                t.Name == "Customs.SupplierInvoice" ||
                t.Name == "Customs.SupplierInvoiceItem" ||
                t.Name == "Customs.SupplierInvoiceItemsConDeclar" ||
                t.Name == "Customs.SupplierInvoiceItemsDescript" ||
                t.Name == "Customs.SupplierInvoiceItemsMod" ||
                t.Name == "Customs.SupplierInvoiceItemsProdIdent" ||
                t.Name == "Customs.SupplierInvoiceItemsQuantity" ||
                t.Name == "Customs.SupplierInvoiceItemsSerialNum" ||
                t.Name == "Customs.SupplierInvoiceModification" ||
                t.Name == "Customs.ConsignmentInternalTransition" ||
                t.Name == "Customs.DeclarationPayment" ||
                t.Name == "Customs.DeclarationPaymentMethod" ||
                t.Name == "Customs.DeclarationPaymentProtest" ||
                t.Name == "Customs.SupplierInvoiceFreightAmount" ||
                t.Name == "Customs.Claim" ||
                t.Name == "Customs.ClaimsRelatedEntity" ||
                t.Name == "Customs.CourierMaster" ||
                 t.Name == "Customs.SupplierInvoicePayment" ||
                   t.Name == "Customs.DeclarationExportRecipient"
                ).ToList();
            return tables;
        }
        public List<ObjectTablePM> GetSomeExportObjectTables(int tenant)
        {
            List<ObjectTablePM> tables = null;
            tables = GetObjectTablesWithTenantZero(tenant).Where(t => t.Name == "Customs.Declaration" ||
                t.Name == "Customs.Consignment" ||
                t.Name == "Customs.ConsignmentPackage" ||
                t.Name == "Customs.SupplierInvoice" ||
                t.Name == "Customs.SupplierInvoiceItem" ||
                t.Name == "Customs.DeclarationExportRecipient"
                ).ToList();


            return tables;
        }
        public IQueryable<ObjectTableList> GetIQueryableEntityList(IQueryable<ObjectTable> iQueryable)
        {
            IQueryable<ObjectTableList> result = from a in iQueryable.Include("FullNameTextCode")
                                                 select new ObjectTableList(a)
                                                 {
                                                     FullNameTextCodeDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.DefaultText : a.Name,
                                                 };
            return result;
        }
        public ObjectTableList GetObjectTableList(string id, int tenant)
        {
            AmitalCloudContext context = (AmitalCloudContext)AmitalCloudContext.GetContext(tenant);
            ObjectTableList ObjectTableList = (from a in context.ObjectTables.Include("FullNameTextCode")
                                               where (a.Tenant == tenant || a.Tenant == 0)
                                               && a.Id == id
                                               && a.InActive == false
                                               select new ObjectTableList(a)
                                               {
                                                   FullNameTextCodeDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.DefaultText : a.Name,
                                               }).FirstOrDefault();
            return ObjectTableList;
        }
        public string GetObjectTableIdByName(string tableName)
        {
            return repository.GetObjectTableIdByName(tableName);
        }
        public string GetObjectTableNamesById(string id, int tenant)
        {
            var item = repository.GetSingleObjectTable(id, tenant, true);
            if (item != null)
            {
                return item.Name;
            }
            return null;
        }
    }
}
