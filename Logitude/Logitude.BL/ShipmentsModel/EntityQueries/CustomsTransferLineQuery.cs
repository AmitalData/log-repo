using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class CustomsTransferLineQuery
    {
        CustomsTransferLineRepository repository;        
        public CustomsTransferLineQuery(int tenant)
        {
            this.repository = new CustomsTransferLineRepository(tenant);
        }

        public CustomsTransferLineQuery(CustomsTransferLineRepository repository)
        {
            this.repository = repository;
        }

        public CustomsTransferLinePM GetSingleCustomsTransferLinePM(string id)
        {
            CustomsTransferLinePM result =

                (from a in repository.context.CustomsTransferLines
                 where a.Id == id
                 select new CustomsTransferLinePM()
                 {
                     Id = a.Id,
                     CustomsTransferHeaderId = a.CustomsTransferHeaderId,
                     ShipmentId = a.ShipmentId,
                     ShipmentNumber = a.ShipmentNumber,
                     Tenant = a.Tenant,
                     SearchFields = a.SearchFields,
                 }).FirstOrDefault();

            return result;
        }

        public IQueryable<CustomsTransferLinePM> GetCustomsTransferLinePMsForTransferHeader(string transferHeaderId, int tenant)
        {
            IQueryable<CustomsTransferLinePM> result =

                (from a in repository.context.CustomsTransferLines
                 where a.CustomsTransferHeaderId == transferHeaderId
                 && a.Tenant == tenant
                 select new CustomsTransferLinePM()
                 {
                     Id = a.Id,
                     CustomsTransferHeaderId = a.CustomsTransferHeaderId,
                     ShipmentId = a.ShipmentId,
                     ShipmentNumber = a.ShipmentNumber,
                     Tenant = a.Tenant,
                     SearchFields = a.SearchFields,
                 });

            return result;
        }
    }
}
