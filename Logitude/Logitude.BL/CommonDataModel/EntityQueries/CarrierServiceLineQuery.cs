using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class CarrierServiceLineQuery
    {
        CarrierServiceLineRepository repository;

        public CarrierServiceLineQuery()
        {
            repository = new CarrierServiceLineRepository();
        }

        public CarrierServiceLineQuery(int tenant)
        {
            repository = new CarrierServiceLineRepository(tenant);
        }

        public CarrierServiceLineQuery(CarrierServiceLineRepository CarrierServiceLineRepository)
        {
            repository = CarrierServiceLineRepository;
        }

        public CarrierServiceLinePM GetSinglePM(string id, int tenant)
        {
            CarrierServiceLinePM entity =

                (from a in repository.context.CarrierServiceLines
                 where a.Tenant == tenant && a.Id == id
                 select new CarrierServiceLinePM()
                 {
                     CardId = a.CardId,
                     Id = a.Id,
                     Tenant = a.Tenant,
                     PartnerTypeId = a.PartnerTypeId,
                     Name = a.Name,
                     Description = a.Description,
                     SearchFields = a.SearchFields,
                     Inactive = a.Inactive,
                 }).FirstOrDefault();

            return entity;
        }

        public List<CarrierServiceLinePM> GetCarrierServiceLinePMsByCardId(string cardId, int tenant)
        {
            List<CarrierServiceLinePM> result =

                (from a in repository.context.CarrierServiceLines
                 where a.Tenant == tenant && a.CardId == cardId
                 select new CarrierServiceLinePM()
                 {
                     CardId = a.CardId,
                     Id = a.Id,
                     Tenant = a.Tenant,
                     PartnerTypeId = a.PartnerTypeId,
                     Name = a.Name,
                     Description = a.Description,
                     SearchFields = a.SearchFields,
                     Inactive = a.Inactive,
                 }).ToList();

            return result;
        }

        public IQueryable<CarrierServiceLineList> GetIQueryableEntityList(IQueryable<CarrierServiceLine> iQueryable)
        {
            IQueryable<CarrierServiceLineList> result =
                from a in iQueryable
                select new CarrierServiceLineList()
                {
                    CardId = a.CardId,
                    Id = a.Id,
                    Tenant = a.Tenant,
                    PartnerTypeId = a.PartnerTypeId,
                    Name = a.Name,
                    Description = a.Description,
                    SearchFields = a.SearchFields,
                    Inactive = a.Inactive,
                };

            return result;
        }
    }
}
