


using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class CustomsShipperQuery
    {
        CustomsShipperRepository repository;

        public CustomsShipperQuery()
        {
            repository = new CustomsShipperRepository();
        }

        public CustomsShipperQuery(int tenant)
        {
            repository = new CustomsShipperRepository(tenant);
        }

        public CustomsShipperQuery(CustomsShipperRepository repository)
        {
            this.repository = repository;
        }

        public IQueryable<CustomsShipperList> GetIQueryableEntityList(IQueryable<CustomsShipper> iQueryable)
        {
            IQueryable<CustomsShipperList> result = from a in iQueryable.Include("Card")
                                                        select new CustomsShipperList()
                                                        {
                                                            Tenant = a.Tenant,
                                                            Id = a.Id,
                                                            CustomsShipperCode = a.CustomsShipperCode,
                                                            ValidDepositionNumber = a.ValidDepositionNumber,
                                                            ValidityStartDate = a.ValidityStartDate,
                                                            ValidityEndDate = a.ValidityEndDate,
                                                            EnglishName = a.Card!=null?a.Card.EnglishName:null,
                                                            ShipperVAT = a.Card != null ? a.Card.VatNumber : null,
                                                            SearchFields =a.SearchFields,
                                                        };


            return result;
        }


        public CustomsShipperPM GetSinglePM(string id, int tenant)
        {
            CustomsShipperPM entity = (from a in repository.context.CustomsShippers
                                           where a.Tenant == tenant
                                           && a.Id == id
                                           select new CustomsShipperPM()
                                           {
                                               Tenant = a.Tenant,
                                               Id = a.Id,
                                               CustomsShipperCode = a.CustomsShipperCode,
                                               ValidDepositionNumber = a.ValidDepositionNumber,
                                               ValidityStartDate = a.ValidityStartDate,
                                               ValidityEndDate = a.ValidityEndDate,
                                               SearchFields = a.SearchFields,

                                           }).FirstOrDefault();

            if (entity != null)
            {
                CardQuery cardQuery = new CardQuery(tenant);
                CardList card = cardQuery.GetCardListForCustomsShipperById(entity.Id, tenant);
                if (card != null)
                {
                    entity.EnglishName = card.EnglishName;
                    entity.ShipperVAT = card.VatNumber;
                    entity.CountryId = card.CountryId;
                    entity.CountryCode = card.CountryCode;
                    entity.CountryName = card.CountryName;
                    entity.LocalName = card.LocalName;
                    entity.Code = card.Code;
                    entity.CreatedByUserId = card.CreatedByUserId;
                    entity.UpdatedByUserId = card.UpdatedByUserId;
                    entity.CreateDate = card.CreateDate;
                    entity.UpdateDate = card.UpdateDate;

                }
            }



            return entity;
        }

        public CustomsShipperPM GetSinglePMByShipperCode(string shipperCode, int tenant)
        {
            CustomsShipperPM entity = (from a in repository.context.CustomsShippers
                                       where a.Tenant == tenant
                                       && a.CustomsShipperCode == shipperCode
                                       select new CustomsShipperPM()
                                       {
                                           Tenant = a.Tenant,
                                           Id = a.Id,
                                           CustomsShipperCode = a.CustomsShipperCode,
                                           ValidDepositionNumber = a.ValidDepositionNumber,
                                           ValidityStartDate = a.ValidityStartDate,
                                           ValidityEndDate = a.ValidityEndDate,
                                           SearchFields = a.SearchFields,

                                       }).FirstOrDefault();

            if (entity != null)
            {
                CardQuery cardQuery = new CardQuery(tenant);
                CardList card = cardQuery.GetCardListForCustomsShipperById(entity.Id, tenant);
                if (card != null)
                {
                    entity.EnglishName = card.EnglishName;
                    entity.ShipperVAT = card.VatNumber;
                    entity.CountryId = card.CountryId;
                    entity.CountryCode = card.CountryCode;
                    entity.CountryName = card.CountryName;
                    entity.LocalName = card.LocalName;
                    entity.Code = card.Code;
                    entity.CreatedByUserId = card.CreatedByUserId;
                    entity.UpdatedByUserId = card.UpdatedByUserId;
                    entity.CreateDate = card.CreateDate;
                    entity.UpdateDate = card.UpdateDate;

                }
            }

            return entity;
        }







        public IQueryable<CustomsShipperPM> GetCustomsShipperPMsByTenant(int tenant)
        {
            IQueryable<CustomsShipperPM> CustomsShipperPMs = from a in repository.context.CustomsShippers
                                                             where a.Tenant == tenant
                                                                     select new CustomsShipperPM()
                                                                     {
                                                                         Tenant = a.Tenant,
                                                                         Id = a.Id,
                                                                         CustomsShipperCode = a.CustomsShipperCode,
                                                                         ValidDepositionNumber = a.ValidDepositionNumber,
                                                                         ValidityStartDate = a.ValidityStartDate,
                                                                         ValidityEndDate = a.ValidityEndDate,
                                                                         SearchFields = a.SearchFields,


                                                                     };
            return CustomsShipperPMs;
        }

        public IQueryable<CustomsShipperList> GetCustomsShipperListsByTenant(int tenant)
        {
            IQueryable<CustomsShipperList> CustomsShipperLists = from a in repository.context.CustomsShippers
                                                                         where a.Tenant == tenant
                                                                         select new CustomsShipperList()
                                                                         {
                                                                             Tenant = a.Tenant,
                                                                             Id = a.Id,
                                                                             CustomsShipperCode = a.CustomsShipperCode,
                                                                             ValidDepositionNumber = a.ValidDepositionNumber,
                                                                             ValidityStartDate = a.ValidityStartDate,
                                                                             ValidityEndDate = a.ValidityEndDate,
                                                                             SearchFields = a.SearchFields,

                                                                         };
            return CustomsShipperLists;
        }


      

    }
}