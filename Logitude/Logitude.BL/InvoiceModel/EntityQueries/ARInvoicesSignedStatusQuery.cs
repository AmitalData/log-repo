using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.EntityQueries
{
  
        public class ARInvoicesSignedStatusQuery
    {
        ARInvoicesSignedStatusRepository repository;

            public ARInvoicesSignedStatusQuery()
            {
                repository = new ARInvoicesSignedStatusRepository();
            }

            public ARInvoicesSignedStatusQuery(int tenant)
            {
                repository = new ARInvoicesSignedStatusRepository(tenant);
            }

            public ARInvoicesSignedStatusQuery(ARInvoicesSignedStatusRepository arInvoicesSignedStatusRepository)
            {
                repository = arInvoicesSignedStatusRepository;
            }

            public IQueryable<ARInvoicesSignedStatusPM> GetInvoicesSignedStatusPMs()
            {
                return from a in repository.context.ARInvoicesSignedStatuses
                       select new ARInvoicesSignedStatusPM()
                       {
                           Code = a.Code,
                           EnglishName = a.EnglishName,
                           SearchFields = a.SearchFields,
                       };
            }

            public ARInvoicesSignedStatusPM GetSingleInvoicesSignedStatusPM(string code)
            {
                return (from a in repository.context.ARInvoicesSignedStatuses
                        where a.Code == code
                        select new ARInvoicesSignedStatusPM()
                        {
                            Code = a.Code,
                            EnglishName = a.EnglishName,
                            SearchFields = a.SearchFields,
                        }).FirstOrDefault();
            }


            public ARInvoicesSignedStatusPM GetSinglePM(string code, int tenant)
            {
                return (from a in repository.context.ARInvoicesSignedStatuses
                        where a.Code == code
                        select new ARInvoicesSignedStatusPM()
                        {
                            Code = a.Code,
                            EnglishName = a.EnglishName,
                            SearchFields = a.SearchFields,
                        }).FirstOrDefault();
            }

            public ARInvoicesSignedStatusPM GetSinglePM(string code)
            {
                return (from a in repository.context.ARInvoicesSignedStatuses
                        where a.Code == code
                        select new ARInvoicesSignedStatusPM()
                        {
                            Code = a.Code,
                            EnglishName = a.EnglishName,
                            SearchFields = a.SearchFields,
                        }).FirstOrDefault();
            }
            public IQueryable<ARInvoicesSignedStatusList> GetIQueryableEntityList(IQueryable<ARInvoicesSignedStatus> iQueryable)
            {
                IQueryable<ARInvoicesSignedStatusList> result = from entity in iQueryable
                                                         select new ARInvoicesSignedStatusList()
                                                         {
                                                             LocalName = entity.LocalName,
                                                             Code = entity.Code,
                                                             EnglishName = entity.EnglishName,
                                                         };
                return result;
            }

        }
   
}
