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
    public class SATInvoiceStatusQuery
    {
        SATInvoiceStatusRepository repository;
        public SATInvoiceStatusQuery()
        {
            repository = new SATInvoiceStatusRepository();
        }


        public SATInvoiceStatusQuery(int tenant)
        {
            repository = new SATInvoiceStatusRepository(tenant);
        }

        public SATInvoiceStatusQuery(SATInvoiceStatusRepository SATInvoiceStatusRepository)
        {
            repository = SATInvoiceStatusRepository;
        }

        public SATInvoiceStatusPM GetSingleSATInvoiceStatusPM(string code)
        {
            return (from a in repository.context.SATInvoiceStatus
                    where a.Code == code
                    select new SATInvoiceStatusPM()
                    {
                        Code = a.Code,
                        Name = a.Name,

                    }).FirstOrDefault();
        }

        public IQueryable<SATInvoiceStatusPM> GetSATInvoiceStatusPMs()
        {
            return (from a in repository.context.SATInvoiceStatus

                    select new SATInvoiceStatusPM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                    });
        }

        public IQueryable<SATInvoiceStatusList> GetIQueryableEntityList(IQueryable<SATInvoiceStatus> iQueryable)
        {
            IQueryable<SATInvoiceStatusList> result = from entity in iQueryable
                                                     select new SATInvoiceStatusList()
                                                     {
                                                         Name = entity.Name,
                                                         Code = entity.Code,
                                                         SearchFields = entity.SearchFields,
                                                     };

            return result;
        }
    }
}