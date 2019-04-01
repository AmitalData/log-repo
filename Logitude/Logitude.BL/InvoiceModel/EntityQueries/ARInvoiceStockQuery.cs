using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Data.InvoiceModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Logitude.BL.InvoiceModel.EntityLists;

namespace Logitude.BL.InvoiceModel.EntityQueries
{
    public class ARInvoiceStockQuery
    {
        ARInvoiceStockRepository repository;
        public ARInvoiceStockQuery()
        {
            repository = new ARInvoiceStockRepository();
        }

        public ARInvoiceStockQuery(ARInvoiceStockRepository ARInvoiceStockRepository)
        {
            repository = ARInvoiceStockRepository;
        }

        public ARInvoiceStockQuery(int tenant)
        {
            repository = new ARInvoiceStockRepository(tenant);
        }

        public IQueryable<ARInvoiceStockPM> GetARInvoiceStockPMsByTenant(int tenant)
        {
            return from a in repository.context.ARInvoiceStocks
                   where a.Tenant == tenant
                   select new ARInvoiceStockPM()
                   {
                       Id = a.Id,
                       Tenant = a.Tenant,
                       Name = a.Name,
                       Description = a.Description,
                       Inactive = a.Inactive,
                       CreateDate = a.CreateDate,
                       CreatedByUserId = a.CreatedByUserId,
                       UpdateDate = a.UpdateDate,
                       UpdatedByUserId = a.UpdatedByUserId,
                       StatusCode = a.StatusCode,
                       StartDate = a.StartDate,
                       EndDate = a.EndDate,
                       Remaining = a.Remaining,
                       Amount = a.Amount,
                       Notes = a.Notes,
                   };
        }

        public ARInvoiceStockPM GetSinglePM(string id, int tenant)
        {
            ARInvoiceStockLineRepository aRInvoiceStockLineRepository = new ARInvoiceStockLineRepository(repository.context);
            ARInvoiceStockLineQuery aRInvoiceStockLineQuery = new ARInvoiceStockLineQuery(aRInvoiceStockLineRepository);
            
            ARInvoiceStockPM entityPM = (from a in repository.context.ARInvoiceStocks.Include("CreatedByUser").Include("UpdatedByUser").Include("Status")
                    where a.Id == id && a.Tenant == tenant
                    select new ARInvoiceStockPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        Name = a.Name,
                        Description = a.Description,
                        Inactive = a.Inactive,
                        CreateDate = a.CreateDate,
                        CreatedByUserId = a.CreatedByUserId,                        
                        UpdateDate = a.UpdateDate,
                        UpdatedByUserId = a.UpdatedByUserId,
                        StatusCode = a.StatusCode,
                        StartDate = a.StartDate,
                        EndDate = a.EndDate,
                        Remaining = a.Remaining,
                        Amount = a.Amount,
                        Notes = a.Notes,
                    }).FirstOrDefault();

            if (entityPM != null)
            {
                entityPM.ARInvoiceStockLines = aRInvoiceStockLineQuery.GetARInvoiceStockLinePMsByStockId(entityPM.Id, tenant).ToList();
            }
            
            return entityPM;
        }

        public IQueryable<ARInvoiceStockList> GetIQueryableEntityList(IQueryable<ARInvoiceStock> iQueryable)
        {
            IQueryable<ARInvoiceStockList> result = from a in iQueryable.Include("CreatedByUser").Include("UpdatedByUser").Include("Status")
                                                    select new ARInvoiceStockList()
                                                    {
                                                        Id = a.Id,
                                                        Tenant = a.Tenant,
                                                        Name = a.Name,
                                                        Description = a.Description,
                                                        Inactive = a.Inactive,
                                                        CreateDate = a.CreateDate,
                                                        CreatedByUserId = a.CreatedByUserId,
                                                        CreatedByUserName = a.CreatedByUser == null ? null : (a.CreatedByUser.Contact == null ? null : a.CreatedByUser.Contact.EnglishName),
                                                        UpdateDate = a.UpdateDate,
                                                        UpdatedByUserId = a.UpdatedByUserId,
                                                        UpdatedByUserName = a.UpdatedByUser == null ? null : (a.UpdatedByUser.Contact == null ? null : a.UpdatedByUser.Contact.EnglishName),
                                                        StatusCode = a.StatusCode,
                                                        StatusName = a.Status == null ? null : a.Status.Name,
                                                        StartDate = a.StartDate,
                                                        EndDate = a.EndDate,
                                                        Remaining = a.Remaining,
                                                        Amount = a.Amount,
                                                        Notes = a.Notes,
                                                    };
            return result;
        }
    }
}
