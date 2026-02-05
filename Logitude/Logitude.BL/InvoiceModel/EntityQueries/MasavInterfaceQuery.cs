using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;

using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityPMs;
using System.Data.Entity;


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


        public IQueryable<MasavInterfaceList> GetIQueryableEntityList(    IQueryable<MasavInterface> iQueryable)
        {
            return
                from entity in iQueryable
                select new MasavInterfaceList
                {
                    Id = entity.Id,
                    Tenant = entity.Tenant,
                    CreateDate = entity.CreateDate,
                    CreatedByUserId = entity.CreatedByUserId,
                    UpdateDate = entity.UpdateDate,
                    UpdatedByUserId = entity.UpdatedByUserId,
                    SearchFields = entity.SearchFields,
                    FromDate = entity.FromDate,
                    ToDate = entity.ToDate,
                    PaymentDate = entity.PaymentDate,
                    TotalPayments =
                        repository.context.APPayments
                            .Count(p => p.MasavInterfaceId == entity.Id),
                    Amount =
                        repository.context.APPayments
                            .Where(p => p.MasavInterfaceId == entity.Id)
                            .Sum(p =>p.AmountInPaymentCurrency) ?? 0,
                    StatusCode = entity.StatusCode,
                    StatusName = entity.Status != null
                        ? entity.Status.LocalName
                        : null
                };
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