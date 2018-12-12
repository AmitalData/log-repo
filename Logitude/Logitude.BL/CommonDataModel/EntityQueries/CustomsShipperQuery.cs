


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
            IQueryable<CustomsShipperList> result = from a in iQueryable
                                                        select new CustomsShipperList()
                                                        {
                                                            Tenant = a.Tenant,
                                                            Id = a.Id,
                                                            CustomsShipperCode = a.CustomsShipperCode,
                                                            ValidDepositionNumber = a.ValidDepositionNumber,
                                                            ValidityStartDate = a.ValidityStartDate,
                                                            ValidityEndDate = a.ValidityEndDate,
                                                            FutureDepositionExist = a.FutureDepositionExist,


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
                                               FutureDepositionExist = a.FutureDepositionExist,
                                           }).FirstOrDefault();



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
                                                                         FutureDepositionExist = a.FutureDepositionExist,
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
                                                                             FutureDepositionExist = a.FutureDepositionExist,
                                                                         };
            return CustomsShipperLists;
        }




    }
}