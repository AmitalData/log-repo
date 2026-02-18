using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class ChargeTypeAccountingQuery
    {
        ChargeTypeAccountingRepository repository;

        public ChargeTypeAccountingQuery()
        {
            repository = new ChargeTypeAccountingRepository();
        }

        public ChargeTypeAccountingQuery(int tenant)
        {
            repository = new ChargeTypeAccountingRepository(tenant);
        }

        public ChargeTypeAccountingQuery(ChargeTypeAccountingRepository rep)
        {
            repository = rep;
        }

        public ChargeTypeAccountingPM GetSinglePM(string id, int tenant)
        {
            ChargeTypeAccountingPM charges = (from d in repository.context.ChargeTypeAccountings.Include("VatType").Include("ChargesType")
                                              where d.Id == id && d.Tenant == tenant
                                              select new ChargeTypeAccountingPM()
                                     {
                                         Id = d.Id,
                                         Tenant = d.Tenant,
                                         ChargeTypeId = d.ChargeTypeId,
                                         VatTypeId = d.VatTypeId,
                                         ReceivableCreditAccount = d.ReceivableCreditAccount,
                                         PayableDebitAccount = d.PayableDebitAccount,
                                         VatTypeName = d.VatType == null ? "" : d.VatType.EnglishName,
                                         ChargeTypeName = d.ChargeType == null ? "" : d.ChargeType.EnglishName,
                                         PayableDebitGLAcountId = d.PayableDebitGLAcountId,
                                         ReceivableCreditGLAccountId = d.ReceivableCreditGLAccountId,
                                     }).FirstOrDefault();

            ChargeTypeAccountingPM securedPm = new ChargeTypeAccountingPM();
            SecuredMapping.GetMappedPM(charges, securedPm, "ChargeTypeAccounting", tenant);

            return securedPm;
        }

        public IQueryable<ChargeTypeAccountingPM> GetChargeTypeAccountingsForChargeType(string chargeTypeId, int tenant)
        {
            IQueryable<ChargeTypeAccountingPM> result = (from d in repository.context.ChargeTypeAccountings.Include("VatType").Include("ChargesType")
                                                         where d.ChargeTypeId == chargeTypeId && d.Tenant == tenant
                                                         select new ChargeTypeAccountingPM()
                                                         {
                                                             Id = d.Id,
                                                             Tenant = d.Tenant,
                                                             ChargeTypeId = d.ChargeTypeId,
                                                             VatTypeId = d.VatTypeId,
                                                             ReceivableCreditAccount = d.ReceivableCreditAccount,
                                                             PayableDebitAccount = d.PayableDebitAccount,
                                                             VatTypeName = d.VatType == null ? "" : d.VatType.EnglishName,
                                                             ChargeTypeName = d.ChargeType == null ? "" : d.ChargeType.EnglishName,
                                                             PayableDebitGLAcountId = d.PayableDebitGLAcountId,
                                                             ReceivableCreditGLAccountId = d.ReceivableCreditGLAccountId,
                                                         });

            return result;
        }

        public IQueryable<ChargeTypeAccountingList> GetIQueryableEntityList(IQueryable<ChargeTypeAccounting> iQueryable)
        {
            IQueryable<ChargeTypeAccountingList> result = from d in iQueryable
                                                          select new ChargeTypeAccountingList()
                                                 {
                                                     Id = d.Id,
                                                     Tenant = d.Tenant,
                                                     ChargeTypeId = d.ChargeTypeId,
                                                     VatTypeId = d.VatTypeId,
                                                     ReceivableCreditAccount = d.ReceivableCreditAccount,
                                                     PayableDebitAccount = d.PayableDebitAccount,
                                                     PayableDebitGLAcountId = d.PayableDebitGLAcountId,
                                                     ReceivableCreditGLAccountId = d.ReceivableCreditGLAccountId,
                                                 };
            return result;
        }

        public ChargeTypeAccountingList GetSingleChargeTypeAccountingList(string id, int tenant)
        {
            ChargeTypeAccountingList entityList = (from d in repository.context.ChargeTypeAccountings
                                                   where d.Tenant == tenant && d.Id == id
                                                   select new ChargeTypeAccountingList()
                                               {
                                                   Id = d.Id,
                                                   Tenant = d.Tenant,
                                                   ChargeTypeId = d.ChargeTypeId,
                                                   VatTypeId = d.VatTypeId,
                                                   ReceivableCreditAccount = d.ReceivableCreditAccount,
                                                   PayableDebitAccount = d.PayableDebitAccount,
                                                   PayableDebitGLAcountId = d.PayableDebitGLAcountId,
                                                   ReceivableCreditGLAccountId = d.ReceivableCreditGLAccountId,
                                               }).FirstOrDefault();

            return entityList;
        }

        public ChargeTypeAccountingList GetSingleChargeTypeAccountingList(string chargeTypeId, string vatTypeId, int tenant)
        {
            return (from d in repository.context.ChargeTypeAccountings
                    where d.ChargeTypeId == chargeTypeId
                    && d.VatTypeId == vatTypeId
                    && d.Tenant == tenant
                    select new ChargeTypeAccountingList()
                    {
                        Id = d.Id,
                        ChargeTypeId = d.ChargeTypeId,
                        VatTypeId = d.VatTypeId,
                        Tenant = d.Tenant,
                        PayableDebitAccount = d.PayableDebitAccount,
                        ReceivableCreditAccount = d.ReceivableCreditAccount,
                        PayableDebitGLAcountId = d.PayableDebitGLAcountId,
                        ReceivableCreditGLAccountId = d.ReceivableCreditGLAccountId,
                    }).FirstOrDefault();
        }
    }
}
