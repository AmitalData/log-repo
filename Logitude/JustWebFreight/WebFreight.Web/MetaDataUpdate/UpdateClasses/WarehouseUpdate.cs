using Logitude.CRM.Data;
using Logitude.CRM.Data.Repsitories;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using WebFreight.Web.CommonDataModel.DomainServices;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using WebFreight.Web.MetaDataUpdate.AddClasses;
using WebFreight.Web.MetaDataUpdate.DetailClasses;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Server.Tools.Counters;
using Logitude.BL.CommonDataModel.EntityLists;

namespace WebFreight.Web.MetaDataUpdate.UpdateClasses
{
    public class WarehouseUpdate
    {
       
        private IWebFreightContext objectContext;
        private ObjectTablePM warehouseEntryObjectTable;
        private ObjectTablePM warehouseReleaseObjectTable;

        private ObjectTableQuery objectTabelQuery;

        public void CreateTableCounters()
        {
          
            objectContext = WebFreightContext.GetContext(0);
            objectTabelQuery = new ObjectTableQuery(new ObjectTableRepository(objectContext));
            CounterRepository counterRepository = new CounterRepository(objectContext);
            CounterDefinitionRepository counterDefinitionRepository = new CounterDefinitionRepository(objectContext);
            List<Counter> counters = counterRepository.All().ToList();

            List<ObjectTablePM> objectTables = objectTabelQuery.GetObjectPMsByTenant(0).ToList();

            #region ObjectTables
            warehouseEntryObjectTable = objectTables.Where(d => d.Name == "WarehouseEntry").FirstOrDefault();
            warehouseReleaseObjectTable = objectTables.Where(d => d.Name == "WarehouseRelease").FirstOrDefault();
            ObjectTablePM GeneralObjectTable = objectTables.Where(d => d.Name == "General").FirstOrDefault();

            #endregion

            TenantQuery tenantQuery = new TenantQuery(0);
            List<TenantList> tenantList = tenantQuery.GetTenantLists();

            foreach (TenantList tenant in tenantList)
            {
                #region Warehouse Entries Counters

                if (!counters.Where(c => c.Code == "WAEC" && c.Tenant == tenant.Id).Any())
                {
                    if (warehouseEntryObjectTable != null)
                    {
                        Counter warehouseEntriesCounter = new Counter()
                        {
                            Id = IdCounter.GetNumber("Counter", tenant.Id).ToString(),
                            ObjectTableId = warehouseEntryObjectTable.Id,
                            Code = "WAEC",
                            Tenant = tenant.Id,
                            Name = "Warehouse Entries",

                        };

                        CounterDefinition warehouseEntry_Counter = new CounterDefinition()
                        {
                            Id = IdCounter.GetNumber("CounterDefinition", tenant.Id).ToString(),
                            CounterId = warehouseEntriesCounter.Id,
                            Tenant = tenant.Id,
                            StartNumber = 1000,

                        };

                        counterRepository.Add(warehouseEntriesCounter);
                        counterDefinitionRepository.Add(warehouseEntry_Counter);
                    }

                }

                #endregion

                #region Warehouse Entries Counters


                if (!counters.Where(c => c.Code == "WARC" && c.Tenant == tenant.Id).Any())
                {
                    if (warehouseReleaseObjectTable != null)
                    {
                        Counter warehouseReleaseCounter = new Counter()
                        {
                            Id = IdCounter.GetNumber("Counter", tenant.Id).ToString(),
                            ObjectTableId = warehouseReleaseObjectTable.Id,
                            Code = "WARC",
                            Tenant = tenant.Id,
                            Name = "Warehouse Releases",

                        };

                        CounterDefinition warehouseRelease_Counter = new CounterDefinition()
                        {
                            Id = IdCounter.GetNumber("CounterDefinition", tenant.Id).ToString(),
                            CounterId = warehouseReleaseCounter.Id,
                            Tenant = tenant.Id,
                            StartNumber = 1000,

                        };

                        counterRepository.Add(warehouseReleaseCounter);
                        counterDefinitionRepository.Add(warehouseRelease_Counter);
                    }

                }

                #endregion

            }

            objectContext.SaveChanges();
        }

         

    }
}