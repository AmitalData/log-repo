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
    public class SATPaymentMethodQuery
    {
        SATPaymentMethodRepository repository;
        public SATPaymentMethodQuery()
        {
            repository = new SATPaymentMethodRepository();
        }


        public SATPaymentMethodQuery(int tenant)
        {
            repository = new SATPaymentMethodRepository(tenant);
        }

        public SATPaymentMethodQuery(SATPaymentMethodRepository SATPaymentMethodRepository)
        {
            repository = SATPaymentMethodRepository;
        }

        public SATPaymentMethodPM GetSingleSATPaymentMethodPM(string code)
        {
            return (from a in repository.context.SATPaymentMethods
                    where a.Code == code
                    select new SATPaymentMethodPM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        LocalName = a.LocalName,
                        SearchFields = a.SearchFields,

                    }).FirstOrDefault();
        }

        public IQueryable<SATPaymentMethodPM> GetSATPaymentMethodPMs()
        {
            return (from a in repository.context.SATPaymentMethods

                    select new SATPaymentMethodPM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        LocalName = a.LocalName,
                        SearchFields = a.SearchFields,

                    });
        }

        public IQueryable<SATPaymentMethodList> GetIQueryableEntityList(IQueryable<SATPaymentMethod> iQueryable)
        {
            IQueryable<SATPaymentMethodList> result = from entity in iQueryable
                                                      select new SATPaymentMethodList()
                                                      {
                                                          Name = entity.Name,
                                                          Code = entity.Code,
                                                          LocalName = entity.LocalName,
                                                          SearchFields = entity.SearchFields,
                                                      };

            return result;
        }
    }
}