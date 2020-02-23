using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class CustomsTransferHeaderQuery
    {
        CustomsTransferHeaderRepository repository;        
        public CustomsTransferHeaderQuery(int tenant)
        {
            this.repository = new CustomsTransferHeaderRepository(tenant);
        }

        public CustomsTransferHeaderQuery(CustomsTransferHeaderRepository repository)
        {
            this.repository = repository;
        }

        public CustomsTransferHeaderPM GetSinglePM(string id, int tenant)
        {
            CustomsTransferLineQuery linesQuery = new CustomsTransferLineQuery(tenant);

            CustomsTransferHeaderPM entityPM =

                (from a in repository.context.CustomsTransferHeaders.Include("CreatedByUser").Include("CreatedByUser.Contact").Include("CustomsTransferType")
                 where a.Id == id && a.Tenant == tenant
                 select new CustomsTransferHeaderPM()
                 {
                     Id = a.Id,
                     CustomsTransferTypeCode = a.CustomsTransferTypeCode,
                     FileName = a.FileName,
                     Tenant = a.Tenant,
                     TransferDate = a.TransferDate,
                     TransferNumber = a.TransferNumber,
                     CreatedByUserId = a.CreatedByUserId,
                     CreatedByUserName = a.CreatedByUser == null ? "" : (a.CreatedByUser.Contact == null ? "" : a.CreatedByUser.Contact.EnglishName),
                     CustomsTransferTypeName = a.CustomsTransferType == null ? "" : a.CustomsTransferType.Name,
                     SearchFields = a.SearchFields,
                     Notes = a.Notes,
                     ShipmentNumber = a.ShipmentNumber
                 }).FirstOrDefault();

            entityPM.CustomsTransferLines = linesQuery.GetCustomsTransferLinePMsForTransferHeader(id, tenant).ToList();

            ShipmentRepository myRepository = new ShipmentRepository(tenant);

            foreach (CustomsTransferLinePM item in entityPM.CustomsTransferLines)
            {
                Shipment entity = myRepository.GetSingleShipment(item.ShipmentId, tenant);

                if (entity != null)
                {
                    item.ArrivalDate = entity.FinalArrivalDate;
                    item.Shipper = entity.ShipperName;
                    item.Consignee = entity.ConsigneeName;
                    item.Status = entity.EntityStatus == null ? "" : entity.EntityStatus.Name;
                }
            }

            return entityPM;
        }
        
        public IQueryable<CustomsTransferHeaderList> GetIQueryableEntityList(IQueryable<CustomsTransferHeader> iQueryable)
        {
            var result = from a in iQueryable
                         select new CustomsTransferHeaderList()
                         {
                             Id = a.Id,
                             CustomsTransferTypeCode = a.CustomsTransferTypeCode,
                             FileName = a.FileName,
                             SearchFields = a.SearchFields,
                             Tenant = a.Tenant,
                             TransferDate = a.TransferDate,
                             TransferNumber = a.TransferNumber,
                             CreatedByUserId = a.CreatedByUserId,
                             Notes = a.Notes,
                             CreatedByUserName = a.CreatedByUser == null ? "" : (a.CreatedByUser.Contact == null ? "" : a.CreatedByUser.Contact.EnglishName),
                             CustomsTransferTypeName = a.CustomsTransferType == null ? "" : a.CustomsTransferType.Name,
                             ShipmentNumber = a.ShipmentNumber
                         };

            return result;
        }
    }
}
