using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;

using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityPMs;


namespace Logitude.BL.InvoiceModel.EntityQueries
{
    public class SATTransferStatusQuery
    {
        SATTransferStatusRepository repository;
        public SATTransferStatusQuery()
        {
            repository = new SATTransferStatusRepository();
        }


        public SATTransferStatusQuery(int tenant)
        {
            repository = new SATTransferStatusRepository(tenant);
        }

        public SATTransferStatusQuery(SATTransferStatusRepository SATTransferStatusRepository)
        {
            repository = SATTransferStatusRepository;
        }

        public SATTransferStatusPM GetSingleSATTransferStatusPM(string code)
        {
            return (from a in repository.context.SATTransferStatus
                    where a.Code == code
                    select new SATTransferStatusPM()
                    {
                        Code = a.Code,
                        Name = a.Name,

                    }).FirstOrDefault();
        }

        public IQueryable<SATTransferStatusPM> GetSATTransferStatusPMs()
        {
            return (from a in repository.context.SATTransferStatus

                    select new SATTransferStatusPM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                    });
        }

        public IQueryable<SATTransferStatusList> GetIQueryableEntityList(IQueryable<SATTransferStatus> iQueryable)
        {
            IQueryable<SATTransferStatusList> result = from entity in iQueryable
                                                     select new SATTransferStatusList()
                                                     {
                                                         Name = entity.Name,
                                                         Code = entity.Code,
                                                         SearchFields = entity.SearchFields,
                                                     };

            return result;
        }
    }
}