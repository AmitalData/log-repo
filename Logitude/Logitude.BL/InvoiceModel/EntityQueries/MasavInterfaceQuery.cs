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
    public class MasavInterfaceQuery
    {
        MasavInterfaceRepository repository;
        public MasavInterfaceQuery()
        {
            repository = new MasavInterfaceRepository();
        }


        public MasavInterfaceQuery(int tenant)
        {
            repository = new MasavInterfaceRepository(tenant);
        }

        public MasavInterfaceQuery(MasavInterfaceRepository MasavInterfaceRepository)
        {
            repository = MasavInterfaceRepository;
        }

       
        public IQueryable<MasavInterfaceList> GetIQueryableEntityList(IQueryable<MasavInterface> iQueryable)
        {
            IQueryable<MasavInterfaceList> result = from entity in iQueryable
                                                     select new MasavInterfaceList()
                                                     {
                                                         
                                                     };

            return result;
        }
        public MasavInterfacePM GetSinglePM(string id , int tenant)
        {
            return (from a in repository.context.MasavInterfaces
                    where a.Id == id && a.Tenant== tenant
                    select new MasavInterfacePM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        CreateDate = a.CreateDate,
                        CreatedByUserId = a.CreatedByUserId,
                        UpdateDate = a.UpdateDate,
                        UpdatedByUserId = a.UpdatedByUserId,
                        SearchFields = a.SearchFields,
                        FromDate = a.FromDate,
                        ToDate = a.ToDate,
                        PaymentDate = a.PaymentDate,
                        Amount = 0,
                        TotalPayments = 0,
                        StatusCode = a.StatusCode,
                        StatusName = a.Status.LocalName,
                    }).FirstOrDefault();

        }
    }
}