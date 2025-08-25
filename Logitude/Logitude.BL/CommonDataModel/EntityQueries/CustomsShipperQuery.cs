


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
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Server.Tools.CustomFields;
using Logitude.BL.InfrastructureModel.EntityQueries;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class CustomsShipperQuery
    {
        CustomsShipperRepository repository;



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
            string objcetTableId = new ObjectTableQuery(0).GetObjectTableIdByName("Card");
            IQueryable<CustomsShipperList> result = from a in iQueryable.Include("Card")
                                                    join customFieldsMainObject in repository.context.CustomFieldsMainObjects.Where(d => d.ObjectTableId == objcetTableId) on a.Id equals customFieldsMainObject.EntityId into customFieldsMainObjectJoin
                                                    from customFieldsMainObject in customFieldsMainObjectJoin.DefaultIfEmpty()
                                                    select new CustomsShipperList()
                                                    {
                                                        Tenant = a.Tenant,
                                                        Id = a.Id,
                                                        CustomsShipperCode = a.CustomsShipperCode,
                                                        ValidDepositionNumber = a.ValidDepositionNumber,
                                                        ValidityStartDate = a.ValidityStartDate,
                                                        ValidityEndDate = a.ValidityEndDate,
                                                        EnglishName = a.Card != null ? a.Card.EnglishName : null,
                                                        ShipperVAT = a.Card != null ? a.Card.VatNumber : null,
                                                        SearchFields = a.SearchFields,
                                                        Field1 = customFieldsMainObject != null ? customFieldsMainObject.Field1 : null,
                                                        Field2 = customFieldsMainObject != null ? customFieldsMainObject.Field2 : null,
                                                        Field3 = customFieldsMainObject != null ? customFieldsMainObject.Field3 : null,
                                                        Field4 = customFieldsMainObject != null ? customFieldsMainObject.Field4 : null,
                                                        Field5 = customFieldsMainObject != null ? customFieldsMainObject.Field5 : null,
                                                        Field6 = customFieldsMainObject != null ? customFieldsMainObject.Field6 : null,
                                                        Field7 = customFieldsMainObject != null ? customFieldsMainObject.Field7 : null,
                                                        Field8 = customFieldsMainObject != null ? customFieldsMainObject.Field8 : null,
                                                        Field9 = customFieldsMainObject != null ? customFieldsMainObject.Field9 : null,
                                                        Field10 = customFieldsMainObject != null ? customFieldsMainObject.Field10 : null,
                                                        Field11 = customFieldsMainObject != null ? customFieldsMainObject.Field11 : null,
                                                        Field12 = customFieldsMainObject != null ? customFieldsMainObject.Field12 : null,
                                                        Field13 = customFieldsMainObject != null ? customFieldsMainObject.Field13 : null,
                                                        Field14 = customFieldsMainObject != null ? customFieldsMainObject.Field14 : null,
                                                        Field15 = customFieldsMainObject != null ? customFieldsMainObject.Field15 : null,
                                                        Field16 = customFieldsMainObject != null ? customFieldsMainObject.Field16 : null,
                                                        Field17 = customFieldsMainObject != null ? customFieldsMainObject.Field17 : null,
                                                        Field18 = customFieldsMainObject != null ? customFieldsMainObject.Field18 : null,
                                                        Field19 = customFieldsMainObject != null ? customFieldsMainObject.Field19 : null,
                                                        Field20 = customFieldsMainObject != null ? customFieldsMainObject.Field20 : null,
                                                        Field21 = customFieldsMainObject != null ? customFieldsMainObject.Field21 : null,
                                                        Field22 = customFieldsMainObject != null ? customFieldsMainObject.Field22 : null,
                                                        Field23 = customFieldsMainObject != null ? customFieldsMainObject.Field23 : null,
                                                        Field24 = customFieldsMainObject != null ? customFieldsMainObject.Field24 : null,
                                                        Field25 = customFieldsMainObject != null ? customFieldsMainObject.Field25 : null,
                                                        Field26 = customFieldsMainObject != null ? customFieldsMainObject.Field26 : null,
                                                        Field27 = customFieldsMainObject != null ? customFieldsMainObject.Field27 : null,
                                                        Field28 = customFieldsMainObject != null ? customFieldsMainObject.Field28 : null,
                                                        Field29 = customFieldsMainObject != null ? customFieldsMainObject.Field29 : null,
                                                        Field30 = customFieldsMainObject != null ? customFieldsMainObject.Field30 : null,
                                                        Field31 = customFieldsMainObject != null ? customFieldsMainObject.Field31 : null,
                                                        Field32 = customFieldsMainObject != null ? customFieldsMainObject.Field32 : null,
                                                        Field33 = customFieldsMainObject != null ? customFieldsMainObject.Field33 : null,
                                                        Field34 = customFieldsMainObject != null ? customFieldsMainObject.Field34 : null,
                                                        Field35 = customFieldsMainObject != null ? customFieldsMainObject.Field35 : null,
                                                        Field36 = customFieldsMainObject != null ? customFieldsMainObject.Field36 : null,
                                                        Field37 = customFieldsMainObject != null ? customFieldsMainObject.Field37 : null,
                                                        Field38 = customFieldsMainObject != null ? customFieldsMainObject.Field38 : null,
                                                        Field39 = customFieldsMainObject != null ? customFieldsMainObject.Field39 : null,
                                                        Field40 = customFieldsMainObject != null ? customFieldsMainObject.Field40 : null,
                                                        Field41 = customFieldsMainObject != null ? customFieldsMainObject.Field41 : null,
                                                        Field42 = customFieldsMainObject != null ? customFieldsMainObject.Field42 : null,
                                                        Field43 = customFieldsMainObject != null ? customFieldsMainObject.Field43 : null,
                                                        Field44 = customFieldsMainObject != null ? customFieldsMainObject.Field44 : null,
                                                        Field45 = customFieldsMainObject != null ? customFieldsMainObject.Field45 : null,
                                                        Field46 = customFieldsMainObject != null ? customFieldsMainObject.Field46 : null,
                                                        Field47 = customFieldsMainObject != null ? customFieldsMainObject.Field47 : null,
                                                        Field48 = customFieldsMainObject != null ? customFieldsMainObject.Field48 : null,
                                                        Field49 = customFieldsMainObject != null ? customFieldsMainObject.Field49 : null,
                                                        Field50 = customFieldsMainObject != null ? customFieldsMainObject.Field50 : null,
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

            if (entity != null)
            {
                new EntityCustomFieldService(new EntityCustomFieldServiceArgs() { ObjectTableName = "CustomsShipper", Tenant = tenant, Type = "PM", Entities = new List<CustomsShipperPM> { entity }.Cast<object>().ToList() }).Set();
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

            if (entity != null)
            {
                new EntityCustomFieldService(new EntityCustomFieldServiceArgs() { ObjectTableName = "CustomsShipper", Tenant = tenant, Type = "PM", Entities = new List<CustomsShipperPM> { entity }.Cast<object>().ToList() }).Set();
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