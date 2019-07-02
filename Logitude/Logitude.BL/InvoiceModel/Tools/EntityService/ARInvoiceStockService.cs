using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.Tools.DataMapping;
using Logitude.BL.InvoiceModel.Tools.TraceEvents;
using Logitude.BL.InvoiceModel.Tools.Validating;
using Logitude.BL.Resolvers;
using Logitude.BL.Security;
using Logitude.Server.Tools.Counters;
using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.EntityService
{
    public class ARInvoiceStockService
    {
        bool isNewEntity;
        private int tenant;
        public ARInvoiceStock Poco { get; set; }
        private ARInvoiceStockPM entityPm;
        private IInvoiceContext objectContext;
        private ARInvoiceStockRepository entityRepository;
        private ARInvoiceStockLineRepository stockLineRepository;
        private ContactPM loggedContact;
        private DateTime todayDate;
        public ARInvoiceStockService(IInvoiceContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new ARInvoiceStockRepository(objectContext);
            this.stockLineRepository = new ARInvoiceStockLineRepository(objectContext);
            this.todayDate = TenantServerConfigration.GetCurrentDateTime(tenant);

            this.GetLoggedData();
        }

        private void GetLoggedData()
        {
            this.loggedContact = LoggedContactResolver.GetLoggedContact(tenant);

            //ContactQuery contactQuery = new ContactQuery(tenant);
            //this.loggedContact = contactQuery.GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), tenant, true);

            //if (this.loggedContact == null)
            //{
            //    loggedContact = contactQuery.GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), tenant);
            //}
        }

        private List<ARInvoiceStockLinePM> aRInvoiceStockLinesChangeSet;
        public void SetChangeSet(List<ARInvoiceStockLinePM> aRInvoiceStockLinesChangeSet)
        {
            this.aRInvoiceStockLinesChangeSet = aRInvoiceStockLinesChangeSet;
        }

        public void Create(ARInvoiceStockPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("ARInvoiceStock", tenant).ToString();
            this.Poco = new ARInvoiceStock();
            this.Poco.Id = this.entityPm.Id;

            ARInvoiceStockValidating.Validate(entityPM, objectContext, this.isNewEntity);
            
            foreach (ARInvoiceStockLinePM item in entityPM.ARInvoiceStockLines)
            {
                this.CreateARInvoiceStockLine(item);
            }

            entityPM.Amount = entityPM.Remaining = entityPM.ARInvoiceStockLines.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).Count();

            ARInvoiceStockTracing.Trace(entityPM, Poco, isNewEntity, loggedContact.Id);
            ARInvoiceStockMapping.MapEntity(entityPM, Poco, isNewEntity, loggedContact.Id);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

            TableLastUpdateClass.UpdateTableHistory(tenant, "ARInvoiceStock");
        }

        public void Update(ARInvoiceStockPM entityPM, bool mapComposition)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleARInvoiceStock(entityPM.Id, entityPM.Tenant);

            ARInvoiceStockValidating.Validate(entityPM, objectContext, this.isNewEntity);

            if (mapComposition)
            {
                this.aRInvoiceStockLinesChangeSet = entityPM.ARInvoiceStockLines;
            }

            this.UpdateARInvoiceStockLinesCollection();

            if (entityPM.Cancelled)
            {
                entityPM.StatusCode = "C";
                entityPM.Inactive = true;
            }

            else if (entityPM.Reactivated)
            {
                this.RecalculateStatusAfterReactivate();                
            }

            else
            {
                entityPM.StatusCode = GetStockStatus(entityPM);
                ARInvoiceStocksStatusRepository aRInvoiceStocksStatusRepository = new ARInvoiceStocksStatusRepository(entityPM.Tenant);
                entityPM.StatusName = aRInvoiceStocksStatusRepository.GetSingleARInvoiceStocksStatus(entityPM.StatusCode).Name;

            }

            entityPM.Amount = entityPM.ARInvoiceStockLines.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).Count();
            entityPM.Remaining = entityPM.ARInvoiceStockLines.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete && !d.IsUsed).Count();

            ARInvoiceStockTracing.Trace(entityPM, Poco, isNewEntity, loggedContact.Id);
            ARInvoiceStockMapping.MapEntity(entityPM, Poco, isNewEntity, loggedContact.Id);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();

            TableLastUpdateClass.UpdateTableHistory(tenant, "ARInvoiceStock");
        }

        private void UpdateARInvoiceStockLinesCollection()
        {
            if (aRInvoiceStockLinesChangeSet != null)
            {
                foreach (ARInvoiceStockLinePM itemPM in aRInvoiceStockLinesChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateARInvoiceStockLine(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateARInvoiceStockLine(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteARInvoiceStockLine(itemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }
        private void CreateARInvoiceStockLine(ARInvoiceStockLinePM itemPM)
        {
            itemPM.Id = IdCounter.GetNumber("ARInvoiceStockLine", tenant).ToString();
            itemPM.ARInvoiceStockId = entityPm.Id;
            itemPM.Tenant = tenant;
            itemPM.CreateDate = todayDate;

            ARInvoiceStockLine itemPoco = new ARInvoiceStockLine()
            {
                Id = itemPM.Id,
                ARInvoiceStockId = itemPM.ARInvoiceStockId,
                Tenant = itemPM.Tenant,
                CreateDate = itemPM.CreateDate,
            };

            ARInvoiceStockMapping.MapARInvoiceStockLine(itemPM, itemPoco, true, loggedContact.Id);
            stockLineRepository.Add(itemPoco);
        }
        private void UpdateARInvoiceStockLine(ARInvoiceStockLinePM itemPM)
        {
            ARInvoiceStockLine itemPoco = stockLineRepository.GetSingleARInvoiceStockLine(itemPM.Id, tenant);

            if (itemPoco != null)
            {
                ARInvoiceStockMapping.MapARInvoiceStockLine(itemPM, itemPoco, false, loggedContact.Id);
                stockLineRepository.Update(itemPoco);
            }
        }
        private void DeleteARInvoiceStockLine(ARInvoiceStockLinePM itemPM)
        {
            ARInvoiceStockLine itemPoco = stockLineRepository.GetSingleARInvoiceStockLine(itemPM.Id, tenant);

            if (itemPoco != null)
            {
                stockLineRepository.Remove(itemPoco);
            }
        }

        private void RecalculateStatusAfterReactivate()
        {
            if (entityPm.Amount > 0 && entityPm.Remaining != 0 && entityPm.Remaining <= entityPm.Amount && entityPm.EndDate != null && entityPm.EndDate <= TenantServerConfigration.GetCurrentDateTime(tenant))
            {
                entityPm.StatusCode = "E";
            }

            else if(entityPm.Amount > 0 && entityPm.Remaining == 0 && (entityPm.EndDate == null || entityPm.EndDate >= TenantServerConfigration.GetCurrentDateTime(tenant)))
            {
                entityPm.StatusCode = "U";
            }

            else if (entityPm.Amount > 0 && entityPm.Remaining <= (entityPm.Amount - 1) && (entityPm.EndDate == null || entityPm.EndDate >= TenantServerConfigration.GetCurrentDateTime(tenant)))
            {
                entityPm.StatusCode = "A";
            }

            else
            {
                entityPm.StatusCode = "N";
            }

            entityPm.Inactive = false;
        }

        private string GetStockStatus(ARInvoiceStockPM stock)
        {
            int stockLinesCount = stock.ARInvoiceStockLines.Where(d => d.IsUsed && d.ChangeSetOp != ChangeSetOperation.Delete).Count();
            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            string statusCode = "";
            
            if (stockLinesCount == 0)
            {
                if (stock.EndDate != null)
                {
                    if (stock.EndDate >= todayDate)
                    {
                        statusCode = "N";
                    }
                    else
                    {
                        statusCode = "E";
                    }
                }
                else
                {
                    statusCode = "N";
                }
            }

            else
            {
                if (stock.Amount == stockLinesCount)
                {
                    statusCode = "U";
                }

                else if ((stock.Amount != stockLinesCount))
                {
                    if (stock.EndDate != null)
                    {
                        if (stock.EndDate >= todayDate)
                        {
                            statusCode = "A";
                        }
                        else
                        {
                            statusCode = "E";
                        }
                    }
                    else
                    {
                        statusCode = "A";
                    }
                }
            }

            return statusCode;
        }
    }
}
