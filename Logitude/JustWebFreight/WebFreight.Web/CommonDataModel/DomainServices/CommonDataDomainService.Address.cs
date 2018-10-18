using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using WebFreight.Web.Helpers;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        public IQueryable<AddressPM> GetAddressesByTenant(int tenant)
        {
            addressRepository = new AddressRepository(tenant);
            return null;
        }

        public void InsertAddress(AddressPM entityPM)
        {
            if (!entityPM.IsCreatedWithPartner)
            {
                if (objectContext == null)
                {
                    objectContext = CommonDataContext.GetContext(entityPM.Tenant);
                }

                AddressService service = new AddressService(objectContext, entityPM.Tenant);
                service.Create(entityPM);
            }
        }

        public void UpdateAddress(AddressPM entityPM)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            AddressService service = new AddressService(objectContext, entityPM.Tenant);
            service.Update(entityPM);
        }

        public void DeleteAddress(AddressPM entityPM)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            addressRepository = new AddressRepository(objectContext);

            Address removedEntity = addressRepository.GetSingleAddress(entityPM.Id, entityPM.Tenant);
            if (removedEntity != null)
            {
                addressRepository.Remove(removedEntity);
            }
        }
    }
}