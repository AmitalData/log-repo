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
    public class QBOGlobalTaxCalculationQuery
    {
        QBOGlobalTaxCalculationRepository repository;
        public QBOGlobalTaxCalculationQuery()
        {
            repository = new QBOGlobalTaxCalculationRepository();
        }


        public QBOGlobalTaxCalculationQuery(int tenant)
        {
            repository = new QBOGlobalTaxCalculationRepository(tenant);
        }

        public QBOGlobalTaxCalculationQuery(QBOGlobalTaxCalculationRepository QBOGlobalTaxCalculationRepository)
        {
            repository = QBOGlobalTaxCalculationRepository;
        }

        public QBOGlobalTaxCalculationPM GetSingleQBOGlobalTaxCalculationPM(string code)
        {
            return (from a in repository.context.QBOGlobalTaxCalculations
                    where a.Code == code
                    select new QBOGlobalTaxCalculationPM()
                    {
                        Code = a.Code,
                        Name = a.Name,

                    }).FirstOrDefault();
        }

        public IQueryable<QBOGlobalTaxCalculationPM> GetQBOGlobalTaxCalculationPMs()
        {
            return (from a in repository.context.QBOGlobalTaxCalculations

                    select new QBOGlobalTaxCalculationPM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                    });
        }

        public IQueryable<QBOGlobalTaxCalculationList> GetIQueryableEntityList(IQueryable<QBOGlobalTaxCalculation> iQueryable)
        {
            IQueryable<QBOGlobalTaxCalculationList> result = from entity in iQueryable
                                                     select new QBOGlobalTaxCalculationList()
                                                     {
                                                         Name = entity.Name,
                                                         Code = entity.Code,
                                                         SearchFields = entity.SearchFields,
                                                     };

            return result;
        }
    }
}