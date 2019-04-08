using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Data.InvoiceModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.InvoiceModel.EntityQueries
{
    public class ARInvoiceStockLineQuery
    {
        private ARInvoiceStockLineRepository repository;

        public ARInvoiceStockLineQuery(int tenant)
        {
            repository = new ARInvoiceStockLineRepository(tenant);
        }

        public ARInvoiceStockLineQuery(ARInvoiceStockLineRepository ARInvoiceStockLineRepository)
        {
            repository = ARInvoiceStockLineRepository;
        }

        public List<ARInvoiceStockLinePM> GetARInvoiceStockLinePMsByStockId(string stockId, int tenant)
        {
            List<ARInvoiceStockLinePM> list = new List<ARInvoiceStockLinePM>();
                
              list  = (from a in repository.context.ARInvoiceStockLines.Include("CreatedByUser").Include("UpdatedByUser")
                                          where a.Tenant == tenant && a.ARInvoiceStockId == stockId
                                          select new ARInvoiceStockLinePM()
                                          {
                                              Id = a.Id,                                              
                                              Tenant = a.Tenant,
                                              ARInvoiceStockId = a.ARInvoiceStockId,
                                              Number = a.Number,
                                              CreateDate = a.CreateDate,
                                              CreatedByUserId = a.CreatedByUserId,
                                              CreatedByUserName = a.CreatedByUser == null ? null : (a.CreatedByUser.Contact == null ? null : a.CreatedByUser.Contact.EnglishName),
                                              UpdateDate = a.UpdateDate,
                                              UpdatedByUserId = a.UpdatedByUserId,
                                              UpdatedByUserName = a.UpdatedByUser == null ? null : (a.UpdatedByUser.Contact == null ? null : a.UpdatedByUser.Contact.EnglishName),
                                              IsUsed = a.IsUsed,
                                              ARInvoiceId = a.ARInvoiceId,                                             
                                          }).ToList();
            

            return list.ToList();
        }

        public ARInvoiceStockLinePM GetSinglePM(string id, int tenant)
        {
            ARInvoiceStockLinePM myResult = (from a in repository.context.ARInvoiceStockLines.Include("CreatedByUser").Include("UpdatedByUser")
                                             where a.Id == id
                                        select new ARInvoiceStockLinePM()
                                        {
                                            Id = a.Id,
                                            Tenant = a.Tenant,
                                            ARInvoiceStockId = a.ARInvoiceStockId,
                                            Number = a.Number,
                                            CreateDate = a.CreateDate,
                                            CreatedByUserId = a.CreatedByUserId,
                                            CreatedByUserName = a.CreatedByUser == null ? null : (a.CreatedByUser.Contact == null ? null : a.CreatedByUser.Contact.EnglishName),
                                            UpdateDate = a.UpdateDate,
                                            UpdatedByUserId = a.UpdatedByUserId,
                                            UpdatedByUserName = a.UpdatedByUser == null ? null : (a.UpdatedByUser.Contact == null ? null : a.UpdatedByUser.Contact.EnglishName),
                                            IsUsed = a.IsUsed,
                                            ARInvoiceId = a.ARInvoiceId,
                                        }).FirstOrDefault();          

            return myResult;
        }        
    }
}
