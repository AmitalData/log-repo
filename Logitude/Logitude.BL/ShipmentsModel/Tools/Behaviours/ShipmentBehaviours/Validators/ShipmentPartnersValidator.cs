using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.Initializers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.Behaviours.ShipmentBehaviours.Validators
{
    public class ShipmentPartnersValidator : IServiceValidator
    {
        public ShipmentPartnersValidatorParameters Args { get; private set; }
        public void Validate(IServiceInitializer initializer)
        {
            ShipmentServiceInitializer serviceInitializer = (ShipmentServiceInitializer)initializer;
            this.Args = new ShipmentPartnersValidatorParameters
            {
                Tenant = serviceInitializer.Tenant,
                TenantPM = serviceInitializer.LoggedTenantPM,
                EntityPM = serviceInitializer.EntityPM,
                CardRepository = serviceInitializer.CardRepository,
            };
            this.RunValidator();
        }

        private void RunValidator()
        {
            if (this.Args != null)
            {
                if (!string.IsNullOrEmpty(this.Args.EntityPM.ShipperId))
                {
                    ValidateShipperAndConsigneePartners(this.Args.EntityPM, "Shipper", this.Args.EntityPM.ShipperId);
                }

                if (!string.IsNullOrEmpty(this.Args.EntityPM.ConsigneeId))
                {
                    ValidateShipperAndConsigneePartners(this.Args.EntityPM, "Consignee", this.Args.EntityPM.ConsigneeId);
                }
            }
        }

        private void ValidateShipperAndConsigneePartners(ShipmentPM entityPM, string partnerTypeName, string partnerId)
        {
            Card partner = this.Args.CardRepository.GetSingleCard(partnerId, entityPM.Tenant);
            if (partner != null)
            {
                if (entityPM.ShipmentLevelCode == "C")
                {
                    if (this.Args.TenantPM.AllowCustomersInAgentsLOV)
                    {
                        if (IsPartnerNotCustomerAndAgent(partner))
                        {
                            throw new ApplicationException(partnerTypeName + " partner type should be agent or customer");
                        }
                    }

                    else
                    {
                        if (partner.PartnerTypeId != "AG")
                        {
                            throw new ApplicationException(partnerTypeName + " partner type should be agent");
                        }
                    }
                }
                else
                {
                    if (this.Args.TenantPM.AllowAgentInCustomersLOV)
                    {
                        if (IsPartnerNotCustomerAndAgent(partner))
                        {
                            SetShipperAndConsigneeValidationExceptionForDirectAndHouseShipment(entityPM, partner, partnerTypeName);
                        }
                    }

                    else
                    {
                        if (partner.PartnerTypeId != "CS")
                        {
                            SetShipperAndConsigneeValidationExceptionForDirectAndHouseShipment(entityPM, partner, partnerTypeName);
                        }
                    }
                }
            }
        }

        private void SetShipperAndConsigneeValidationExceptionForDirectAndHouseShipment(ShipmentPM entityPM, Card card, string partnerTypeName)
        {
            if (IsInlandDomesticShipment(entityPM))
            {
                if (card.PartnerTypeId != "WH")
                {
                    throw new ApplicationException(partnerTypeName + " partner type should be customer or Warehouse");
                }
            }
            else
            {
                throw new ApplicationException(partnerTypeName + " partner type should be customer");
            }
        }

        private bool IsInlandDomesticShipment(ShipmentPM entityPM)
        {
            return entityPM.DirectionId == "D" && entityPM.TransportModeId == "I";
        }

        private bool IsPartnerNotCustomerAndAgent(Card card)
        {
            return (card.PartnerTypeId != "CS" && card.PartnerTypeId != "AG");
        }

        public class ShipmentPartnersValidatorParameters {
            public int Tenant { get; set; }
            public TenantPM TenantPM { get; set; }
            public CardRepository CardRepository { get; set; }
            public ShipmentPM EntityPM { get;  set; }
        }

    }
}
